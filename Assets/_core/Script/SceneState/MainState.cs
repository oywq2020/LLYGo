using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SceneStateRegion;
using Script.Event;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SceneStateRegion
{
    public class MainState : AbstractState
    {
        private IUIkit _uIkit;

        private bool _isBagOpen =false;
        public MainState(SceneStateController stateController) : base("Main", stateController)
        {
        }

        public override void StateStart()
        {
           //load weapon bar
           _uIkit = GameFacade.Instance.GetInstance<IUIkit>();

           _uIkit.OpenPanel("ShortBagPanel");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public override void StateUpdate()
        {
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
    }
}

