using System;
using System.Threading;
using _core.Script.Live;
using _core.Script.Utility.Extension;
using Cysharp.Threading.Tasks;
using Script.Event;
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

        private bool _canAttack = true;

        #endregion

        public void OnInit()
        {
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void ApproveAttack(Animator animator,Action duringAttack)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (_canAttack)
                {
                    StartHit(animator);
                }
                
                duringAttack.Invoke();
            }
        }


        private async void StartHit(Animator animator)
        {
            _canAttack = false;
            
            //start attack
            GameFacade.Instance.SendEvent<OnStartAttack>();
            
            animator.SetTrigger("Attack");
            await UniTask.Delay(TimeSpan.FromSeconds(cd*3/4));
            
            GameFacade.Instance.SendEvent<OnEndAttack>();

            _canAttack = true;
        }


        public void OnExit()
        {
            
            
        }
        public void OnHit(int attackIndex)
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

        public void EndAttack()
        {
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(damageTransform.position, damageRadius);
        }
        
        Vector3 CameraForward()
        {
            var x = this.transform.position.x - Camera.main.transform.position.x;
            var z = transform.position.z - Camera.main.transform.position.z;
            return new Vector3(x, 0, z);
        }

        Quaternion TurnTo(Vector3 cameraDir, float offset = 0)
        {
            Quaternion q = Quaternion.identity;
            q.SetLookRotation(cameraDir);
            return Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, q.eulerAngles.y + offset, 0),
                Time.deltaTime * 8);
        }
    }
}