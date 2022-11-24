using Script.Abstract;
using UnityEngine;

namespace Script.UI
{
    public interface IUIPanel : IPoolable
    {
        bool isOnOpen { get;  set; }

        void OnOpen();
       // void SetUILayer(UILayer layer);
    }
}