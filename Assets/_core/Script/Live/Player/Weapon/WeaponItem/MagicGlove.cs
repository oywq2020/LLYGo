using System;
using Cysharp.Threading.Tasks;
using Script.Abstract;
using Script.Event;
using UnityEngine;

namespace Player
{
    public class MagicGlove : MonoBehaviour, IWeapon
    {
        public float Cd { get; } = 0.833f;

        private IGameObjectPool _objectPool;

        private GameObject _portalHolder;

        private bool _canAttack = true;

        private async UniTask Start()
        {
            await GameLoop.Instance.Setup();
            _objectPool = GameFacade.Instance.GetInstance<IGameObjectPool>();
        }

        public async void OnInit()
        {
            //await start
            await Start();

            //create a magic portal to follows
            _portalHolder = _objectPool.Dequeue("Portal", transform.parent.GetComponent<CharacterBodyMapper>().follows);
            _portalHolder.transform.localPosition = Vector3.zero;
        }

        public void ApproveAttack(Animator animator,Action duringAttack)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (_canAttack)
                {
                    _canAttack = false;
                    StartHit(animator).Forget();
                }
             
                duringAttack.Invoke();
            }
           
        }

        public void OnExit()
        {
            //unload effect 
            _objectPool.Enqueue(_portalHolder);
        }

        private async UniTask StartHit(Animator animator)
        {
           

            Debug.Log("Magic Impulse");
            
            //start attack
            GameFacade.Instance.SendEvent<OnStartAttack>();
            
            animator.SetTrigger("Attack");
            
            await UniTask.Delay(TimeSpan.FromSeconds(Cd * 3 / 4));

            //start attack
            GameFacade.Instance.SendEvent<OnEndAttack>();
            
            await UniTask.Delay(TimeSpan.FromSeconds(Cd * 1 / 4));
            _canAttack = true;

        }

        public void OnHit(int attackIndex)
        {
            var dequeue = _objectPool.Dequeue("LightningRotateBall");

            var characterBodyMapper = transform.parent.GetComponent<CharacterBodyMapper>();
            dequeue.transform.position = characterBodyMapper.firePoint.position;
            dequeue.transform.rotation = characterBodyMapper.firePoint.rotation;
        }

        public void EndAttack()
        {
        }
    }
}