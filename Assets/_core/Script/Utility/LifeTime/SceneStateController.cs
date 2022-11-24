using Cysharp.Threading.Tasks;
using SceneStateRegion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneStateController
{
    //scene current state
    private AbstractState _abstractState;

    //await scene loading 
    private bool _canUpdate = false;

    //set scene state
    public async UniTask SetState(AbstractState state, bool isLoadScene = true,bool isfirst = false)
    {
        _canUpdate = false;

        //if not first then release the lasted state
        _abstractState?.StateEnd();

        if (isfirst)
        {
            //if this scene is first of whole game than don't load scene
            _abstractState = state;
            _abstractState.StateStart();
            _canUpdate = true;
            return;
        }
        
        //judge if load scene
        if (isLoadScene)
        {
            //entry loading scene
            await this.SetState(new LoadingState(this, state.SceneName), false);
        }
        else
        {
            //update current scene state
            _abstractState = state;
            //loading directly
           await Addressables.LoadSceneAsync(_abstractState.SceneName);
            _abstractState.StateStart();
        }

        _canUpdate = true;
    }

    /// <summary>
    /// current state real time update
    /// </summary>
    public void StateUpdate()
    {
        if (_canUpdate)
        {
            _abstractState?.StateUpdate();
        }
    }
    
}