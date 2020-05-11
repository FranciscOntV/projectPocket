using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PK
{

    public class PlayerActor : BaseActor
    {

        private bool jumpNext = false;

        private Animator animator;
        public GameObject dropShadow;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        public new void Awake()
        {
            MapChunkManager._.LoadAllChunks(transform.position);
            base.Awake();
            dropShadow.SetActive(false);
            animator = GetComponentInChildren<Animator>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        public new void Update()
        {
            base.Update();
        }

        protected override bool DestinationBlocked(Vector3 position)
        {
            bool blocked = false;
            MapChunk chunk = MapChunkManager.GetChunkAt(position);
            if (!chunk)
                return true;
            Vector3Int tilePosition = chunk.permissionLayer.WorldToCell(position);
            TileBase tile = chunk.permissionLayer.GetTile(tilePosition);

            if (!tile)
                return false;

            // Try Jump
            if (tile.name == Constants.TILE_PERMISSION_JUMP_LEFT && facing == Facing.Left)
            {
                if (CanJumpTo(position + Vector3.left))
                {
                    targetPosition += Vector3.left;
                    jumpNext = true;
                    return false;
                }
            }

            if (tile.name == Constants.TILE_PERMISSION_JUMP_RIGHT && facing == Facing.Right)
            {
                if (CanJumpTo(position + Vector3.right))
                {
                    targetPosition += Vector3.right;
                    jumpNext = true;
                    return false;
                }
            }

            if (tile.name == Constants.TILE_PERMISSION_JUMP_UP && facing == Facing.Up)
            {
                if (CanJumpTo(position + Vector3.up))
                {
                    targetPosition += Vector3.up;
                    jumpNext = true;
                    return false;
                }
            }

            if (tile.name == Constants.TILE_PERMISSION_JUMP_DOWN && facing == Facing.Down)
            {
                if (CanJumpTo(position + Vector3.down))
                {
                    targetPosition += Vector3.down;
                    jumpNext = true;
                    return false;
                }
            }

            blocked = !(tile.name == Constants.TILE_PERMISSION_WALKABLE || tile.name == Constants.TILE_PERMISSION_ABOVE_LEVEL);

            if (!blocked && chunk != currentChunk)
            {
                currentChunk = chunk;
                MapChunkManager._.PerformShift(position, position - transform.position);
            }

            return blocked;
        }

        protected bool CanJumpTo(Vector3 position)
        {
            bool canJump = false;
            MapChunk chunk = MapChunkManager.GetChunkAt(position);
            if (!chunk)
                return false;
            Vector3Int tilePosition = chunk.permissionLayer.WorldToCell(position);
            TileBase tile = chunk.permissionLayer.GetTile(tilePosition);

            canJump = (tile.name == Constants.TILE_PERMISSION_WALKABLE || tile.name == Constants.TILE_PERMISSION_ABOVE_LEVEL);

            if (canJump && chunk != currentChunk)
            {
                currentChunk = chunk;
                MapChunkManager._.PerformShift(position, (position - transform.position) / 2f);
            }

            return canJump;
        }

        protected new IEnumerator PerformMovement()
        {
            if (jumpNext){
                animator.SetBool(Constants.ANIMATOR_JUMP_KEY, true);
                jumpNext = false;
                dropShadow.SetActive(true);
            }
            canMove = false;
            moving = true;
            float velocity = Time.deltaTime * (int)walkSpeed;
            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, velocity);
                yield return null;
            }
            dropShadow.SetActive(false);
            animator.SetBool(Constants.ANIMATOR_JUMP_KEY, false);
            canMove = true;
            moving = false;
        }
    }
}
