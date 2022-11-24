using UnityEngine;

namespace Script.UI
{
    public class LayerAdaptor
    {
        //take 
        public static Transform GetTransform(UILayer layer)
        {
           return GameObject.Find($"UIRoot/{layer.ToString()}").transform;
        }
    }
}