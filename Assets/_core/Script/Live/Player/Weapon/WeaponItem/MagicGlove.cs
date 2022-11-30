using System;
using Cysharp.Threading.Tasks;
using Script.Abstract;
using UnityEngine;

namespace Player
{
    public class MagicGlove : MonoBehaviour, IWeapon
    {
        public float Cd { get; }

        private IGameObjectPool _objectPool;

        private GameObject _portalHolder;

        private async UniTask Start()
        {
            await GameLoop.Instance.Setup();
            _objectPool = GameFacade.Instance.GetInstance<IGameObjectPool>();
        }

        public async void Init()
        {
            //await start
            await Start();

            //create a magic portal to follows
            _portalHolder = _objectPool.Dequeue("Portal", transform.parent.GetComponent<CharacterBodyMapper>().follows);
            _portalHolder.transform.localPosition = Vector3.zero;
        }

        public void ApproveAttack()
        {
            
        }

        public void Exit()
        {
            //unload effect 
            _objectPool.Enqueue(_portalHolder);
        }

        public void StartHit()
        {
        }

        public void Hit()
        {
            var dequeue = _objectPool.Dequeue("LightningRotateBall");

            var characterBodyMapper = transform.parent.GetComponent<CharacterBodyMapper>();
            dequeue.transform.position = characterBodyMapper.firePoint.position;
            dequeue.transform.rotation = characterBodyMapper.firePoint.rotation;
        }

        public void EndHit()
        {
        }
    }
}