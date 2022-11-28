using System;
using System.Collections;
using System.Collections.Generic;
using Script.Abstract;
using Script.AssetFactory;
using Script.Facade;
using Script.UI;
using UnityEngine;

public class GameFacade : MonoBehaviour
{
    public static GameFacade Instance;

    private IContainer _container;

    private IEventHolder _eventHolder;

    private GameObject _characterHolder;

    // Start is called before the first frame update
    private void Awake()
    {
        #region GenerateInstance

        var find = GameObject.Find("GameFacade");
        if (find != null && find != gameObject)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        #endregion
        
        _container = new Container();
        //Instance register list
        RegisterList();
    }

    private void RegisterList()
    {
        //there is for registering mono instance

        #region RegisterResourceFactory

        //register common assetFactory
        var assetFactory = Register<IAssetFactory>(new ResourceFactory());

        //register GameObject pool by injecting from assetFactory internally 
        var gameObjectPool = Register<IGameObjectPool>(new ResourceFactoryProxy(assetFactory));

        #endregion

        #region UIkit

        //register UIkit by injecting from GameObject pool
        Register<IUIkit>(new UIkit(gameObjectPool));

        #endregion

        #region EventSystem

        _eventHolder = Register<IEventHolder>(new EventHolder());

        #endregion
    }


    #region FacadeForWholeGame

    //get the instance registered in RegisterList above
    public T GetInstance<T>() where T : class
    {
        return _container.GetInstance<T>();
    }

    //provide function of registering event directly
    public void RegisterEvent<T>(Action<T> action) where T : new()
    {
        _eventHolder.Register<T>(action);
    }

    public void UnRegisterEvent<T>(Action<T> action) where T : new()
    {
        _eventHolder.UnRegister<T>(action);
    }

    public void SendEvent<T>() where T : new()
    {
        _eventHolder.Send<T>();
    }

    public void SendEvent<T>(T t)where T : new()
    {
        _eventHolder.Send(t);
    }
    
    #endregion


    private T Register<T>(T obj, Mode mode = Mode.Singleton) where T : class
    {
        _container.Register<T>(obj, mode);
        return obj;
    }



    #region Player

   public void SetCharacter(GameObject character)
   {
       _characterHolder = character;
   }

   public GameObject GetCurrentCharacter()
   {
       return _characterHolder;
   }
    #endregion
}