using System;
using StoreKit.Domain.Contracts;
using StoreKit.Domain.Extensions;

namespace StoreKit.Domain.Entities.Catalog
{
    public class Comments : AuditableEntity, IMustHaveTenant
    {
        public string CommentatorName { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime CreationTime { get; private set; }
        public string TenantKey { get; set; }
        public Comments(string title, string description, string commentatorName)
        {
            CommentatorName = commentatorName;
            Title = title;
            Description = description;
            CreationTime = DateTime.Now.ToUniversalTime();
        }
        public Comments Update(string name, string description, string commentatorName)
        {
            if (name != null && !Title.NullToString().Equals(name)) Title = name;
            if (description != null && !Description.NullToString().Equals(description)) Description = description;
            if (commentatorName != null && !CommentatorName.NullToString().Equals(commentatorName)) CommentatorName = commentatorName;
            return this;
        }
    }
}