using UnityEngine;

namespace Player
{
    public class MagicGlove:MonoBehaviour,IWeapon
    {
        public float Cd { get; }
        public void Init()
        {
            Debug.Log("Init Magic Glove");
        }

        public void Exit()
        {
            Debug.Log("Exit Magic Glove");
        }

        public void StartHit()
        {
           
        }

        public void Hit()
        {
            Debug.Log("Magic impulse");
        }

        public void EndHit()
        {
            
        }
    }
}