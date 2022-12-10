using System;
using _core.Script.Live;
using Cysharp.Threading.Tasks;
using Script.Abstract;
using UnityEngine;

namespace _core.Script.Effect
{
    public class BallExplodeController : MonoBehaviour,IPoolable
    {
        [SerializeField] private float _damageScope = 16;
        [SerializeField] private float _explodeSpeed = 25;
        [SerializeField] private int _damage = 100;
        
        private SphereCollider _sphereCollider;
        private void Start()
        {
            _sphereCollider = GetComponent<SphereCollider>();
        }

        

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _damageScope);
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("Hit");
            
            if (other.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Hit enemy");
                other.gameObject.GetComponent<IDamageable>().GetHit(_damage);
            }
        }
        
        async void Explosion()
        {
            while (true)
            {
                await UniTask.Yield();
                if (_sphereCollider.radius < _damageScope)
                {
                    _sphereCollider.radius += Time.deltaTime * _explodeSpeed;
                }
                else
                {
                    _sphereCollider.enabled = false;

                    //awaiting some seconds for displaying this effect of explosion
                    await UniTask.Delay(TimeSpan.FromSeconds(2f));
                    //enqueue this explode
                    Debug.Log("EnqueueExplan");
                    GameFacade.Instance.GetInstance<IGameObjectPool>().Enqueue(gameObject);
                    break;
                }
            }
        }

        public void Disable()
        {
           gameObject.SetActive(false);
        }

        public void Init()
        {
            gameObject.SetActive(true);
            Explosion();
         
        }
    }
}