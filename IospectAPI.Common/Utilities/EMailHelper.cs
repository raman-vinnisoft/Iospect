﻿using IospectAPI.Common.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace IospectAPI.Common.Utilities
{
    public  class EMailHelper
    {
        private static IConfiguration _configuration;
        public EMailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
         
        }
        public bool SendMail(EmailModel emailModel)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = _configuration.GetSection("Smtp:Host").Value;
                smtpClient.Credentials = new NetworkCredential(_configuration.GetSection("Smtp:UserName").Value, _configuration.GetSection("Smtp:Password").Value);
                smtpClient.Port = Convert.ToInt32(_configuration.GetSection("Smtp:Port").Value);

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_configuration.GetSection("Smtp:From").Value);
                    mail.To.Add(emailModel.Recipient);
                    mail.Subject = emailModel.Subject;
                    mail.Body = emailModel.Body;
                    mail.IsBodyHtml = true;

                    if (!string.IsNullOrEmpty(emailModel.ReplyToListEmail))
                    {
                        mail.ReplyToList.Add(new MailAddress(emailModel.ReplyToListEmail, emailModel.ReplyToListName)); ;
                    }

                    try
                    {
                        smtpClient.Send(mail);
                        
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        public string GetTemplate(string filename)
        {
            string content = string.Empty;
            DirectoryInfo inf = new DirectoryInfo(Directory.GetCurrentDirectory());
            string filePath = Path.Combine($"{inf.Parent.FullName}/IospectAPI.Common/EmailTemplates/{filename}");
            if (File.Exists(filePath))
            {
                FileStream file = File.Open(filePath, FileMode.Open); ;
                using (StreamReader reader = new StreamReader(file))
                {
                    content = reader.ReadToEnd();
                }
            }
            return content;
        }
    }
}
