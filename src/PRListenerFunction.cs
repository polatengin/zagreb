using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Linq;

namespace zagreb
{
  public static class PRListenerFunction
  {
    [FunctionName(nameof(PRListenerFunction))]
    public static async Task<IActionResult> Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
      ILogger log)
    {
      log.LogInformation("a new PR has been created...");

      req.Headers.TryGetValue("X-GitHub-Event", out StringValues eventName);
      req.Headers.TryGetValue("X-Hub-Signature", out StringValues signature);
      req.Headers.TryGetValue("X-GitHub-Delivery", out StringValues delivery);

      log.LogInformation($"EventName : {eventName}");
      log.LogInformation($"Signature : {signature}");
      log.LogInformation($"Delivery : {delivery}");

      var body = await new StreamReader(req.Body).ReadToEndAsync();

      log.LogInformation($"Body has been read to end ({body.Length} characters)");

      var payload = JsonConvert.DeserializeObject<WebHookPayload>(body);

      log.LogInformation($"Body parsed and stored in the payload object (pull request number is {payload.Number})");

      var isAzureRelated = payload?.PullRequest?.Labels.Count(e => e.Name == "Azure") > 0;

      if (!isAzureRelated)
      {
        return new OkObjectResult("");
      }

      var repo = payload?.Repository?.FullName;
      log.LogInformation($"Repo : {repo}");

      var branch = payload?.PullRequest?.Head?.Ref;
      log.LogInformation($"Branch : {branch}");

      log.LogInformation("CI pipeline is about to be triggered");

      using (var client = new HttpClient())
      {
        var GITHUB_PAT = Environment.GetEnvironmentVariable("GITHUB_PAT");
        var GITHUB_ACCOUNT_NAME = Environment.GetEnvironmentVariable("GITHUB_ACCOUNT_NAME");
        var GITHUB_REPO_NAME = Environment.GetEnvironmentVariable("GITHUB_REPO_NAME");
        var GITHUB_ACTION_ID = Environment.GetEnvironmentVariable("GITHUB_ACTION_ID");

        var request = new HttpRequestMessage(HttpMethod.Post, $"https://api.github.com/repos/{GITHUB_ACCOUNT_NAME}/{GITHUB_REPO_NAME}/actions/workflows/{GITHUB_ACTION_ID}/dispatches")
        {
          Headers = {
            Authorization = new AuthenticationHeaderValue("token", GITHUB_PAT),
            UserAgent = { ProductInfoHeaderValue.Parse("RunPipeline") },
            Accept = { MediaTypeWithQualityHeaderValue.Parse("application/vnd.github.v3+json") }
          },
          Content = new StringContent($"{{ \"ref\":\"master\", \"inputs\": {{\"repo\": \"{repo}\", \"branch\": \"{branch}\"}} }}")
        };
        using (var response = await client.SendAsync(request))
        {
          log.LogInformation($"CI pipeline is triggered, response is : {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
        }
      }

      return new OkObjectResult("OK");
    }
  }
}
