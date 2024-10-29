using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

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

    public UniTask<IList<T>?> LoadAssetsAsync<T>(string key) where T : UnityEngine.Object
    {
        var handle = Addressables.LoadAssetsAsync<T>(key, null);
        return LoadAssetsAsync(handle);
    }

    async UniTask<IList<T>?> LoadAssetsAsync<T>(AsyncOperationHandle<IList<T>> handle) where T : UnityEngine.Object
    {
        await handle.Task;
        if (!handle.IsDone || !handle.IsValid() || handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"[AssetResource] Load Error : {handle.IsDone}, {handle.IsValid()}, {handle.Status}");
            Addressables.Release(handle);
            return null;
        }
        else
        {
            foreach (var obj in handle.Result)
            {
                var key = obj.name;
                if (!_loaded.ContainsKey(key))
                {
                    _loaded.Add(key, obj);
                }
            }
            _handles.Add(handle);
            return handle.Result;
        }
    }

    public async UniTask<IList<T>?> LoadAssetsAtAsync<T>(string label, Func<string, bool> patternMatch = null!) where T : UnityEngine.Object
    {
        var findTask = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        await findTask.Task;
        if (findTask.Status != AsyncOperationStatus.Succeeded)
        {
            Addressables.Release(findTask);
            return null;
        }

        IList<IResourceLocation> keys = findTask.Result.Where(x => patternMatch?.Invoke(x.PrimaryKey) ?? true).ToList();
        var _keys = keys.ToList();
        var handle = Addressables.LoadAssetsAsync<T>(keys, callback: null);
        Addressables.Release(findTask);
        return await handle;
    }

    public void Dispose()
    {
        _handles.ForEach(Addressables.Release);
        _handles.Clear();
        _loaded.Clear();
    }
}

