namespace SampleWebService.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }

        public int ProductBrandId { get; set; }
        public ProductBrand ProductBrand { get; set; }

        public int ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }
    }
}
