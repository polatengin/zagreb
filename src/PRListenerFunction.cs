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
    }
  }
}
