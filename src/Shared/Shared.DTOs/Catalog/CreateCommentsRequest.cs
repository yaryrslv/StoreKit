namespace StoreKit.Shared.DTOs.Catalog
{
    public class CreateCommentsRequest : IMustBeValid
    {
        public string CommentatorName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}