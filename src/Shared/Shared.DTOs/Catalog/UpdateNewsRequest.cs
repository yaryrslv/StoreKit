namespace StoreKit.Shared.DTOs.Catalog
{
    public class UpdateNewsRequest : IMustBeValid
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}