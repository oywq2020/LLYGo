using System;
using System.Collections;
using System.Collections.Generic;
using _core.Script.Bag.ScriptableObj.Item;
using PlayerRegion;
using Script.Event;
using Script.Facade;
using UnityEngine;
using UnityEngine.UI;

public class BagMgr : MonoBehaviour
{
    public GameObject itemPrototype;

    private InventoryScrObj _currentPlayerBag;

    //list range
    [SerializeField] private int startLimit = 4;

    [SerializeField] private int endLimit = 12;

    private void Start()
    {
        //get current player's bag
        _currentPlayerBag = CurrentPlayer.Instance._bag;

        //register event of bag refresh
        GameFacade.Instance.RegisterEvent<OnPlayerBagRefresh>(PlayerBagRefreshEvent);

        //Refresh bag's inventory
        RefreshInventory();
    }

    
    
    
    private void RefreshInventory()
    {
        for (int i = startLimit; i < _currentPlayerBag.itemList.Count; i++)
        {
            //prevent index out of this bag grid and itemList is null
            if (i > endLimit || !_currentPlayerBag.itemList[i]) continue;
         
            var item = _currentPlayerBag.itemList[i];
            Debug.Log("Item"+item);

            if (transform.GetChild(i - startLimit).transform.childCount != 0)
            {
                if (!item.isEquip)
                {
                    //update item count
                    transform.GetChild(i - startLimit).transform.GetChild(0).GetChild(0)
                        .GetComponent<Text>().text = ((NormalItem)item).count.ToString();
                }
               
            }
            else
            {
                //create item prototype
                var itemObj = Instantiate(itemPrototype, transform.GetChild(i - startLimit).transform);
                itemObj.GetComponent<Image>().sprite = item.itemImage.LoadAssetAsync().WaitForCompletion();

                if (!item.isEquip)
                {
                    itemObj.transform.GetChild(0).GetComponent<Text>().text = ((NormalItem)item).count.ToString();
                }
                else
                {
                    itemObj.transform.GetChild(0).GetComponent<Text>().text = "";
                }
              
            }
        }
    }

    private void PlayerBagRefreshEvent(OnPlayerBagRefresh e)
    {
        RefreshInventory();
    }


    private void OnDestroy()
    {
        //unregister event of bag refresh
        GameFacade.Instance?.UnRegisterEvent<OnPlayerBagRefresh>(PlayerBagRefreshEvent);
    }
}