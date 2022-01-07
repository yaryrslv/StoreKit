namespace StoreKit.Shared.DTOs.Catalog.Comment
{
    public class CreateCommentRequest : IMustBeValid
    {
        public string CommentatorName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}