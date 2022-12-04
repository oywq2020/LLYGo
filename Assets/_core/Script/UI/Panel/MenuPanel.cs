using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SceneStateRegion;
using Script.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : AbstractUIPanel
{
    [SerializeField] private Button startGameBtn;
    [SerializeField] private Button accountBtn;
    [SerializeField] private Button endGameBtn;

    public override void OnOpen()
    {
        startGameBtn.onClick.AddListener(() =>
        {
         
            GameLoop.Instance.Controller.SetState(new MainState(GameLoop.Instance.Controller)).Forget();
        });

        accountBtn.onClick.AddListener(() =>
        {
            GameFacade.Instance.GetInstance<IUIkit>().OpenPanel("AccountInfoPanel");
        });
        
        
        endGameBtn.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }

    protected override void Onclose()
    {
        Debug.Log("Onclose");
    }

    private void OnDestroy()
    {
        Debug.Log("MenuPanelDestroy");
    }
}