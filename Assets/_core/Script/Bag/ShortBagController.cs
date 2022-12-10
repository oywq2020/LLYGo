using System;
using Script.Event;
using Script.Event.CharacterMove;
using Script.Facade;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace _core.Script.Bag
{
    public class ShortBagController : MonoBehaviour
    {
        private float _axis;

        private int _shortBagIndex; //indicate current index of short bag

        private Color _originColor = new Color(0.4433962f, 0.4433962f, 0.4433962f, 1f);
        private Color _selectedColor = new Color(0.7924528f, 0.6018156f, 0.6018156f, 1f);

        private int _childrenCount;

        private bool _isPause;

        private void Start()
        {
            _childrenCount = transform.childCount;
            Select(0);

            //register event
            GameFacade.Instance.RegisterEvent<OnWeaponBagRefreshed>(WeaponBagRefreshed).UnRegisterOnDestroy(gameObject);
            GameFacade.Instance.RegisterEvent<ChangeWheelState>(e =>
            {
                _isPause = !e.IsEnable;
            });

        }

        private void FixedUpdate()
        {
            if (_isPause) return;

            _axis = Input.GetAxis("Mouse ScrollWheel");

            if (_axis > 0.1)
            {
                //ScrollWheel scrolls up
                _shortBagIndex = (--_shortBagIndex + _childrenCount) % _childrenCount;
                Select(_shortBagIndex);
            }

            if (_axis < -0.1)
            {
                //ScrollWheel scrolls down
                _shortBagIndex = (++_shortBagIndex + _childrenCount) % _childrenCount;
                Select(_shortBagIndex);
            }
        }

        private void Select(int index)
        {
            for (int i = 0; i < _childrenCount; i++)
            {
                transform.GetChild(i).GetComponent<Image>().color = _originColor;
            }

            transform.GetChild(index).GetComponent<Image>().color = _selectedColor;

            //send Event
            GameFacade.Instance.SendEvent(new OnShortIndexChanged() {Index = _shortBagIndex});
        }

        void WeaponBagRefreshed(OnWeaponBagRefreshed e)
        {
            //reset the index
            Select(_shortBagIndex);
        }

        private void OnDestroy()
        {
            GameFacade.Instance?.UnRegisterEvent<OnWeaponBagRefreshed>(WeaponBagRefreshed);
        }
    }
}