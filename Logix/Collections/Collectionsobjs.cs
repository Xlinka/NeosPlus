using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using BaseX;
namespace FrooxEngine.Logix.Collections
{
    public class CollectionsItemValue<T> : SyncObject, IEquatable<CollectionsItemValue<T>>,ICollectionsObj<T>
    {
        public readonly Sync<T> Value;
        public T Get()
        {
            return Value.Value;
        }

        public void Set(T obj)
        {
            Value.Value = obj;
        }

        public bool SameValue(T other)
        {
            return EqualityComparer<T>.Default.Equals(other, this.Value.Value);
        }

        public override bool Equals(object other)
        {
            if (other is CollectionsItemValue<T>)
            {
                return this.Equals((CollectionsItemValue<T>)other);
            }
            return false;
        }

        public bool Equals(CollectionsItemValue<T> other)
        {
            return EqualityComparer<T>.Default.Equals(other.Value.DirectValue, this.Value.DirectValue);
        }

    }

    public class CollectionsItemRef<T> : SyncObject, IEquatable<CollectionsItemRef<T>>, ICollectionsObj<T> where T : class, IWorldElement
    {
        public readonly SyncRef<T> Value;

        public T Get()
        {
            return Value.Target;
        }

        public void Set(T obj)
        {
            Value.Target = obj;
        }
        public override bool Equals(object other)
        {
            if (other is CollectionsItemRef<T>)
            {
                return this.Equals((CollectionsItemRef<T>)other);
            }
            return false;
        }

        public bool SameValue(T other)
        {
            return EqualityComparer<RefID>.Default.Equals(other.ReferenceID, this.Value.Value);
        }

        public bool Equals(CollectionsItemRef<T> other)
        {
            return EqualityComparer<RefID>.Default.Equals(other.Value.Value, this.Value.Value);
        }

    }
}
