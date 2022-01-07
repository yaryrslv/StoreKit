namespace StoreKit.Shared.DTOs.Catalog
{
    public class CreateCategoryRequest : IMustBeValid
    {
        public string Name { get; set; }
    }
}