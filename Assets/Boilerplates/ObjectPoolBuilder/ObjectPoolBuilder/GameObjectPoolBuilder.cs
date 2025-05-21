using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameObjectPoolBuilder
{
    private IObjectPool<GameObject> _objectPool;
    private GameObject _prefab;
    private int _defaultSize = 10;
    private int _maxSize = 100;

    public GameObjectPoolBuilder(GameObject prefab)
    {
        CheckIfPoolable(prefab);
        _prefab = prefab;
    }

    private void CheckIfPoolable(GameObject gameObject)// Gameobjects with IPoolable are the only ones that can be added to the pool
    {
        if (gameObject.GetComponent<IPoolable>() == null)
        {
            throw new System.NullReferenceException($"Please implement {nameof(IPoolable)} to enable pooling for {gameObject.name}.");
        }
    }

    private GameObject CreatePoolObject()
    {
        var objectInstance = Object.Instantiate(_prefab); 
        var poolable = objectInstance.GetComponent<IPoolable>();
        poolable.Pool = _objectPool;

        objectInstance.gameObject.SetActive(false);

        return objectInstance;
    }

#region Builder Methods

    public GameObjectPoolBuilder WithInitialSize(int initialSize)
    {
        _defaultSize = initialSize;
        return this;
    }

    public GameObjectPoolBuilder WithMaxSize(int maxSize)
    {
        _maxSize = maxSize;
        return this;
    }

    public IObjectPool<GameObject> Build()
    {
        _objectPool = new ObjectPool<GameObject>(
            () => CreatePoolObject(), 
            pooledObject => pooledObject.SetActive(true), 
            pooledObject => pooledObject.SetActive(false), 
            pooledObject => Object.Destroy(pooledObject.gameObject), 
            true, _defaultSize, _maxSize);

        return _objectPool;
    }
    
#endregion
}
