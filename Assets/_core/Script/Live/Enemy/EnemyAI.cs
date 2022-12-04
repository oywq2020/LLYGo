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
    public partial class EnemyAI : LiveEntity, IPoolable
    {
        //enemy attribute
        public float viewRange = 10;

        public int attack1Damage = 5;
        public float attack1Range = 3;
      
        
        
        //long hand attack
        public int attack2Damage = 10;
        public float attack2Range = 6;

        public int exp = 100;
        
        //monitor
        [SerializeField] private State state;
        [SerializeField] private float destination;

        private Animator _animator;
        private NavMeshAgent _agent;

        private FSM<State> _fsm = new FSM<State>();
        private Transform _playerTransForm;
        
        public void Disable()
        {
            gameObject.SetActive(false);
        }

        void IPoolable.Init()
        {
            Init();
            gameObject.SetActive(true);
        }

        protected override void Init()
        {
            //init variable
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            _playerTransForm = GameObject.FindWithTag("Player").transform;
            
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
        
        Quaternion TurnTo(Vector3 cameraDir, float offset = 0)
        {
            Quaternion q = Quaternion.identity;
            q.SetLookRotation(cameraDir);
            return Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, q.eulerAngles.y + offset, 0),
                Time.deltaTime * 8);
        }

        AnimationClip GetClip(string clipName)
        {
            var currentAnimatorClipInfo = _animator.runtimeAnimatorController.animationClips;
            foreach (var animatorClipInfo in currentAnimatorClipInfo)
            {
                if (animatorClipInfo.name.Equals(clipName))
                {
                    return animatorClipInfo;
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