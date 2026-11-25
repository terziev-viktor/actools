extern alias Original;
using Newtonsoft.Json;
using Orig = Original::AcManager.Internal;

namespace AcManager.Internal {
    /// <summary>
    /// Same shape and JSON contract as the original <c>WebDownloadRule</c> (instances are cached as
    /// JSON by ModsWebBrowser). Values, including the computed <see cref="Domain"/>, come from an
    /// original instance.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class WebDownloadRule {
        private readonly Orig.WebDownloadRule _original;

        [JsonConstructor]
        public WebDownloadRule(string siteUrl, string rule, string[] data) {
            _original = new Orig.WebDownloadRule(siteUrl, rule, data);
        }

        [JsonProperty("siteUrl")]
        public string SiteUrl => _original.SiteUrl;

        [JsonProperty("rule")]
        public string Rule => _original.Rule;

        [JsonProperty("data")]
        public string[] Data => _original.Data;

        public string Domain => _original.Domain;
    }
}
