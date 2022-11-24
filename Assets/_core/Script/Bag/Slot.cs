using System;
using UnityEngine;
using UnityEngine.UI;

namespace _core.Script.Bag
{
    public class Slot : MonoBehaviour
    {
        public AbstractItemScrObj slotAbstractItemScrObj;

        private Image _slotImage;

        private Text _slotNum;

        private void Start()
        {
            _slotImage = GetComponent<Image>();
            _slotNum = GetComponentInChildren<Text>();
        }
    }
}