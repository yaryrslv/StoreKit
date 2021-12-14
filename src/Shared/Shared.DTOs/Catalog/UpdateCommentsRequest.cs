namespace StoreKit.Shared.DTOs.Catalog
{
    public class UpdateCommentsRequest : IMustBeValid
    {
        public string CommentatorName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}