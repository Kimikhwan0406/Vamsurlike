using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    static T instance;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if(null == instance)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static T Instance
    {
        get
        {
            return instance;
        }
    }
}
