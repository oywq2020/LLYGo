using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

public class Tips : AbstractUIPanel
{
    private IUIkit _uIkit;
    // Start is called before the first frame update

    protected override async UniTask DefaultPreOpen()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        await canvasGroup.DOFade(1, 0.5f).SetEase(Ease.Linear).ToUniTask();
        await UniTask.Delay(TimeSpan.FromSeconds(0.7));
        await canvasGroup.DOFade(0, 0.5f).SetEase(Ease.Linear).ToUniTask();
    }

    public override void OnOpen()
    {
        _uIkit = GameFacade.Instance.GetInstance<IUIkit>();
        _uIkit.ClosePanel(gameObject);
    }

    protected override void Onclose()
    {
    }

    public async void SetColorOnce(Color color)
    {
        var primaryColor = GetComponent<Text>().color;

        GetComponent<Text>().color = color;

        await UniTask.WaitUntil(() => !isOnOpen);
        
        GetComponent<Text>().color = primaryColor;
    }
}