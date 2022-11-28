using _core.Script.Live;
using UnityEngine;
using UnityEngine.UI;

namespace _core.Script.Enemy
{
    public class EnemyPrototype :LiveEntity
    {

        private BloodBarController _bloodController;
        
        protected override void Init()
        {
            _bloodController =   transform.Find("BloodBarCanvas").GetComponent<BloodBarController>();
        }
        
        protected override void OnGetHit()
        {
            //update bloodBar
            _bloodController.UpdateBloodBar(currentHp / wholeHp);

           
        }

     

        protected override void OnDead()
        {
            
        }

      
    }
}