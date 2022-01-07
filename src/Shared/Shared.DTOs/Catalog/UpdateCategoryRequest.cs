namespace StoreKit.Shared.DTOs.Catalog
{
    public class UpdateCategoryRequest : IMustBeValid
    {
        public string Name { get; set; }
    }
}