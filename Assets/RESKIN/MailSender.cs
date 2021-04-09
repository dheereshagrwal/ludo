using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Text.RegularExpressions;

public class MailSender : MonoBehaviour
{
    // Paytm
    public InputField Paytmname;
    public InputField Paytmemail;
    public InputField Paytmphone;
    
    // PayPal
    public InputField PayPalname;
    public InputField PayPalemail;
    public InputField PayPalId;

    public RedeemManager redeemManagerS;
    public string email;
    public string password;
    public GameObject DetailsErrorWindow;
    public GameObject mailSendErrorWindow;
    public GameObject sucessWindow;

    public void SendEmail(string paymentGateway)
    {
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        SmtpServer.Timeout = 10000;
        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        SmtpServer.UseDefaultCredentials = false;
        SmtpServer.Port = 587;

        mail.From = new MailAddress(email);
        mail.To.Add(new MailAddress(email));

        mail.Subject = "Payment Details";

        if (paymentGateway == "Paytm")
        {
            if (Paytmname.text != "" && Paytmemail.text != "" && Paytmphone.text != "")
            {
                mail.Body = "PaymentGateway = " + paymentGateway + "\n" + "MoneyToPay = " + redeemManagerS.priceforMail + "\n" + "Name = " + Paytmname.text + "\n" + "Email = " + Paytmemail.text + "\n" + "Paytm Number = " + Paytmphone.text;
            }
            else
            {
                DetailsErrorWindow.SetActive(true);
            }
        }
        else if (paymentGateway == "PayPal")
        {
            if (PayPalname.text != "" && PayPalemail.text != "")
            {
                mail.Body = "PaymentGateway = " + paymentGateway + "\n" + "MoneyToPay = " + redeemManagerS.priceforMail + "\n" + "Name = " + PayPalname.text + "\n" + "Email = " + PayPalemail.text + "\n" + "PayPal ID = " + PayPalId.text;
            }
            else
            {
                DetailsErrorWindow.SetActive(true);
            }
        }

        SmtpServer.Credentials = new System.Net.NetworkCredential(email, password) as ICredentialsByHost; SmtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };

        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure ;
   
        try
        {
            SmtpServer.Send(mail);
            GameManager.Instance.playfabManager.addCoinsRequest(-Convert.ToInt32(redeemManagerS.coinsforMail));
            sucessWindow.SetActive(true);
        }
        catch (Exception e)
        {
            mailSendErrorWindow.SetActive(true);
        }

    }


    public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    public static bool ValidateEmail(string email)
    {
        if (email != null)
             return Regex.IsMatch(email, MatchEmailPattern);
        else
             return false;
    }
}

