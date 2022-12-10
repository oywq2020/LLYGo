using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Script.Abstract;
using Script.Event;
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
                _fsm.ChangeState(State.GetHit);
                transform.Find("BloodBarCanvas").GetComponent<BloodBarController>().UpdateBloodBar((float)currentHp / (float)wholeHp);
              
            }
        }

        protected override void OnDead()
        {
            Debug.Log("Dead");

            _fsm.ChangeState(State.Dead);
            
            //trigger the dead event and pass this master exp
            GameFacade.Instance.SendEvent(new OnMasterDead()
            {
                Exp = exp
            });
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
                    _agent.enabled = true;
                    _agent.isStopped = false;
                 
                })
                .OnFixedUpdate(() =>
                {

                    switch (attackMode)
                    {
                        case AttackMode.DistanceFirst:
                           
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
                            
                            break;
                        case AttackMode.Random:
                            //the attack1 is common attack and attack2 is special attack in this case
                            if (randomSeed>3)
                            {
                                if (destination<=attack2Range)
                                {
                                    //the 70% is common attack 
                                    _fsm.ChangeState(State.Attack2);
                       
                                    //reset the random seed
                                    randomSeed = Random.Range(0, 9);
                                    return;
                                }
                            }
                            else
                            {
                                if (destination<=attack1Range)
                                {
                                    _fsm.ChangeState(State.Attack1);
                       
                                    //reset the random seed
                                    randomSeed = Random.Range(0, 9);
                                    return;
                                }
                  
                            }
                            break;
                    }

                    Debug.Log("Walk");
                    
                    //the player is out of the range of Enemy's view 
                    if (destination > viewRange)
                    {
                        //player out of the view Range
                        _fsm.ChangeState(State.Idle);
                        return;
                    }
                    
                    //track player
                    if (_agent.isActiveAndEnabled)
                    {
                        _agent.SetDestination(_playerTransForm.position);
                    }
                    //set work to ture
                    _animator.SetBool("Walk",true);
                    
                }).OnExit(() =>
                {
                    _agent.isStopped = true;
                    _animator.SetBool("Walk",false);
                    _agent.enabled = false;
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