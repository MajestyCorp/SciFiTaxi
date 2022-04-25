using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scifi.Tools;
using Scifi.AI;

namespace Scifi.Generators
{
    public class TrafficGenerator : MonoBehaviour, IInitializer
    {
        public static TrafficGenerator Instance { get; private set; } = null;

        [SerializeField]
        private List<CarItem> cars;

        private CityGenerator _cityGen;


        #region initializer
        public void InitInstance()
        {
            Instance = this;
            InitCarPools();
        }

        /// <summary>
        /// all managers are initialized
        /// so we can get params from CityGenerator
        /// and place all cars on map
        /// </summary>
        public void Initialize()
        {
            _cityGen = CityGenerator.Instance;

            for(int i=0;i<cars.Count;i++)
                PlaceCars(cars[i]);
        }
        #endregion

        /// <summary>
        /// respawn car on the edge of "chunks"
        /// </summary>
        public void RespawnCar(Transform car)
        {
            car.position = GetRespawnPosition();
            car.gameObject.SetActive(true);
        }

        private void InitCarPools()
        {
            for (int i = 0; i < cars.Count; i++)
                cars[i].InitPool();
        }

        private void PlaceCars(CarItem item)
        {
            Transform t;
            for(int i=0;i<item.amount;i++)
            {
                t = item.GetPooledCar();
                //set random position inside huge chunks circle
                //and then normalize one of X or Z axis
                t.position = GetRandomPosition();
                t.gameObject.SetActive(true);
            }
        }

        private Vector3 GetRandomPosition()
        {
            Vector3 position;
            position = Random.insideUnitCircle * _cityGen.ChunkWidth * (_cityGen.GenerationRadius - 1f);

            //swap Y and Z pos
            position.z = position.y;
            position.y = 0f;

            return ClipPos(position);
        }

        private Vector3 GetRespawnPosition()
        {
            Vector3 position;
            position = Random.insideUnitCircle.normalized * _cityGen.ChunkWidth * (_cityGen.GenerationRadius - 1f);

            //swap Y and Z pos
            position.z = position.y;
            position.y = 0f;

            //add player offset
            position.x += _cityGen.CenterPosition.x * _cityGen.ChunkWidth;
            position.z += _cityGen.CenterPosition.y * _cityGen.ChunkWidth;

            return ClipPos(position);
        }

        private Vector3 ClipPos(Vector3 position)
        {
            //Stick X or Z to "road" position
            if (Random.value > 0.5f)
                position.x = Mathf.FloorToInt((position.x) / _cityGen.ChunkWidth) * _cityGen.ChunkWidth;
            else
                position.z = Mathf.FloorToInt((position.z + _cityGen.ChunkWidth) / _cityGen.ChunkWidth) * _cityGen.ChunkWidth;
            return position;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;

            for (int i = 0; i < cars.Count; i++)
                cars[i].Destroy();
        }
    }


    [System.Serializable]
    public class CarItem
    {
        public CarAI carPrefab;
        public int amount;

        private Pooler<Transform> pool = null;

        public void InitPool()
        {
            if (carPrefab.gameObject.activeSelf)
                carPrefab.gameObject.SetActive(false);

            pool = new Pooler<Transform>(carPrefab.transform, amount);
        }

        public Transform GetPooledCar()
        {
            return pool.GetPooledObject();
        }

        public void Destroy()
        {
            if (pool != null)
                pool.Clear();
        }
    }
}