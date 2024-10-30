using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using VVVVVV.Runtime.DIContainer;

namespace VVVVVV.Runtime.World;

public record struct RoomKey
(
    SpaceType Space,
    Vector2Int RoomPos
);

public class RoomLoader : IDisposable
{
    Dictionary<RoomKey, RoomJson> _roomDataDict = null!;
    Tile[] _tiles = null!;
    GameObject _pf_room = null!;

    public async UniTask LoadAsync()
    {
        var assetManager = ServiceRegistry.App.AssetManager;
        var roomDataTask = assetManager.LoadAssetsAtAsync<TextAsset>("Room", x => x.Contains("room_data/spacestation"));
        var tileTask = assetManager.LoadAssetsAsync<Tile>("Tile");
        var roomPrefabTask = assetManager.LoadAssetAsync<GameObject>("PF_RoomBase");

        var res = await UniTask.WhenAll(roomDataTask, tileTask, roomPrefabTask);

        _roomDataDict = res.Item1
            .Select(LoadJson)
            .ToDictionary(x => new RoomKey(SpaceType.SpaceStation, x.Pos));
        _tiles = res.Item2.OrderBy(x => int.Parse(x.name.Split("_")[^1])).ToArray();
        _pf_room = res.Item3;
    }

    RoomJson LoadJson(TextAsset text)
    {
        var filenameSplit = text.name.Split(",").Select(int.Parse).ToList();
        var json = JsonUtility.FromJson<RoomJson>(text.text);
        json.Pos = new Vector2Int(filenameSplit[0], filenameSplit[1]);
        return json;
    }

    public void LoadAllRooms()
    {
        // TODO: Handle this
        var allRooms = _roomDataDict.Keys.Select(Get).ToList();
    }

    public Room Get(RoomKey key)
    {
        var room = GameObject.Instantiate(_pf_room).GetComponent<Room>();
        room.Init(_roomDataDict[key], _tiles);
        return room;
    }

    public void Dispose()
    {
    }
}
