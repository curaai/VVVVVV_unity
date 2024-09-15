using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace VVVVVV.Utils.Asset;

public class AddressableAssetManager : IDisposable
{
    readonly List<AsyncOperationHandle> _handles = new();
    readonly Dictionary<string, UnityEngine.Object> _loaded = new();

    public async UniTask<T?> LoadAssetAsync<T>(string key) where T : UnityEngine.Object
    {
        if (_loaded.ContainsKey(key))
            return _loaded[key] as T;

        var handle = Addressables.LoadAssetAsync<T>(key);
        await handle.Task;
        if (!handle.IsDone || !handle.IsValid() || handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"[AssetResource] Load Error : {key}, {handle.IsDone}, {handle.IsValid()}, {handle.Status}");
            Addressables.Release(handle);
            return null;
        }
        else
        {
            if (!_loaded.ContainsKey(key))
            {
                _loaded.Add(key, handle.Result);
            }

            _handles.Add(handle);
            return handle.Result;
        }
    }

    public void Dispose()
    {
        _handles.ForEach(Addressables.Release);
        _handles.Clear();

        _loaded.Clear();
    }
}

