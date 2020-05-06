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
    }
}
