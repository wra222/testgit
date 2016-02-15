/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: 
 *              
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2010-2-20   itc207024           create
 * 
 * Known issues:Any restrictions about this file 
 *          
 */
using System;
using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using log4net;
using System.Configuration;



/// <summary>
/// Summary description for MailSender
/// </summary>
namespace com.inventec.fisreport.system.util
{
    public class MailSender
    {
        private string strMessage = "";
        private MailMessage message = null;

        private string body = "";
        private string subject = "";
        private SmtpClient client = null;
        private IList cc = new ArrayList();
        private IList bcc = new ArrayList();
        private bool mailSent = false;//标志邮件是否发送成功

        private static readonly ILog log = LogManager.GetLogger("fisLog");
        
        public MailSender()
        {
            
        }

        public  bool sendMail(IList to, IList cc, IList bcc, IList attachments, string subject, string body)
        {
            try
            {
                if (!isHaveReceiver(to, cc, bcc))
                {
                    log.Debug("No mailto");
                    return false;
                }

                this.subject = subject;
                this.body = body;
                this.bcc = bcc;
                this.cc = cc;
                dealMailMessage();
                dealRecever(to, "to");
                dealRecever(cc, "cc");
                dealRecever(bcc, "bcc");
                dealAttachments(attachments);

                dealSmtpClient();
            }
            catch(Exception e)
            {
                log.Debug(e.Message);
                log.Debug(e.StackTrace);
                throw new ApplicationException("Send Mail Error!");
            }
            return true;

        }

        public void dealRecever(IList to, string receverType)
        {

            foreach (string strTo in to)
            {

                if (!strTo.Trim().Equals(""))
                {
                    MailAddress addr = new MailAddress(strTo);
                    if (receverType.Equals("to"))
                    {
                        message.To.Add(addr);
                    }
                    else if (receverType.Equals("cc"))
                    {
                        message.CC.Add(addr);
                    }
                    else if (receverType.Equals("bcc"))
                    {
                        message.Bcc.Add(addr);
                    }


                }
            }


        }

        public void dealAttachments(IList attachments)
        {
	    if (attachments == null || attachments.Count == 0)
            {
                return;
            }

            foreach (Attachment attachment in attachments)
            {
                message.Attachments.Add(attachment);
            }
        }



        public void dealMailMessage()
        {
            try
            {
                message = new MailMessage();


              
                message.Subject = subject;
                message.SubjectEncoding = System.Text.Encoding.UTF8; 


                message.BodyEncoding = System.Text.Encoding.UTF8; 
                message.Body = body;
                
               
                message.IsBodyHtml = true;
            }
            catch (Exception e)
            {
                log.Debug("" + e.StackTrace);
            }

          
        }

        public void dealSmtpClient()
        {
            client = new SmtpClient();
            //client.Host = ConfigurationManager.AppSettings["SmtpMailServer"].ToString(); ;
            //message.From = new MailAddress(ConfigurationManager.AppSettings["MailFrom"].ToString());
            client.EnableSsl = false;
            client.UseDefaultCredentials = true;
            //client.Timeout = Int32.Parse(ConfigurationManager.AppSettings["SendMailTimeOut"].ToString());
            //client.SendCompleted += new
                    //SendCompletedEventHandler(SendCompletedCallback);

                client.Send(message);
        }



        public  void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {

            String token = (string)e.UserState;

            if (e.Error != null)
            {

                log.Debug( e.Error.ToString());

            }

            else
            {
                mailSent = true;
                log.Debug("mail send successful");

            }

        }

        private bool isHaveReceiver(IList to, IList cc, IList bcc)
        {
            if ((to == null || to.Count == 0) && (cc == null || cc.Count == 0) && (bcc == null || bcc.Count == 0))
            {
                return false;
            }

            return true;
        }

    }
}

