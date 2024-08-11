using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Linq;
using System;
using VVVVVV.Utils.Extension;

namespace VVVVVV.Editor.Tools;

public class ConstructRoom : EditorWindow
{
    static readonly string TileDataDir = "Assets/_ART/Tiles1/";
    static readonly string RoomJsonDir = "Assets/Data/room/spacestation";
    static readonly string RoomPrefabExportDir = "Assets/Prefabs/Rooms/SpaceStation";
    static readonly string RoomPrefabBasePath = "Assets/Prefabs/PF_RoomBase.prefab";
    static readonly Vector2Int RoomSize = new(40, 30);

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
        prefab.name = $"{json.Pos.x},{json.Pos.y}-{json.name}";

        var tilemap = prefab.GetComponentInChildren<Tilemap>();
        for (int i = 0; i < RoomSize.x; i++)
        {
            for (int j = 0; j < RoomSize.y; j++)
            {
                var tileIdx = json.tiles[j * RoomSize.x + i];
                tilemap.SetTile(new(i, (RoomSize.y - 1) - j), _tiles[tileIdx]);
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
