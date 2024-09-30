using FourtitudeAsiaAPITest.Handler;
using FourtitudeAsiaAPITest.Interfaces;
using FourtitudeAsiaAPITest.Models;
using FourtitudeAsiaAPITest.Utilities;
using log4net;
using System.Linq.Expressions;
using System.Reflection;

namespace FourtitudeAsiaAPITest.Services
{
    public class TransactionService : ITransactionService
    {
        public async Task<TrxRequestModel> GetValidTrx()
        {
            var trx = Utility.DeserializeFromFile<TrxRequestModel>("Data/TrxValid.json");
            //tweak trx to be valid
            //trx.timestamp= DateTime.Now;
            return trx;
        }

        public ResponseModel AuthenticateTrx(TrxRequestModel model)
        {
            var resp = new ResponseModel();
            var pService = new PartnerService();
            var partner = pService.GetPartnerByNo(model.partnerrefno).Result;
            var modelPwd = model.partnerpassword;
            var partnerPwd = Utility.ConvertToBase64(partner.Password);
            var isAuthenticated = modelPwd.Equals(partnerPwd);
            if (isAuthenticated)
            {               
                resp.result = 1;
            }
            else
            {               
                resp.result = 0;
                resp.resultmessage = "Access Denied!";
                resp.resultmessagedescription = "Unauthorized partner or Signature Mismatch";
            }
            return resp;
        }


        public ResponseModel IsNotExpired(TrxRequestModel model)
        {
            var resp = new ResponseModel();
            var isNotExpired = false;
            var curDateTime = DateTime.Now;
            var minDateTime = curDateTime.AddMinutes(-5);
            var maxDateTime = curDateTime.AddMinutes(5);

            if (model.timestamp >= minDateTime && model.timestamp <= maxDateTime)
            {
                isNotExpired = true;
            }
            if (isNotExpired)
            {
                resp.result = 1;
            }
            else
            {
                resp.result = 0;
                resp.resultmessage = "Expired.";
                resp.resultmessagedescription = "Provided timestamp exceed server time +-5min";
            }
            return resp;
        }
        public ResponseModel IsTotalAmountValid(TrxRequestModel model)
        {
            var resp = new ResponseModel();
            var isValidAmt = false;
            var ttlAmt = model.totalamount;
            var itemsTtlAmt = model.items.Sum(i => i.qty * i.unitprice);
            if (ttlAmt == itemsTtlAmt)
            {
                isValidAmt = true;
            }
            if (isValidAmt)
            {
                resp.result = 1;
            }
            else
            {
                resp.result = 0;
                resp.resultmessage = "Invalid Total Amount.";
                resp.resultmessagedescription = "The total value stated in itemDetails array not equal to value in totalamount";
            }
            return resp;
        }
        public ResponseModel IsRequiredParmValid(TrxRequestModel model)
        {
            var resp = new ResponseModel();
            var isParmRequired = false;
            string[] requiredParm = new string[] {  "partnerkey", "partnerrefno", "partnerpassword", "totalamount", "items", "timestamp", "sig" };
            var props = model.GetType().GetProperties().Select(p => p.Name).ToList();
            var missingParms = requiredParm.Except(props);
            if (missingParms.Count() == 0)
            {
                isParmRequired = true;
            }
            if (isParmRequired)
            {
                resp.result = 1;
            }
            else
            {
               
                resp.result = 0;
                resp.resultmessage = missingParms.FirstOrDefault()+ " is Required.";
                resp.resultmessagedescription = "The total value stated in itemDetails array not equal to value in totalamount";
            }
            return resp;
        }
        public decimal GetDiscount(TrxRequestModel model)
        {
            decimal discount = 0;

            switch (model.totalamount)
            {
                case int amt when amt < 200:
                    discount = 0;
                    break;
                case int amt when amt >= 200 && amt <= 500:
                    discount = 5;
                    break;
                case int amt when amt > 500 && amt <= 800:
                    discount = 7;
                    break;
                case int amt when amt > 800 && amt <= 1200:
                    discount = 10;
                    break;
                case int amt when amt > 1200:
                    discount = 15;
                    break;
                default:
                    break;
            }

            if (Utility.IsPrimeNumber(model.totalamount) && model.totalamount > 500)
            {
                discount += 8;
            }
            if (Utility.IsNumberEndsWith5(model.totalamount))
            {
                discount += 10;
            }
            var capDiscount = (decimal)0.2 * model.totalamount;
            if (discount > (decimal)capDiscount)
            {
                discount = capDiscount;
            }
            return discount;
        }
    }
}
