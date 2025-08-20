using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace SampleWebService.Models
{
    public class Page
    {
        public int Id { get; set; }
        public string PageName { get; set; }

        // DB’de JSONB olarak saklanacak
        [Column(TypeName = "jsonb")]
        public string Data { get; set; }

        // API tarafında object olarak çalışacak
        [NotMapped]
        public object JsonData
        {
            get => string.IsNullOrEmpty(Data) ? null : JsonSerializer.Deserialize<object>(Data);
            set => Data = JsonSerializer.Serialize(value);
        }
    }
}
