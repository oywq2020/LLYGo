using System;
using UnityEngine;

namespace _core.Script.Enemy
{
    /// <summary>
    /// this class must be attached to those attached Collider
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TriggerKit : MonoBehaviour
    {
        private Collider _collider;

        private string _targetTag;

        private Action<GameObject> _recall;
        
        private void Start()
        {
            _collider = GetComponent<Collider>();

            _collider.enabled = false;
        }
        
        public void StartListening(Action<GameObject> recall,string myTag)
        {
            _recall = recall;
            _targetTag = myTag;
            
            _collider.enabled = true;
        }

        public void StopListening()
        {
            _recall = null;
            _targetTag = "";

            _collider.enabled = false;
        }
        
        
        
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("TriggerEnter");
            if (_targetTag!=""&&other.gameObject.CompareTag(_targetTag))
            {
                _recall?.Invoke(other.gameObject);
            }
        }
    }
}