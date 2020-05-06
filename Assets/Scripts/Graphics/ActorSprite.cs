using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    [CreateAssetMenu(fileName = "ActorSprite", menuName = "Spriteset/Actor", order = 1)]
    public class ActorSprite : ScriptableObject
    {
        [SerializeField]
        private Sprite[] sprites = new Sprite[12];
        [SerializeField]
        private ActorSpriteType type = ActorSpriteType.RegularMirrored;

        public bool flip { get; private set; } = false;

        public Sprite GetSprite(Facing direction, int step)
        {
            int index = 0;
            switch (type)
            {
                case ActorSpriteType.RegularMirrored:
                    index = GetMirroredIndex(direction, step);
                    break;
                case ActorSpriteType.RegularTriFrame:
                    index = GetTriFrameIndex(direction, step);
                    break;
                case ActorSpriteType.RegularFourDirection:
                    index = GetFourDirectionIndex(direction, step);
                    break;
                case ActorSpriteType.FullyAnimated:
                    index = GetFullAnimationIndex(direction, step);
                    break;
            }
            return sprites[index];
        }

        private int GetMirroredIndex(Facing direction, int step)
        {
            flip = false;
            if (direction == Facing.Right)
            {
                direction = Facing.Left;
                flip = true;
            }

            if ((direction == Facing.Down || direction == Facing.Up) && step > 1)
                flip = true;

            int index = (int)direction * 2;
            index += step > 0 ? 1 : 0;

            return index;
        }

        private int GetFourDirectionIndex(Facing direction, int step)
        {
            flip = false;

            if ((direction == Facing.Up || direction == Facing.Down) && step > 1)
                flip = true;

            int index = (int)direction * 2;
            index += step > 0 ? 1 : 0;

            return index;
        }

        private int GetTriFrameIndex(Facing direction, int step)
        {
            flip = false;
            if (direction == Facing.Right)
            {
                direction = Facing.Left;
                flip = true;
            }

            int index = (int)direction * 2;
            index += step;

            return index;
        }

        private int GetFullAnimationIndex(Facing direction, int step)
        {
            flip = false;

            int index = (int)direction * 2;
            index += step;

            return index;
        }
    }
}