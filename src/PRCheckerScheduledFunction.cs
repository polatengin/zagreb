using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.Azure.Cosmos.Table;

namespace zagreb
{
  public static class PRCheckerScheduledFunction
  {
    [FunctionName(nameof(PRCheckerScheduledFunction))]
    public static async Task Run(
      [TimerTrigger("* */15 * * * *")] TimerInfo myTimer,
      [Table("zagreb")] CloudTable table,
      ILogger log)
    {
      log.LogInformation("schedule...");

      var prs = await GitHubHelper.GetPullRequestsAsync();

      foreach (var pr in prs)
      {
        log.LogInformation($"{pr.Title} -> {string.Join(',', pr.Labels.Select(e => e.Name).ToList())}");

        var partitionKey = pr.Head.Repo.Name;
        var rowKey = pr.Number.ToString();

        pr.PartitionKey = partitionKey;
        pr.RowKey = rowKey;

        var result = table.Execute(TableOperation.Retrieve<PullRequest>(partitionKey, rowKey));

        if (result?.Result == null)
        {
          table.Execute(TableOperation.InsertOrMerge(pr));

          log.LogInformation("pr has been saved");
        }

        var fromTable = result?.Result as PullRequest;

        if (fromTable == null || fromTable?.UpdatedAt != pr.UpdatedAt)
        {
          log.LogWarning($"fromTable: {fromTable?.UpdatedAt}");
          log.LogWarning($"pr: {pr?.UpdatedAt}");

          table.Execute(TableOperation.InsertOrMerge(pr));

          log.LogInformation($"{pr.Number} either detected first time or has been changed since last time");

          var repo = pr.Head.Repo.FullName;
          log.LogInformation($"Repo : {repo}");

          var branch = pr.Head.Ref;
          log.LogInformation($"Branch : {branch}");

          var isTriggered = await GitHubHelper.TriggerWorkflowAsync(repo, branch);

          if (isTriggered)
          {
            log.LogInformation("CI pipeline is successfully triggered");
          }
        }
      }
    }
  }
}
