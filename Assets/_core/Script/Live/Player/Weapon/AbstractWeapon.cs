using UnityEngine;

namespace Player
{
    
    /// <summary>
    /// this abstract is for completing  some default operation  for truly weapon
    /// </summary>
    public abstract class AbstractWeapon : MonoBehaviour,IWeapon
    {
        public float Cd { get; }
        public void Init()
        {
           
        }

        public void ApproveAttack()
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
           
        }

        public void EndHit()
        {
            
        }
    }
}