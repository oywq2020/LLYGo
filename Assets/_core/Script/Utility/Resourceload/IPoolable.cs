using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Script.Abstract
{
    public interface IPoolable
    {
        void Disable();
        void Init();
        
    }
}