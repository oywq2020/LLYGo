using System;
using Cysharp.Threading.Tasks;
using Script.Abstract;
using Script.Event;
using Script.Facade;
using UnityEngine;
using UnityEngine.UIElements;


namespace SceneStateRegion
{
    public enum SceneState
    {
        Start,
        Main
    }

    /// <summary>
    /// abstract state for scene
    /// </summary>
    public abstract class AbstractState
    {
        public string SceneName { get; }
        public SceneStateController StateController;

        public AbstractState(string sceneName, SceneStateController stateController)
        {
            SceneName = sceneName;
            StateController = stateController;
        }

        //called when ever entry this state
        public virtual void StateStart()
        {
            DefaultRegister();
            //if you want inherit it, you must preserve it at the last line of override method
            //this operation is for synchronizing methods that must execute behind loading  
            GameLoop.Instance.Set(true);
        }

        //called when update info for every frame
        public virtual void StateUpdate()
        {
        }

        //called when leaves this state
        public virtual void StateEnd()
        {
            DefaultUnRegister();

            //resource all asset loaded in this scene
            GameFacade.Instance.GetInstance<IGameObjectPool>().ReleaseAll();

            //this operation is for synchronizing methods that must execute behind loading  
            GameLoop.Instance.Set(false);
            Debug.Log("SceneEnd");
        }

        protected virtual void DefaultRegister()
        {
        }

        protected virtual void DefaultUnRegister()
        {
        }
        
    }
}