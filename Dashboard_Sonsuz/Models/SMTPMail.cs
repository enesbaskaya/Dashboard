﻿using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{

    public enum MailTypes
    {
        SUPPORT, INFORMATION, SUCCESS, PAYMENT, WARNING
    }

    public class SMTPMail
    {

        public SMTPMail()
        {

        }

        public MailTypes mailType { get; set; }
        public string header { get; set; }

        public string mail { get; set; }

        public string content { get; set; }

        public string logoPath
        {
            get
            {
                return "https://www.bimacvar.com/api/images/bmv_small.png";
            }
        }

        public string imagePath
        {
            get
            {
                switch (mailType)
                {
                    case MailTypes.SUPPORT:
                        return "https://www.bimacvar.com/api/images/mail/support.jpg";
                    case MailTypes.INFORMATION:
                        return "https://www.bimacvar.com/api/images/mail/information.jpg";
                    case MailTypes.SUCCESS:
                        return "https://www.bimacvar.com/api/images/mail/success.jpg";
                    case MailTypes.PAYMENT:
                        return "https://www.bimacvar.com/api/images/mail/payment.png";
                    case MailTypes.WARNING:
                        return "https://www.bimacvar.com/api/images/mail/warning.png";
                    default:
                        return "https://www.bimacvar.com/api/images/mail/default.png";
                }
            }
        }


        public async Task sendAsync(IConfiguration _config)
        {

            string FilePath = "wwwroot/mail/generalMail.html";
            StreamReader str = new StreamReader(FilePath);
            string text = await str.ReadToEndAsync();
            str.Close();
            text = text.Replace("[HEADER]", header);
            text = text.Replace("[CONTENT]", content);
            text = text.Replace("[IMAGE]", imagePath);
            text = text.Replace("[BMVLOGO]", logoPath);
            text = text.Replace("[YEAR]", DateTime.Now.Year.ToString());


            string usernameMail = _config.GetValue<String>("SMTP:Username");
            string host = _config.GetValue<String>("SMTP:Host");
            int port = _config.GetValue<int>("SMTP:Port");
            string password = _config.GetValue<String>("SMTP:Password");

            if (usernameMail != null && host != null && password != null)
            {
                if (usernameMail.Length > 0 && port > 0 && host.Length > 0 && password.Length > 0)
                {
                    var email = new MimeMessage();
                    email.To.Add(MailboxAddress.Parse(mail));
                    email.From.Add(new MailboxAddress("BiMaçVar!", usernameMail));
                    email.Cc.Add(new MailboxAddress("BiMaçVar!", usernameMail));
                    email.Bcc.Add(new MailboxAddress("BiMaçVar!", usernameMail));

                    email.Subject = header;
                    email.Body = new TextPart(TextFormat.Html)
                    {

                        Text = text
                    };

                    using var smtp = new SmtpClient();

                    smtp.Connect(host, port, false);
                    smtp.Authenticate(usernameMail, password);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
            }
        }


    }
}
