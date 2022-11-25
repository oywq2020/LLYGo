using System;

namespace Player
{
    interface IWeapon
    {
        float Cd { get; }

        void Init();
        void Exit();

        void StartHit();
        void Hit();
        void EndHit();
    }
}