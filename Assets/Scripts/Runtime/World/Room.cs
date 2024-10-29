using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace VVVVVV.Runtime.World
{
    public class Room : MonoBehaviour
    {
        public Vector2Int RoomPos;
        public string RoomName;

        public void InitTiles(RoomJson json, Tile[] tiles)
        {
            var bg = transform.Find("BG").GetComponent<Tilemap>();
            var wall = transform.Find("Wall").GetComponent<Tilemap>();
            var hurtAble = transform.Find("Hurtable").GetComponent<Tilemap>();
            var roomSize = Constant.ROOM_TILE_SIZE;

            for (int i = 0; i < roomSize.x; i++)
            {
                for (int j = 0; j < roomSize.y; j++)
                {
                    var tileIdx = json.tiles[j * roomSize.x + i];
                    var tileMap = tileIdx switch
                    {
                        < 80 => hurtAble,
                        (>= 80) and (< 680) => wall,
                        _ => bg,
                    };
                    tileMap.SetTile(new(i, (roomSize.y - 1) - j), tiles[tileIdx]);
                }
            }
        }
    }

    [Serializable]
    public record struct RoomJson
    {
#pragma warning disable IDE1006 // Naming Styles
        // Use camel_case in json
        public string name;
        public int[] tiles;
#pragma warning restore IDE1006 // Naming Styles
        public Vector2Int Pos;
    }
}
