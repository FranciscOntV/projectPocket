using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PK
{
    public class MapChunk : MonoBehaviour
    {
        public Coordinate X = Coordinate.A;
        public Coordinate Y = Coordinate.B;

        public Tilemap permissionLayer;

        public string Coords()
        {
            return $"{X.ToString()}{Y.ToString()}";
        }

        void OnDrawGizmos()
        {
            float x = .5f + ((float)X * (Constants.MAP_CHUNK_SIZE) + (Constants.MAP_CHUNK_SIZE /2f));
            float y = .5f + (((float)Constants.MAP_CHUNK_AMOUNT - (float)Y) * (Constants.MAP_CHUNK_SIZE) - (Constants.MAP_CHUNK_SIZE /2f));
            Vector3 position = new Vector3(x, y, 0f);
            Vector3 size = new Vector3(Constants.MAP_CHUNK_SIZE, Constants.MAP_CHUNK_SIZE, Constants.MAP_CHUNK_SIZE);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(position, size);
        }
    }
}
