using StoreKit.Shared.DTOs;

namespace StoreKit.Application.Services.LocalStorage
{
    public class CropParametersDto : IDto
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}