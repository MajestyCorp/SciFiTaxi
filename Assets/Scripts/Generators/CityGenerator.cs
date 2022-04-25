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
        private Chunk chunkTemplate;

        [SerializeField]
        private float chunkWidth = 6f;

        [SerializeField, Tooltip("Generation chunk radius")]
        private int generationRadius = 3;

        private Pooler<Chunk> _chunksPool = null;

        //list of active chunks
        private List<Chunk> _chunks = new List<Chunk>();

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
            chunkTemplate.InitPrefab();
            _chunksPool = new Pooler<Chunk>(chunkTemplate, 5);
        }
        #endregion

        #region chunk generation
        private void InitCity()
        {
            int sqrRadius = generationRadius * generationRadius;
            Chunk chunk;
            Vector2Int coords = Vector2Int.zero;

            ClearChunks();

            for(coords.x =-generationRadius; coords.x <=generationRadius;coords.x++)
                for(coords.y =-generationRadius;coords.y<=generationRadius;coords.y++)
                {
                    if(coords.sqrMagnitude <= sqrRadius)
                    {
                        chunk = _chunksPool.GetPooledObject();
                        chunk.InitInstance(GetSeed(coords), coords, chunkWidth);
                        chunk.InitBuildings();
                        _chunks.Add(chunk);
                    }
                }
        }

        private void ClearChunks()
        {
            for (int i = 0; i < _chunks.Count; i++)
                _chunks[i].Disable();

            _chunks.Clear();
        }

        private int GetSeed(Vector2Int coords)
        {
            return (coords.x % 100) * 100 + coords.y % 100;
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
            Chunk chunk;
            Vector2Int coords = Vector2Int.zero;
            Vector2Int globalCoords;

            //check all chunks, add new and remove old

            //first, remove old chunks to decrease total list
            for (int i = _chunks.Count - 1; i >= 0; i--)
                if (_chunks[i].GetSqrDist(newCenter) > sqrRadius)
                {
                    _chunks[i].Disable();
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
                        chunk = _chunksPool.GetPooledObject();
                        chunk.InitInstance(GetSeed(globalCoords), globalCoords, chunkWidth);

                        //do not create whole chunk at once
                        //instead make small delays between creating each building
                        //so final results wont have lags when big chunk is created
                        //result chunk will be placed in corChunk, because no out params can be used
                        yield return StartCoroutine(chunk.InitBuildingsDelayed());

                        _chunks.Add(chunk);
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

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}