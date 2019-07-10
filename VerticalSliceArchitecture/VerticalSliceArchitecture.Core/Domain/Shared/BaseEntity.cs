using System;
using System.Collections.Generic;
using System.Text;

namespace VerticalSliceArchitecture.Core.Domain.Shared
{
    public abstract class BaseEntity<T>
    {
        public T Id { get;private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime UpdatedAt { get;private set; }  
        
        public void SetUpdatedAt()
        {
            UpdatedAt = DateTime.Now;
        }
    }
}
