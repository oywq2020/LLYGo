using System;
using _core.Script.Bag.ScriptableObj.Item;
using Cysharp.Threading.Tasks;
using PlayerRegion;
using Script.Event;
using Script.Facade;
using UnityEngine;
using UnityEngine.UI;

namespace _core.Script.UI.Skill
{
    public class SkillPanel : MonoBehaviour
    {

        private Image _skillImage;
        private Image _skillImageMask;
        private float _cd;
        private float _timer = 0;

        private void Start()
        {
            _skillImage = transform.Find("SkillImage").GetComponent<Image>();
            _skillImageMask = transform.Find("SkillImageMask").GetComponent<Image>();
            _skillImageMask.gameObject.SetActive(false);

            GameFacade.Instance.RegisterEvent<OnReleaseSkill>(e =>
            {
                if (_timer != 0) return;

                _cd = e.SkillCd;

                _skillImageMask.gameObject.SetActive(true);
                EntryCd().Forget();
            }).UnRegisterOnDestroy(gameObject);

            GameFacade.Instance.RegisterEvent<OnShortIndexChanged>(e =>
            {
                var weaponSOBJ = CurrentPlayer.Instance.GetBag().itemList[e.Index];
                if (weaponSOBJ && weaponSOBJ.isEquip)
                {
                    var skillImageSprite = GameFacade.Instance.GetInstance<IAssetFactory>()
                        .LoadAsset<Sprite>((weaponSOBJ as WeaponItem).skillSprite.editorAsset.name);
                    
                    //assign the different sprite
                    _skillImage.sprite = skillImageSprite;
                    _skillImageMask.sprite = skillImageSprite;
                }
                else
                {
                    //empty-handed case
                }
            }).UnRegisterOnDestroy(gameObject);
        }

        private async UniTask EntryCd()
        {
            while (true)
            {
                _timer += Time.deltaTime;

                _skillImageMask.fillAmount = (_cd - _timer) / _cd;

                if (_timer > _cd)
                {
                    _timer = 0;
                    break;
                }

                await UniTask.Yield();
            }
        }
    }
}