using StoreKit.Shared.DTOs.General.Requests;

namespace StoreKit.Shared.DTOs.Identity.Requests
{
    public class UpdateProfileRequest : IMustBeValid
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public FileUploadRequest Image { get; set; }
    }
}