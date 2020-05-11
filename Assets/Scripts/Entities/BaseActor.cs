using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PK
{
    public class BaseActor : MonoBehaviour
    {
        public bool moving = false;

        [SerializeField]
        protected ActorSprite sprites;
        private SpriteRenderer sprite;
        [SerializeField]
        protected WalkingSpeed walkSpeed = WalkingSpeed.Normal;
        public Facing facing = Facing.Down;
        [SerializeField]
        protected Speed animationSpeed = Speed.Normal;
        private Timer animationTimer = new Timer(1f);
        private int step = 1;
        private bool stepping = false;

        // Movement Vars
        protected MapChunk currentChunk;
        protected Vector3 targetPosition;
        protected bool stopAnimation = false;
        protected bool canMove = true;

        public void Awake()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            animationTimer.onFinish += NextStep;
            animationTimer.Start();
            targetPosition = transform.position;
            currentChunk = MapChunkManager.GetChunkAt(transform.position);
        }

        // Update is called once per frame
        public void Update()
        {
            if (moving)
                animationTimer.Update(Time.deltaTime * (float)animationSpeed * Constants.ANIMATION_SPEED_MULTIPLIER);
            sprite.sprite = sprites.GetSprite(facing, CalculateStep());
            sprite.flipX = sprites.flip;
        }

        private int CalculateStep()
        {
            if (!stepping || (!moving && stopAnimation))
                return 0;
            return step;
        }

        private void NextStep()
        {
            if (stepping)
            {
                stepping = false;
                step++;
                if (step > 2)
                    step = 1;
                animationTimer.Reset(true);
                return;
            }
            stepping = true;
            animationTimer.Reset(true);
        }

        public void Move(Vector2Int direction)
        {
            stopAnimation = false;
            if (direction == Vector2Int.up) MoveUp();
            else if (direction == Vector2Int.down) MoveDown();
            else if (direction == Vector2Int.left) MoveLeft();
            else if (direction == Vector2Int.right) MoveRight();

        }

        public void MoveUp()
        {
            if (moving) return;
            facing = Facing.Up;
            if (DestinationBlocked(transform.position + Vector3.up))
            {
                stopAnimation = true;
                return;
            }
            targetPosition += Vector3.up;
            StartCoroutine("PerformMovement");
        }

        public void MoveDown()
        {
            if (moving) return;
            facing = Facing.Down;
            if (DestinationBlocked(transform.position + Vector3.down))
            {
                stopAnimation = true;
                return;
            }
            targetPosition += Vector3.down;
            StartCoroutine("PerformMovement");
        }

        public void MoveLeft()
        {
            if (moving) return;
            facing = Facing.Left;
            if (DestinationBlocked(transform.position + Vector3.left))
            {
                stopAnimation = true;
                return;
            }
            targetPosition += Vector3.left;
            StartCoroutine("PerformMovement");
        }

        public void MoveRight()
        {
            if (moving) return;
            facing = Facing.Right;
            if (DestinationBlocked(transform.position + Vector3.right))
            {
                stopAnimation = true;
                return;
            }
            targetPosition += Vector3.right;
            StartCoroutine("PerformMovement");
        }

        public void StopAnimation()
        {
            stopAnimation = true;
        }

        protected virtual bool DestinationBlocked(Vector3 position)
        {
            bool blocked = false;

            Vector3Int tilePosition = currentChunk.permissionLayer.WorldToCell(position);
            TileBase tile = currentChunk.permissionLayer.GetTile(tilePosition);

            if (!tile)
                return false;

            blocked = (tile.name != Constants.TILE_PERMISSION_WALKABLE || tile.name != Constants.TILE_PERMISSION_ABOVE_LEVEL);

            return blocked;
        }

        protected IEnumerator PerformMovement()
        {
            canMove = false;
            moving = true;
            float velocity = Time.deltaTime * (int)walkSpeed;
            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, velocity);
                yield return null;
            }
            canMove = true;
            moving = false;
        }
    }
}
