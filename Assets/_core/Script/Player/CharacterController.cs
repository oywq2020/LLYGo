using System;
using System.Collections;
using System.IO;
using Cysharp.Threading.Tasks;
using MagicaCloth;
using QFramework;
using Script.Event;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

namespace Player
{
    public enum PlayerState
    {
        Attack,
        Normal
    }

    // interface IPlayer
    // {
    //     //Provide injection interface 
    //     Action<PlayerState> OnStateChanged { get; set; }
    //     AudioSource PlayerAuiSource { get; set; }
    // }

    /// <summary>
    /// assemble all player physically behaviours 
    /// </summary>
    public class CharacterController : MonoBehaviour
    {
        public float walkSpeed = 2f;
        public float runSpeed = 4f;
        public float jumpSpeed = 5f;
        public float gravity = 20f;
        
        private UnityEngine.CharacterController _characterController;
        private Vector3 _moveDirection = Vector3.zero;
        private Camera _viewCamera;
        private Animator _animator;

        private bool updatePause = false;

        private void Start()
        {
            //assigning 
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<UnityEngine.CharacterController>();
            _viewCamera = Camera.main;
            
            //registering
          
        }

      
        private void Update()
        {
            if (updatePause) return;
            
            Movement();
        }

        #region Registering
        //register it to your relative event
        void Pause(OnMouseEntryGUI e)
        {
            updatePause = true;
        }
        void Continue(OnMouseExitGUI e)
        {
            updatePause = false;
        }


        private void OnDestroy()
        {
            GameFacade.Instance?.UnRegisterEvent<OnMouseEntryGUI>(Pause);
            GameFacade.Instance?.UnRegisterEvent<OnMouseExitGUI>(Continue);
        }

        #endregion
       
        
        #region MoveMent

          // move and Jump
        void Movement()
        {
            float tempSpeed = walkSpeed;
            if (_characterController.isGrounded)
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                _moveDirection = new Vector3(h, 0, v);


                //listening state by v
                if (!h.Equals(0) || !v.Equals(0))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //only player press the leftShift and v is not zero does the character entry Run state
                        tempSpeed = runSpeed;
                    }
                }

                if (Input.GetButton("Jump"))
                {
                    _moveDirection.y = jumpSpeed;
                    _animator.SetTrigger("Jump");
                }

                if (v > 0.1f)
                {
                    //rotate to forward of camera
                    transform.rotation = TurnTo(CameraForward());
                }

                if (v < -0.1f)
                {
                    //rotate to back of camera
                    transform.rotation = TurnTo(-CameraForward());
                }

                if (h > 0.1f)
                {
                    //rotate to left of camera
                    transform.rotation = TurnTo(CameraForward(), 90);
                }

                if (h < -0.1f)
                {
                    //rotate to right of camera
                    transform.rotation = TurnTo(CameraForward(), -90);
                }
            }


            //set current speed to blender tree
            _animator.SetFloat("Speed", _characterController.velocity.magnitude);

            //covert _moveDirection vector from the world coordinate system to the system that relates the camera forward
            _moveDirection = Quaternion.LookRotation(CameraForward()) * _moveDirection;

            _moveDirection.y -= gravity * Time.deltaTime;
            _characterController.Move(_moveDirection * tempSpeed * Time.deltaTime);
        }

        //forward direction in camera forward
        Vector3 CameraForward()
        {
            var x = this.transform.position.x - _viewCamera.transform.position.x;
            var z = transform.position.z - _viewCamera.transform.position.z;
            return new Vector3(x, 0, z);
        }

        Quaternion TurnTo(Vector3 cameraDir, float offset = 0)
        {
            Quaternion q = Quaternion.identity;
            q.SetLookRotation(cameraDir);
            return Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, q.eulerAngles.y + offset, 0),
                Time.deltaTime * 10);
        }

        #endregion
      
    }
}