using System;
using UnityEngine;
using VVVVVV.Utils.Asset;

namespace VVVVVV.Runtime.DIContainer;

public class AppScope : IDisposable
{
    public readonly AddressableAssetManager AssetManager = new();

    public void Dispose()
    {
        AssetManager.Dispose();
    }
}
