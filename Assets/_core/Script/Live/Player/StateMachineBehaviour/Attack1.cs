using System;
using UnityEngine;
using UnityEngine.Animations;

namespace _core.Script.Live.Player.StateMachineBehaviour
{
    public class Attack1 : UnityEngine.StateMachineBehaviour
    {
        public Action onStateExit;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            base.OnStateExit(animator, stateInfo, layerIndex, controller);
            onStateExit?.Invoke();
        }
    }
}