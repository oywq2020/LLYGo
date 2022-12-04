namespace _core.Script.Live.Player.Character
{
    public interface ICharacterStatus
    {
         void InitCharacterStatus(int initExp);
         void InitCharacterStatus(int initExp, int hp, int mp);
    }
    
}