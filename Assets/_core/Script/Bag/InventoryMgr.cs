using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMgr : MonoBehaviour
{
    private static InventoryMgr Instance;

    public InventoryScrObj myBag;

    public GameObject slotGrid;
    
    
    
    private void Awake()
    {
        Instance = this;
    }
}
