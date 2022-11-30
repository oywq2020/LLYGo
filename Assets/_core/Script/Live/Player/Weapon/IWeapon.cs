using System;

namespace Player
{
    interface IWeapon
    {
        float Cd { get; }

        void Init();

        void ApproveAttack();
        void Exit();

        void StartHit();
        void Hit();
        void EndHit();
    }
}