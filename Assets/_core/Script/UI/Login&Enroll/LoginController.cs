using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MysqlUtility;
using PlayerRegion;
using SceneStateRegion;
using Script.Event;
using Script.Facade;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

public class LoginController : AbstractUIPanel
{
    
    public GameFacade accountEntity;
    
    private InputField _nameInput;
    private InputField _passwordInput;
    private Toggle _rememberToggle;

    private IUIkit _uIkit;

    private bool _isRememberPassword = true;
    
    public override void OnOpen()
    {
        //get input
        _nameInput = transform.Find("InputName").GetComponent<InputField>();
        _passwordInput = transform.Find("InputPassword").GetComponent<InputField>();

        //register button event
        transform.Find("LoginBtn").GetComponent<Button>().onClick.AddListener(EngageLogin);
        transform.Find("EnrollBtn").GetComponent<Button>().onClick.AddListener(Enroll);
        
        ToggleInit();

        InputFieldInit();

        //assign
        _uIkit = GameFacade.Instance.GetInstance<IUIkit>();
        
        Debug.Log("OnOpen");

        if (_isRememberPassword)
        {
          
        }
        
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
                    //create a current player for whole game
                    if (CurrentPlayer.Instance)
                    {
                        
                    }
                    
                  //  var account = GameFacade.Instantiate(accountEntity);
                    


                    GameLoop.Instance.Controller.SetState(new MainState(GameLoop.Instance.Controller)).Forget();


                    if (_isRememberPassword)
                    {
                        //record the last account
                        PlayerPrefs.SetString("Account",myName);
                        //record the password
                        PlayerPrefs.SetString("Password",myPassword);
                        PlayerPrefs.Save();
                    }
                   
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

    void ToggleInit()
    {
        var toggle = transform.Find("RememberToggle").GetComponent<Toggle>();

        //register listening event
        toggle.onValueChanged.AddListener(e =>
        {
            if (e)
            {
                _isRememberPassword = true;
                PlayerPrefs.SetInt("IsRemember",1);
            }
            else
            {
                _isRememberPassword = false;
                PlayerPrefs.SetInt("IsRemember",-1);
                
                //clear the store of password
                PlayerPrefs.SetString("Password","");
                PlayerPrefs.Save();
            }
           
        });
        
        
        //init toggle
        var flag = PlayerPrefs.GetInt("IsRemember");
        if (flag!=0)
        {
            if (flag==1)
            {
                toggle.isOn = true;
            }else if (flag == -1)
            {
                toggle.isOn = false;
            }
        }
    }

    void InputFieldInit()
    {
        //remember account by default
        _nameInput.text = PlayerPrefs.GetString("Account");
        
        if (_isRememberPassword)
        {
            _passwordInput.text = PlayerPrefs.GetString("Password");
        }
    }
}