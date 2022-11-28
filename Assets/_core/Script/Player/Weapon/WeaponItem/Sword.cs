using System;
using System.Collections.Generic;
using _core.Script.Live;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Player
{
    public class Sword : MonoBehaviour, IWeapon
    {
        public float cd;

        public float Cd
        {
            get => cd;
        }

        public float damage = 50;

        public Transform swordHead;
        public Transform swordTail;

        private bool _startDamage = false;

        private List<GameObject> _hitGBList = new List<GameObject>();


        public async void StartHit()
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(10));
            _startDamage = true;
        }

        public void Hit()
        {
        }


        private void FixedUpdate()
        {
            if (!_startDamage) return;
            if (Physics.Linecast(swordHead.position, swordTail.position, out RaycastHit raycastHit))
            {
                Debug.Log("Check GameObject");
                var colliderGameObject = raycastHit.collider.gameObject;
                if (!_hitGBList.Contains(colliderGameObject) &&
                    colliderGameObject.CompareTag("Enemy"))
                {
                    //hit without repeat 
                    colliderGameObject.GetComponent<IDamageable>().GetHit(damage);
                    _hitGBList.Add(colliderGameObject);
                }
            }
        }


        public void EndHit()
        {
            _startDamage = false;

            //clear list of gameobject
            _hitGBList.Clear();
        }


        public void Init()
        {
        }

        public void Exit()
        {
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(swordHead.position, swordTail.position);
        }
    }
}