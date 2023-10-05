using UnityEngine;

public interface IBaseSingleton
{
    void OnCreateInstance();
    void OnDestroyInstance();
}

public abstract class BaseSingleton<T> where T : class, IBaseSingleton, new()
{
    private static T instance;
    public static T Instance => instance;

    public static T CreateInstance()
    {
        if (Instance == null)
        {
            instance = new T();
            instance.OnCreateInstance();
        }
        return Instance;
    }

    public static void DestroyInstance()
    {
        Instance.OnDestroyInstance();
        instance = null;
    }
}


public abstract class BaseMonoSingleton<T> : MonoBehaviour where T : class
{
    public static T Instance { get; private set; }

    public static T1 GetInstance<T1>() where T1 : class, T
    {
        return Instance as T1;
    }

    [SerializeField]
    private bool isDontDestroyLoadScene = true;
    public bool IsDontDestroyLoadScene => isDontDestroyLoadScene;

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this as T;
            if (IsDontDestroyLoadScene) DontDestroyOnLoad(gameObject);
        }
    }
}