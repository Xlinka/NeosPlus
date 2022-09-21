using System;
using System.Collections.Generic;

namespace FrooxEngine.LogiX.Collections;

public static class CollectionHelper<T, TU> where T : ISyncList, IEnumerable<TU>
{
    public static Action<T, TU> Append { get; }
    public static Func<T, int, TU> Get { get; }

    static CollectionHelper()
    {
        var baseType = typeof(T).GetGenericTypeDefinition();
        Type helperType = null;
        if (baseType == typeof(SyncRefList<>))
        {
            helperType = typeof(CollectionsHelperSyncRefList<>).MakeGenericType(typeof(T).GenericTypeArguments[0]);
        }
        var append = (Action<T, TU>) Delegate.CreateDelegate(typeof(Action<T, TU>), helperType.GetMethod("Append"));
        Append = append;
        var get = (Func<T, int, TU>) Delegate.CreateDelegate(typeof(Func<T, int, TU>), helperType.GetMethod("Get"));
        Get = get;
    }
}