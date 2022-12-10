using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PlayerRegion
{
    //abstract from Player
    //non-agent in .net framework
    public class CurrentPlayer
    {
        private InventoryScrObj _bag;

        private string _account;

        //judge a wonderful 

      //todo for final complete 
        public CurrentPlayer(string account)
        {
            //get the current acount
            _account = account;
            Debug.Log("Load BaG");
            //load the bag belonged the account
            _bag =  Resources.Load<InventoryScrObj>("Bag/Inventory/Bag0");
        }


        public void UpdateAccount(string account)
        {
            _account = account;
        }
        public string GetAccount()
        {
            return _account;
        }

        public InventoryScrObj GetBag()
        {
            return _bag;
        }

        public void Dispose()
        {
            Debug.Log("PlayerDisposable");
        }

        ~CurrentPlayer()
        {
            Debug.Log("PlayerDestroy");
        }
    }
}