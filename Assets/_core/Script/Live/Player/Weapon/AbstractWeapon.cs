using System;
using UnityEngine;

namespace Player
{
    
    /// <summary>
    /// this abstract is for completing  some default operation  for truly weapon
    /// </summary>
    public abstract class AbstractWeapon : MonoBehaviour,IWeapon
    {
        public float Cd { get; }
        public void OnInit()
        {
           
        }

        public void ApproveAttack(Animator animator,Action duringAttack)
        {
            
        }

        public void OnExit()
        {
          
        }

        public void StartHit()
        {
           
        }

        public void OnHit()
        {
           
        }

        public void EndAttack()
        {
            
        }
    }
}