using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VVVVVV.Utils.Asset;

namespace VVVVVV.Runtime.DIContainer;

public class AppScope : IDisposable, IScope
{
    public readonly AddressableAssetManager AssetManager = new();

    public UniTask SetupAsync()
    {
        return UniTask.CompletedTask;
    }

    public void TearUp()
    {
    }

    public void TearDown()
    {
        Dispose();
    }

    public void Dispose()
    {
        AssetManager.Dispose();
    }
}
