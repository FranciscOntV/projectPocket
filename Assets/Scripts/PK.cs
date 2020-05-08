using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public enum Facing
    {
        Down,
        Up,
        Left,
        Right
    }

    public enum ActorSpriteType
    {
        RegularMirrored,
        RegularTriFrame,
        RegularFourDirection,
        FullyAnimated
    }

    public enum Speed
    {
        Zero,
        VerySlow = 2,
        Slow = 4,
        Normal = 6,
        Fast = 8,
        VeryFast = 10
    }

    public enum WalkingSpeed
    {
        Zero,
        VerySlow = 1,
        Slow = 2,
        Normal = 3,
        Fast = 4,
        VeryFast = 5
    }

    public enum Coordinate {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z
    }

    public class Constants
    {
        public const float ANIMATION_SPEED_MULTIPLIER = 1f;

        public const float MAP_CHUNK_SIZE = 10f;
        public const int MAP_CHUNK_AMOUNT = 3;
        public const string MAP_CHUNK_UNDEFINED = "00";

        public const string TILE_PERMISSION_NONE  = "tilePermission_0";
        public const string TILE_PERMISSION_WALKABLE  = "tilePermission_1";

        public const string MAP_CHUNK_INSTANCE_NAME = "Chunk-";
    }
}
