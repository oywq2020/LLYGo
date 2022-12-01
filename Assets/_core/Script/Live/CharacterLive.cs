using _core.Script.Utility.Extension;
using Script.Event;
using UnityEngine;

namespace _core.Script.Live
{
    public class CharacterLive:LiveEntity
    {
        private Animator _animator;
        protected override void Init()
        {
            _animator = GetComponent<Animator>();
            
          
        }

        protected override void OnDead()
        {
            //play git hit animation
            _animator.SetTrigger("Dead");
        }

        protected override void OnGetHit(float damage)
        {
            //Update bleed slider via Event
            GameFacade.Instance.SendEvent(new OnCharacterInjured()
            {
                Damage = damage,
                Duration = _animator.runtimeAnimatorController.animationClips.GetClip("GetHit").length
            });
            
            //play git hit animation
            _animator.SetTrigger("GetHit");
        }
    }
}