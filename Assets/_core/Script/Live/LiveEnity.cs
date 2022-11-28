using System;
using Script.UI;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace _core.Script.Live
{
    public abstract class LiveEntity:MonoBehaviour,IDamageable
    {
        [SerializeField]
        protected float wholeHp;

        [SerializeField]
        protected float currentHp;

        private bool _isDead = false;

        private void Start()
        {
            currentHp = wholeHp;
            Init();
        }


        public virtual void GetHit(float damage)
        {
            currentHp -= damage;
            OnGetHit();
            
            if (currentHp<=0)
            {
                currentHp = 0;

                _isDead = true;
                
                //call OnDead
                OnDead();
            }
        }


        protected abstract void Init();
        protected abstract void OnDead();

        protected abstract void OnGetHit();
    }
}