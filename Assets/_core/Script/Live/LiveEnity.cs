using UnityEngine;
namespace _core.Script.Live
{
    public abstract class LiveEntity:MonoBehaviour,IDamageable
    {
        [SerializeField]
        protected int wholeHp;

        [SerializeField]
        protected int currentHp;

        protected bool _isDead = false;

        protected virtual void Start()
        {
            currentHp = wholeHp;
            Init();
        }


        public virtual void GetHit(int damage)
        {
            currentHp -= damage;
            OnGetHit(damage);
            
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

        protected abstract void OnGetHit(float damage);
    }
}