using Script.Event;

namespace _core.Script.Live
{
    public class CharacterLive:LiveEntity
    {
        protected override void OnDead()
        {
           
        }

        protected override void OnGetHit()
        {
            //Update bleed slider via Event
            GameFacade.Instance.SendEvent(new OnCharacterInjured()
            {
                currentHpPercent = currentHp/wholeHp
            });
        }
    }
}