namespace StoreKit.Shared.DTOs.Catalog.News
{
    public class UpdateNewsRequest : IMustBeValid
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}