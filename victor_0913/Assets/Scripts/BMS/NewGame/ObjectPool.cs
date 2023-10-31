using System.Collections.Generic;
using UnityEngine;
//The ObjectPooling pattern that i use 
public class ObjectPool
{
    private GameObject Prefab;
    private int Size;
    private List<GameObject> AvailableObjectsPool;
    private GameObject Holder;

    public ObjectPool(GameObject Prefab, int Size, GameObject Holder)
    {
        this.Prefab = Prefab;
        this.Size = Size;
        this.Holder = Holder;
        AvailableObjectsPool = new List<GameObject>(Size);
    }

    public static ObjectPool CreateInstance(GameObject Prefab, int Size)
    {
        GameObject Holder = new GameObject(Prefab + " Pool");
        ObjectPool pool = new ObjectPool(Prefab, Size, Holder);
        pool.CreateObjects(Holder);

        return pool;
    }

    private void CreateObjects(GameObject parent)
    {
        for (int i = 0; i < Size; i++)
        {
            GameObject poolableObject = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity, parent.transform);
            poolableObject.gameObject.SetActive(false); // PoolableObject handles re-adding the object to the AvailableObjects
            AvailableObjectsPool.Add(poolableObject);
        }
    }

    public T GetPooledObject<T>() where T : PoolableObject
    {
        foreach (GameObject child in AvailableObjectsPool)
        {
            if (child.TryGetComponent<T>(out T component))
            {
                return component;
            }
            else
            {
                if (child.TryGetComponent<PoolableObject>(out PoolableObject obj))
                {
                    continue;
                }
                else
                {
                    T type = child.AddComponent<T>();
                    type.Parent = this;
                    return type;
                }

            }
        }
        return null;
    }

    public void RemoveIndex(int index)
    {
        AvailableObjectsPool.RemoveAt(index);
    }

    public void RemovePoolableObject(GameObject poolableObject)
    {
        AvailableObjectsPool.Remove(poolableObject);
    }

    public void ReturnObjectToPool(GameObject Object)
    {
        AvailableObjectsPool.Add(Object);
    }

    public List<GameObject> GetAvailableObjectsPool()
    {
        return AvailableObjectsPool;
    }

    public GameObject GetHolder()
    {
        return Holder;
    }
}