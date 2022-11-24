using _core.Script.Bag.ScriptableObj.Item;
using PlayerRegion;
using Script.Event;
using UnityEngine;


/// <summary>
/// as a reflection object for passing item data to bag
/// </summary>
public class Item : MonoBehaviour
{
    public AbstractItemScrObj thisAbstractItemScrObj;

    private InventoryScrObj _currentPlayerBag;


    private void Start()
    {
        //get current player bag
        _currentPlayerBag = CurrentPlayer.Instance._bag;
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
            //judge whether out index
            if (_currentPlayerBag.itemList.Count>_currentPlayerBag.maxCount) return;
            
            _currentPlayerBag.itemList.Add(thisAbstractItemScrObj);

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

        }
        
        //send this event
        GameFacade.Instance.SendEvent<OnPlayerBagRefresh>();
    }
}
