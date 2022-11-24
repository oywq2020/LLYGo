namespace Player
{
    interface IWeapon
    {
        float Cd { get; }
        void Play();
    }
}