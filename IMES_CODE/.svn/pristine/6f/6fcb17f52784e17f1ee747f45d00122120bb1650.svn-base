using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using log4net;
using System.Reflection;

namespace IMES.Query.DB
{
    public class SendMail
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void Send(string mailFrom,
                                                    string[] mailTo,
                                                    string mailSubject,
                                                    string mailBody,
                                                    string mailServer)
        {
            try
            {
                MailMessage mailMsg = new MailMessage();

                if (string.IsNullOrEmpty(mailSubject) || mailTo.Length == 0)
                    return;

                mailMsg.From = new MailAddress(mailFrom);

                if (mailTo.Length > 0)
                {
                    for (int i = 0; i < mailTo.Length; ++i)
                    {
                        if (mailTo[i].Trim().Length > 0)
                        {
                            mailMsg.To.Add(new MailAddress(mailTo[i]));
                        }
                    }
                }


                
                mailMsg.Subject = mailSubject;
                mailMsg.Body = mailBody;
                mailMsg.IsBodyHtml = true;

                SmtpClient client = new SmtpClient(mailServer, 25);
                if (mailMsg.To.Count > 0)
                {
                    client.Send(mailMsg);
                }

            }
            catch (Exception e)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name, e);            
                
            }
        }

        public static void Send(string mailFrom,
                                                    string[] mailTo,
                                                    string[] mailCC,
                                                    string mailSubject,
                                                    string mailBody,
                                                    string mailServer)
        {
            try
            {
                MailMessage mailMsg = new MailMessage();

                if (string.IsNullOrEmpty(mailSubject) || mailTo.Length == 0)
                    return;

                mailMsg.From = new MailAddress(mailFrom);

                if (mailTo.Length > 0)
                {
                    for (int i = 0; i < mailTo.Length; ++i)
                    {
                        if (mailTo[i].Trim().Length > 0)
                        {
                            mailMsg.To.Add(new MailAddress(mailTo[i]));
                        }
                    }
                }


                if (mailCC.Length > 0)
                {
                    for (int i = 0; i < mailCC.Length; ++i)
                    {
                        if (mailCC[i].Trim().Length > 0)
                        {
                            mailMsg.CC.Add(new MailAddress(mailCC[i]));
                        }
                    }
                }



                mailMsg.Subject = mailSubject;
                mailMsg.Body = mailBody;
                mailMsg.IsBodyHtml = true;

                SmtpClient client = new SmtpClient(mailServer, 25);
                if (mailMsg.To.Count > 0)
                {
                    client.Send(mailMsg);
                }

            }
            catch (Exception e)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name, e); 
            }
        }
    }
}
