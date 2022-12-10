using System;
using Script.Abstract;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _core.Script.Effect
{

    [RequireComponent(typeof(SphereCollider))]

    public class BallController : MonoBehaviour
    {
        public float speed = 30;

        public float duration = 5;

        public AssetReferenceGameObject _explodePref;

        private float timer = 0;
        
        private void Update()
        {
            transform.Translate(new Vector3(0, 0, 1) * speed*Time.deltaTime);

            timer += Time.deltaTime;
            
            if (timer>duration)
            {
                //instantiate GameObject
                var dequeue = GameFacade.Instance.GetInstance<IGameObjectPool>().Dequeue(_explodePref.RuntimeKey.ToString());
                dequeue.transform.position = transform.position;
                
                GameFacade.Instance.GetInstance<IGameObjectPool>().Enqueue(gameObject);
                //reset timer
                timer = 0;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                Debug.Log("Explode!!!");
                
                //instantiate explode prefab
                
                var dequeue = GameFacade.Instance.GetInstance<IGameObjectPool>().Dequeue(_explodePref.RuntimeKey.ToString());
                dequeue.transform.position = transform.position;
                
                //enqueue this ball
                GameFacade.Instance.GetInstance<IGameObjectPool>().Enqueue(gameObject);
                //reset timer
                timer = 0;
            }
          
        }
        
        
    }
}