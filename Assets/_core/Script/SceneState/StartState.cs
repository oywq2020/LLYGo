
using Cysharp.Threading.Tasks;

using Script.Facade;
using Script.UI;

using UnityEngine;

namespace SceneStateRegion
{
    public class StartState:AbstractState
    {
        private IUIkit _uIkit;
        private IEventHolder _eventHolder;
        
        //Introduction panel 
        private CanvasGroup _introductionPanel;
        public StartState(SceneStateController stateController) : base("Start", stateController)
        {
        }

        public  override  void StateStart()
        {
            #region ShowIntroduction

            // //show introduction panel
            // await GameObject.Find("UIRoot/Common/IntroductionPanel")
            //     .GetComponent<CanvasGroup>().DOFade(1, 1).ToUniTask();
            //
            // await UniTask.Delay(TimeSpan.FromSeconds(3));
            //
            // await GameObject.Find("UIRoot/Common/IntroductionPanel")
            //     .GetComponent<CanvasGroup>().DOFade(0, 2).ToUniTask();

                #endregion
                
            _uIkit = GameFacade.Instance.GetInstance<IUIkit>();
            _uIkit.OpenPanel("Login");
            
            //register event
           

            //for synchronize scene
            base.StateStart();
        }
        
        public override void StateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StateController.SetState(new MainState(StateController)).Forget();
            }
        }
        
    }
}