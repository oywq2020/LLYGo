using _core.Script.Live;
using Script.Abstract;
using UnityEngine;

namespace Script.AssetFactory
{
    /// <summary>
    /// this class is for default poolizing GameObject,a default option when you Dequeue a GameObject
    /// without an component implemented IPoolable interface
    /// </summary>
    public class PoolizeGBDefault : MonoBehaviour, IPoolable
    {
        public virtual void Disable()
        {
           
           gameObject.SetActive(false);
        }

        public virtual void Init()
        {
            gameObject.SetActive(true);
        }
    }
}