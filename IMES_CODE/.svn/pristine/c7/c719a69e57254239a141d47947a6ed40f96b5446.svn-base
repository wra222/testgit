using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using log4net;

namespace AlarmMail
{
    public static class MailTool
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static public bool SendMail(List<string> mailTo,
                                                  List<string> mailCC,
                                                  string mailSubject,
                                                  string mailBody,
                                                  string attachFile,
                                                  out string error)
        {

            try
            {
                error = "";
                MailMessage mailMsg = new MailMessage();
                if (string.IsNullOrEmpty(mailSubject) || mailTo.Count == 0)
                    return false;

                string mailServer = ConfigurationManager.AppSettings["MailServer"];
                string mailFrom = ConfigurationManager.AppSettings["MailFromAddress"];

                mailMsg.From = new MailAddress(mailFrom);

                foreach (string add in mailTo)
                {
                    if (add.TrimEnd().Length > 0)
                    {
                        mailMsg.To.Add(new MailAddress(add.TrimEnd()));
                    }
                }

                foreach (string add in mailCC)
                {
                    if (add.TrimEnd().Length > 0)
                    {
                        mailMsg.CC.Add(new MailAddress(add.TrimEnd()));
                    }
                }

                mailMsg.Subject = mailSubject;
                mailMsg.Body = mailBody;
                mailMsg.IsBodyHtml = true;
                if (!string.IsNullOrEmpty(attachFile))
                {
                    Attachment att = new Attachment(attachFile);
                    mailMsg.Attachments.Add(att);
                }
              
                SmtpClient client = new SmtpClient(mailServer, 25);
                if (mailMsg.To.Count > 0)
                {
                    client.Send(mailMsg);
                }
                return true;
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod(), e);
                error = e.Message;
                return false;

            }

        }
    }
}
