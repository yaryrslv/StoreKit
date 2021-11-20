using System.Collections.Generic;

namespace StoreKit.Domain.Constants
{
    public static class DefaultPermissions
    {
        public static List<string> Basics => new List<string>()
        {
            PermissionConstants.Products.Search,
            PermissionConstants.Products.View,
            PermissionConstants.Brands.Search,
            PermissionConstants.Brands.View,
            PermissionConstants.News.Search,
            PermissionConstants.News.View
        };
    }
}