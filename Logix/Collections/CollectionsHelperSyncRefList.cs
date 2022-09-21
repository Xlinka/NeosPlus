namespace FrooxEngine.LogiX.Collections;

public static class CollectionsHelperSyncRefList<T> where T : class, IWorldElement
{
    public static void Append(SyncRefList<T> collection, T obj) => collection.Add(obj);
    public static T Get(SyncRefList<T> collection, int index) => collection[index];
}