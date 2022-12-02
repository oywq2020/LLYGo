using System;
using StateAI;
using UnityEngine;
using _core.Script.FSM;
using _core.Script.Live;
using Cysharp.Threading.Tasks;
using Script.Abstract;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _core.Script.Enemy
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public partial class EnemyAI
    {
        #region LifeEvent

        protected override void OnGetHit(float damage)
        {
            if (!_isDead)
            {
                Debug.Log("get hit");
                transform.Find("BloodBarCanvas").GetComponent<BloodBarController>().UpdateBloodBar(currentHp / wholeHp);
                _fsm.ChangeState(State.GetHit);
            }
        }

        protected override void OnDead()
        {
            Debug.Log("Dead");

            _fsm.ChangeState(State.Dead);
        }
        #endregion
        #region StateMachine

        //Idle state 
        private void Idle()
        {
            _fsm.State(State.Idle)
                .OnEnter(() =>
                {
                    _animator.SetBool("Idle",true);
                })
                .OnFixedUpdate(() =>
                {
                    if (destination < viewRange)
                    {
                        _fsm.ChangeState(State.Walk);
                    }
                })
                .OnExit(() =>
                {
                    _animator.SetBool("Idle",false);
                });
        }
        //Find Player
        
        //Walk state
        private void Walk()
        {
            _fsm.State(State.Walk)
                .OnEnter(() =>
                {
                    _agent.isStopped = false;
                    _animator.SetBool("Walk",true);
                })
                .OnFixedUpdate(() =>
                {
                    //the second attack way
                    if (destination <= attack2Range && destination > attack1Range)
                    {
                        //entry attack range
                        _fsm.ChangeState(State.Attack2);
                        return;
                    }
                    else if (destination <= attack1Range)
                    {
                        //entry attack range
                        _fsm.ChangeState(State.Attack1);
                        return;
                    }

                    if (destination > viewRange)
                    {
                        //player out of the view Range
                        _fsm.ChangeState(State.Idle);
                        return;
                    }
                    
                    //track player
                    _agent.SetDestination(_playerTransForm.position);
                    
                }).OnExit(() =>
                {
                    _agent.isStopped = true;
                    _animator.SetBool("Walk",false);
                });
        }

        //Attack state
        private void Attack1()
        {
            //thread lock
            bool myLock = true;


            _fsm.State(State.Attack1)
                .OnEnter(async () =>
                {
                    _animator.SetBool("Attack1",true);
                    
                    Debug.Log("wait:"+GetClip("Attack1").length);
                    await UniTask.Delay(TimeSpan.FromSeconds(GetClip("Attack1").length+0.5f));

                    _animator.SetBool("Attack1",false);
                    _fsm.ChangeState(State.Idle);
                })
                .OnFixedUpdate(async () =>
                {
                    //turn to player by interpolation during attack state
                    transform.rotation = TurnTo(_playerTransForm.position - transform.position);
                });
        }

        private void Attact2()
        {
            //thread lock
            bool myLock = true;

            _fsm.State(State.Attack2)
                .OnEnter(async () =>
                {
                    _animator.SetBool("Attack2",true);
                    await UniTask.Delay(TimeSpan.FromSeconds(GetClip("Attack2").length));

                    _animator.SetBool("Attack2",false);
                    _fsm.ChangeState(State.Idle);
                })
                .OnFixedUpdate(async () =>
                {
                    //turn to player by interpolation
                    transform.rotation = TurnTo(_playerTransForm.position - transform.position);
                })
                .OnExit(() =>
                {
                    _animator.SetBool("Attack2",false);
                });
        }

        //Damage state
        private void Damage()
        {
            //thread lock
            bool myLock = true;

            _fsm.State(State.GetHit)
                .OnEnter(async () =>
                {
                    _animator.SetTrigger("GetHit");
                    await UniTask.Delay(TimeSpan.FromSeconds(GetClip("GetHit").length));
                    
                    _fsm.ChangeState(State.Idle);
                });
        }

        //Dead state
        private void Dead()
        {
            bool myLock = true;
            _fsm.State(State.Dead)
                .OnEnter(async () =>
                {
                    _animator.SetTrigger("Dead");

                    //clear fsm
                    _fsm.Clear();

                    await UniTask.Delay(TimeSpan.FromSeconds(GetClip("Dead").length + 5f));

                    //enqueue this game object
                    GameFacade.Instance.GetInstance<IGameObjectPool>().Enqueue(gameObject);
                });
        }

        #endregion
        private void RegisterStateMachine()
        {
            Idle();
            Walk();
            Attack1();
            Attact2();
            Damage();
            Dead();
        }
    }
}