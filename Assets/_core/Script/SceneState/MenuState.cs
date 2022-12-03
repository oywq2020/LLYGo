using Script.UI;
using UnityEngine;

namespace SceneStateRegion
{
    public class MenuState:AbstractState
    {
        private IUIkit _uiKit;
        public MenuState(SceneStateController stateController) : base("Menu", stateController)
        {
        }

        public override void StateStart()
        {
            Debug.Log("MenuSceneStart");
            _uiKit =GameFacade.Instance.GetInstance<IUIkit>();
            Debug.Log("Uikit Open");
            
            _uiKit.OpenPanel("MenuPanel");
            
            Debug.Log("Panel Open");
            
            base.StateStart();
        }

        public override void StateEnd()
        {
            //todo something
            
            
            base.StateEnd();
        }
    }
}