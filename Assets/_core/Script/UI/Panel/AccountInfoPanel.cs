using System;
using System.Collections;
using System.Collections.Generic;
using _core.AcountInfo;
using MysqlUtility;
using Script.Event;
using Script.Facade;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;
using CharacterInfo = _core.AcountInfo.CharacterInfo;


public class AccountInfoPanel : AbstractUIPanel
{
    [SerializeField] private Text name;
    [SerializeField] private Text sex;
    [SerializeField] private Text age;
    [SerializeField] private Text characterName;
    [SerializeField] private Text characterLevel;
    [SerializeField] private Button modifyPlayerInfoBtn;
    [SerializeField] private Button modifyPasswordBtn;
    [SerializeField] private Button returnBtn;

    public override void OnOpen()
    {
        //register button event
        modifyPlayerInfoBtn.onClick.AddListener(() =>
        {
            GameFacade.Instance.GetInstance<IUIkit>().OpenPanel("ModifyPanel");
        });
        modifyPasswordBtn.onClick.AddListener(() =>
        {
            GameFacade.Instance.GetInstance<IUIkit>().OpenPanel("ModifyPasswordPanel");
        });
        returnBtn.onClick.AddListener(this.CloseSelf);
        
        RefreshPanel();
        
        //register event
        GameFacade.Instance.RegisterEvent<OnInfoCHanged>(e =>
        {
            RefreshPanel();
        }).UnRegisterOnDestroy(gameObject);
    }


    public void RefreshPanel()
    {
        var accountInfo = MysqlTool.GetInfoByKey<AccountInfo>(GameFacade.Instance.GetAccount());
        //display the info
        name.text = $"姓名：{accountInfo.Account}";
        sex.text = $"性别：{ accountInfo.Sex}";
        age.text = $"年龄：{DateTime.Now.Year- Int32.Parse(accountInfo.BornYear)}";

        var characterInfo = MysqlTool.GetInfoByKey<CharacterInfo>(GameFacade.Instance.GetAccount(),DatabaseTable.CharacterInfoTable);
        
        characterName.text = $"角色名：{characterInfo.Name}";
        characterLevel.text = $"经验值：{characterInfo.Exp}";
    }
    protected override void Onclose()
    {
        
    }
}
