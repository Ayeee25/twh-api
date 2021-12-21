using System;
using System.Data;
using System.Configuration;
using System.Web;

//using System.Web.Mail;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

/// <summary>
/// Summary description for MailHelper
/// </summary>
public static class MailHelper
{


    
    public static bool EnviarMail(bool flat_prueba, 
    string SMTP_SERVER , string SMTP_MAIL, string SMTP_PASSWORD,
    string correo_prueba,
    String destinatario
    , string subject
    , string body
    , bool html = true)
    {
        MailMessage mail = new MailMessage();
        if (flat_prueba)
        {
            string[] dest = correo_prueba.Split(';');
            foreach (var item in dest)
            {
                if (item != "") mail.To.Add(item);
            }
        }
        else
        {
            string[] dest = destinatario.Split(';');
            foreach (var item in dest)
            {
                if (item != "") mail.To.Add(item);
            }
        }
        mail.From = new MailAddress("soporte@tys.com.pe");
        mail.Subject =  subject;

        mail.Body = body;
        if (html) mail.IsBodyHtml = true;
        mail.Priority = MailPriority.High;

        SmtpClient mSmtpClient = new SmtpClient();
        mSmtpClient.Host = SMTP_SERVER;
        mSmtpClient.Timeout = 100;
        mSmtpClient.UseDefaultCredentials = false;
        mSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        mSmtpClient.Port = 587;
        mSmtpClient.EnableSsl = true;
        mSmtpClient.Timeout = 30000;
        mSmtpClient.Credentials = new System.Net.NetworkCredential("soporte@tys.com.pe", "soportetys@");

        try { mSmtpClient.Send(mail); return true; }
        catch (Exception ex) { throw ex; }
    }
}