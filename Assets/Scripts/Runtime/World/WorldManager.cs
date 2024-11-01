using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace VVVVVV.Runtime.World;

public class WorldManager : IDisposable
{
    public List<Room> Rooms = new();
    RoomLoader _roomLoader = new();

    public async UniTask SetupAsync()
    {
        await _roomLoader.SetupAsync();
    }

    public Room LoadRoom(RoomKey roomKey)
    {
        var room = _roomLoader.Get(roomKey);
        Rooms.Add(room);
        return room;
    }

    public void Test_LoadAll()
    {
        _roomLoader.LoadAllRooms();
    }

    public void Dispose()
    {
        Rooms.ForEach(UnityEngine.Object.Destroy);
        Rooms.Clear();
    }
}
