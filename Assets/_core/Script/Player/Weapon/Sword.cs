using UnityEngine;

namespace Player
{
    public class Sword:MonoBehaviour,IWeapon
    {
        public float cd;
        
        public float Cd
        {
            get =>cd;
        }
        public void Play()
        {
            
        }
    }
}