using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item",menuName = "Inventory/New Item")]
public class AbstractItemScrObj : ScriptableObject
{
   public string itemName;
   public Sprite itemImage;
   
   //divide items into two categories
   public bool  isEquip;
   [TextArea]
   public string info;
}
