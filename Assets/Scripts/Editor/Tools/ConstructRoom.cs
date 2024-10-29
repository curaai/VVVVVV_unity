using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Linq;
using System;
using VVVVVV.Runtime.World;
using VVVVVV.Utils.Extension;
using VVVVVV.Runtime;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using System.IO;

namespace VVVVVV.Editor.Tools;

public class ConstructRoom : EditorWindow
{
    static readonly string TileDataDir = "Assets/_ART/Tiles1/";
    static readonly string RoomJsonDir = "Assets/Data/room/spacestation";
    static readonly string RoomPrefabExportDir = "Assets/Prefabs/Rooms/SpaceStation";
    static readonly string RoomPrefabBasePath = "Assets/Prefabs/PF_RoomBase.prefab";

    static AddressableAssetSettings AddrSettings => AddressableAssetSettingsDefaultObject.Settings;
    static AddressableAssetGroup RoomGroup => AddrSettings.FindGroup("rooms");

    static Tile[] _tiles = null!;

    [MenuItem("VVVVVV/ConstructRoom")]
    static void ShowWindow()
    {
        _tiles = LoadAssets<Tile>("Tile", TileDataDir);

        CleanDirectory();

        LoadJsons().ForEach(CreateRoomPrefab);

        AddrSettings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, RoomGroup, true);
        AssetDatabase.SaveAssets();
    }

    static void CleanDirectory()
    {
        if (Directory.Exists(RoomPrefabExportDir)) { Directory.Delete(RoomPrefabExportDir, true); }
        Directory.CreateDirectory(RoomPrefabExportDir);
    }

    static void CreateRoomPrefab(RoomJson json)
    {
        var prefabSrc = AssetDatabase.LoadAssetAtPath<GameObject>(RoomPrefabBasePath);
        var prefab = (GameObject)PrefabUtility.InstantiatePrefab(prefabSrc);

        prefab.name = $"{json.Pos.x},{json.Pos.y}";
        var exportPath = RoomPrefabExportDir + $"/{prefab.name}.prefab";

        var room = prefab.GetComponent<Room>();
        room.RoomPos = new Vector2Int(json.Pos.x, json.Pos.y);
        room.RoomName = json.name;

        ConstructRoom(prefab, json);

        PrefabUtility.SaveAsPrefabAsset(prefab, exportPath);
        RegisterAddressable(json, exportPath);
        DestroyImmediate(prefab);

        static void ConstructRoom(GameObject prefab, RoomJson json)
        {
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
        }
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

    static void RegisterAddressable(RoomJson json, string assetPath)
    {
        var guid = AssetDatabase.AssetPathToGUID(assetPath);
        var spaceName = "spacestation";

        var entry = AddrSettings.CreateOrMoveEntry(guid, RoomGroup, readOnly: true, postEvent: true);
        entry.address = $"{spaceName}-{json.Pos.x},{json.Pos.y}";
        entry.SetLabel("Room", true);
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
