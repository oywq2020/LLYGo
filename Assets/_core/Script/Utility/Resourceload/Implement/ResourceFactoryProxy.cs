
using System.Collections.Generic;
using Script.Abstract;
using UnityEngine;


namespace Script.AssetFactory
{
    public class ResourceFactoryProxy:IGameObjectPool //:IAssetFactory
    {
        private IAssetFactory _resourceFactory;

        private Dictionary<string, Queue<GameObject>> _disableObjects = new Dictionary<string, Queue<GameObject>>();

        private List<string> _loadedAssetKeys = new List<string>();
        public ResourceFactoryProxy(IAssetFactory resourceFactory)
        {
            _resourceFactory = resourceFactory;
        }

        public void Enqueue(GameObject gameObject)
        {
            //Disable this object
            gameObject.GetComponent<IPoolable>().Disable();

            //find the corresponding key in this dictionary for queue
            if (_disableObjects.TryGetValue(gameObject.name,
                out Queue<GameObject> queue))
            {
                queue.Enqueue(gameObject);
            }
            else
            {
                Queue<GameObject> newQueue = new Queue<GameObject>();
                newQueue.Enqueue(gameObject);
                //trim the redundant "(Clone)" for GameObject
                _disableObjects.Add(gameObject.name, newQueue);
            }
            
        }
        
        public GameObject Dequeue(string key,Transform parent = null)
        {
            GameObject gameObject;
            if (_disableObjects.TryGetValue(key, out Queue<GameObject> queue))
            {
                if (queue.Count > 0)
                {
                    //dequeue from GameObject queue
                    gameObject = queue.Dequeue();
                }
                else
                {
                    //if the gameobjects in GameObject Pool are exhausted, load it
                    //note: you do not need put it into pool cause the pool is for disabled GameObject
                    gameObject = _resourceFactory.InstantiateGameObject(key,parent);
                    
                    //record this asset key
                    _loadedAssetKeys.Add(key);
                }
            }
            else
            {
                //if not found, load it
                //note: you do not need put it into pool cause the pool is for disabled GameObject
                gameObject = _resourceFactory.InstantiateGameObject(key,parent);
                
                //record this asset key
                _loadedAssetKeys.Add(key);
            }
            
            //naming this game object
            gameObject.name = key;
            
            //initiate this gameObject 
            gameObject.GetComponent<IPoolable>().Init();
            return gameObject;
        }

        //clean all of GameObject Pool and cache of reference
        public void ReleaseAll()
        {
            foreach (var keyValuePair in _disableObjects)
            {
                while (keyValuePair.Value.Count != 0)
                {
                    GameObject.Destroy(keyValuePair.Value.Dequeue());
                }
                _resourceFactory.Release(keyValuePair.Key);
                
                _loadedAssetKeys.Remove(keyValuePair.Key);
            }

            //to confirm all loaded asset references are released
            foreach (var VARIABLE in _loadedAssetKeys)
            {
                _resourceFactory.Release(VARIABLE);
            }
        }

        void PrintDictionary()
        {
            foreach (var keyValuePair in _disableObjects)
            {
                Debug.Log(keyValuePair.Key);
                foreach (var gameObject in keyValuePair.Value)
                {
                    Debug.Log(gameObject);
                }
            }
        }
    }
}