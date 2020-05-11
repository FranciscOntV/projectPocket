using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PK
{
    [RequireComponent(typeof(ChunkRegistry))]
    public class MapChunkManager : MonoBehaviour
    {
        public Transform player;
        public static MapChunkManager _;

        public ChunkGrid loadedChunks = new ChunkGrid();

        private ChunkRegistry registry;

        void OnDrawGizmos()
        {
            float x = .5f;
            float y = Constants.MAP_CHUNK_SIZE * (float)Constants.MAP_CHUNK_AMOUNT + .5f;
            // Vertical lines
            Gizmos.color = Color.red;
            for (int n = 0; n <= Constants.MAP_CHUNK_AMOUNT; n++)
            {
                Vector2 pos1 = new Vector2(x, 0.5f);
                Vector2 pos2 = new Vector2(x, y);
                Gizmos.DrawLine(pos1, pos2);
                x += Constants.MAP_CHUNK_SIZE;
            }
            x = Constants.MAP_CHUNK_SIZE * (float)Constants.MAP_CHUNK_AMOUNT + .5f;
            y = .5f;
            // Horizontal
            for (int m = 0; m <= Constants.MAP_CHUNK_AMOUNT; m++)
            {
                Vector2 pos3 = new Vector2(0.5f, y);
                Vector2 pos4 = new Vector2(x, y);
                Gizmos.DrawLine(pos3, pos4);
                y += Constants.MAP_CHUNK_SIZE;
            }
        }

        private void Awake()
        {
            if (_ == null)
            {
                _ = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            registry = GetComponent<ChunkRegistry>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void LoadAllChunks(Vector3 position)
        {
            Vector2Int chunkPosition = _.CalculateChunk(position);

            chunkPosition.x -= 1;
            chunkPosition.y -= 1;
            loadedChunks.topLeft = LoadChunk(chunkPosition);
            chunkPosition.x += 1;
            loadedChunks.topCenter = LoadChunk(chunkPosition);
            chunkPosition.x += 1;
            loadedChunks.topRight = LoadChunk(chunkPosition);
            chunkPosition.y += 1;
            chunkPosition.x -= 2;

            loadedChunks.midLeft = LoadChunk(chunkPosition);
            chunkPosition.x += 1;
            loadedChunks.midCenter = LoadChunk(chunkPosition);
            chunkPosition.x += 1;
            loadedChunks.midRight = LoadChunk(chunkPosition);
            chunkPosition.y += 1;
            chunkPosition.x -= 2;
            loadedChunks.lowLeft = LoadChunk(chunkPosition);
            chunkPosition.x += 1;
            loadedChunks.lowCenter = LoadChunk(chunkPosition);
            chunkPosition.x += 1;
            loadedChunks.lowRight = LoadChunk(chunkPosition);
        }

        public void PerformShift(Vector3 position, Vector3 direction)
        {
            Vector2Int pos = _.CalculateChunk(position);
            if (direction == Vector3.up)
            {
                GameObject TL = LoadChunk(new Vector2Int(pos.x - 1, pos.y - 1));
                GameObject TC = LoadChunk(new Vector2Int(pos.x, pos.y - 1));
                GameObject TR = LoadChunk(new Vector2Int(pos.x + 1, pos.y - 1));

                foreach (GameObject oldChunk in loadedChunks.ShiftUp(TL, TC, TR))
                    Destroy(oldChunk);
            }

            if (direction == Vector3.down)
            {
                GameObject LL = LoadChunk(new Vector2Int(pos.x - 1, pos.y + 1));
                GameObject LC = LoadChunk(new Vector2Int(pos.x, pos.y + 1));
                GameObject LR = LoadChunk(new Vector2Int(pos.x + 1, pos.y + 1));

                foreach (GameObject oldChunk in loadedChunks.ShiftDown(LL, LC, LR))
                    Destroy(oldChunk);
            }

            if (direction == Vector3.left)
            {
                GameObject TL = LoadChunk(new Vector2Int(pos.x - 1, pos.y - 1));
                GameObject ML = LoadChunk(new Vector2Int(pos.x - 1, pos.y));
                GameObject LL = LoadChunk(new Vector2Int(pos.x - 1, pos.y + 1));

                foreach (GameObject oldChunk in loadedChunks.ShiftLeft(TL, ML, LL))
                    Destroy(oldChunk);
            }

            if (direction == Vector3.right)
            {
                GameObject TR = LoadChunk(new Vector2Int(pos.x + 1, pos.y - 1));
                GameObject MR = LoadChunk(new Vector2Int(pos.x + 1, pos.y));
                GameObject LR = LoadChunk(new Vector2Int(pos.x + 1, pos.y + 1));

                foreach (GameObject oldChunk in loadedChunks.ShiftRight(TR, MR, LR))
                    Destroy(oldChunk);
            }
        }

        private GameObject LoadChunk(Vector2Int pos)
        {
            GameObject chunk = registry.GetChunk(new Vector2Int(pos.x, pos.y));
            if (!chunk)
                return null;
            string coordinate = ChunkRegistry.AsCoordinate(pos.x, pos.y);
            GameObject currentChunk = Instantiate(chunk, Vector3.zero, Quaternion.identity);
            currentChunk.name = $"{Constants.MAP_CHUNK_INSTANCE_NAME}{coordinate}";


            return currentChunk;
        }

        public static MapChunk GetChunkAt(Vector3 position)
        {
            Vector2Int chunkPosition = _.CalculateChunk(position);
            string coordinate = ChunkRegistry.AsCoordinate(chunkPosition.x, chunkPosition.y);
            GameObject chunk = _.loadedChunks.SearchChunk($"{Constants.MAP_CHUNK_INSTANCE_NAME}{coordinate}");

            if (chunk)
                return chunk.GetComponent<MapChunk>();

            return null;
        }

        public Vector2Int CalculateChunk(Vector3 position)
        {
            float coordX = (position.x - 1) / (float)Constants.MAP_CHUNK_SIZE;
            float coordY = position.y / (float)Constants.MAP_CHUNK_SIZE;
            int x = Mathf.FloorToInt(coordX);
            int y = Constants.MAP_CHUNK_AMOUNT - Mathf.CeilToInt(coordY);
            return new Vector2Int(x, y);
        }
    }

    public class ChunkGrid
    {
        public GameObject topCenter;
        public GameObject topLeft;
        public GameObject topRight;
        public GameObject midCenter;
        public GameObject midLeft;
        public GameObject midRight;
        public GameObject lowCenter;
        public GameObject lowLeft;
        public GameObject lowRight;

        public GameObject SearchChunk(string chunkName)
        {
            if (topCenter && topCenter.name == chunkName) return topCenter;
            if (topLeft && topLeft.name == chunkName) return topLeft;
            if (topRight && topRight.name == chunkName) return topRight;

            if (midCenter && midCenter.name == chunkName) return midCenter;
            if (midLeft && midLeft.name == chunkName) return midLeft;
            if (midRight && midRight.name == chunkName) return midRight;

            if (lowCenter && lowCenter.name == chunkName) return lowCenter;
            if (lowLeft && lowLeft.name == chunkName) return lowLeft;
            if (lowRight && lowRight.name == chunkName) return lowRight;

            return null;
        }

        public GameObject[] ShiftUp(GameObject newTopLeft, GameObject newTopCenter, GameObject newTopRight)
        {
            GameObject[] moveOut = new GameObject[3];
            moveOut.SetValue(lowLeft, 0);
            moveOut.SetValue(lowCenter, 1);
            moveOut.SetValue(lowRight, 2);

            lowLeft = midLeft;
            lowCenter = midCenter;
            lowRight = midRight;

            midLeft = topLeft;
            midCenter = topCenter;
            midRight = topRight;

            topLeft = newTopLeft;
            topCenter = newTopCenter;
            topRight = newTopRight;

            return moveOut;
        }

        public GameObject[] ShiftDown(GameObject newLowLeft, GameObject newLowCenter, GameObject newLowRight)
        {
            GameObject[] moveOut = new GameObject[3];
            moveOut.SetValue(topLeft, 0);
            moveOut.SetValue(topCenter, 1);
            moveOut.SetValue(topRight, 2);

            topLeft = midLeft;
            topCenter = midCenter;
            topRight = midRight;

            midLeft = lowLeft;
            midCenter = lowCenter;
            midRight = lowRight;

            lowLeft = newLowLeft;
            lowCenter = newLowCenter;
            lowRight = newLowRight;

            return moveOut;
        }

        public GameObject[] ShiftRight(GameObject newTopRight, GameObject newMidRight, GameObject newLowRight)
        {
            GameObject[] moveOut = new GameObject[3];
            moveOut.SetValue(topLeft, 0);
            moveOut.SetValue(midLeft, 1);
            moveOut.SetValue(lowLeft, 2);

            topLeft = topCenter;
            midLeft = midCenter;
            lowLeft = lowCenter;

            topCenter = topRight;
            midCenter = midRight;
            lowCenter = lowRight;

            topRight = newTopRight;
            midRight = newMidRight;
            lowRight = newLowRight;

            return moveOut;
        }

        public GameObject[] ShiftLeft(GameObject newTopLeft, GameObject newMidLeft, GameObject newLowLeft)
        {
            GameObject[] moveOut = new GameObject[3];
            moveOut.SetValue(topRight, 0);
            moveOut.SetValue(midRight, 1);
            moveOut.SetValue(lowRight, 2);

            topRight = topCenter;
            midRight = midCenter;
            lowRight = lowCenter;

            topCenter = topLeft;
            midCenter = midLeft;
            lowCenter = lowLeft;

            topLeft = newTopLeft;
            midLeft = newMidLeft;
            lowLeft = newLowLeft;

            return moveOut;
        }
    }
}
