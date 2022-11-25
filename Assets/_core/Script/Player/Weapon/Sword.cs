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

        public void Init()
        {
            
        }

        public void Exit()
        {
           
        }

        public void StartHit()
        {
            
        }

        public void Hit()
        {
            Debug.Log("Sword Hit");
        }

        public void EndHit()
        {
           
        }
    }
}