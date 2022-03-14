using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scifi
{
    public class Pooler
    {
        public GameObject PooledObject { get; private set; } = null;
        private List<GameObject> _list;

        public Pooler(GameObject pooledObject, int pooledAmount = 1)
        {

            this.PooledObject = pooledObject;
            InitPooler(pooledAmount);
            
        }

        public Pooler(Transform pooledObject, int pooledAmount = 1)
        {
            this.PooledObject = pooledObject.gameObject;
            InitPooler(pooledAmount);
        }

        private void InitPooler(int pooledAmount)
        {
            GameObject obj;

            _list = new List<GameObject>();
            for (int i = 0; i < pooledAmount; i++)
            {
                obj = (GameObject)Object.Instantiate(this.PooledObject);
                if (obj.activeSelf)
                    obj.SetActive(false);
                _list.Add(obj);
            }
        }

        public GameObject GetPooledObject()
        {
            for (int i = 0; i < _list.Count; i++)
                if ((_list[i] != null) && (!_list[i].activeSelf))
                    return _list[i];

            //we are here coz all objects are active
            //so we need to extend list
            GameObject obj;
            obj = (GameObject)Object.Instantiate(PooledObject);
            if(obj.activeSelf)
                obj.SetActive(false);
            _list.Add(obj);
            return obj;
        }

        public void Clear()
        {
            for (int i = 0; i < _list.Count; i++)
                if (_list[i] != null)
                GameObject.Destroy(_list[i]);

            _list.Clear();
        }

        public void OnDestroy(float delay = 0f)
        {
            for (int i = 0; i < _list.Count; i++)
#if UNITY_EDITOR
                //this check just to avoid errors when Play mode is stopped in Unity
                //Unity destroys objects, including pooled things, so check is needed
                if (_list[i] != null)
#endif
                    GameObject.Destroy(_list[i], delay);
        }
    }
}