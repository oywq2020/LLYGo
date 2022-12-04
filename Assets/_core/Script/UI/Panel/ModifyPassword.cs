using System.Text.RegularExpressions;
using DG.Tweening;
using MysqlUtility;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _core.Script.UI.Panel
{
    public class ModifyPassword:AbstractUIPanel
    {
        [SerializeField] private InputField oldPassword;
        [SerializeField] private InputField newPassword;
        [SerializeField] private Button cFBtn;
        [SerializeField] private Button returnBtn;

        private bool _passwordCf = false;
        
        public override void OnOpen()
        {
            cFBtn .onClick.AddListener(CFModifyPassword);
            returnBtn.onClick.AddListener(CloseSelf);
            
            //register event
            newPassword.onValueChanged.AddListener(value =>
            {
                var tip = transform.Find("NewPassword/tip").gameObject;
                //dynamically match this 
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

        private void CFModifyPassword()
        {
            
            var password = MysqlTool.GetPassword(GameFacade.Instance.GetAccount());
            if (oldPassword.text==password)
            {
                if (oldPassword.text == newPassword.text)
                {
                    var openPanel = GameFacade.Instance.GetInstance<IUIkit>().OpenPanel("Tips");
                    openPanel.GetComponent<Text>().text = "新密码与原密码相同！";
                    openPanel.GetComponent<Tips>().SetColorOnce(Color.red);
                    return;
                }


                if (_passwordCf)
                {
                    var updatePassword = MysqlTool.UpdatePassword(GameFacade.Instance.GetAccount(), oldPassword.text);
                    if (updatePassword!=0)
                    {
                        var openPanel = GameFacade.Instance.GetInstance<IUIkit>().OpenPanel("Tips");
                        openPanel.GetComponent<Text>().text = "修改成功！";
                        openPanel.GetComponent<Tips>().SetColorOnce(Color.green);
                        
                        //close self 
                        CloseSelf();
                    }
                }
                else
                {
                    var openPanel = GameFacade.Instance.GetInstance<IUIkit>().OpenPanel("Tips");
                    openPanel.GetComponent<Text>().text = "密码格式错误！";
                    openPanel.GetComponent<Tips>().SetColorOnce(Color.red);
                }
                
            }
            else
            {
                var openPanel = GameFacade.Instance.GetInstance<IUIkit>().OpenPanel("Tips");
                openPanel.GetComponent<Text>().text = "原密码输入错误！";
                openPanel.GetComponent<Tips>().SetColorOnce(Color.red);
            }
        }

        protected override void Onclose()
        {
           
        }
    }
}