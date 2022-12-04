using System;
using Script.Event;
using Script.Facade;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _core.Script.UI.Interface
{
    public class MainGamePanel : AbstractUIPanel
    {
        public StatsManager statsManager;

        public override void OnOpen()
        {
            GameFacade.Instance.RegisterEvent<OnCharacterInjured>(UpdateBleedSlider);
        }


        //Start function for simulation
        private void Start()
        {
            
            
            //register Event 
            GameFacade.Instance.RegisterEvent<OnCharacterInjured>(UpdateBleedSlider)
                .UnRegisterOnDestroy(gameObject);

            GameFacade.Instance.RegisterEvent<OnCharacterStatusUpdate>(UpdateStatus)
                .UnRegisterOnDestroy(gameObject);

            GameFacade.Instance.RegisterEvent<OnManaUpdate>(UpdateManaSlider)
                .UnRegisterOnDestroy(gameObject);
        }

        private void UpdateManaSlider(OnManaUpdate e)
        {
            if (e.Delta>=1)
            {
                //Restore the mana of display
                statsManager.manaManager.RestoreMana(e.Delta);
            }

            if (e.Delta<=-1)
            {
                //Consume the mana of display
                statsManager.manaManager.ConsumeMana(e.Delta);
            }
        }
        
        private void UpdateBleedSlider(OnCharacterInjured e)
        {
            //update the health of KHBar
            statsManager.healthManager.TakeDamage((int) e.Damage);
        }

        private void UpdateStatus(OnCharacterStatusUpdate e)
        {
            //init HUB;
            Debug.Log("Update HUB");
            statsManager.driveManager.CurrDrive = e.Exp;

            //update the max Hp and Mp by current exp
            UpdateMaxHpMp();

            //set current Hp and Mp
            statsManager.healthManager.SetCurrHealth(e.CurrentHp);
            statsManager.manaManager.SetCurrMana(e.CurrentMp);
        }

        private void UpdateMaxHpMp()
        {
            //update the max point of hp and mp
            statsManager.healthManager.SetMaxHealth(550 + 35 * statsManager.driveManager.CurrCharge);
            statsManager.manaManager.SetMaxMana(80 + 16 * statsManager.driveManager.CurrCharge);
        }

        protected override void Onclose()
        {
            GameFacade.Instance?.UnRegisterEvent<OnCharacterInjured>(UpdateBleedSlider);
        }
    }
}