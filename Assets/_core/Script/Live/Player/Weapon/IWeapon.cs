using System;
using UnityEngine;

namespace Player
{
    interface IWeapon
    {
        float Cd { get; }

        void OnInit();
        void ApproveAttack(Animator animator,Action duringAttack);
        void OnExit();
        void OnHit();
        void EndAttack();
    }
}