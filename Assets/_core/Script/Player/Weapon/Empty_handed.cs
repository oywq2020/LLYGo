using System.Threading;
using UnityEngine;

namespace Player
{
    public class Empty_handed : MonoBehaviour,IWeapon
    {
        public float cd;
        
        public float Cd
        {
            get =>cd;
        }

        public void Play()
        {
            //card frame
            Thread.Sleep(50);
        }
    }
}