using System.Collections;
using System.Collections.Generic;
using _core.Script.Bag.ScriptableObj.Item;
using PlayerRegion;
using UnityEngine;
using UnityEngine.UI;

public class ShortBagMgr : MonoBehaviour
{
    public GameObject itemPrototype;

    private InventoryScrObj _currentPlayerBag;
    
    [SerializeField]
    private int itemListOffset = 0;
    private void Start()
    {
        //get current player's bag
        _currentPlayerBag = CurrentPlayer.Instance._bag;
        
        //Refresh bag's inventory
        RefreshInventory();
    }

    private void RefreshInventory()
    {
        for (int i = itemListOffset; i < _currentPlayerBag.itemList.Count; i++)
        {
            var item = _currentPlayerBag.itemList[i];
            
            //init
            var itemObj = Instantiate(itemPrototype, transform.GetChild(i-itemListOffset).transform);
            itemObj.GetComponent<Image>().sprite = item.itemImage.LoadAssetAsync().WaitForCompletion();

            if (!item.isEquip)
            {
                itemObj.transform.GetChild(0).GetComponent<Text>().text = ((NormalItem)item).count.ToString();
            }
        }
    }
}
