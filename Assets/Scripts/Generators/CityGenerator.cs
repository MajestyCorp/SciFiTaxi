using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scifi
{
    public class CityGenerator : MonoBehaviour, IInitializer
    {
        public static CityGenerator Instance { get; private set; } = null;
        private const string c_spot = "SPOT";

        public float ChunkWidth => chunkWidth;
        public int GenerationRadius => generationRadius;
        public int CenterX { get; private set; }
        public int CenterY { get; private set; }

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

        private Pooler chunksPool = null;
        private System.Random _random;

        private List<Chunk> chunks = new List<Chunk>();
        private Transform corChunk = null;//chunk which is used in CreateChunkDelayed

        public class Chunk
        {
            public int X { get; private set; }
            public int Y { get; private set; }
            public Transform Object { get; private set; }
            public Chunk(int x, int y, Transform chunk, float chunkWidth)
            {
                X = x;
                Y = y;
                Object = chunk;
                chunk.localPosition = new Vector3(x * chunkWidth, 0f, y * chunkWidth);
            }

            public int GetSqrDist(int x, int y)
            {
                return (X - x) * (X - x) + (Y - y) * (Y - y);
            }

            public bool IsSameCoords(int x, int y)
            {
                return X == x && Y == y;
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
            chunksPool = new Pooler(chunkTemplate, 5);
        }
        #endregion

        #region chunk generation
        private void InitCity()
        {
            int sqrRadius = generationRadius * generationRadius;
            int seed;
            //int cSeed =  Random.Range(int.MinValue, int.MaxValue);

            ClearChunks();

            for(int i=-generationRadius;i<=generationRadius;i++)
                for(int j=-generationRadius;j<=generationRadius;j++)
                {
                    if(i*i + j*j <= sqrRadius)
                    {
                        seed = (i % 100) * 100 + j % 100;// + cSeed;
                        chunks.Add(new Chunk(i, j, CreateChunk(seed), chunkWidth));
                    }
                }
        }

        private void ClearChunks()
        {
            for (int i = 0; i < chunks.Count; i++)
                DisableChunk(chunks[i].Object);

            chunks.Clear();

        }
        #endregion

        private void Awake()
        {
            
            StartCoroutine(Tracker());
        }

        public bool ChunkExistAtPos(Vector3 position)
        {
            int x, y;
            x = Mathf.FloorToInt((position.x) / chunkWidth);
            y = Mathf.FloorToInt((position.z + chunkWidth) / chunkWidth);

            return ChunkExist(x, y);
        }

        private IEnumerator Tracker()
        {
            float minSize = chunkWidth / 2f;
            int newX, newY, oldX, oldY;

            //here we will check chunks, generate new and remove old
            oldX = Mathf.FloorToInt((trackedObject.position.x) / chunkWidth);
            oldY = Mathf.FloorToInt((trackedObject.position.z + chunkWidth) / chunkWidth);

            while (true)
            {
                //get player position related to grid position
                //since block is not well centered (bottom left position is (0;-6)), we need to add chunk width to Z
                newX = Mathf.FloorToInt((trackedObject.position.x) / chunkWidth);
                newY = Mathf.FloorToInt((trackedObject.position.z + chunkWidth) / chunkWidth);

                CenterX = newX;
                CenterY = newY;

                if(newX != oldX || newY != oldY)
                {
                    yield return StartCoroutine(TrackerUpdate(newX, newY));

                    oldX = newX;
                    oldY = newY;
                } else
                    yield return new WaitForSeconds(0.2f);
            }
        }

        private IEnumerator TrackerUpdate(int newX, int newY)
        {
            int sqrRadius = generationRadius * generationRadius;
            int seed;

            //check all chunks, add new and remove old

            //first, remove old chunks to decrease total list
            for (int i = chunks.Count - 1; i >= 0; i--)
                if (chunks[i].GetSqrDist(newX, newY) > sqrRadius)
                {
                    DisableChunk(chunks[i].Object);
                    chunks.RemoveAt(i);

                    //make small delay after each action
                    yield return null;
                }

            //then add new chunks
            for (int i = -generationRadius; i <= generationRadius; i++)
                for (int j = -generationRadius; j <= generationRadius; j++)
                {
                    if (i * i + j * j <= sqrRadius && !ChunkExist(newX + i, newY + j))
                    {
                        seed = ((newX + i) % 100) * 100 + (newY + j) % 100;

                        //do not create whole chunk at once
                        //instead make small delays between creating each building
                        //so final results wont have lags when big chunk is created
                        //result chunk will be placed in corChunk, because no out params can be used
                        yield return StartCoroutine(CreateChunkDelayed(newX + i, newY + j, seed));

                        chunks.Add(new Chunk(newX + i, newY + j, corChunk, chunkWidth));
                    } 
                }
        }

        private IEnumerator CreateChunkDelayed(int x, int y, int seed)
        {
            Transform child, house;

            _random = new System.Random(seed);

            corChunk = chunksPool.GetPooledObject().transform;
            //set chunk position here because we do delayed creation
            //otherwise this chunk will blink and will be visible in wrong position
            corChunk.localPosition = new Vector3(x * chunkWidth, 0f, y * chunkWidth);
            corChunk.gameObject.SetActive(true);
            BuildingGenerator.Instance.SetSeed(seed);
            yield return null;

            for (int i = 0; i < corChunk.childCount; i++)
            {
                child = corChunk.GetChild(i);
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

        private bool ChunkExist(int x, int y)
        {
            for (int i = 0; i < chunks.Count; i++)
                if (chunks[i].IsSameCoords(x, y))
                    return true;
            return false;
        }

        private Transform CreateChunk(int seed)
        {
            Transform chunk = chunksPool.GetPooledObject().transform;
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