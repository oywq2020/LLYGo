using Script.Event;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _core.Script.UI.Interface
{
    public class CharacterPanel:AbstractUIPanel
    {
        public Image bleedImg;
        public override void OnOpen()
        {
           GameFacade.Instance.RegisterEvent<OnCharacterInjured>(UpdateBleedSlider);
        }

        public void UpdateBleedSlider(OnCharacterInjured e)
        {
            bleedImg.fillAmount = e.currentHpPercent;
        }
        
        
        
        
        protected override void Onclose()
        {
            GameFacade.Instance?.UnRegisterEvent<OnCharacterInjured>(UpdateBleedSlider);
        }
        
        
    }
}