using UnityEngine;

namespace Script.UI
{
    public interface IUIkit
    {
        GameObject OpenPanel(string key,UILayer layer = UILayer.Common,bool canDuplicate = false);
        void ClosePanel(GameObject obj);
        void ClosePanel(string key);
    }
}