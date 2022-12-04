using _core.Script.Live.Player.Character;
using Script.Abstract;
using Script.Event;
using Script.Facade;
using Script.UI;
using UnityEngine;

namespace SceneStateRegion
{
    public class MainState : AbstractState
    {
        private IUIkit _uIkit;

        private bool _isBagOpen =false;

        private IUnRegisterList _unRegisterList = new UnRegister();
        public MainState(SceneStateController stateController) : base("Main", stateController)
        {
            
        }

        public override async void StateStart()
        {
            base.StateStart();
           //load weapon bar
           _uIkit = GameFacade.Instance.GetInstance<IUIkit>();
           _uIkit.OpenPanel("ShortBagPanel");
           //register event
           RegisterList();
           
           //create character
           
           
           var gameObject = GameFacade.Instance.GetInstance<IGameObjectPool>().Dequeue("lly");
           gameObject.GetComponent<ICharacterStatus>().InitCharacterStatus(500);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public override void StateUpdate()
        {
            //listening the keyboard event in main scene
            
            if (Input.GetKeyDown(KeyCode.B))
            {
                //open bag
                if (_isBagOpen)
                {
                    _uIkit.ClosePanel("BagPanel");
                }
                else
                {
                    _uIkit.OpenPanel("BagPanel");
                }

                _isBagOpen = !_isBagOpen;
            }
        }


        private void RegisterList()
        {
            //register OnMasterDead for increasing the EXP of character
            GameFacade.Instance.RegisterEvent<OnMasterDead>(e =>
            {
                //to catch the message of the master dying
                GameFacade.Instance.SendEvent(new OnExpIncreased()
                {
                    //add  exp for character
                    increment = e.Exp
                });
            });
        }
        
        public override void StateEnd()
        {
            
            
            base.StateEnd();
        }
    }
}

