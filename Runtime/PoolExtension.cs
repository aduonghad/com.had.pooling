using System.Collections.Generic;
using UnityEngine;

public static class PoolExtensions {
    #region CreatePool
    public static void CreatePool<T>(this T prefab) where T : Component {
        PoolManager.CreatePool(prefab, 0);
    }
    public static void CreatePool<T>(this T prefab, int initialPoolSize) where T : Component {
        PoolManager.CreatePool(prefab, initialPoolSize);
    }
    public static void CreatePool(this GameObject prefab) {
        PoolManager.CreatePool(prefab, 0);
    }
    public static void CreatePool(this GameObject prefab, int initialPoolSize) {
        PoolManager.CreatePool(prefab, initialPoolSize);
    }
    #endregion

    #region Spawn
    public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Quaternion rotation, bool createPool = false) where T : Component {
        return PoolManager.Spawn(prefab, parent, position, rotation);
    }
    public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation, bool createPool = false) where T : Component {
        return PoolManager.Spawn(prefab, null, position, rotation);
    }
    public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, bool createPool = false) where T : Component {
        return PoolManager.Spawn(prefab, parent, position, Quaternion.identity);
    }
    public static T Spawn<T>(this T prefab, Vector3 position, bool createPool = false) where T : Component {
        return PoolManager.Spawn(prefab, null, position, Quaternion.identity);
    }
    public static T Spawn<T>(this T prefab, Transform parent, bool createPool = false) where T : Component {
        return PoolManager.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
    }
    public static T Spawn<T>(this T prefab, bool createPool = false) where T : Component {
        return PoolManager.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
    }
    public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, bool createPool = false) {
        return PoolManager.Spawn(prefab, parent, position, rotation, createPool);
    }
    public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation, bool createPool = false) {
        return PoolManager.Spawn(prefab, null, position, rotation, createPool);
    }
    public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, bool createPool = false) {
        return PoolManager.Spawn(prefab, parent, position, Quaternion.identity, createPool);
    }
    public static GameObject Spawn(this GameObject prefab, Vector3 position, bool createPool = false) {
        return PoolManager.Spawn(prefab, null, position, Quaternion.identity, createPool);
    }
    public static GameObject Spawn(this GameObject prefab, Transform parent, bool createPool = false) {
        return PoolManager.Spawn(prefab, parent, Vector3.zero, Quaternion.identity, createPool);
    }
    public static GameObject Spawn(this GameObject prefab, bool createPool = false) {
        return PoolManager.Spawn(prefab, null, Vector3.zero, Quaternion.identity, createPool);
    }
    #endregion

    #region Recycle
    public static void Recycle<T>(this T obj) where T : Component {
        PoolManager.Recycle(obj);
    }
    public static void Recycle(this GameObject obj) {
        PoolManager.Recycle(obj);
    }

    public static void RecycleAll<T>(this T prefab) where T : Component {
        PoolManager.RecycleAll(prefab);
    }
    public static void RecycleAll(this GameObject prefab) {
        PoolManager.RecycleAll(prefab);
    }
	#endregion

	#region Get Pooled Value
	public static bool IsCreatePool(this GameObject prefab) {
		return PoolManager.IsSpawned(prefab.gameObject);//.CountPooled(prefab);
	}
	public static int CountPooled<T>(this T prefab) where T : Component {
        return PoolManager.CountPooled(prefab);
    }
    public static int CountPooled(this GameObject prefab) {
        return PoolManager.CountPooled(prefab);
    }

    public static int CountSpawned<T>(this T prefab) where T : Component {
        return PoolManager.CountSpawned(prefab);
    }
    public static int CountSpawned(this GameObject prefab) {
        return PoolManager.CountSpawned(prefab);
    }

    public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list, bool appendList) {
        return PoolManager.GetSpawned(prefab, list, appendList);
    }
    public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list) {
        return PoolManager.GetSpawned(prefab, list, false);
    }
    public static List<GameObject> GetSpawned(this GameObject prefab) {
        return PoolManager.GetSpawned(prefab, null, false);
    }
    public static List<T> GetSpawned<T>(this T prefab, List<T> list, bool appendList) where T : Component {
        return PoolManager.GetSpawned(prefab, list, appendList);
    }
    public static List<T> GetSpawned<T>(this T prefab, List<T> list) where T : Component {
        return PoolManager.GetSpawned(prefab, list, false);
    }
    public static List<T> GetSpawned<T>(this T prefab) where T : Component {
        return PoolManager.GetSpawned(prefab, null, false);
    }

    public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list, bool appendList) {
        return PoolManager.GetPooled(prefab, list, appendList);
    }
    public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list) {
        return PoolManager.GetPooled(prefab, list, false);
    }
    public static List<GameObject> GetPooled(this GameObject prefab) {
        return PoolManager.GetPooled(prefab, null, false);
    }
    public static List<T> GetPooled<T>(this T prefab, List<T> list, bool appendList) where T : Component {
        return PoolManager.GetPooled(prefab, list, appendList);
    }
    public static List<T> GetPooled<T>(this T prefab, List<T> list) where T : Component {
        return PoolManager.GetPooled(prefab, list, false);
    }
    public static List<T> GetPooled<T>(this T prefab) where T : Component {
        return PoolManager.GetPooled(prefab, null, false);
    }
    #endregion

    #region Destroy Pooled Object
    public static void DestroyPooled(this GameObject prefab) {
        PoolManager.DestroyPooled(prefab);
    }
    public static void DestroyPooled<T>(this T prefab) where T : Component {
        PoolManager.DestroyPooled(prefab.gameObject);
    }

    public static void DestroyAll(this GameObject prefab) {
        PoolManager.DestroyAll(prefab);
    }
    public static void DestroyAll<T>(this T prefab) where T : Component {
        PoolManager.DestroyAll(prefab.gameObject);
    }
    #endregion
}