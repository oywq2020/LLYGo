using System;
using Cinemachine;
using Script.Event;
using UnityEngine;

namespace _core.Script.Player
{
    public class CameraLimiter : MonoBehaviour
    {
        private CinemachineFreeLook _cinemachine;

        private float _originX;
        private float _originY;

        private void Start()
        {
            _cinemachine = GetComponent<CinemachineFreeLook>();

            _originX = _cinemachine.m_XAxis.m_MaxSpeed;
            _originY = _cinemachine.m_YAxis.m_MaxSpeed;

            GameFacade.Instance.RegisterEvent<OnMouseEntryGUI>(StartLimiter);
            GameFacade.Instance.RegisterEvent<OnMouseExitGUI>(StopLimiter);
        }


        void StartLimiter(OnMouseEntryGUI e)
        {
            _cinemachine.m_XAxis.m_MaxSpeed = 0;
            _cinemachine.m_YAxis.m_MaxSpeed = 0;
        }

        void StopLimiter(OnMouseExitGUI e)
        {
            _cinemachine.m_XAxis.m_MaxSpeed = _originX;
            _cinemachine.m_YAxis.m_MaxSpeed = _originY;
        }

        private void OnDestroy()
        {
            GameFacade.Instance?.UnRegisterEvent<OnMouseEntryGUI>(StartLimiter);
            GameFacade.Instance?.UnRegisterEvent<OnMouseExitGUI>(StopLimiter);
        }
    }
}