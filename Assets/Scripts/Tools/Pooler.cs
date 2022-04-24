using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scifi.Tools
{
    /// <summary>
    /// This pool uses deactivated objects for pooling. To release pooled object just deactivate it.
    /// Important! Do not re-use deactivated objects by itself!
    /// </summary>
    /// <typeparam name="T">Any type of Component</typeparam>
    public class Pooler<T> where T : Component
    {
        public T PooledObject { get; private set; } = null;
        private List<T> _cachedItems;

        public Pooler(T pooledObject, int pooledAmount = 1)
        {

            this.PooledObject = pooledObject;
            InitPooler(pooledAmount);
        }

        private void InitPooler(int pooledAmount)
        {
            _cachedItems = new List<T>();
            for (int i = 0; i < pooledAmount; i++)
            {
                AddCachedNew();
            }
        }

        public T GetPooledObject()
        {
            for (int i = 0; i < _cachedItems.Count; i++)
                if (!_cachedItems[i].gameObject.activeSelf)
                    return _cachedItems[i];

            //we are here coz all objects are active
            //so we need to extend list
            return AddCachedNew();
        }

        private T AddCachedNew()
        {
            T obj;
            obj = GameObject.Instantiate(PooledObject);
            if (obj.gameObject.activeSelf)
                obj.gameObject.SetActive(false);
            _cachedItems.Add(obj);
            return obj;
        }

        public void Clear()
        {
            for (int i = 0; i < _cachedItems.Count; i++)
                if (_cachedItems[i] != null)
                GameObject.Destroy(_cachedItems[i].gameObject);

            _cachedItems.Clear();
        }
    }
}