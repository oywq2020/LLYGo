using System.Threading;
using UnityEngine;

namespace Player
{
    public class Empty_handed : MonoBehaviour,IWeapon
    {
        #region WeaponAttribute

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

        #endregion
        
        
        
        
        
        public void StartHit()
        {
            //card frame
            Thread.Sleep(50);
        }

        public void Hit()
        {
            Debug.Log("Handed Hit");
        }

        public void EndHit()
        {
            
        }
    }
}