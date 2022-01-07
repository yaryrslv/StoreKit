using System.Collections.Generic;

namespace StoreKit.Domain.Constants
{
    public static class DefaultPermissions
    {
        public static List<string> Basics => new List<string>()
        {
            PermissionConstants.Products.Search,
            PermissionConstants.Products.View,
            PermissionConstants.Categories.Search,
            PermissionConstants.Categories.View,
            PermissionConstants.News.Search,
            PermissionConstants.News.View,
            PermissionConstants.Comments.Search,
            PermissionConstants.Comments.View
        };
    }
}