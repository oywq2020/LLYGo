using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateAI

{
    public enum State
    {
        Idle,
        Walk,
        Attack,
        Damage,
        Dead
    }
    
    
    
    
    public abstract class AbstractFSM : MonoBehaviour
    {
        [SerializeField]  State _currentState;
        [SerializeField] protected Transform _player;
        [SerializeField] protected float _distance;

        protected virtual void Start()
        {
            _currentState = State.Idle;
            _player = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
            _distance = Vector3.Distance(_player.transform.position, transform.position);
            switch (_currentState)
            {
                case State.Idle:
                    StateIdle();
                    break;
                case State.Walk:
                    StateWalk();
                    break;
                case State.Attack:
                    StateAttack();
                    break;
                case State.Damage:
                    StateDamage();
                    break;
                case State.Dead:
                    StateDead();
                    break;
            }
        }
        
        protected abstract void StateIdle();


        protected abstract void StateWalk();

        protected abstract void StateAttack();

        protected abstract void StateDamage();

        protected abstract void StateDead();

        public virtual void ChangeIdle()
        {
            _currentState = State.Idle;
        }

        public virtual void ChangeWalk()
        {
            _currentState = State.Walk;
        }

        public virtual void ChangeAttack()
        {
            _currentState = State.Attack;
        }

        public virtual void ChangeDamage()
        {
            _currentState = State.Damage;
        }

        public virtual void ChangeDead()
        {
            _currentState = State.Dead;
        }
    }
}