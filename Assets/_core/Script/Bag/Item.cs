using _core.Script.Bag.ScriptableObj.Item;
using PlayerRegion;
using Script.Event;
using UnityEngine;
using UnityEngine.AddressableAssets;


/// <summary>
/// as a reflection object for passing item data to bag
/// </summary>
public class Item : MonoBehaviour
{
    public AbstractItemScrObj  thisAbstractItemScrObj;

    private InventoryScrObj _currentPlayerBag;


    private void Start()
    {
        //get current player bag
        _currentPlayerBag = GameFacade.Instance.GetBag();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AddNewItem();
            Destroy(gameObject);
        }
    }

     void AddNewItem()
    {
        //add new item
        if (!_currentPlayerBag.itemList.Contains(thisAbstractItemScrObj))
        {
            //judge whether the bag is full
            if (_currentPlayerBag.itemList.Count>_currentPlayerBag.maxCount) return;

            //travel out empty slot in my bag list and put this item into it
            for (int i = 0; i < _currentPlayerBag.maxCount; i++)
            {
                if (! _currentPlayerBag.itemList[i])
                {
                    _currentPlayerBag.itemList[i] = thisAbstractItemScrObj;
                    break;
                }
            }
            
            //if this 
            if (!thisAbstractItemScrObj.isEquip)
            {
                ((NormalItem)thisAbstractItemScrObj).count++;
            }
            
        }
        else
        {
            if (!thisAbstractItemScrObj.isEquip)
            {
                ((NormalItem)thisAbstractItemScrObj).count++;
            }
            else
            {
                //the equip don't permit overlap,need fill another slot
                for (int i = 0; i < _currentPlayerBag.maxCount; i++)
                {
                    if (! _currentPlayerBag.itemList[i])
                    {
                        _currentPlayerBag.itemList[i] = thisAbstractItemScrObj;
                        break;
                    }
                }
            }

        }
        
        //send this event
        GameFacade.Instance.SendEvent<OnPlayerBagRefresh>();
    }
}
