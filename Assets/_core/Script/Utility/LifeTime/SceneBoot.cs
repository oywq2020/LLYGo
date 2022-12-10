using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using SceneStateRegion;
using UnityEngine;

/// <summary>
/// This class is for simulating scene start and start the current scene
/// you should disable it's GameObject when you complete this scene test
/// </summary>
public class SceneBoot : MonoBehaviour
{
    public string stateName;
    public string accountName;

    public bool IsStartScence = false;

    private void Awake()
    {
        if (IsStartScence)
        {
            GameLoop.Instance.Controller.SetState(new StartState(GameLoop.Instance.Controller),false,true).Forget();
        }
        else
        {
            if (stateName != "")
            {
                //Boot different SceneState with given name by reflection 
                Type type = Type.GetType("SceneStateRegion." + stateName + "State");
                var instance = Activator.CreateInstance(type, GameLoop.Instance.Controller) as AbstractState;
                GameLoop.Instance.Controller.SetState(instance, false, true).Forget();
            }

            GameFacade.Instance.SetPlayer(accountName);
        }
    }
}