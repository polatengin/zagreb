using System;
using Newtonsoft.Json;

namespace zagreb
{
  public partial class Sender
  {
    [JsonProperty("login")]
    public string Login { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("node_id")]
    public string NodeId { get; set; }

    [JsonProperty("avatar_url")]
    public Uri AvatarUrl { get; set; }

    [JsonProperty("gravatar_id")]
    public string GravatarId { get; set; }

    [JsonProperty("url")]
    public Uri Url { get; set; }

    [JsonProperty("html_url")]
    public Uri HtmlUrl { get; set; }

    [JsonProperty("followers_url")]
    public Uri FollowersUrl { get; set; }

    [JsonProperty("following_url")]
    public string FollowingUrl { get; set; }

    [JsonProperty("gists_url")]
    public string GistsUrl { get; set; }

    [JsonProperty("starred_url")]
    public string StarredUrl { get; set; }

    [JsonProperty("subscriptions_url")]
    public Uri SubscriptionsUrl { get; set; }

    [JsonProperty("organizations_url")]
    public Uri OrganizationsUrl { get; set; }

    [JsonProperty("repos_url")]
    public Uri ReposUrl { get; set; }

    [JsonProperty("events_url")]
    public string EventsUrl { get; set; }

    [JsonProperty("received_events_url")]
    public Uri ReceivedEventsUrl { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("site_admin")]
    public bool SiteAdmin { get; set; }
  }

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
