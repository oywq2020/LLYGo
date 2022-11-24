using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory",menuName = "Inventory/New Inventory")]
public class InventoryScrObj : ScriptableObject
{
   //bag list
   public List<AbstractItemScrObj> itemList = new List<AbstractItemScrObj>();

   public int maxCount = 13;
}
