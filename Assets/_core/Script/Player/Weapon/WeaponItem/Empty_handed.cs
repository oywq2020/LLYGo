using System.Threading;
using _core.Script.Live;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    public class Empty_handed : MonoBehaviour, IWeapon
    {
        #region WeaponAttribute

        public float cd;

        public float damage;
        public Transform damageTransform;
        public float damageRadius;

        public float Cd
        {
            get => cd;
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
            var overlapSphere = Physics.OverlapSphere(damageTransform.position, damageRadius);
            foreach (var collider1 in overlapSphere)
            {
                if (collider1.gameObject.CompareTag("Enemy"))
                {
                    //cause damage
                    collider1.gameObject.GetComponent<IDamageable>().GetHit(damage);
                }
            }
        }

        public void EndHit()
        {
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(damageTransform.position, damageRadius);
        }
    }
}