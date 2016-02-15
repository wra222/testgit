using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
namespace ArchiveDB
{
     class Mail
    {
        private string  _smtp;
        private string  _toList;
        private bool _isTest;
        private string _dbName;
        private bool _enableMail;
        public Mail()
        {
            Config config = new Config();
            _smtp=config.GetValue("SMTP");
            _toList = config.GetValue("MailTo");
            _isTest = false;
            _dbName=config.GetValue("DBName");
            _enableMail = config.GetValue("EnableMail") == "Y" ? true : false;
        }
        public bool IsTest
        {
            set { _isTest = value; }
            get { return _isTest; }
        } 
        public  void Send(string subject, string body)
        {
            if (!_enableMail) { return; }
    //       return;
            MailMessage message = new MailMessage();
            MailAddress from = new MailAddress("CQFis_Agent@inventec.com.cn", "Archive " + _dbName + " DB");
            message.From = from;
            if (!IsTest)
            {
                string[] toListArr = _toList.Split(',');
                foreach (String addr in toListArr)
                {
                    message.To.Add(new MailAddress(addr));

                }
            }
            else
            {
                message.To.Add("he.jia-you@inventec.com.cn");
            }
          
  
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;//E-mail編碼
            message.Body = body;
            message.Subject = subject;
            SmtpClient smtpClient = new SmtpClient(_smtp, 25);//設定E-mail Server和port
            try
            { smtpClient.Send(message); }
            catch(Exception e)
            {
                if (IsTest) { throw e; }
            }
           
            // EnableBtnExcelandMail(true); showErrorMessage("Mail successfully", this);

            //message.To.Add("Chen.BensonPM@inventec.com");

            //List<string> lst = new List<string>() { "87ReportMailList" };
            //DataTable dt = GetSysSetting(lst, CmbDBType.ddlGetConnection());
            //string mailValue = dt.Rows[0]["Value"].ToString();

            ////   showErrorMessage(mailValue, this); return;
            //if (string.IsNullOrEmpty(mailValue))
            //{
            //    EnableBtnExcelandMail(true); showErrorMessage("Please define mail list", this); return;
            //}
          //  string[] mailList = mailValue.Split(';');
         //   string addr = "";
            //if (IsDebug)
            //{
            //    message.To.Add(new MailAddress("Chen.BensonPM@inventec.com"));

            //}
            //else
            //{
            //    foreach (String s in mailList)
            //    {
            //        addr = s;
            //        if (s.IndexOf("@") == -1)
            //        {
            //            addr = s + "@inventec.com.cn";
            //        }
            //        message.To.Add(new MailAddress(addr));
            //    }
            //}
         
        }


    }
}
