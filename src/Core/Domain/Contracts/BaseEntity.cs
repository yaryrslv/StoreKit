using MassTransit;
using System;

namespace StoreKit.Domain.Contracts
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }

        protected BaseEntity()
        {
            Id = NewId.Next().ToGuid();
        }
    }
}