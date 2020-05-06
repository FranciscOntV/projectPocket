using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class ChunkRegistry : MonoBehaviour
    {
        public MapChunk defaultChunk;
        public MapChunk[] mapChunks;
        private Dictionary<string, MapChunk> registeredChunks = new Dictionary<string, MapChunk>();

        void Awake()
        {
            foreach (MapChunk chunk in mapChunks)
            {
                Register(chunk);
            }
        }

        public MapChunk GetChunk(Vector2Int coord)
        {
            string entry = AsCoordinate(coord.x, coord.y);
            MapChunk chunk = null;
            if (registeredChunks.ContainsKey(entry))
            {
                chunk = registeredChunks[entry];
            }
            else
            {
                Debug.LogWarning($"Chunk for {coord.x}{coord.y} was not found!");
            }

            return chunk;
        }

        private void Register(MapChunk chunk)
        {
            if (!chunk)
                return;
            string entry = $"{chunk.X.ToString()}{chunk.Y.ToString()}";
            if (registeredChunks.ContainsKey(entry))
            {
                Debug.LogWarning($"A duplicated registry for {entry} was intended.");
            }
            else
            {
                registeredChunks.Add(entry, chunk);
                chunk.gameObject.SetActive(false);
            }
        }

        private string AsCoordinate(int x, int y)
        {
            if (x > 26 || y > 26 || x < 0 || y < 0)
                return Constants.MAP_CHUNK_UNDEFINED;
            string coordinate = $"{((Coordinate)x).ToString()}{((Coordinate)y).ToString()}";

            return coordinate;
        }
    }
}
