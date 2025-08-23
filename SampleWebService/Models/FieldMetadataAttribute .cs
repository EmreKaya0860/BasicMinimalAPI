namespace SampleWebService.Models
{
    public class FieldMetadataAttribute : Attribute
    {
        public string Label { get; }
        public string InputType { get; }
        public bool Required { get; }
        public string ApiUrl { get; }
        public string UpperItem { get; }
        public bool OnlyTable { get; }
        public FieldMetadataAttribute(string label, string inputType = "text", bool required = false, string apiUrl="", string upperItem="", bool onlyTable = false)
        {
            Label = label;
            InputType = inputType;
            Required = required;
            ApiUrl = apiUrl;
            UpperItem = upperItem;
            OnlyTable = onlyTable;
        }
    }
}
