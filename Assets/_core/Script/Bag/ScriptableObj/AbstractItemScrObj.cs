using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class AbstractItemScrObj : ScriptableObject
{
   public string itemName;
   public AssetReferenceSprite itemImage;
   
   //divide items into two categories
   public bool  isEquip;
   [TextArea]
   public string info;
}
