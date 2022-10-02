namespace FrooxEngine.LogiX.Collections;

public static class CollectionsHelperSyncRefList<T> where T : class, IWorldElement
{
    public static void Append(SyncRefList<T> collection, T obj) => collection.Add(obj);
    public static void Insert(SyncRefList<T> collection, int index, T obj) => collection.Insert(index, obj);
    public static void Remove(SyncRefList<T> collection, int index) => collection.RemoveAt(index);
    public static void Set(SyncRefList<T> collection, int index, T obj) => collection.GetElement(index).Target = obj;
}

public static class CollectionsHelperSyncFieldList<T>
{
    public static void Append(SyncFieldList<T> collection, T obj) => collection.Add(obj);
    public static void Insert(SyncFieldList<T> collection, int index, T obj) => collection.Insert(index, obj);
    public static void Remove(SyncFieldList<T> collection, int index) => collection.RemoveAt(index);
    public static void Set(SyncFieldList<T> collection, int index, T obj) => collection.GetElement(index).Value = obj;
}

public static class CollectionsHelperSyncAssetList<T> where T : class, IAsset
{
    public static void Append(SyncAssetList<T> collection, IAssetProvider<T> obj) => collection.Add(obj);

    public static void Insert(SyncAssetList<T> collection, int index, IAssetProvider<T> obj) =>
        collection.Insert(index, obj);

    public static void Remove(SyncAssetList<T> collection, int index) => collection.RemoveAt(index);

    public static void Set(SyncAssetList<T> collection, int index, IAssetProvider<T> obj) =>
        collection.GetElement(index).Target = obj;
}

public static class CollectionsHelperSyncDelegateList<T> where T : class
{
    public static void Append(SyncDelegateList<T> collection, T obj) => collection.Add(obj);
    public static void Insert(SyncDelegateList<T> collection, int index, T obj) => collection.Insert(index, obj);
    public static void Remove(SyncDelegateList<T> collection, int index) => collection.RemoveAt(index);

    public static void Set(SyncDelegateList<T> collection, int index, T obj) =>
        collection.GetElement(index).Target = obj;
}

public static class CollectionsHelperSyncRelayList<T> where T : class, IChangeable
{
    public static void Append(SyncRelayList<T> collection, T obj) => collection.Add(obj);
    public static void Insert(SyncRelayList<T> collection, int index, T obj) => collection.Insert(index, obj);
    public static void Remove(SyncRelayList<T> collection, int index) => collection.RemoveAt(index);
    public static void Set(SyncRelayList<T> collection, int index, T obj) => collection.GetElement(index).Target = obj;
}