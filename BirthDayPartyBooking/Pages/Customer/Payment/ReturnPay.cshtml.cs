using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Protocols;
using System;

namespace BirthDayPartyBooking.Pages.Customer.Payment
{
    public class ReturnPayModel : PageModel
    {
        public void OnGet()
        {
            string vnp_HashSecret = "YNUDIRUGCVFYUHTPAKZTREPIYHRHPFSI";
            var vnpayData = Request.Query;
            VnPayLibrary vnpay = new VnPayLibrary();

            foreach (var pair in vnpayData)
            {
                var key = pair.Key;
                var value = pair.Value;
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value);
                }
            }

            long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            String vnp_SecureHash = "YNUDIRUGCVFYUHTPAKZTREPIYHRHPFSI";
            String TerminalID = "SF27X1PR";
            long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
            string bankCode = vnpay.GetResponseData("vnp_BankCode");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
            if (checkSignature)
            {
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                }
                else
                {
                    //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                }
                //displayTmnCode.InnerText = "Ma Website (Terminal ID):" + TerminalID;
                //displayTxnRef.InnerText = "Ma giao dich thanh toan:" + orderId.ToString();
                //displayVnpayTranNo.InnerText = "Ma giao dich tai VNPAY:" + vnpayTranId.ToString();
                //displayAmount.InnerText = "So tien thanh toan (VND):" + vnp_Amount.ToString();
                //displayBankCode.InnerText = "Ngan hang thanh toan:" + bankCode;
            }
            else
            {
                //log.InfoFormat("Invalid signature, InputData={0}", Request.RawUrl);
                //displayMsg.InnerText = "Co loi xay ra trong quá trinh xu ly";
            }
            RedirectToPage("/OrderHistory");
        }
    }
}
