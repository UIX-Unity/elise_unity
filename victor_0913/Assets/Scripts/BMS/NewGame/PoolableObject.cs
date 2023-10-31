using UnityEngine;

//Every class inherit to this class is applied to Object Pooling and have parent object
public class PoolableObject : MonoBehaviour
{
    public ObjectPool Parent;

    public virtual void OnDisable()
    {
        Parent.ReturnObjectToPool(this.gameObject);
    }
}