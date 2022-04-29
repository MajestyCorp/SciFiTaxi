using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scifi.Tools;

namespace Scifi.Generators
{
    public class BuildingGenerator : MonoBehaviour, IInitializer
    {
        public static BuildingGenerator Instance { get; private set; } = null;

        //every building module has "JOINT" game object - its a point to attach upper floors
        private const string c_joint = "JOINT";

        [Header("Settings"), SerializeField]
        private List<FloorTypes> floorTypes;

        private System.Random _random = new System.Random();

        #region initialize stuff
        void IInitializer.InitInstance()
        {
            Instance = this;
            InitPooling();
        }

        void IInitializer.Initialize()
        { }

        private void InitPooling()
        {
            for (int i = 0; i < floorTypes.Count; i++)
                floorTypes[i].Init();
        }
        #endregion

        public void SetSeed(int seed)
        {
            _random = new System.Random(seed);

            for (int i = 0; i < floorTypes.Count; i++)
                floorTypes[i].SetRandom(_random);
        }

        public void DisableBuilding(Transform item)
        {
            //recursivelly detach and disable all childs except JOINT objects
            Transform child;

            //make loop from end to start, because we are removing childs one by one
            for (int i = item.childCount - 1; i >= 0 ; i--)
            {
                child = item.GetChild(i);
                if(child.name.CompareTo(c_joint) != 0)
                    DisableBuilding(child);
            }

            item.parent = null;
            item.gameObject.SetActive(false);
        }

        /// <summary>
        /// Generates new random building
        /// </summary>
        /// <returns>Building transform</returns>
        public Transform Generate()
        {
            Transform lastFloor = null;

            for (int i = 0; i < floorTypes.Count; i++)
                lastFloor = floorTypes[i].AddFloors(lastFloor);

            return lastFloor;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;

            DestroyPool();
        }

        private void DestroyPool()
        {
            for (int i = 0; i < floorTypes.Count; i++)
                floorTypes[i].Clear();
        }

        [System.Serializable]
        private class FloorTypes
        {
            [SerializeField]
            private string typeName;
            [SerializeField, Min(0), Tooltip("Minimum number of floors of this type")]
            private int minFloors = 0;
            [SerializeField, Min(1), Tooltip("Maximum number of floors of this type")]
            private int maxFloors;

            [SerializeField]
            private List<Transform> floorPrefabs;

            //we make pool of each building item
            //list of all items (bottom + middle + top) for fast pooling
            private List<Pooler<Transform>> _poolerItems = null;

            private System.Random _random;

            public void Init()
            {
                _poolerItems = new List<Pooler<Transform>>();

                for (int i = 0; i < floorPrefabs.Count; i++)
                    _poolerItems.Add(new Pooler<Transform>(floorPrefabs[i], 1));
            }

            public void SetRandom(System.Random random)
            {
                _random = random;
            }

            public Transform AddFloors(Transform upperFloor)
            {
                Transform lastFloor = upperFloor;
                int floors = _random.Next(minFloors, maxFloors + 1);

                for (int i = 0; i < floors; i++)
                    lastFloor = AddFloorAtBottom(lastFloor);

                return lastFloor;
            }

            public void Clear()
            {
                for (int i = 0; i < _poolerItems.Count; i++)
                    _poolerItems[i].Clear();
            }

            private Transform AddFloorAtBottom(Transform upperFloor)
            {
                Transform result;
                Transform floor;
                Transform joint;

                result = upperFloor;

                //select random prefab from list and get pooled item
                floor = GetPooledByItem(floorPrefabs[_random.Next(0, floorPrefabs.Count)]);

                //make random rotation 0/90/180/270
                floor.rotation = Quaternion.Euler(0f, _random.Next(0, 4) * 90f, 0f);
                floor.gameObject.SetActive(true);

                //if we have upper floor - attach upper floor to this one
                if (upperFloor != null && HasJoint(floor, out joint))
                {
                    upperFloor.parent = floor;
                    upperFloor.localPosition = joint.localPosition;
                }

                return floor;
            }

            private bool HasJoint(Transform floor, out Transform joint)
            {
                Transform child;
                joint = null;

                for (int i = 0; i < floor.childCount; i++)
                {
                    child = floor.GetChild(i);
                    if (child.name.CompareTo(c_joint) == 0)
                    {
                        joint = child;
                        return true;
                    }
                }

                return false;
            }

            private Transform GetPooledByItem(Transform floorPrefab)
            {
                int index;
                index = floorPrefabs.IndexOf(floorPrefab);
                return _poolerItems[index].GetPooledObject();
            }
        }
    }
}