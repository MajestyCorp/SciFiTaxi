using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scifi.Tools;

namespace Scifi.Generators
{
    public class CityGenerator : MonoBehaviour, IInitializer
    {
        public static CityGenerator Instance { get; private set; } = null;
        private const string c_spot = "SPOT";

        public float ChunkWidth => chunkWidth;
        public int GenerationRadius => generationRadius;
        public Vector2Int CenterPosition { get; private set; }

        [SerializeField]
        private Transform trackedObject;

        [SerializeField]
        private Transform chunkTemplate;

        [SerializeField]
        private float chunkWidth = 6f;

        [SerializeField, Tooltip("Generation chunk radius")]
        private int generationRadius = 3;

        [SerializeField, Tooltip("Randomize house position")]
        private float randomHouseRadius = 0.5f;

        private Pooler<Transform> _chunksPool = null;
        private System.Random _random;

        private List<Chunk> _chunks = new List<Chunk>();
        private Transform _cachedChunk = null;//chunk which is used in CreateChunkDelayed

        public class Chunk
        {
            public Vector2Int Position { get; private set; }
            public Transform Object { get; private set; }
            public Chunk(Vector2Int position, Transform chunk, float chunkWidth)
            {
                Object = chunk;
                Position = position;
                chunk.localPosition = new Vector3(position.x * chunkWidth, 0f, position.y * chunkWidth);
            }

            public int GetSqrDist(Vector2Int fromPosition)
            {
                return (Position - fromPosition).sqrMagnitude;
            }
        }

        #region initialize stuff
        public void InitInstance()
        {
            Instance = this;
            InitPooling();
        }

        public void Initialize()
        {
            InitCity();
        }

        private void InitPooling()
        {
            _chunksPool = new Pooler<Transform>(chunkTemplate, 5);
        }
        #endregion

        #region chunk generation
        private void InitCity()
        {
            int sqrRadius = generationRadius * generationRadius;
            int seed;
            Vector2Int coords = Vector2Int.zero;

            ClearChunks();

            for(coords.x =-generationRadius; coords.x <=generationRadius;coords.x++)
                for(coords.y =-generationRadius;coords.y<=generationRadius;coords.y++)
                {
                    if(coords.sqrMagnitude <= sqrRadius)
                    {
                        seed = (coords.x % 100) * 100 + coords.y % 100;// + cSeed;
                        _chunks.Add(new Chunk(coords, CreateChunk(seed), chunkWidth));
                    }
                }
        }

        private void ClearChunks()
        {
            for (int i = 0; i < _chunks.Count; i++)
                DisableChunk(_chunks[i].Object);

            _chunks.Clear();

        }
        #endregion

        private void Awake()
        {
            StartCoroutine(Tracker());
        }

        public bool ChunkExistAtPos(Vector3 position)
        {
            Vector2Int coords;
            coords = Vector2Int.FloorToInt(new Vector2(position.x/chunkWidth, 
                                                       (position.z +chunkWidth) / chunkWidth));
            return ChunkExist(coords);
        }

        private IEnumerator Tracker()
        {
            float minSize = chunkWidth / 2f;
            Vector2Int newPos;

            //here we will check chunks, generate new and remove old
            CenterPosition = Vector2Int.FloorToInt(new Vector2(trackedObject.position.x / chunkWidth,
                                                               (trackedObject.position.z + chunkWidth) / chunkWidth));
            
            while (true)
            {
                //get player position related to grid position
                //since block is not well centered (bottom left position is (0;-6)), we need to add chunk width to Z
                newPos = Vector2Int.FloorToInt(new Vector2(trackedObject.position.x / chunkWidth,
                                                           (trackedObject.position.z + chunkWidth) / chunkWidth));

                if (newPos != CenterPosition)
                {
                    yield return StartCoroutine(TrackerUpdate(newPos));

                    CenterPosition = newPos;
                }
                else
                {
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }

        private IEnumerator TrackerUpdate(Vector2Int newCenter)
        {
            int sqrRadius = generationRadius * generationRadius;
            int seed;
            Vector2Int coords = Vector2Int.zero;
            Vector2Int globalCoords;

            //check all chunks, add new and remove old

            //first, remove old chunks to decrease total list
            for (int i = _chunks.Count - 1; i >= 0; i--)
                if (_chunks[i].GetSqrDist(newCenter) > sqrRadius)
                {
                    DisableChunk(_chunks[i].Object);
                    _chunks.RemoveAt(i);

                    //make small delay after each action
                    yield return null;
                }

            //then add new chunks
            for (coords.x = -generationRadius; coords.x <= generationRadius; coords.x++)
                for (coords.y = -generationRadius; coords.y <= generationRadius; coords.y++)
                {
                    globalCoords = newCenter + coords;
                    if (coords.sqrMagnitude <= sqrRadius && !ChunkExist(globalCoords))
                    {
                        seed = ((globalCoords.x) % 100) * 100 + (globalCoords.y) % 100;

                        //do not create whole chunk at once
                        //instead make small delays between creating each building
                        //so final results wont have lags when big chunk is created
                        //result chunk will be placed in corChunk, because no out params can be used
                        yield return StartCoroutine(CreateChunkDelayed(globalCoords, seed));

                        _chunks.Add(new Chunk(globalCoords, _cachedChunk, chunkWidth));
                    } 
                }
        }

        private IEnumerator CreateChunkDelayed(Vector2Int globalCoords, int seed)
        {
            Transform child, house;

            _random = new System.Random(seed);

            _cachedChunk = _chunksPool.GetPooledObject();
            //set chunk position here because we do delayed creation
            //otherwise this chunk will blink and will be visible in wrong position
            _cachedChunk.localPosition = new Vector3(globalCoords.x * chunkWidth, 0f, globalCoords.y * chunkWidth);
            _cachedChunk.gameObject.SetActive(true);
            BuildingGenerator.Instance.SetSeed(seed);
            yield return null;

            for (int i = 0; i < _cachedChunk.childCount; i++)
            {
                child = _cachedChunk.GetChild(i);
                if (child.name.CompareTo(c_spot) == 0)
                {
                    house = BuildingGenerator.Instance.Generate();
                    house.parent = child;
                    house.localPosition = new Vector3(((float)_random.NextDouble() - 0.5f) * randomHouseRadius,
                                                      0f,
                                                      ((float)_random.NextDouble() - 0.5f) * randomHouseRadius);
                    yield return null;
                }
            }
        }

        private bool ChunkExist(Vector2Int coords)
        {
            for (int i = 0; i < _chunks.Count; i++)
                if (_chunks[i].Position == coords)
                    return true;
            return false;
        }

        private Transform CreateChunk(int seed)
        {
            Transform chunk = _chunksPool.GetPooledObject();
            Transform child, house;

            _random = new System.Random(seed);

            chunk.gameObject.SetActive(true);
            BuildingGenerator.Instance.SetSeed(seed);

            for(int i=0;i<chunk.childCount;i++)
            {
                child = chunk.GetChild(i);
                if(child.name.CompareTo(c_spot)==0)
                {
                    house = BuildingGenerator.Instance.Generate();
                    house.parent = child;
                    house.localPosition = new Vector3(((float)_random.NextDouble() - 0.5f) * randomHouseRadius,
                                                      0f,
                                                      ((float)_random.NextDouble() - 0.5f) * randomHouseRadius);
                }
            }

            return chunk;
        }

        private void DisableChunk(Transform chunk)
        {
            Transform child;

            //make loop at all SPOTs
            for (int i = 0; i < chunk.childCount; i++)
            {
                child = chunk.GetChild(i);

                if (child.name.CompareTo(c_spot) == 0)
                {
                    //if we have child object inside SPOT - its a house, disassemble it
                    if (child.childCount > 0)
                        BuildingGenerator.Instance.DisableBuilding(child.GetChild(0));
                }
            }

            chunk.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        
    }
}