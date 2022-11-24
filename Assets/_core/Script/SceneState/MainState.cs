using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SceneStateRegion;
using Script.Event;
using UnityEngine;
using UnityEngine.UI;

namespace SceneStateRegion
{
    public class MainState : AbstractState
    {
        public MainState(SceneStateController stateController) : base("Main", stateController)
        {
        }

        public override void StateStart()
        {
           
        }
    }
}

