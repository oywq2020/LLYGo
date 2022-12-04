using System;
using _core.Script.Enemy;
using _core.Script.Live;
using Cysharp.Threading.Tasks;
using HighlightPlus;
using Script.Abstract;
using Script.Event;
using UnityEngine;

namespace Player
{
    public class Sword : MonoBehaviour, IWeapon
    {
        private float cd = 1.4f;

        [SerializeField] private float skillCd = 5;

        [SerializeField] private float duringTime = 3;

        private bool _canSkill = true;

        //to flag the effect duration
        private bool _isEffect = false;

        private HighlightEffect _highlightEffect;
        
        public float Cd
        {
            get => cd;
        }

        public int damage = 50;
        private TriggerKit _triggerKit;
        private bool _canAttack = true;


        public void OnInit()
        {
            _triggerKit = GetComponent<TriggerKit>();
            _highlightEffect = GetComponentInChildren<HighlightEffect>();

            _highlightEffect.highlighted = false;
        }


        //the weapon is get the Approve to execute its operation
        public void ApproveAttack(Animator animator, Action duringAttack)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (_canAttack)
                {
                    //increase the skillcounter
                    _canAttack = false;

                    StartHit(animator).Forget();
                }

                duringAttack.Invoke();
            }
            else
            {
                //prevent mistaken link attack
                animator.SetBool("Attack", false);

                //make the attackCount to zero when stop attack
            }


            //listening skill release
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!_canAttack) return;

                //enable effect 
                _highlightEffect.highlighted = true;
                //double the damage
                damage *= 2;

                //trigger event
                GameFacade.Instance.SendEvent(new OnReleaseSkill()
                {
                    SkillCd = skillCd
                });

                EntrySkillCd();
            }
        }


        private async UniTask StartHit(Animator animator)
        {
            _canAttack = false;
            animator.SetBool("Attack", true);
            BeforeAttack(animator);
            GameFacade.Instance.SendEvent<OnStartAttack>();
            await UniTask.Delay(TimeSpan.FromSeconds(cd * 4 / 5));
            _triggerKit.StopListening();
            GameFacade.Instance.SendEvent<OnEndAttack>();

            _canAttack = true;
        }

        void BeforeAttack(Animator animator)
        {
            //start attack check
            _triggerKit.StartListening(gb => { gb.GetComponent<IDamageable>().GetHit(damage); }, "Enemy");

            //update current animation clip length to cd
        }


        public void OnHit(int attackIndex)
        {
            // if (_isEffect)
            // {
            //
            //     GameObject effect = new GameObject();
            //     //the duration of skill
            //     if (attackIndex == 0)
            //     {
            //          effect = GameFacade.Instance.GetInstance<IGameObjectPool>().Dequeue("Sword Slash 2",
            //             transform.parent.GetComponent<CharacterBodyMapper>().follows);
            //         //increase the counter
            //     }
            //
            //     else if (attackIndex == 1)
            //     {
            //         effect = GameFacade.Instance.GetInstance<IGameObjectPool>().Dequeue("Sword Slash 3",
            //             transform.parent.GetComponent<CharacterBodyMapper>().follows);
            //         //increase the counter
            //        
            //     }
            //
            //     else if (attackIndex == 2)
            //     {
            //         effect = GameFacade.Instance.GetInstance<IGameObjectPool>().Dequeue("Sword Slash 5",
            //             transform.parent.GetComponent<CharacterBodyMapper>().follows);
            //         //increase the counter
            //         effect.transform.SetParent(null);
            //     }
            // }
        }

        public void EndAttack()
        {
            _triggerKit.StopListening();
        }


        public void OnExit()
        {
            //undo the effect for skill
            _highlightEffect.highlighted = false;
        }

        private async void EntrySkillCd()
        {
            _isEffect = true;
            _canSkill = false;
            await UniTask.Delay(TimeSpan.FromSeconds(duringTime));
            _isEffect = false;
            //undo the effect for skill
            _highlightEffect.highlighted = false;

            //undo the augment damage
            damage /= 2;

            await UniTask.Delay(TimeSpan.FromSeconds(skillCd - duringTime));
            _canSkill = true;
        }
    }
}