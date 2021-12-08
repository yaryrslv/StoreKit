using System.ComponentModel;

namespace StoreKit.Domain.Constants
{
    public static class PermissionConstants
    {
        [DisplayName("Identity")]
        [Description("Identity Permissions")]
        public static class Identity
        {
            public const string Register = "Permissions.Identity.Register";
        }

        [DisplayName("Roles")]
        [Description("Roles Permissions")]
        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string ListAll = "Permissions.Roles.ViewAll";
            public const string Register = "Permissions.Roles.Register";
            public const string Update = "Permissions.Roles.Update";
            public const string Remove = "Permissions.Roles.Remove";
        }

        [DisplayName("Products")]
        [Description("Products Permissions")]
        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
            public const string Search = "Permissions.Products.Search";
        }

        [DisplayName("Brands")]
        [Description("Brands Permissions")]
        public static class Brands
        {
            public const string View = "Permissions.Brands.View";
            public const string Create = "Permissions.Brands.Create";
            public const string Edit = "Permissions.Brands.Edit";
            public const string Delete = "Permissions.Brands.Delete";
            public const string Search = "Permissions.Brands.Search";
        }

        [DisplayName("News")]
        [Description("News Permissions")]
        public static class News
        {
            public const string View = "Permissions.News.View";
            public const string Create = "Permissions.News.Create";
            public const string Edit = "Permissions.News.Edit";
            public const string Delete = "Permissions.News.Delete";
            public const string Search = "Permissions.News.Search";
        }

        [DisplayName("Role Claims")]
        [Description("Role Claims Permissions")]
        public static class RoleClaims
        {
            public const string View = "Permissions.RoleClaims.View";
            public const string Create = "Permissions.RoleClaims.Create";
            public const string Edit = "Permissions.RoleClaims.Edit";
            public const string Delete = "Permissions.RoleClaims.Delete";
            public const string Search = "Permissions.RoleClaims.Search";
        }

        [DisplayName("TagTypes")]
        [Description("TagTypes Permissions")]
        public static class TagTypes
        {
            public const string View = "Permissions.TagTypes.View";
            public const string Create = "Permissions.TagTypes.Create";
            public const string Edit = "Permissions.TagTypes.Edit";
            public const string Delete = "Permissions.TagTypes.Delete";
            public const string Search = "Permissions.TagTypes.Search";
        }
    }
}