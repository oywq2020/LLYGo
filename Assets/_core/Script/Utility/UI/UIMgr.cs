
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    private IUIkit _uIkit;
    private async void Start()
    {
        transform.Find("Design").gameObject.SetActive(false);
        
        
        //await this scene initiation synchronously
        await GameLoop.Instance.Setup();
        
        _uIkit = GameFacade.Instance.GetInstance<IUIkit>();
        
        
     
    }
   
}
