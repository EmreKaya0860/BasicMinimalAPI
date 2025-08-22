namespace SampleWebService.Models
{
    public class FieldMetadataAttribute : Attribute
    {
        public string Label { get; }
        public string InputType { get; }
        public bool Required { get; }
        public string ApiUrl { get; }

        public FieldMetadataAttribute(string label, string inputType = "text", bool required = false, string apiUrl="")
        {
            Label = label;
            InputType = inputType;
            Required = required;
            ApiUrl = apiUrl;
        }
    }
}
