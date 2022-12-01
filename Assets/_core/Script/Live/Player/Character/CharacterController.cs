using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Script.Event;
using Script.Facade;
using UnityEngine;


namespace Player
{
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

        private bool _updatePause = false;

        //the origin variable is for making the character to ground
        private float _currentSpeed = 0.01f;


        private CancellationTokenSource _cancelSource;

        private void Start()
        {
            //assigning 
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<UnityEngine.CharacterController>();
            _viewCamera = Camera.main;

            //registering
            GameFacade.Instance.RegisterEvent<OnStartAttack>(e => { Pause(); }).UnRegisterOnDestroy(gameObject);
            GameFacade.Instance.RegisterEvent<OnEndAttack>(e => { Continue(); }).UnRegisterOnDestroy(gameObject);
            GameFacade.Instance.RegisterEvent<OnCharacterInjured>(OnInjured).UnRegisterOnDestroy(gameObject);

            //register character to GameFacede
            GameFacade.Instance.SetCharacter(gameObject);
        }


        private void Update()
        {
            if (_updatePause) return;
            Movement();
        }

        #region Registering

        //register it to your relative event
        void Pause()
        {
            _updatePause = true;
        }

        void Continue()
        {
            _updatePause = false;
        }

        void OnInjured(OnCharacterInjured e)
        {
            if (_cancelSource!=null)
            {
                //cancel last task
                _cancelSource.Cancel();
                _cancelSource.Dispose();
            }
            
            //
            _cancelSource = new CancellationTokenSource();

            UniTask.RunOnThreadPool(async () =>
            {
                Pause();
                await UniTask.Delay(TimeSpan.FromSeconds(e.Duration-0.2));
                Continue();
            },true,_cancelSource.Token);
        }

        #endregion


        #region MoveMent

        // move and Jump
        void Movement()
        {
            if (_characterController.isGrounded)
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");

                //set threshold
                if (Mathf.Abs(h) < 0.05f && Mathf.Abs(v) < 0.05f)
                {
                    h = 0;
                    v = 0;
                }

                _moveDirection = new Vector3(h, 0, v);

                if (!h.Equals(0) || !v.Equals(0))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //only player press the leftShift and v is not zero does the character entry Run state
                        _currentSpeed = Mathf.Lerp(_currentSpeed, runSpeed, Time.deltaTime * 8);
                    }
                    else
                    {
                        _currentSpeed = Mathf.Lerp(_currentSpeed, walkSpeed, Time.deltaTime * 8);
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
            _characterController.Move(_moveDirection * _currentSpeed * Time.deltaTime);
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
                Time.deltaTime * 8);
        }

        #endregion
    }
}