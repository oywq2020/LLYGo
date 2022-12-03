using System;
using System.Collections.Generic;
using UnityEditorInternal;
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

        //to prevent the duplicate cause damage
        private List<Collider> _colliders = new  List<Collider>();

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
            
            _colliders.Clear();
        }

        public void StopListening()
        {
            _recall = null;
            _targetTag = "";

            if (_collider!=null)
            {
                _collider.enabled = false;
            }
            
            _colliders.Clear();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!_colliders.Contains(other)&&_targetTag!=""&&other.gameObject.CompareTag(_targetTag))
            {
                _recall?.Invoke(other.gameObject);
                _colliders.Add(other);
            }
        }
        
        
        
    }
}