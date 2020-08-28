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
    }
  }
}
