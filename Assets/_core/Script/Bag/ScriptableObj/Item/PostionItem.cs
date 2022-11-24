using UnityEngine;

namespace _core.Script.Bag.ScriptableObj.Item
{
    [CreateAssetMenu(fileName = "New Potion",menuName = "Inventory/New Potion")]
    public class NormalItem : AbstractItemScrObj
    {
        public string imageKey;
        public int count;
    }
}