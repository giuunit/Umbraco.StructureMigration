using Newtonsoft.Json;

namespace GiuUnit.Umbraco.StructureMigration
{
    public abstract class MigrationProcessBase : IMigrationProcess
    {
        [JsonProperty(PropertyName = "destination")]
        public abstract string Destination { get; }

        [JsonProperty(PropertyName = "origin")]
        public abstract string Origin { get; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include, PropertyName = "label")]
        public string Label
        {
            get
            {
                return $"[{Origin}] to [{Destination}]";
            }
        }
    }
}
