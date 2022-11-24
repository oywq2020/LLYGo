
using UnityEngine;

namespace PlayerRegion
{
    //abstract from Player
    public class CurrentPlayer : MonoBehaviour
    {
        public static CurrentPlayer Instance;

        public InventoryScrObj _bag;
        private void Awake()
        {
            Instance = this;
        }
    }
}