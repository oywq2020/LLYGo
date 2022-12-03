using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DG.Tweening;
using MysqlUtility;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnrollController : AbstractUIPanel
{
    private InputField _accountInput;
    private InputField _passwordInput;
    private Dropdown _sexDropdown;
    private Dropdown _bornYearDropdown;

    private bool _nameCf = false;
    private bool _passwordCf = false;

    private IUIkit _uIkit;
    public override void OnOpen()
    {
        //initiate ui tool
        _uIkit = GameFacade.Instance.GetInstance<IUIkit>();
        
        //get input
        _accountInput = transform.Find("InputAccount").GetComponent<InputField>();
        _passwordInput = transform.Find("InputPassword").GetComponent<InputField>();
        _sexDropdown = transform.Find("SexDropdown").GetComponent<Dropdown>();
        _bornYearDropdown = transform.Find("BornYearDropdown").GetComponent<Dropdown>();

        //initiate bornyear
        InitiateBornYear();
        //register Input check event
        RegisterInputCheckEvent();
        //register button event
        
        transform.Find("EnrollBtn").GetComponent<Button>().onClick.AddListener(Enroll);
        transform.Find("ReturnBtn").GetComponent<Button>().onClick.AddListener(Return);
    }

    protected override void Onclose()
    {
    }
    
    void Enroll()
    {
        //get input 
        var myAccount = _accountInput.text;
        var myPassword = _passwordInput.text;
        var mySex = _sexDropdown.captionText.text;
        var myBornYear = _bornYearDropdown.captionText.text;

        //format check for input stage

        if (!FormatCheck())
        {
            _uIkit.OpenPanel("Tips").GetComponent<Text>().text = "账号或密码格式错误！";
            return;
        }
         
        
        var password = MysqlTool.GetPassword(myAccount);
        if (password == "")
        {
            //insert the player into database
            MysqlTool.AddPlayer(myAccount, myPassword, mySex, myBornYear);
            
            //register successfully
            var openPanel = _uIkit.OpenPanel("Tips");
            openPanel.GetComponent<Text>().text = "注册成功！";
            openPanel.GetComponent<Tips>().SetColorOnce(Color.green);
            
            //close this panel
            _uIkit.ClosePanel(gameObject);
        }
        else
        {
            //this name is already enrolled
            _uIkit.OpenPanel("Tips").GetComponent<Text>().text = "改账号已经被注册！";
        }
    }


    void RegisterInputCheckEvent()
    {
        _accountInput.onValueChanged.AddListener(value =>
        {
            var tip = transform.Find("InputName/tip").gameObject;
            //dynamically match this 
            if (Regex.IsMatch(value, "^[-_a-zA-Z0-9]{4,16}$"))
            {
                _nameCf = true;
                tip.SetActive(false);
            }
            else
            {
                _nameCf = false;

                tip.GetComponent<Text>().text = "账号由4到16位，字母数字下划线，减号组成";
                tip.SetActive(true);

                //ease appear
                var canvasGroup = tip.GetComponent<CanvasGroup>();
                canvasGroup.alpha = 0;
                canvasGroup.DOFade(1, 0.3f);
            }
        });
        _passwordInput.onValueChanged.AddListener(value =>
        {
            //dynamically match this 
            var tip = transform.Find("InputPassword/tip").gameObject;
            
            if (Regex.IsMatch(value, "(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[$@!%*#?&])[A-Za-z\\d$@!%*#?&]{6,}$"))
            {
                _passwordCf = true;
                tip.SetActive(false);
            }
            else
            {
                _passwordCf = false;
                
                tip.GetComponent<Text>().text = "密码由至少由一个大写、小写和一个特色字符与数字组成";
                tip.SetActive(true);
                
                //ease appear
                var canvasGroup = tip.GetComponent<CanvasGroup>();
                canvasGroup.alpha = 0;
                canvasGroup.DOFade(1, 0.3f);
            }
        });
    }

    bool FormatCheck()
    {
        if (_nameCf&&_passwordCf)
        {
            return true;
        }
        return false;
    }

    void InitiateBornYear()
    {
        List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
        for (int i = 2022; i > 1500; i--)
        {
            optionDatas.Add(new Dropdown.OptionData(i.ToString()));
        }
        _bornYearDropdown.options = optionDatas;
        _bornYearDropdown.value = 20;
    }

    void Return()
    {
        _uIkit.ClosePanel(gameObject);
    }

  
}