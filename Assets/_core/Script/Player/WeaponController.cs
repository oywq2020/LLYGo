using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Player;
using Script.Event;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;


public class WeaponController : MonoBehaviour
{
    public Transform playerPalm;

    private Animator _animator;

    //play attack for specified weapon
    private IWeapon _weapon;
    private IAssetFactory _assetFactory;

    //private struct variable
    private bool _canAttack = true;
    private int _index; //index for selecting weapon

    private void Start()
    {
        //get weapon interface
        _weapon = playerPalm.GetChild(0).GetComponent<IWeapon>();
        _animator = GetComponent<Animator>();

        _assetFactory = GameFacade.Instance.GetInstance<IAssetFactory>();
        
        //register event
        GameFacade.Instance.RegisterEvent<OnShortIdexChanged>(ShortIndexChanged);
    }

    private void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
        }

        //switch weapon
        var axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0)
        {
            if (axis > 0.1)
            {
                _index++;
            }
            else if (axis < -0.1)
            {
                _index--;
            }
        }
    }

    private void OnDestroy()
    {
        GameFacade.Instance?.UnRegisterEvent<OnShortIdexChanged>(ShortIndexChanged);
    }

    void ShortIndexChanged(OnShortIdexChanged e)
    {
        
    }
    
    void SwitchWeapon()
    {
        _assetFactory.InstantiateGameObject("sword", playerPalm);
        _animator.runtimeAnimatorController = _assetFactory.LoadAsset<AnimatorController>("knight");
    }
    
    async void RecoverCd()
    {
        _canAttack = false;
        await UniTask.Delay(TimeSpan.FromSeconds(_weapon.Cd));
        _canAttack = true;
    }
}