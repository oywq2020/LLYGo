using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using _core.AcountInfo;
using DG.Tweening;
using MysqlUtility;
using Script.Event;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

public class ModifyPanel : AbstractUIPanel
{
    [SerializeField] private InputField newName;
    [SerializeField] private Dropdown sexDropdown;
    [SerializeField] private Dropdown bornYearDropdown;
    [SerializeField] private Button cFBtn;
    [SerializeField] private Button returnBtn;

    private bool _nameCf = true;
    private AccountInfo _originInfo;

    public override void OnOpen()
    {
        RefreshPanel();

        //register event
        newName.onValueChanged.AddListener(value =>
        {
            var tip = transform.Find("NewNameInput/tip").gameObject;
            //dynamically match this 
            if (Regex.IsMatch(value, "^[-_a-zA-Z0-9]{4,16}$"))
            {
                _nameCf = true;
                tip.SetActive(false);
            }
            else
            {
                _nameCf = false;
                tip.GetComponent<Text>().text = "账号仅由4到16位，字母数字下划线，减号组成";
                tip.SetActive(true);
                //ease appear
                var canvasGroup = tip.GetComponent<CanvasGroup>();
                canvasGroup.alpha = 0;
                canvasGroup.DOFade(1, 0.3f);
            }
        });

        cFBtn.onClick.AddListener(() =>
        {
            //to indicate whether account is modified;
            bool isAccountChanged = false;
            bool isSexChanged = false;
            bool isBornYear = false;

            var targetInfo = new AccountInfo();

            //judge whether the value is modified 
            if (_originInfo.Account != newName.text)
            {
                targetInfo.Account = newName.text;

                isAccountChanged = true;
            }

            if (_originInfo.Sex != SexAdaptor(sexDropdown.value))
            {
                targetInfo.Sex = SexAdaptor(sexDropdown.value);

                isSexChanged = true;
            }

            if (_originInfo.BornYear != bornYearDropdown.captionText.text)
            {
                targetInfo.BornYear = bornYearDropdown.captionText.text;
                isBornYear = true;
            }

            if (isAccountChanged || isSexChanged || isBornYear)
            {
                if (!_nameCf)
                {
                    //put tips 
                    var openPanel = GameFacade.Instance.GetInstance<IUIkit>().OpenPanel("Tips");
                    openPanel.GetComponent<Text>().text = "账号格式错误！";
                    openPanel.GetComponent<Tips>().SetColorOnce(Color.green);
                    return;
                }
                
                
                //the info is not modified
                var effectedRow = MysqlTool.UpdateDataByKey(GameFacade.Instance.GetAccount(), targetInfo);
                if (effectedRow != 0)
                {
                    //put tips 
                    var openPanel = GameFacade.Instance.GetInstance<IUIkit>().OpenPanel("Tips");
                    openPanel.GetComponent<Text>().text = "修改成功！";
                    openPanel.GetComponent<Tips>().SetColorOnce(Color.green);

                    if (isAccountChanged)
                    {
                        //change the account for GameFacade
                        GameFacade.Instance.UpdateAccount(targetInfo.Account);
                    }
                    
                    //sent event
                    GameFacade.Instance.SendEvent<OnInfoCHanged>();
                }
                
            }
            else
            {
                //the info is modified 
                var openPanel = GameFacade.Instance.GetInstance<IUIkit>().OpenPanel("Tips");
                openPanel.GetComponent<Text>().text = "信息未修改！";
                openPanel.GetComponent<Tips>().SetColorOnce(Color.red);
            }
        });

        returnBtn.onClick.AddListener(CloseSelf);
    }

    protected override void Onclose()
    {
    }


    void RefreshPanel()
    {
        InitiateBornYear();
        _originInfo = MysqlTool.GetInfoByKey<AccountInfo>(GameFacade.Instance.GetAccount());

        newName.text = _originInfo.Account;

        sexDropdown.value = SexAdaptor(_originInfo.Sex);

        //init the bronYear
        bornYearDropdown.value = 2022 - int.Parse(_originInfo.BornYear);
    }

    void InitiateBornYear()
    {
        List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
        for (int i = 2022; i > 1500; i--)
        {
            optionDatas.Add(new Dropdown.OptionData(i.ToString()));
        }

        bornYearDropdown.options = optionDatas;
        bornYearDropdown.value = 20;
    }

    private string SexAdaptor(int i)
    {
        if (i == 0)
        {
            return "男";
        }
        else
        {
            return "女";
        }
    }

    private int SexAdaptor(string sex)
    {
        if (sex == "男")
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
}