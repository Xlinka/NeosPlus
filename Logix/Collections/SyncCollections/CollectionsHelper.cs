using System;
using System.Collections.Generic;

namespace FrooxEngine.LogiX.Collections;

public static class CollectionsHelperList
{
    public static readonly Dictionary<Type, Type> HelperMapping = new()
    {
        //SyncTypeList is not here because it isn't generic and breaks literally everything here
        {typeof(SyncRefList<>), typeof(CollectionsHelperSyncRefList<>)},
        {typeof(SyncFieldList<>), typeof(CollectionsHelperSyncFieldList<>)},
        {typeof(SyncAssetList<>), typeof(CollectionsHelperSyncAssetList<>)},
        {typeof(SyncDelegateList<>), typeof(CollectionsHelperSyncDelegateList<>)},
        {typeof(SyncRelayList<>), typeof(CollectionsHelperSyncRelayList<>)},
    };
}
public static class CollectionsHelper<T, TU> where T : ISyncList, IEnumerable<TU>
{
    //HACK: the types that inherit from SyncElementList<> only have IEnumerable<> shared between them all
    //the normal nodes that only need IEnumerable can use these fine, but the rest of the operations need
    //a custom helper for their operations
    //this implementation is based on Coder<T> but doesn't use cursed IL injection and instead creates the delegates
    //from reflection
    //a better implementation on the neos side would have each class inherit from IList<>
    public static Action<T, TU> Append { get; }
    public static Action<T, int, TU> Insert { get; }
    //remove actually doesn't need the helper but the way i've got it set up it's easier to have it here
    public static Action<T, int> Remove { get; }
    public static Action<T, int, TU> Set { get; }
    static CollectionsHelper()
    {
        var baseType = typeof(T).GetGenericTypeDefinition();
        if (!CollectionsHelperList.HelperMapping.TryGetValue(baseType, out var type)) return;
        var helperType = type.MakeGenericType(typeof(T).GenericTypeArguments[0]);
        Append = (Action<T, TU>) Delegate.CreateDelegate(typeof(Action<T, TU>), helperType.GetMethod("Append"));
        Insert = (Action<T, int, TU>) Delegate.CreateDelegate(typeof(Action<T, int, TU>), helperType.GetMethod("Insert"));
        Remove = (Action<T, int>) Delegate.CreateDelegate(typeof(Action<T, int>), helperType.GetMethod("Remove"));
        Set = (Action<T, int, TU>) Delegate.CreateDelegate(typeof(Action<T, int, TU>), helperType.GetMethod("Set"));
    }
}