using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scifi.Generators
{
    public class Chunk : MonoBehaviour
    {
        private const string c_spot = "SPOT";

        public Vector2Int Position { get; private set; }
        protected float NextRandomValue => ((float)_random.NextDouble() - 0.5f) * randomizeHousePosition;

        [SerializeField]
        private float randomizeHousePosition = 0.15f;
        [SerializeField, Header("Runtime data")]
        private List<Transform> spots;

        private List<Transform> _buildings;
        private float _chunkWidth;
        private int _seed;
        private System.Random _random;

        public void InitPrefab()
        {
            InitSpots();
        }

        public void InitInstance(int seed, Vector2Int position, float chunkWidth)
        {
            Position = position;
            _chunkWidth = chunkWidth;
            _seed = seed;
            _random = new System.Random(seed);

            transform.localPosition = new Vector3(position.x * chunkWidth, 0f, position.y * chunkWidth);

            BuildingGenerator.Instance.SetSeed(_seed);

            gameObject.SetActive(true);
        }

        private void InitSpots()
        {
            Transform child;

            spots = new List<Transform>();

            //loop at childs and search "SPOT" objects
            for (int i = 0; i < transform.childCount; i++)
            {
                child = transform.GetChild(i);
                if (child.name.CompareTo(c_spot) == 0)
                    spots.Add(child);
            }

        }

        public void InitBuildings()
        {
            Clear();

            for (int i = 0; i < spots.Count; i++)
                AddBuilding(spots[i]);
        }

        public IEnumerator InitBuildingsDelayed()
        {
            Clear();

            for (int i = 0; i < spots.Count; i++)
            {
                AddBuilding(spots[i]);
                yield return null;
            }
        }

        private void AddBuilding(Transform spot)
        {
            Transform house;

            house = BuildingGenerator.Instance.Generate();
            house.parent = spot;
            house.localPosition = new Vector3(NextRandomValue, 0f, NextRandomValue);
            _buildings.Add(house);
        }

        public void Clear()
        {
            if (_buildings == null)
            {
                _buildings = new List<Transform>();
            }
            else
            {
                for (int i = 0; i < _buildings.Count; i++)
                    BuildingGenerator.Instance.DisableBuilding(_buildings[i]);

                _buildings.Clear();
            }
        }

        public void Disable()
        {
            Clear();
            gameObject.SetActive(false);
        }

        public int GetSqrDist(Vector2Int fromPosition)
        {
            return (Position - fromPosition).sqrMagnitude;
        }
    }
}