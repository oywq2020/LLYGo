using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SceneStateRegion;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public SceneStateController Controller;

    public static GameLoop Instance;

    private bool _senecStateInit = false;

    private void Awake()
    {
        var find = GameObject.Find("GameLoop");
        //avoid the generate the same object
        if (find != null && find != this.gameObject)
        {
            Destroy(this.gameObject);
        }

        Controller = new SceneStateController();
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
    private void Update()
    {
        //different state need different data
        Controller?.StateUpdate();
    }


    //provide tool for GamaObject for current scene state
    public void Set(bool senecStateInit)
    {
        Debug.Log("Set senceStateInit to "+senecStateInit);
        _senecStateInit = senecStateInit;
    }

    public async UniTask Setup()
    {
        Debug.Log("Call for SetUp and _senceStateInit："+_senecStateInit);
        if (_senecStateInit) return;
        //await for the completion of current scene state
        await UniTask.WaitUntil(() => _senecStateInit);
        Debug.Log("SetUp Complete");
        
    }
}