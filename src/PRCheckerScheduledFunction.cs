using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Linq;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

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

      using (var client = new HttpClient())
      {
        var GITHUB_PAT = Environment.GetEnvironmentVariable("GITHUB_PAT");
        var GITHUB_ACCOUNT_NAME = Environment.GetEnvironmentVariable("GITHUB_ACCOUNT_NAME");
        var GITHUB_REPO_NAME = Environment.GetEnvironmentVariable("GITHUB_REPO_NAME");
        var GITHUB_ACTION_ID = Environment.GetEnvironmentVariable("GITHUB_ACTION_ID");

        var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{GITHUB_ACCOUNT_NAME}/{GITHUB_REPO_NAME}/pulls")
        {
          Headers = {
            Authorization = new AuthenticationHeaderValue("token", GITHUB_PAT),
            UserAgent = { ProductInfoHeaderValue.Parse("RunPipeline") },
            Accept = { MediaTypeWithQualityHeaderValue.Parse("application/vnd.github.v3+json") }
          }
        };
        using (var response = await client.SendAsync(request))
        {
          if (response.IsSuccessStatusCode)
          {
            var body = await response.Content.ReadAsStringAsync();

            var prs = JsonConvert.DeserializeObject<PullRequest[]>(body);

            foreach (var pr in prs)
            {
              log.LogInformation($"{pr.Title} -> {string.Join(',', pr.Labels.Select(e => e.Name).ToList())}");

              var partitionKey = pr.Head.Repo.Name;
              var rowKey = pr.Number.ToString();

              if (pr.Labels.Count(e => e.Name == "Azure") > 0)
              {
                var result = table.Execute(TableOperation.Retrieve<PullRequest>(partitionKey, rowKey));

                if (result?.Result == null)
                {
                  pr.PartitionKey = partitionKey;
                  pr.RowKey = rowKey;

                  table.Execute(TableOperation.InsertOrMerge(pr));

                  result.Result = pr;

                  log.LogInformation("pr has been saved");
                }
              }
            }
          }
        }
      }
    }
  }
}
