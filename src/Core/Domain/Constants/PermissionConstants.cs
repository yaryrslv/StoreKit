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

        [DisplayName("Categories")]
        [Description("Category Permissions")]
        public static class Categories
        {
            public const string View = "Permissions.Categories.View";
            public const string Create = "Permissions.Categories.Create";
            public const string Edit = "Permissions.Categories.Edit";
            public const string Delete = "Permissions.Categories.Delete";
            public const string Search = "Permissions.Categories.Search";
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
        [DisplayName("Comments")]
        [Description("Comments Permissions")]
        public static class Comments
        {
            public const string View = "Permissions.Comments.View";
            public const string Create = "Permissions.Comments.Create";
            public const string Edit = "Permissions.Comments.Edit";
            public const string Delete = "Permissions.Comments.Delete";
            public const string Search = "Permissions.Comments.Search";
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

        [DisplayName("Pages")]
        [Description("Pages Permissions")]
        public static class Pages
        {
            public const string View = "Permissions.Pages.View";
            public const string Create = "Permissions.Pages.Create";
            public const string Edit = "Permissions.Pages.Edit";
            public const string Delete = "Permissions.Pages.Delete";
            public const string Search = "Permissions.Pages.Search";
        }

        [DisplayName("StaticPages")]
        [Description("Pages Permissions")]
        public static class StaticPages
        {
            public const string View = "Permissions.StaticPages.View";
            public const string Create = "Permissions.StaticPages.Create";
            public const string Edit = "Permissions.StaticPages.Edit";
            public const string Delete = "Permissions.StaticPages.Delete";
            public const string Search = "Permissions.StaticPages.Search";
        }
    }
}