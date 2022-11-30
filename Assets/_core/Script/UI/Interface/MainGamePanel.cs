using System;
using Script.Event;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _core.Script.UI.Interface
{
    public class MainGamePanel:AbstractUIPanel
    {
        public StatsManager statsManager;
        
        public override void OnOpen()
        {
           GameFacade.Instance.RegisterEvent<OnCharacterInjured>(UpdateBleedSlider);
        }

        
        //Start function for simulation
        private void Start()
        {
            GameFacade.Instance.RegisterEvent<OnCharacterInjured>(UpdateBleedSlider);
            //todo delete the function when complete this test
        }

        public void UpdateBleedSlider(OnCharacterInjured e)
        {
            //update the health of KHBar
           statsManager.healthManager.TakeDamage((int)e.Damage);
        }
        
        
        
        
        protected override void Onclose()
        {
            GameFacade.Instance?.UnRegisterEvent<OnCharacterInjured>(UpdateBleedSlider);
        }
        
        
    }
}