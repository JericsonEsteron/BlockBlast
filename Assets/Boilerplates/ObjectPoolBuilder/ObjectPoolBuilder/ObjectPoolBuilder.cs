using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolBuilder<T> where T : MonoBehaviour
{
    private IObjectPool<T> _objectPool;
    private T _prefab;
    private int _defaultSize = 10;
    private int _maxSize = 100;

    public ObjectPoolBuilder(T prefab)
    {
        CheckIfPoolable(prefab);
        _prefab = prefab;
    }

    private void CheckIfPoolable(T prefab)// Gameobjects with IPoolable are the only ones that can be added to the pool
    {
        Debug.Log(prefab);
        if (prefab.gameObject.TryGetComponent(out IPoolable<T> poolable))
        {
            return;
        }
        else
            throw new System.NullReferenceException($"Please implement {nameof(IPoolable<T>)} to enable pooling for {prefab.name}.");
    }

    private T CreatePoolObject()
    {
        var objectInstance = Object.Instantiate(_prefab); 
        var poolable = objectInstance.GetComponent<IPoolable<T>>();
        poolable.Pool = _objectPool;

        objectInstance.gameObject.SetActive(false);

        return objectInstance;
    }

#region Builder Methods

    public ObjectPoolBuilder<T> WithInitialSize(int initialSize)
    {
        _defaultSize = initialSize;
        return this;
    }

    public ObjectPoolBuilder<T> WithMaxSize(int maxSize)
    {
        _maxSize = maxSize;
        return this;
    }

    public IObjectPool<T> Build()
    {
        _objectPool = new ObjectPool<T>(
            () => CreatePoolObject(), 
            pooledObject => pooledObject.gameObject.SetActive(true), 
            pooledObject => pooledObject.gameObject.SetActive(false), 
            pooledObject => Object.Destroy(pooledObject.gameObject), 
            true, _defaultSize, _maxSize);

        return _objectPool;
    }
    
#endregion
}
