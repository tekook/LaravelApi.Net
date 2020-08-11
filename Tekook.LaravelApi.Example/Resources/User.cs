using Newtonsoft.Json;
using Tekook.LaravelApi.Resources;

namespace Tekook.LaravelApi.Example.Resources
{
    public class User : Resource
    {
        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        public override object GetPrimaryKeyValue() => this.Id;
    }
}