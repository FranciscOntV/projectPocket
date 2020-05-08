using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class ChunkRegistry : MonoBehaviour
    {
        public GameObject defaultChunk;
        public GameObject[] mapChunks;
        private Dictionary<string, GameObject> registeredChunks = new Dictionary<string, GameObject>();

        void Awake()
        {
            foreach (GameObject chunk in mapChunks)
            {
                if (chunk)
                    Register(chunk);
            }
        }

        public MapChunk GetChunkData(Vector2Int coord)
        {
            string entry = AsCoordinate(coord.x, coord.y);
            GameObject chunk = null;
            MapChunk chunkData = null;
            if (registeredChunks.ContainsKey(entry))
            {
                chunk = registeredChunks[entry];
                chunkData = chunk.GetComponent<MapChunk>();
            }
            else
            {
                Debug.LogWarning($"Chunk for {coord.x}{coord.y} was not found!");
            }

            return chunkData;
        }

        public GameObject GetChunk(Vector2Int coord)
        {
            string entry = AsCoordinate(coord.x, coord.y);
            GameObject chunk = null;
            if (registeredChunks.ContainsKey(entry))
            {
                chunk = registeredChunks[entry];
            }
            else
            {
                Debug.LogWarning($"Chunk for {entry} was not found!");
            }

            return chunk;
        }

        private void Register(GameObject chunk)
        {
            MapChunk chunkData = chunk.GetComponent<MapChunk>();
            if (!chunkData)
                return;

            string entry = $"{chunkData.X.ToString()}{chunkData.Y.ToString()}";
            if (registeredChunks.ContainsKey(entry))
            {
                Debug.LogWarning($"A duplicated registry for {entry} was intended.");
            }
            else
            {
                registeredChunks.Add(entry, chunk);
            }
        }

        public static string AsCoordinate(int x, int y)
        {
            if (x > 26 || y > 26 || x < 0 || y < 0)
                return Constants.MAP_CHUNK_UNDEFINED;
            string coordinate = $"{((Coordinate)x).ToString()}{((Coordinate)y).ToString()}";

            return coordinate;
        }
    }
}
