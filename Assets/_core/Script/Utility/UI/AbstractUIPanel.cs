using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Script.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AbstractUIPanel : MonoBehaviour, IUIPanel
    {
        private bool _isFirstLoad = true;

        //provide a attribute outside to signify whether this panel is open?
        public bool isOnOpen { get; set; } = false;

        public virtual async void Init()
        {
            //initiate variable
            isOnOpen = true;
            
            //call for every loads
            gameObject.SetActive(true);

            await DefaultPreOpen();
            
            OnOpen();
        }


        protected virtual async UniTask DefaultPreOpen()
        {
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            await canvasGroup.DOFade(1, 0.5f).SetEase(Ease.Linear).ToUniTask();
        }

        protected virtual async UniTask DefaultPreClose()
        {
            await GetComponent<CanvasGroup>().DOFade(0, 0.3f).SetEase(Ease.Linear).ToUniTask();
        }
        
        public virtual async void Disable()
        {
            Onclose();
            await DefaultPreClose();
            gameObject.SetActive(false);
            isOnOpen = false;
        }


        public abstract void OnOpen();


        protected abstract void Onclose();
    }
}