using UnityEngine;
using System.Collections.Generic;

public sealed class PoolManager : Component {
    static PoolManager instance = null;
    #region Variables
    Dictionary<GameObject, List<GameObject>> pooledObjects = new Dictionary<GameObject, List<GameObject>>();
    Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();
    Transform poolParent;
    #endregion

    public static PoolManager Instance {
        get {
            if(instance == null) {
                instance = new GameObject("PoolManager").AddComponent<PoolManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    #region CreatePool
    public static void CreatePool<T>(T prefab, int initialPoolSize) where T : Component {
        CreatePool(prefab.gameObject, initialPoolSize);
    }
    public static void CreatePool(GameObject prefab, int initialPoolSize) {
        if (prefab == null) return;
        List<GameObject> list = null;// = new List<GameObject>();

        if (Instance.pooledObjects.ContainsKey(prefab)) {
            Instance.pooledObjects.TryGetValue(prefab, out list);
        }

        if (list == null) {
            list = new List<GameObject>();
            Instance.pooledObjects.Add(prefab, list);
        }

        if (initialPoolSize <= list.Count) return;

        //bool active = prefab.activeSelf;
        //prefab.SetActive(false);
        while (list.Count < initialPoolSize) {
            var obj = GameObject.Instantiate(prefab, Instance.poolParent);
            obj.SetActive(false);
            list.Add(obj);
        }
        //prefab.SetActive(active);
    }
    #endregion

    #region Spawn
    public static T Spawn<T>(T prefab, Transform parent, Vector3 position, Quaternion rotation, bool createPoolIfNeed = false) where T : Component {
        return Spawn(prefab.gameObject, parent, position, rotation, createPoolIfNeed).GetComponent<T>();
    }
    public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, bool createPoolIfNeed = false) where T : Component {
        return Spawn(prefab.gameObject, null, position, rotation, createPoolIfNeed).GetComponent<T>();
    }
    public static T Spawn<T>(T prefab, Transform parent, Vector3 position, bool createPoolIfNeed = false) where T : Component {
        return Spawn(prefab.gameObject, parent, position, Quaternion.identity, createPoolIfNeed).GetComponent<T>();
    }
    public static T Spawn<T>(T prefab, Vector3 position, bool createPoolIfNeed = false) where T : Component {
        return Spawn(prefab.gameObject, null, position, Quaternion.identity, createPoolIfNeed).GetComponent<T>();
    }
    public static T Spawn<T>(T prefab, Transform parent, bool createPoolIfNeed = false) where T : Component {
        return Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity, createPoolIfNeed).GetComponent<T>();
    }
    public static T Spawn<T>(T prefab, bool createPoolIfNeed = false) where T : Component {
        return Spawn(prefab.gameObject, null, Vector3.zero, Quaternion.identity, createPoolIfNeed).GetComponent<T>();
    }
    public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, bool createPoolIfNeed = false) {
        return Spawn(prefab, parent, position, Quaternion.identity, createPoolIfNeed);
    }
    public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, bool createPoolIfNeed = false) {
        return Spawn(prefab, null, position, rotation, createPoolIfNeed);
    }
    public static GameObject Spawn(GameObject prefab, Transform parent, bool createPoolIfNeed = false) {
        return Spawn(prefab, parent, Vector3.zero, Quaternion.identity, createPoolIfNeed);
    }
    public static GameObject Spawn(GameObject prefab, Vector3 position, bool createPoolIfNeed = false) {
        return Spawn(prefab, null, position, Quaternion.identity, createPoolIfNeed);
    }
    public static GameObject Spawn(GameObject prefab, bool createPoolIfNeed = false) {
        return Spawn(prefab, null, Vector3.zero, Quaternion.identity, createPoolIfNeed);
    }
    public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, bool createPoolIfNeed) {
        List<GameObject> list;
        GameObject obj = null;
        if (Instance.pooledObjects.TryGetValue(prefab, out list)) {
            if (list.Count > 0) {
                while (obj == null && list.Count > 0) {
                    obj = list[0];
                    list.RemoveAt(0);
                }
                if (obj != null) {
                    return Instance.GetObject(obj, prefab, parent, position, rotation);
                }
            }
            return Instance.GetObject(obj, prefab, parent, position, rotation);
        }
        else return Instance.GetObject(obj, prefab, parent, position, rotation, createPoolIfNeed, createPoolIfNeed);
    }

    private GameObject GetObject(GameObject obj, GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, bool addToSpawns = true, bool createPool = false) {
        if (createPool) CreatePool(prefab, 0);
        if (obj == null) obj = GameObject.Instantiate(prefab, position, rotation, parent);
        obj.transform.SetParent(parent);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        if (addToSpawns) Instance.spawnedObjects.Add(obj, prefab);
        return obj;
    }
    #endregion

    #region Recycle
    public static void Recycle<T>(T obj) where T : Component {
        Recycle(obj.gameObject);
    }
    public static void Recycle(GameObject obj) {
        GameObject prefab;
        if (Instance.spawnedObjects.TryGetValue(obj, out prefab))
            Recycle(obj, prefab);
        else
            GameObject.Destroy(obj);
    }
    static void Recycle(GameObject obj, GameObject prefab) {
        Instance.pooledObjects[prefab].Add(obj);
        Instance.spawnedObjects.Remove(obj);
        obj.transform.SetParent(Instance.poolParent);
        obj.SetActive(false);
    }

    public static void RecycleAll<T>(T prefab) where T : Component {
        RecycleAll(prefab.gameObject);
    }
    public static void RecycleAll(GameObject prefab) {
        List<GameObject> tempList = new List<GameObject>();
        foreach (var item in Instance.spawnedObjects)
            if (item.Value == prefab)
                tempList.Add(item.Key);
        for (int i = 0; i < tempList.Count; ++i)
            Recycle(tempList[i]);
    }
    public static void RecycleAll() {
        List<GameObject> tempList = new List<GameObject>();
        tempList.AddRange(Instance.spawnedObjects.Keys);
        for (int i = 0; i < tempList.Count; ++i)
            Recycle(tempList[i]);
    }
    #endregion

    #region Get Pooled Value
    public static bool IsSpawned(GameObject obj) {
        return Instance.spawnedObjects.ContainsKey(obj);
    }
	public static int CountPooled<T>(T prefab) where T : Component {
        return CountPooled(prefab.gameObject);
    }
    public static int CountPooled(GameObject prefab) {
        List<GameObject> list;
        if (Instance.pooledObjects.TryGetValue(prefab, out list))
            return list.Count;
        return 0;
    }

    public static int CountSpawned<T>(T prefab) where T : Component {
        return CountSpawned(prefab.gameObject);
    }
    public static int CountSpawned(GameObject prefab) {
        int count = 0;
        foreach (var instancePrefab in Instance.spawnedObjects.Values)
            if (prefab == instancePrefab)
                ++count;
        return count;
    }

    public static int CountAllPooled() {
        int count = 0;
        foreach (var list in Instance.pooledObjects.Values)
            count += list.Count;
        return count;
    }

    public static List<GameObject> GetPooled(GameObject prefab, List<GameObject> list, bool appendList) {
        if (list == null)
            list = new List<GameObject>();
        if (!appendList)
            list.Clear();
        List<GameObject> pooled;
        if (Instance.pooledObjects.TryGetValue(prefab, out pooled))
            list.AddRange(pooled);
        return list;
    }
    public static List<T> GetPooled<T>(T prefab, List<T> list, bool appendList) where T : Component {
        if (list == null)
            list = new List<T>();
        if (!appendList)
            list.Clear();
        List<GameObject> pooled;
        if (Instance.pooledObjects.TryGetValue(prefab.gameObject, out pooled))
            for (int i = 0; i < pooled.Count; ++i)
                list.Add(pooled[i].GetComponent<T>());
        return list;
    }

    public static List<GameObject> GetSpawned(GameObject prefab, List<GameObject> list, bool appendList) {
        if (list == null)
            list = new List<GameObject>();
        if (!appendList)
            list.Clear();
        foreach (var item in Instance.spawnedObjects)
            if (item.Value == prefab)
                list.Add(item.Key);
        return list;
    }
    public static List<T> GetSpawned<T>(T prefab, List<T> list, bool appendList) where T : Component {
        if (list == null)
            list = new List<T>();
        if (!appendList)
            list.Clear();
        var prefabObj = prefab.gameObject;
        foreach (var item in Instance.spawnedObjects)
            if (item.Value == prefabObj)
                list.Add(item.Key.GetComponent<T>());
        return list;
    }
    #endregion

    #region Destroy Pooled Object
    public static void DestroyPooled(GameObject prefab) {
        List<GameObject> pooled;
        if (Instance.pooledObjects.TryGetValue(prefab, out pooled)) {
            for (int i = 0; i < pooled.Count; ++i)
                GameObject.Destroy(pooled[i]);
            pooled.Clear();
        }
    }
    public static void DestroyPooled<T>(T prefab) where T : Component {
        DestroyPooled(prefab.gameObject);
    }

    public static void DestroyAll(GameObject prefab) {
        RecycleAll(prefab);
        DestroyPooled(prefab);
    }
    public static void DestroyAll<T>(T prefab) where T : Component {
        DestroyAll(prefab.gameObject);
    }
    public static void DestroyAll() {
        try {
            RecycleAll();
            List<GameObject> tempList = new List<GameObject>();
            tempList.AddRange(Instance.pooledObjects.Keys);
            for (int i = 0; i < tempList.Count; ++i)
                DestroyPooled(tempList[i]);
        }catch { }
    }
    #endregion
}
