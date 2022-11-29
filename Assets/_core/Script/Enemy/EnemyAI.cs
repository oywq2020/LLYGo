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
    public enum State
    {
        Idle,
        Walk,
        Attack1,
        Attack2,
        GetHit,
        Dead
    }

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAI : LiveEntity, IPoolable
    {
        //enemy attribute
        public float viewRange = 10;

        public float attack1Damage = 5;
        public float attack1Range = 3;
        public float attack1Cd;

        //long hand attack
        public float attack2Damage = 10;
        public float attack2Range = 6;
        public float attack2Cd;

        public float speed = 4;

        //monitor
        [SerializeField] private State state;
        [SerializeField] private float destination;

        private Animator _animator;
        private NavMeshAgent _agent;

        private FSM<State> _fsm = new FSM<State>();
        private Transform _playerTransForm;

        private int _attackFlag;


        protected override void Init()
        {
            //init variable
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            _playerTransForm = GameObject.FindWithTag("Player").transform;

            _attackFlag = Random.Range(0, 2); //0,1 rand seed to flag the way of attack

            //register this enemy state
            RegisterStateMachine();

            //register event
            _fsm.OnStateChanged += e =>
            {
                //update the current state 
                state = e;
            };

            //set start state
            _fsm.StartState(State.Idle);
        }


        #region LifeEvent

        protected override void OnGetHit()
        {
            Debug.Log("get hit");
            transform.Find("BloodBarCanvas").GetComponent<BloodBarController>().UpdateBloodBar(currentHp / wholeHp);
            _fsm.ChangeState(State.GetHit);
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
                    _agent.speed = 0;

                    //set idle animator
                    _animator.SetTrigger("Idle");

                    Debug.Log("Entry Idle");
                }).OnFixedUpdate(() =>
                {
                    Debug.Log(" Idle");
                    if (destination < viewRange)
                    {
                        Debug.Log("Set walk");
                        _fsm.ChangeState(State.Walk);
                    }
                });
        }

        //Find Player


        //Walk state
        private void Walk()
        {
            _fsm.State(State.Walk)
                .OnEnter(() =>
                {
                    //set speed 
                    _agent.speed = 4;

                    _agent.SetDestination(_playerTransForm.position);
                    _animator.SetTrigger("Walk");
                })
                .OnFixedUpdate(() =>
                {
                    //track player
                    _agent.SetDestination(_playerTransForm.position);

                    //keep walk animation
                    if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    {
                        _animator.SetTrigger("Walk");
                    }

                    //judge that the player is in attack range
                    if (_attackFlag == 0)
                    {
                        //the first attack way
                        if (destination < attack1Range)
                        {
                            //entry attack range
                            _fsm.ChangeState(State.Attack1);

                            //reset seed
                            _attackFlag = Random.Range(0, 2);
                            return;
                        }
                    }
                    else if (_attackFlag == 1)
                    {
                        //the second attack way
                        if (destination < attack2Range)
                        {
                            //entry attack range
                            _fsm.ChangeState(State.Attack2);

                            //reset seed
                            _attackFlag = Random.Range(0, 2);
                            return;
                        }
                    }

                    if (destination > viewRange)
                    {
                        //player out of the view Range
                        _fsm.ChangeState(State.Idle);
                        return;
                    }
                });
        }

        //Attack state
        private void Attack1()
        {
            //thread lock
            bool myLock = true;


            _fsm.State(State.Attack1)
                .OnEnter(() =>
                {
                    //set speed 
                    _agent.speed = 0;
                })
                .OnFixedUpdate(async () =>
                {
                    if (destination > attack1Range)
                    {
                        //the player is out of the attack range
                        _fsm.ChangeState(State.Walk);
                        return;
                    }

                    //turn to player by interpolation
                    transform.rotation = TurnTo(_playerTransForm.position - transform.position);


                    if (myLock)
                    {
                        myLock = false;

                        _animator.SetTrigger("Attack1");
                        await UniTask.Delay(TimeSpan.FromSeconds(GetClip("Attack1",0).length));

                        myLock = true;
                    }
                }).OnExit(() => { });
        }

        private void Attact2()
        {
            //thread lock
            bool myLock = true;

            _fsm.State(State.Attack2)
                .OnEnter(() =>
                {
                    //set speed 
                    _agent.speed = 0;
                })
                .OnFixedUpdate(async () =>
                {
                    if (destination > attack2Range)
                    {
                        //the player is out of the attack range
                        _fsm.ChangeState(State.Walk);
                        return;
                    }

                    //turn to player by interpolation
                    transform.rotation = TurnTo(_playerTransForm.position - transform.position);

                    if (myLock)
                    {
                        myLock = false;
                        
                        await UniTask.Delay(TimeSpan.FromSeconds(GetClip("Attack2",0).length));

                        myLock = true;
                    }
                });
        }

        //Damage state
        private void Damage()
        {
            _fsm.State(State.GetHit)
                .OnEnter(async () => { _animator.SetTrigger("GetHit"); }).OnFixedUpdate((() =>
                {
                    if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
                    {
                        //proved that the enemy is no longer getting hit then convert it to idle
                        _fsm.ChangeState(State.Idle);
                        

                    }
                }));
        }

        //Dead state
        private void Dead()
        {
            _fsm.State(State.Dead)
                .OnEnter(() => { _animator.SetTrigger("Dead"); }).OnFixedUpdate(() =>
                {
                    if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
                    {
                        //the dead animation play is completed then entry game object pool

                        GameFacade.Instance.GetInstance<IGameObjectPool>().Enqueue(gameObject);
                    }
                });
        }

        #endregion


        private void FixedUpdate()
        {
            //Update destination
            if (_playerTransForm)
            {
                var transformPosition = transform.position;
                var position = _playerTransForm.position;
                var positionX = (transformPosition.x - position.x) * (transformPosition.x - position.x);
                var positionY = (transformPosition.z - position.z) * (transformPosition.z - position.z);
                destination = Mathf.Sqrt(positionX + positionY);
            }

            _fsm.FixedUpdate();
        }

        private void RegisterStateMachine()
        {
            Idle();
            Walk();
            Attack1();
            Attact2();
            Damage();
            Dead();
        }


        #region IPoolable

        void IPoolable.Init()
        {
            gameObject.SetActive(true);

            //set start State
            _fsm.StartState(State.Idle);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        #endregion


        Quaternion TurnTo(Vector3 cameraDir, float offset = 0)
        {
            Quaternion q = Quaternion.identity;
            q.SetLookRotation(cameraDir);
            return Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, q.eulerAngles.y + offset, 0),
                Time.deltaTime * 8);
        }

        AnimationClip GetClip(string clipName, int layer)
        {
            var currentAnimatorClipInfo = _animator.clip(layer);
            foreach (var animatorClipInfo in currentAnimatorClipInfo)
            {
                if (animatorClipInfo.clip.name.Equals(clipName))
                {
                    return animatorClipInfo.clip;
                }
                
            }
            return null;
        }
        
        
        //Draw out the viewRange by green sphere
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, viewRange);
        }
    }
}