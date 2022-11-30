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
                    _agent.isStopped = true;

                    //set idle animator
                    _animator.SetTrigger("Idle");
                }).OnFixedUpdate(() =>
                {
                    if (destination < viewRange)
                    {
                        _fsm.ChangeState(State.Walk);
                    }
                });
        }
        //Find Player
        
        //Walk state
        private void Walk()
        {
            bool myLock = true;

            _fsm.State(State.Walk)
                .OnEnter(() => { })
                .OnFixedUpdate(async () =>
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


                    if (myLock)
                    {
                        myLock = false;
                        _animator.SetTrigger("Walk");
                        _agent.isStopped = false;

                        await UniTask.Delay(TimeSpan.FromSeconds(GetClip("Walk").length));

                        _agent.isStopped = true;
                        myLock = true;
                    }
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
                    _animator.SetTrigger("Attack1");
                    await UniTask.Delay(TimeSpan.FromSeconds(GetClip("Attack1").length));

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
                    _animator.SetTrigger("Attack2");
                    await UniTask.Delay(TimeSpan.FromSeconds(GetClip("Attack2").length));

                    _fsm.ChangeState(State.Idle);
                })
                .OnFixedUpdate(async () =>
                {
                    //turn to player by interpolation
                    transform.rotation = TurnTo(_playerTransForm.position - transform.position);
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