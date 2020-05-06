using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PK
{
    public class MapChunkManager : MonoBehaviour
    {
        public Transform player;
        public static MapChunkManager _;

        private Vector2Int playerChunkPosition;
        private Vector2 playerChunkBorder;

        public MapChunk currentChunk;
        public MapChunk[] adjacentChunks = new MapChunk[8];

        void OnDrawGizmos()
        {
            float x = .5f;
            float y = Constants.MAP_CHUNK_SIZE * (float)Constants.MAP_CHUNK_AMOUNT + .5f;
            // Vertical lines
            for (int n = 0; n <= Constants.MAP_CHUNK_AMOUNT; n++)
            {
                Gizmos.color = Color.white;
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
                Gizmos.color = Color.white;
                Vector2 pos3 = new Vector2(0.5f, y);
                Vector2 pos4 = new Vector2(x, y);
                Gizmos.DrawLine(pos3, pos4);
                y += Constants.MAP_CHUNK_SIZE;
            }
        }

        void OnGUI()
        {
            GUI.color = Color.black;
            GUI.Label(new Rect(16, 16, 200, 24), "Position: " + playerChunkPosition.ToString());
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
        }

        // Update is called once per frame
        void Update()
        {
            playerChunkPosition = CalculateChunk(player.position);
        }

        public Vector2Int CalculateChunk(Vector3 position)
        {
            float coordX = (position.x - 1) / (float)Constants.MAP_CHUNK_SIZE;
            float coordY = position.y / (float)Constants.MAP_CHUNK_SIZE;
            int x = Mathf.FloorToInt(coordX);
            int y = Constants.MAP_CHUNK_AMOUNT - Mathf.CeilToInt(coordY);
            return new Vector2Int(x, y);
        }

        /**
        using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FL
{
    public class RoomData : MonoBehaviour
    {
        public Vector2Int roomSize = new Vector2Int(1, 1);
        public bool trackPosition = true;
        public Vector2Int roomPosition = new Vector2Int(1, 1);

        private Vector2Int playerOffset = new Vector2Int(-1, -1);
        private Vector2Int previousOffset = new Vector2Int(-1, -1);
        private Vector2 minCoords = new Vector2(0f, 0f);
        private Vector2 lowerBounds = new Vector2(0f, 0f);
        private Vector2 upperBounds = new Vector2(0f, 0f);
        private Vector2 playerRelativePosition = new Vector2(0f, 0f);

        private bool trackingEnabled = false;
        private Transform player;

        void Awake()
        {
            player = PlayerController._current.transform;
            previousOffset = new Vector2Int(-1, -1);
            RoomManager._current.onLoadStart += this.DisableTracking;
            RoomManager._current.onLoadFinished += this.EnableTracking;
            minCoords = new Vector2(-(Constants.ROOM_BOUNDS_WIDTH / 2f) * roomSize.x, (Constants.ROOM_BOUNDS_HEIGHT / 2f) * roomSize.y);
            lowerBounds = new Vector2(-(Constants.ROOM_WIDTH / 2f) * (roomSize.x + 1), (Constants.ROOM_HEIGHT / 2f) * (roomSize.y + 1));
            upperBounds = new Vector2((Constants.ROOM_WIDTH / 2f) * (roomSize.x + 1), -(Constants.ROOM_HEIGHT / 2f) * (roomSize.y + 1));
            RoomManager.SetCurrentRoom(this);
        }

        

        public void EnableTracking()
        {
            trackingEnabled = true;
        }

        public void DisableTracking()
        {
            RoomManager._current.onLoadFinished -= this.EnableTracking;
            RoomManager._current.onLoadStart -= this.DisableTracking;
            trackingEnabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (trackPosition && trackingEnabled)
            {
                RecalcRelativePosition();
                RecalcOffset();
                MapManager._current.UpdateToken(playerRelativePosition);
                if (previousOffset != playerOffset)
                {
                    UpdateMapPosition();
                    previousOffset = playerOffset;
                }
            }
        }

        public bool IsOutOfBounds(Vector3 point)
        {
            if (point.x < lowerBounds.x || point.x > upperBounds.x)
                return true;
            if (point.y > lowerBounds.y || point.y < upperBounds.y)
                return true;
            return false;
        }

        private Vector2 CalculateBounds()
        {
            return new Vector2(-(Constants.ROOM_WIDTH / 2f) * roomSize.x, (Constants.ROOM_HEIGHT / 2f) * roomSize.y);
        }

        private void RecalcOffset()
        {
            if (!player)
                player = PlayerController._current.transform;
            playerOffset.x = Mathf.FloorToInt((player.position.x - minCoords.x) / Constants.ROOM_BOUNDS_WIDTH);
            playerOffset.y = Mathf.FloorToInt((minCoords.y - player.position.y) / Constants.ROOM_BOUNDS_HEIGHT);
        }

        private void RecalcRelativePosition()
        {
            if (!player)
                player = PlayerController._current.transform;
            playerRelativePosition.x = roomPosition.x + ((player.position.x - minCoords.x) / Constants.ROOM_BOUNDS_WIDTH);
            playerRelativePosition.y = roomPosition.y + ((minCoords.y - player.position.y) / Constants.ROOM_BOUNDS_HEIGHT);
        }

        private void UpdateMapPosition()
        {
            Vector2Int actualPosition = new Vector2Int(roomPosition.x + playerOffset.x, roomPosition.y + playerOffset.y);
            MapManager._current.Visit(actualPosition);
        }
    }
}
*/
    }
}
