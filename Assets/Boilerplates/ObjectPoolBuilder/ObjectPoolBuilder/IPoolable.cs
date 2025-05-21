using UnityEngine;
using UnityEngine.Pool;

public interface IPoolable
{
    IObjectPool<GameObject> Pool { get; set; }
    void ReturnToPool(float delay = 0f);
}
