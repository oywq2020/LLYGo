using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _core.Script.Bag.ScriptableObj.Item
{
    
    [CreateAssetMenu(fileName = "New Weapon",menuName = "Inventory/New Weapon")]
    public class WeaponItem:AbstractItemScrObj
    {
        //for address relative resource for this weapon

        public AssetReferenceGameObject swordGameObjectRf;

        public AssetReference aniCtrl;
        
        //  sprite of  Skills of weapon, there only define one due to limited time
        public AssetReferenceSprite skillSprite;


    }
}