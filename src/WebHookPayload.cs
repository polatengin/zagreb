using System;
using Newtonsoft.Json;

namespace zagreb
{
  public partial class Links
  {
    [JsonProperty("self")]
    public Comments Self { get; set; }

    [JsonProperty("html")]
    public Comments Html { get; set; }

    [JsonProperty("issue")]
    public Comments Issue { get; set; }

    [JsonProperty("comments")]
    public Comments Comments { get; set; }

    [JsonProperty("review_comments")]
    public Comments ReviewComments { get; set; }

    [JsonProperty("review_comment")]
    public Comments ReviewComment { get; set; }

    [JsonProperty("commits")]
    public Comments Commits { get; set; }

    [JsonProperty("statuses")]
    public Comments Statuses { get; set; }
  }

  public partial class Comments
  {
    [JsonProperty("href")]
    public string Href { get; set; }
  }
}
