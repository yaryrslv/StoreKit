namespace StoreKit.Shared.DTOs.Catalog
{
    public class CreateNewsRequest : IMustBeValid
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}