using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MysqlUtility;
using SceneStateRegion;
using Script.Event;
using Script.Facade;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

public class LoginController : AbstractUIPanel
{
    private InputField _nameInput;
    private InputField _passwordInput;

    private IUIkit _uIkit;

    public override void OnOpen()
    {
        //get input
        _nameInput = transform.Find("InputName").GetComponent<InputField>();
        _passwordInput = transform.Find("InputPassword").GetComponent<InputField>();

        //register button event
        transform.Find("LoginBtn").GetComponent<Button>().onClick.AddListener(EngageLogin);
        transform.Find("EnrollBtn").GetComponent<Button>().onClick.AddListener(Enroll);

        //assign
        _uIkit = GameFacade.Instance.GetInstance<IUIkit>();
        
        Debug.Log("OnOpen");
    }

    protected override void Onclose()
    {
    }

    void EngageLogin()
    {
        //get input 
        var myName = _nameInput.text;
        var myPassword = _passwordInput.text;

        //check input
        if (myName != "" && myPassword != "")
        {
            var password = MysqlTool.GetPassword(myName);
            if (password != "")
            {
                if (password == myPassword)
                {
                    GameLoop.Instance.Controller.SetState(new MainState(GameLoop.Instance.Controller)).Forget();
                    Debug.Log("login successfully");
                }
                else
                {
                    // password error
                    _uIkit.OpenPanel("Tips").GetComponent<Text>().text = "密码错误！";
                }
            }
            else
            {
                // not find this name from database
                _uIkit.OpenPanel("Tips").GetComponent<Text>().text = "找不到账号，请注册！";
            }
        }
        else
        {
            // not string is input

            _uIkit.OpenPanel("Tips").GetComponent<Text>().text = "账号或密码为空！";
        }
    }

    void Enroll()
    {
        _uIkit.OpenPanel("Enroll");
    }
}