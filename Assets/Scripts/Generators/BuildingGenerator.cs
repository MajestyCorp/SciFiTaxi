using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scifi.Tools;

namespace Scifi.Generators
{
    public class BuildingGenerator : MonoBehaviour, IInitializer
    {
        public static BuildingGenerator Instance { get; private set; } = null;
        private const string c_joint = "JOINT";

        [Header("Settings"), SerializeField, Min(1)]
        private int minFloors = 1;

        [SerializeField, Min(1)]
        private int maxFloors = 3;

        [SerializeField]
        private List<Transform> bottomItems;

        [SerializeField]
        private List<Transform> middleItems;

        [SerializeField]
        private List<Transform> topItems;

        //own stuff
        //we make pool of each building item
        //list of all items (bottom + middle + top) for fast pooling
        private List<Transform> _indexItems = null;
        private List<Pooler<Transform>> _poolerItems = null;

        private System.Random _random = new System.Random();

        #region initialize stuff
        public void InitInstance()
        {
            Instance = this;
            InitPooling();
        }

        public void Initialize()
        {
        }

        private void InitPooling()
        {
            //we will use indexItems for fast search of needed index in poolerItems
            _indexItems = new List<Transform>();
            for (int i = 0; i < bottomItems.Count; i++)
                _indexItems.Add(bottomItems[i]);
            for (int i = 0; i < middleItems.Count; i++)
                _indexItems.Add(middleItems[i]);
            for (int i = 0; i < topItems.Count; i++)
                _indexItems.Add(topItems[i]);

            //now init all pooling objects
            _poolerItems = new List<Pooler<Transform>>();
            for (int i = 0; i < bottomItems.Count; i++)
                _poolerItems.Add(new Pooler<Transform>(bottomItems[i]));
            for (int i = 0; i < middleItems.Count; i++)
                _poolerItems.Add(new Pooler<Transform>(middleItems[i]));
            for (int i = 0; i < topItems.Count; i++)
                _poolerItems.Add(new Pooler<Transform>(topItems[i]));
        }
        #endregion

        public void SetSeed(int seed)
        {
            _random = new System.Random(seed);
        }

        private Transform GetPooledByItem(Transform buildingItem)
        {
            int index;
            index = _indexItems.IndexOf(buildingItem);

            if(index<0)
            {
                _indexItems.Add(buildingItem);
                _poolerItems.Add(new Pooler<Transform>(buildingItem));
                index = _indexItems.Count - 1;
            }

            return _poolerItems[index].GetPooledObject();
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
            int floors = _random.Next(minFloors, maxFloors + 1);
            List<Transform> items;
            Transform upperFloor = null;

            //start generation from the roof
            //now decide if we have all 3 types of items, or less
            if(floors>=3)
            {
                //we have at least 3 floors, so we can use all 3 types
                for(int i=1;i<=floors;i++)
                {
                    items = i == 1 ? topItems : i == floors ? bottomItems : middleItems;

                    upperFloor = CreateFloor(upperFloor, items);
                }
            } else
            {
                //we have 1 or 2 floors
                for (int i = 1; i <= floors; i++)
                {
                    items = i == 1 ? topItems : middleItems;

                    upperFloor = CreateFloor(upperFloor, items);
                }
            }

            return upperFloor;
        }

        private Transform CreateFloor(Transform upperFloor, List<Transform> listItems)
        {
            //select random prefab from list and get pooled item
            Transform floor = GetPooledByItem(listItems[_random.Next(0, listItems.Count)]);
            Transform child;

            //make random rotation 0/90/180/270
            floor.rotation = Quaternion.Euler(0f, _random.Next(0, 4) * 90f, 0f);
            floor.gameObject.SetActive(true);

            //if we have upper floor - attach upper floor to this one
            if(upperFloor !=null)
            {
                for (int i = 0; i < floor.childCount; i++)
                {
                    child = floor.GetChild(i);
                    if (child.name.CompareTo(c_joint) == 0)
                    {
                        //we found JOINT object, set same position for upperFloor
                        upperFloor.parent = floor;
                        upperFloor.localPosition = child.localPosition;
                        break;
                    }
                }
            }

            return floor;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;

            DestroyPool();
        }

        private void DestroyPool()
        {
            if (_poolerItems != null)
                for (int i = 0; i < _poolerItems.Count; i++)
                    _poolerItems[i].Clear();
        }
    }
}