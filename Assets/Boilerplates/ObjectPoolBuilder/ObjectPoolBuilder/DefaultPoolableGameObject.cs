using UnityEngine;
using UnityEngine.Pool;

public class DefaultPoolableGameObject : MonoBehaviour, IPoolable
{
    public IObjectPool<GameObject> Pool { get; set; }

    public void ReturnToPool(float delay = 0f)
    {
        Invoke(nameof(Release), delay);
    }

    private void Release()
    {
        Pool.Release(gameObject);
    }
}
