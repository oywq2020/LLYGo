
using Cysharp.Threading.Tasks;
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
    private AbstractState _targetState;
    private AsyncOperationHandle<SceneInstance> _asyncOperationHandle;
    
    public LoadingState(SceneStateController stateController,AbstractState targetState) : base("Loading", stateController)
    {
        _targetState = targetState;
    }

    private Image _loadBar;
    private Text _processText;
    
    private float _waitTime = 0;
    private float _totallTime = 1.5f;
    private bool _isLoadingCompleted = false;

    public override void StateStart()
    {
        
        _loadBar = GameObject.Find("Canvas/LoadingPanel/Slider").GetComponent<Image>();
        _processText = GameObject.Find("Canvas/LoadingPanel/ProcessText").GetComponent<Text>();
        
        
        //background loading 
        _asyncOperationHandle = Addressables.LoadSceneAsync(_targetState.SceneName, LoadSceneMode.Single, false);
        
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
        
        //if the scene load completed in background
        if ( _asyncOperationHandle.IsDone&&_waitTime>_totallTime)
        {
            if (!_isLoadingCompleted)
            {
                //start activate thread 
                ActivateTargetScene().Forget();
                _isLoadingCompleted = true;
            }
          
        }
    }

  private async UniTask ActivateTargetScene()
    {
        //entry game scene
        await UniTask.WaitUntil(() => _asyncOperationHandle.Result.ActivateAsync().isDone);
        //truly set target state
        StateController.SetState(_targetState, false, true).Forget();
    }
    
}
