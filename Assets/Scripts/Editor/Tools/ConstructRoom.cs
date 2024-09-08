using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Linq;
using System;
using VVVVVV.Runtime.World;
using VVVVVV.Utils.Extension;
using VVVVVV.Runtime;

namespace VVVVVV.Editor.Tools;

public class ConstructRoom : EditorWindow
{
    static readonly string TileDataDir = "Assets/_ART/Tiles1/";
    static readonly string RoomJsonDir = "Assets/Data/room/spacestation";
    static readonly string RoomPrefabExportDir = "Assets/Prefabs/Rooms/SpaceStation";
    static readonly string RoomPrefabBasePath = "Assets/Prefabs/PF_RoomBase.prefab";

    static Tile[] _tiles = null!;

    [MenuItem("VVVVVV/ConstructRoom")]
    static void ShowWindow()
    {
        _tiles = LoadAssets<Tile>("Tile", TileDataDir);
        LoadJsons().ForEach(CreateRoomPrefab);
    }

    static void CreateRoomPrefab(RoomJson json)
    {
        var prefabSrc = AssetDatabase.LoadAssetAtPath<GameObject>(RoomPrefabBasePath);
        var prefab = (GameObject)PrefabUtility.InstantiatePrefab(prefabSrc);

        var room = prefab.AddComponent<Room>();
        var name = json.name.Replace("?", "_");
        prefab.name = $"{json.Pos.x},{json.Pos.y}-{name}";

        var bg = prefab.transform.Find("BG").GetComponent<Tilemap>();
        var wall = prefab.transform.Find("Wall").GetComponent<Tilemap>();
        var hurtAble = prefab.transform.Find("Hurtable").GetComponent<Tilemap>();
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
                tileMap.SetTile(new(i, (roomSize.y - 1) - j), _tiles[tileIdx]);
            }
        }
        PrefabUtility.SaveAsPrefabAsset(prefab, RoomPrefabExportDir + $"/{prefab.name}.prefab");
        DestroyImmediate(prefab);
    }

    static RoomJson[] LoadJsons()
    {
        return LoadAssets<TextAsset>("TextAsset", RoomJsonDir)
            .Select(x =>
            {
                var filenameSplit = x.name.Split(",").Select(int.Parse).ToList();
                var json = JsonUtility.FromJson<RoomJson>(x.text);
                json.Pos = new Vector2Int(filenameSplit[0], filenameSplit[1]);
                return json;
            }).ToArray();
    }

    static T[] LoadAssets<T>(string typeStr, string directory) where T : UnityEngine.Object
    {
        return AssetDatabase.FindAssets($"t:{typeStr}", new[] { directory })
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(x => (T)AssetDatabase.LoadAssetAtPath(x, typeof(T)))
            .ToArray();
    }

    [Serializable]
    record struct RoomJson
    {
#pragma warning disable IDE1006 // Naming Styles
        // Use camel_case in json
        public string name;
        public int[] tiles;
#pragma warning restore IDE1006 // Naming Styles
        public Vector2Int Pos;
    }
}
