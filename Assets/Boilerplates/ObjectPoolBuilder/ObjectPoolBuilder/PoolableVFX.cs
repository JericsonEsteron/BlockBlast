using UnityEngine;
using UnityEngine.Pool;

public class PoolableVFX : MonoBehaviour, IPoolable
{
    [SerializeField] float _returnToPoolDelay = 1f;
    public IObjectPool<GameObject> Pool { get; set; }


    void OnEnable()
    {
        ReturnToPool(_returnToPoolDelay);
    }

    public void ReturnToPool(float delay = 0f)
    {
        Invoke(nameof(Release), delay);
    }

    private void Release()
    {
        Pool.Release(gameObject);
    }
}
