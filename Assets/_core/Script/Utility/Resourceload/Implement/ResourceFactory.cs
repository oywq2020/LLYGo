using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

public class ResourceFactory:IAssetFactory
{

    private readonly Dictionary<string,  Object> _assetCache = new Dictionary<string, Object>();
    
    //commonly loading
    public T LoadAsset<T>(string key) where T : class
    {
        if (_assetCache.TryGetValue(key, out Object obj))
        {
            return obj as T;
        }
        else
        {
            var waitForCompletion = Addressables.LoadAssetAsync<T>(key).WaitForCompletion();
            //add to cache
            _assetCache.Add(key, waitForCompletion as Object);

            return waitForCompletion;
        }

    }
    
    //Load and instantiate GameObject
    public GameObject InstantiateGameObject(string key,Transform parent = null)
    {
        if (_assetCache.TryGetValue(key,out Object obj))
        {
            var gameObject = GameObject.Instantiate(obj as GameObject,parent);
            gameObject.name = key;
            return gameObject;
        }
        else
        {
            var waitForCompletion = Addressables.LoadAssetAsync<GameObject>(key).WaitForCompletion();
            _assetCache.Add(key,waitForCompletion);
            return GameObject.Instantiate(waitForCompletion,parent);
        }
    }

    //separate release
    public void Release(string key)
    {
        if (_assetCache.TryGetValue(key,out Object obj))
        {
            Addressables.Release(obj);
            _assetCache.Remove(key);
        }
    }

    //clean all cache
    public void ReleaseAll()
    {
        //release by addressables
        foreach (var VARIABLE in _assetCache)
        {
            Addressables.Release(VARIABLE.Value);
        }
        
        //clear cache
        _assetCache.Clear();
    }
    
 


}