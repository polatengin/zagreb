using System;
using Newtonsoft.Json;

namespace zagreb
{
  public partial class Comments
  {
    [JsonProperty("href")]
    public string Href { get; set; }
  }
}
