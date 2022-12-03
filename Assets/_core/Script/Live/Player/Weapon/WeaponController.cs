using System;
using System.Collections;
using System.Collections.Generic;
using _core.Script.Bag.ScriptableObj.Item;
using Cysharp.Threading.Tasks;
using Player;
using PlayerRegion;
using Script.Event;
using Script.Facade;
using Script.UI;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using Object = System.Object;


public class WeaponController : MonoBehaviour
{
    public Transform playerPalm;

    private Animator _animator;

    //play attack for specified weapon
    private IWeapon _weapon;
    private IAssetFactory _assetFactory;
    private GameObject _weaponHolder;
    private AnimatorController _aniCtrlHolder;

    private GameObject _lastWeapon;

    //private struct variable
    private bool _canAttack = true;
    private int _index; //index for selecting weapon
    private bool _pause = false;

    private Action<int> _hit;
    private Action _endAttack;


    private void Start()
    {
        _animator = GetComponent<Animator>();

        _assetFactory = GameFacade.Instance.GetInstance<IAssetFactory>();

        //register event
        GameFacade.Instance.RegisterEvent<OnShortIndexChanged>(ShortIndexChanged).UnRegisterOnDestroy(gameObject);
        GameFacade.Instance.RegisterEvent<OnMouseEntryGUI>(Pause).UnRegisterOnDestroy(gameObject);
        GameFacade.Instance.RegisterEvent<OnMouseExitGUI>(Continue).UnRegisterOnDestroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (_pause) return;

        //approve this weapon to listening and attack
        _weapon?.ApproveAttack(_animator, () =>
        {
            //turn to camera forward
            transform.rotation = TurnTo(CameraForward());
        });
    }


    private void OnDestroy()
    {
    }

    void ShortIndexChanged(OnShortIndexChanged e)
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
            if (_lastWeapon&&!_lastWeapon.Equals(_weaponHolder))
            {
               //exist weapon switch then release lastWeapon
               Addressables.Release(_lastWeapon);
            }

            _lastWeapon = _weaponHolder;
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
        await UniTask.Delay(TimeSpan.FromSeconds(_weapon.Cd * 2 / 3));

        //for insurance we reset this attack trigger advance
        _animator.ResetTrigger("Attack");

        //end attack previously for better control sense 
        GameFacade.Instance.SendEvent<OnEndAttack>();

        await UniTask.Delay(TimeSpan.FromSeconds(_weapon.Cd / 3));
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
            _weapon?.OnExit();

            _weapon = _weaponHolder.GetComponent<IWeapon>();
            _hit = _weapon.OnHit;
            _endAttack = _weapon.EndAttack;

            //init equipment
            _weapon.OnInit();
        }
        else
        {
            //exit the last equipment
            _weapon?.OnExit();

            _weapon = playerPalm.transform.Find("Empty_Handed").GetComponent<IWeapon>();
            _hit = _weapon.OnHit;
            _endAttack = _weapon.EndAttack;

            //exit the last equipment
            _weapon?.OnInit();
        }
    }

    public void Hit(int attackIndex)
    {
      
        _hit.Invoke(attackIndex);
    }

    public void EndAttack()
    {
        _endAttack.Invoke();
    }

    #endregion

    Vector3 CameraForward()
    {
        var x = this.transform.position.x - Camera.main.transform.position.x;
        var z = transform.position.z - Camera.main.transform.position.z;
        return new Vector3(x, 0, z);
    }

    Quaternion TurnTo(Vector3 cameraDir, float offset = 0)
    {
        Quaternion q = Quaternion.identity;
        q.SetLookRotation(cameraDir);
        return Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, q.eulerAngles.y + offset, 0),
            Time.deltaTime * 8);
    }
}