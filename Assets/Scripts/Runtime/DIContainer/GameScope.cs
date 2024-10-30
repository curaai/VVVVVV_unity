using Cysharp.Threading.Tasks;
using UnityEngine;
using VVVVVV.Runtime.World;

namespace VVVVVV.Runtime.DIContainer;

public class GameScope : IScope
{
    public WorldManager WorldManager => _worldManager;

    WorldManager _worldManager = null!;

    public async UniTask SetupAsync()
    {
        _worldManager = new();
        await _worldManager.SetupAsync();
    }

    public void TearUp()
    {
        _worldManager.Test_LoadAll();
    }

    public void TearDown()
    {
    }
}
