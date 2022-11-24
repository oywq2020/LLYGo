using System;
using System.Collections;
using System.Collections.Generic;
using _core.Script.Bag.ScriptableObj.Item;
using Cysharp.Threading.Tasks;
using Player;
using PlayerRegion;
using Script.Event;
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

  
    

    private void Start()
    {
        //get weapon interface
        _weapon = playerPalm.GetChild(0).GetComponent<IWeapon>();
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
            _weapon.Play();

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
        var currentItem = CurrentPlayer.Instance._bag.itemList[e.Index];

        
        
        if (currentItem&&currentItem.isEquip)
        {
            //switch equipment
            //load this weapon
            _weaponHolder= (currentItem as WeaponItem).swordGameObjectRf.InstantiateAsync(playerPalm).WaitForCompletion();
            
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
            if (playerPalm.childCount>2)
            {
                //character's hand only can hold one item;
                Addressables.Release(playerPalm.GetChild(1).gameObject);
            }
        }
        else
        {
            _animator.runtimeAnimatorController = _assetFactory.LoadAsset<AnimatorController>("EmptyHanded");

            if (playerPalm.childCount>1)
            {
              Addressables.Release(_weaponHolder);
            }
        }
      
        //refresh _weapon
        _weapon = playerPalm.GetChild(0).GetComponent<IWeapon>();
     
    }
    
    async void RecoverCd()
    {
        _canAttack = false;
        await UniTask.Delay(TimeSpan.FromSeconds(_weapon.Cd));
        _canAttack = true;
    }


    void Pause(OnMouseEntryGUI e)
    {
        _pause = true;
    }
    void Continue(OnMouseExitGUI e)
    {
        _pause = false;
    }
}