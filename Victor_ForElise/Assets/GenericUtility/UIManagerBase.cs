using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------
//
// v1.1 ('20.05.06)
//  - TryGetManager<T> 함수 추가 
//
// ---------------------------------------------

public class UIManagerBase : MonoBehaviour
{
    private static UIManagerBase instance;

    public static T GetManager<T>() where T : UIManagerBase
    {
        if(instance == null)
        {
            instance = FindObjectOfType<T>();
        }

        T manager = instance as T;
        if(manager == null)
        {
            manager = FindObjectOfType<T>();
        }

        instance = manager;
        return manager;
    }

    public static bool TryGetManager<T>(out T manager) where T : UIManagerBase
    {
        if (instance == null)
        {
            instance = FindObjectOfType<T>();
        }

        manager = instance as T;
        if (manager == null)
        {
            manager = FindObjectOfType<T>();
        }

        instance = manager;
        return (instance != null);
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
