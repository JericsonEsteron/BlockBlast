using UnityEngine;
using UnityEngine.Pool;

public interface IPoolable<T> where T : MonoBehaviour
{
    IObjectPool<T> Pool { get; set; }
    void ReturnToPool(float delay = 0f);
}
