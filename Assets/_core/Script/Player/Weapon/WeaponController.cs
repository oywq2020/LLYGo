using System;
using System.Collections;
using System.Collections.Generic;
using _core.Script.Bag.ScriptableObj.Item;
using Cysharp.Threading.Tasks;
using Player;
using PlayerRegion;
using Script.Event;
using Script.UI;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;


public class WeaponController : MonoBehaviour
{
    public Transform playerPalm;

    private Animator _animator;

    //play attack for specified weapon
    private IWeapon _weapon;
    private IAssetFactory _assetFactory;
    private GameObject _weaponHolder;
    private AnimatorController _aniCtrlHolder;

    //private struct variable
    private bool _canAttack = true;
    private int _index; //index for selecting weapon
    private bool _pause = false;

    private Action _hit;
    private Action _endAttack;


    private void Start()
    {
        _animator = GetComponent<Animator>();

        _assetFactory = GameFacade.Instance.GetInstance<IAssetFactory>();

        //register event
        GameFacade.Instance.RegisterEvent<OnShortIdexChanged>(ShortIndexChanged);
        GameFacade.Instance.RegisterEvent<OnMouseEntryGUI>(Pause);
        GameFacade.Instance.RegisterEvent<OnMouseExitGUI>(Continue);
    }

    private void Update()
    {
        if (_pause) return;

        //left mouse is default for normal attack
        if (Input.GetKey(KeyCode.Mouse0) && _canAttack)
        {
            //play
            _weapon.StartHit();

            //animator play
            _animator.SetTrigger("Attack");
            //enter cd
            RecoverCd();
        }
    }

    private void OnDestroy()
    {
        GameFacade.Instance?.UnRegisterEvent<OnShortIdexChanged>(ShortIndexChanged);
        GameFacade.Instance?.UnRegisterEvent<OnMouseEntryGUI>(Pause);
        GameFacade.Instance?.UnRegisterEvent<OnMouseExitGUI>(Continue);
    }

    void ShortIndexChanged(OnShortIdexChanged e)
    {
        var currentItem = CurrentPlayer.Instance.GetBag().itemList[e.Index];

        if (currentItem && currentItem.isEquip)
        {
            //switch equipment
            //load this weapon
            _weaponHolder = (currentItem as WeaponItem).swordGameObjectRf.InstantiateAsync(playerPalm)
                .WaitForCompletion();

            //assign name 
            _weaponHolder.name = currentItem.itemName;

            //switch animator controller
            if ((currentItem as WeaponItem).aniCtrl.Asset)
            {
                _animator.runtimeAnimatorController = (currentItem as WeaponItem).aniCtrl.Asset as AnimatorController;
            }
            else
            {
                _animator.runtimeAnimatorController = (currentItem as WeaponItem).aniCtrl
                    .LoadAssetAsync<AnimatorController>().WaitForCompletion();
            }

            //release other item before
            if (playerPalm.childCount > 2)
            {
                //character's hand only can hold one item;
                Addressables.Release(playerPalm.GetChild(1).gameObject);
            }
        }
        else
        {
            _animator.runtimeAnimatorController = _assetFactory.LoadAsset<AnimatorController>("EmptyHanded");
            if (playerPalm.childCount > 1)
            {
                Addressables.Release(_weaponHolder);
            }
        }

        RefreshHandedWeapon(currentItem);
    }

    async void RecoverCd()
    {
        //start attack
        GameFacade.Instance.SendEvent<OnStartAttack>();
        
        _canAttack = false;
        await UniTask.Delay(TimeSpan.FromSeconds(_weapon.Cd / 2));

        //for insurance we reset this attack trigger advance
        _animator.ResetTrigger("Attack");

        //end attack previously for better control sense 
        GameFacade.Instance.SendEvent<OnEndAttack>();
        
        await UniTask.Delay(TimeSpan.FromSeconds(_weapon.Cd / 2));
        _canAttack = true;
        
        //start attack
       
    }


    void Pause(OnMouseEntryGUI e)
    {
        _pause = true;
    }

    void Continue(OnMouseExitGUI e)
    {
        _pause = false;
    }


    #region AnimationFrameEvent

    void RefreshHandedWeapon(AbstractItemScrObj scrObj)
    {
        if (scrObj)
        {
            //exit the last equipment
            _weapon?.Exit();

            _weapon = playerPalm.transform.Find(scrObj.itemName).GetComponent<IWeapon>();
            _hit = _weapon.Hit;
            _endAttack = _weapon.EndHit;

            //init equipment
            _weapon.Init();
        }
        else
        {
            //exit the last equipment
            _weapon?.Exit();

            _weapon = playerPalm.transform.Find("Empty_Handed").GetComponent<IWeapon>();
            _hit = _weapon.Hit;
            _endAttack = _weapon.EndHit;

            //exit the last equipment
            _weapon?.Init();
        }
    }

    public void Hit()
    {
       

        _hit.Invoke();
    }

    public void EndAttack()
    {
        _endAttack.Invoke();

       
    }

    #endregion
}