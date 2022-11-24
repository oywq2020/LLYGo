
using SceneStateRegion;
using Script.Event;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingState : AbstractState
{
    private string _targetScene;
    private AsyncOperationHandle<SceneInstance> _asyncOperationHandle;
    
    public LoadingState(SceneStateController stateController,string targetScene) : base("Loading", stateController)
    {
        _targetScene = targetScene;
    }

    private Image _loadBar;
    private Text _processText;
    
    private float _waitTime = 0;
    private float _totallTime = 1.5f;

    public override void StateStart()
    {
        
        _loadBar = GameObject.Find("Canvas/LoadingPanel/Slider").GetComponent<Image>();
        _processText = GameObject.Find("Canvas/LoadingPanel/ProcessText").GetComponent<Text>();
        
        
        //background loading 
        _asyncOperationHandle = Addressables.LoadSceneAsync(_targetScene, LoadSceneMode.Single, false);
        
        base.StateStart();
    }

    
    
    
    
    public override void StateUpdate()
    {
        base.StateUpdate();

        if (_loadBar&&_processText)
        {
            _loadBar.fillAmount = _waitTime/_totallTime;
        
            _processText.text = (int)(_waitTime/_totallTime*100)+ "%";
        }
        if (_asyncOperationHandle.PercentComplete< _waitTime/_totallTime)
        {
            //when current progress bar is faster than actual PercentComplete
            //show it via assigning actual PercentComplete to it and stopping it increment
            _waitTime = _asyncOperationHandle.PercentComplete*_totallTime;
        }
        else
        {
            _waitTime += Time.deltaTime;
        }
        
        
        if ( _asyncOperationHandle.IsDone&&_waitTime>_totallTime)
        {
            //entry game scene
           _asyncOperationHandle.Result.ActivateAsync();
        }
    }
    
}
