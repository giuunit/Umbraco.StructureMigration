using Newtonsoft.Json;

namespace GiuUnit.Umbraco.StructureMigration.ViewModels
{
    public class PropertyTypeViewModel
    {
        [JsonProperty(PropertyName ="name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName ="alias")]
        public string Alias { get; set; }
        [JsonProperty(PropertyName ="propertyEditorAlias")]
        public string PropertyEditorAlias { get; set; }
        [JsonProperty(PropertyName = "contentTypeAlias")]
        public string ContentTypeAlias { get; set; }

        [JsonProperty(PropertyName = "isMemberType")]
        public bool IsMemberType { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include, PropertyName="label")]
        public string Label
        {
            get
            {
                return $"[{ContentTypeAlias}][{PropertyEditorAlias}] {Name}";
            }
        }
    }
}
