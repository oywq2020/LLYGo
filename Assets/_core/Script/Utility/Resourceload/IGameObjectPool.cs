using UnityEngine;

namespace Script.Abstract
{
    //implemented by those that need governed by Object Pool
    public interface IGameObjectPool
    {
         void Enqueue(GameObject obj);
         GameObject Dequeue(string key);
         void ReleaseAll();
    }
}