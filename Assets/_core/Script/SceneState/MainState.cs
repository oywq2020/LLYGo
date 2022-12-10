using _core.Script.Live.Player.Character;
using Cysharp.Threading.Tasks;
using DialogueQuests;
using Script.Abstract;
using Script.Event;
using Script.Event.Camera;
using Script.Event.CharacterMove;
using Script.Facade;
using Script.UI;
using UnityEngine;

namespace SceneStateRegion
{
    public class MainState : AbstractState
    {
        private IUIkit _uIkit;

        private bool _isBagOpen = false;

        private IUnRegisterList _unRegisterList = new UnRegister();

        public MainState(SceneStateController stateController) : base("Main", stateController)
        {
        }

        public override async void StateStart()
        {
            base.StateStart();
            //load weapon bar
            _uIkit = GameFacade.Instance.GetInstance<IUIkit>();
            //register event
            RegisterList();

            //create character


            // var gameObject = GameFacade.Instance.GetInstance<IGameObjectPool>().Dequeue("lly");
            // gameObject.GetComponent<ICharacterStatus>().InitCharacterStatus(500);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public override void StateUpdate()
        {
            //listening the keyboard event in main scene

            if (Input.GetKeyDown(KeyCode.B))
            {
                //open bag
                if (_isBagOpen)
                {
                    _uIkit.ClosePanel("BagPanel");
                }
                else
                {
                    _uIkit.OpenPanel("BagPanel");
                }

                _isBagOpen = !_isBagOpen;
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                Debug.Log("Set CanSpeak to one");
               NarrativeData.Get().SetCustomInt("CanSpeak",1);
            }
        }


        private void RegisterList()
        {
            //register OnMasterDead for increasing the EXP of character
            GameFacade.Instance.RegisterEvent<OnMasterDead>(e =>
            {
                //to catch the message of the master dying
                GameFacade.Instance.SendEvent(new OnExpIncreased()
                {
                    //add  exp for character
                    increment = e.Exp
                });
            }).AddToUnRegisterList(_unRegisterList);

            //register the pause of character movement 
           


            EventTransfer();
        }


        //used to transfer event making higher layer event to specific event of operation
        private async void EventTransfer()  
        {
            
            GameFacade.Instance.RegisterEvent<OnMouseEntryGUI>(e =>
            {
                GameFacade.Instance.SendEvent(new ChangeWeaponState()
                {
                    IsCanAttack = false
                });
            }).AddToUnRegisterList(_unRegisterList);
            
            GameFacade.Instance.RegisterEvent<OnMouseExitGUI>(e =>
            {
                GameFacade.Instance.SendEvent(new ChangeWeaponState()
                {
                    IsCanAttack = true
                });
            }).AddToUnRegisterList(_unRegisterList);

            GameFacade.Instance.RegisterEvent<OnStartAttack>(e =>
            {
                GameFacade.Instance.SendEvent(new ChangeMoveState()
                {
                    IsCanMove = false
                });
            });
            GameFacade.Instance.RegisterEvent<OnEndAttack>(e =>
            {
                GameFacade.Instance.SendEvent(new ChangeMoveState()
                {
                    IsCanMove = true
                });
            });

            GameFacade.Instance.RegisterEvent<OnMouseEntryGUI>(e =>
            {
                GameFacade.Instance.SendEvent(new ChangeCameraState()
                {
                    IsPause = true
                });
            });
            
            GameFacade.Instance.RegisterEvent<OnMouseExitGUI>(e =>
            {
                GameFacade.Instance.SendEvent(new ChangeCameraState()
                {
                    IsPause = false
                });
            });
            
            

            //wait this instance instantiating 
            await UniTask.WaitUntil(() => NarrativeManager.Get()!=null);
            
            Debug.Log("Register NarrativeMangerEvent");
            NarrativeManager.Get().onPauseGameplay += () =>
            {
                GameFacade.Instance.SendEvent(new ChangeMoveState()
                {
                    IsCanMove = false
                });
                
                GameFacade.Instance.SendEvent(new ChangeWeaponState()
                {
                    IsCanAttack = false
                });
                
                GameFacade.Instance.SendEvent(new ChangeWheelState()
                {
                    IsEnable = false
                });
                
                GameFacade.Instance.SendEvent(new ChangeCameraState()
                {
                    IsPause = true
                });
            };
            
            NarrativeManager.Get().onUnpauseGameplay += () =>
            {
                
                GameFacade.Instance.SendEvent(new ChangeMoveState()
                {
                    IsCanMove = true
                });
                GameFacade.Instance.SendEvent(new ChangeWeaponState()
                {
                    IsCanAttack = true
                });
                GameFacade.Instance.SendEvent(new ChangeWheelState()
                {
                    IsEnable = true
                });
                
                GameFacade.Instance.SendEvent(new ChangeCameraState()
                {
                    IsPause = false
                });
            };
            
           
        }

        public override void StateEnd()
        {
            //unRegister all event registered in this main state
            _unRegisterList.UnRegisterAll();
            base.StateEnd();
        }
    }
}