using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PlayerRegion
{
    //abstract from Player
    public class CurrentPlayer : MonoBehaviour
    {
        public static CurrentPlayer Instance;

        public InventoryScrObj _bag;

        private string _account;

        //judge a wonderful 

      //todo for final complete 
        // public CurrentPlayer(string account)
        // {
        //     _account = account;
        //     
        //     //init bag
        //     _bag =  Addressables.LoadAssetAsync<InventoryScrObj>($"Bag[{0}]").WaitForCompletion();
        //
        //     //assign this to Instance
        //     Instance = this;
        //
        // }

        public InventoryScrObj GetBag()
        {
            return _bag;
        }
        
        
        private void Awake()
        {
            Instance = this;
        }

        public string GetCurrentAccount()
        {
            return _account;
        }
    }
}