using AutoScript.Share;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AutoScript.Server
{
    /// <summary>
    /// 账号类
    /// </summary>
    public class Account
    {
        //定义私有成员 用于保存所有账号
        private static List<Account> AllAccounts=new List<Account>();

        public static List<Account> GetAccounts()
        {

           return AllAccounts; 
        }

        public static List<Account> Reload(string fileName) {

            AllAccounts.Clear();

            DataTable dt = Utility.LoadFileData(fileName, Encoding.UTF8);
            int i = 0;
            foreach (DataRow r in dt.Rows)
            {
                Account account = new Account()
                {
                    Index = i++,
                    UserName = r["用户名"].ToString(),
                    Password = r["密码"].ToString()
                };
                AllAccounts.Add(account);
            }
            return AllAccounts;

        }

        public static Account GetNextAccount()
        {
            Account account = AllAccounts.FirstOrDefault(t => string.IsNullOrEmpty(t.Status) || t.Status == "Stop");
            if (account != null) account.Status = "Ready";
            return account;
        }
        /// <summary>
        /// 状态变化事件
        /// </summary>
        public static event EventHandler<Account> OnStatusChangedEvent;







        //定义属性,用于描述账号的信息
        public int Index { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Hwnd { get; set; }
        public string DeviceName {  get; set; }
        private string _stauts = "";


        public string Status
        {
            get { return _stauts; }
            set { 
                
                if (_stauts != value) {
                    //产生一个事件
                    _stauts = value;
                    if (OnStatusChangedEvent != null)
                    {
                        OnStatusChangedEvent(this, null);

                    }
                }
                else
                {
                    _stauts = value;
                }
            
            }
        }
    }
}