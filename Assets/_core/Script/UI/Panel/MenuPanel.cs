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
    [SerializeField] private Button startGame;
    [SerializeField] private Button endGame;

    public override void OnOpen()
    {
        Debug.Log("Menu scene Init");
        startGame.onClick.AddListener(() =>
        {
            Debug.Log("Start Game");
            GameLoop.Instance.Controller.SetState(new MainState(GameLoop.Instance.Controller)).Forget();
        });

        endGame.onClick.AddListener(() =>
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
    }
}