using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Linq;
using System.Collections.Generic;

namespace zagreb
{
  public static class GitHubHelper
  {
    public static async Task<IEnumerable<PullRequest>> GetPullRequestsAsync()
    {
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

            return prs.Where(e => e.Labels.Count(e => e.Name == "Azure") > 0);
          }
        }

        return Enumerable.Empty<PullRequest>();
      }
    }

    public static async Task<bool> TriggerWorkflowAsync(string targetRepoName, string targetBranchName, long? pr_number)
    {
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
          Content = new StringContent($"{{ \"ref\":\"master\", \"inputs\": {{\"repo\": \"{targetRepoName}\", \"branch\": \"{targetBranchName}\", \"target_repository\": \"{GITHUB_ACCOUNT_NAME}/{GITHUB_REPO_NAME}\", \"target_pr\": \"{pr_number}\"}} }}")
        };
        using (var response = await client.SendAsync(request))
        {
          if (response.IsSuccessStatusCode)
          {
            return true;
          }
        }

        return false;
      }
    }
  }
}
