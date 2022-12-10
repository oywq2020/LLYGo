using System;
using _core.Script.Live;
using UnityEngine;

namespace _core.Script.Enemy.BigMonster
{
    public class Golem : EnemyAI
    {
        [Header("Attack Recall")] [SerializeField]
        private Transform attackPoint;

        [SerializeField] private float attackRadius = 1;


        private void LeftHandHit()
        {
            var colliders = Physics.OverlapSphere(attackPoint.position, attackRadius);

            foreach (var collider1 in colliders)
            {
                if (collider1.CompareTag("Player"))
                {
                    collider1.GetComponent<IDamageable>().GetHit(attack1Damage);
                }
            }
        }

        private void RightHandHit()
        {
            var colliders = Physics.OverlapSphere(attackPoint.position, attackRadius);

            foreach (var collider1 in colliders)
            {
                if (collider1.CompareTag("Player"))
                {
                    collider1.GetComponent<IDamageable>().GetHit(attack1Damage);
                }
            }
        }

        void RotationHit()
        {
            var colliders = Physics.OverlapSphere(attackPoint.position, attackRadius);

            foreach (var collider1 in colliders)
            {
                if (collider1.CompareTag("Player"))
                {
                    collider1.GetComponent<IDamageable>().GetHit(attack2Damage);
                }
            }
        }



        private void OnDrawGizmosSelected()
        {
            //Draw the range of attack
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(attackPoint.position, attackRadius);
        }
    }
}