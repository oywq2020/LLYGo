using System;
using System.Collections.Generic;
using _core.Script.Bag.ScriptableObj.Item;
using PlayerRegion;
using Script.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _core.Script.Bag
{
    public class ItemOnBag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Transform _originalParent;
        private List<AbstractItemScrObj> _bagItemList;

        public void OnBeginDrag(PointerEventData eventData)
        {
            //preserve original parent
            _originalParent = transform.parent;

            transform.SetParent(transform.parent.parent.parent);
            transform.position = eventData.position;

            //cancel its ray block
            GetComponent<CanvasGroup>().blocksRaycasts = false;

            //get current inventory for this bag
            _bagItemList = CurrentPlayer.Instance._bag.itemList;
            
           
        }

        public void OnDrag(PointerEventData eventData)
        {
            GameFacade.Instance.SendEvent<OnMouseEntryGUI>();
            transform.position = eventData.position;
         
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject &&
                eventData.pointerCurrentRaycast.gameObject.CompareTag("Slot"))
            {
                //if the end drag position is on slot
                if (eventData.pointerCurrentRaycast.gameObject.transform.childCount == 0)
                {
                    //this slot is null
                    transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);

                    //exchange this inventory list
                    var index = eventData.pointerCurrentRaycast.gameObject.GetComponent<Index>().GetIndex();
                    _bagItemList[index] = _bagItemList[_originalParent.GetComponent<Index>().GetIndex()];
                    _bagItemList[_originalParent.GetComponent<Index>().GetIndex()] = null;
                }
            }
            else if (eventData.pointerCurrentRaycast.gameObject &&
                     eventData.pointerCurrentRaycast.gameObject.CompareTag("Item"))
            {
                //if this slot already has item
                //get index of current slot
                var index = eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<Index>()
                    .GetIndex();

                if (!_bagItemList[index].isEquip &&
                    !_bagItemList[_originalParent.GetComponent<Index>().GetIndex()].isEquip &&
                    _bagItemList[index].itemName ==
                    _bagItemList[_originalParent.GetComponent<Index>().GetIndex()].itemName)
                {
                    //if both are normal item merge them
                    var temp = _bagItemList[_originalParent.GetComponent<Index>().GetIndex()];
                    ((NormalItem) _bagItemList[index]).count += ((NormalItem) temp).count;
                    _bagItemList[_originalParent.GetComponent<Index>().GetIndex()] = null;
                    //destroy self
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("this non-normal item");
                    //exchange both position
                    transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent);
                    eventData.pointerCurrentRaycast.gameObject.transform.SetParent(_originalParent);
                    
                    //exchange this inventory list
                    var temp = _bagItemList[index];
                    _bagItemList[index] = _bagItemList[_originalParent.GetComponent<Index>().GetIndex()];
                    _bagItemList[_originalParent.GetComponent<Index>().GetIndex()] = temp;
                }
            }
            else
            {
                transform.SetParent(_originalParent);
            }

            //reset its ray block
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            
            //refresh bag
            GameFacade.Instance.SendEvent<OnPlayerBagRefresh>();
        }
    }
}