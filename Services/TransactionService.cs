using FourtitudeAsiaAPITest.Handler;
using FourtitudeAsiaAPITest.Interfaces;
using FourtitudeAsiaAPITest.Models;
using FourtitudeAsiaAPITest.Utilities;
using System.Linq.Expressions;
using System.Reflection;

namespace FourtitudeAsiaAPITest.Services
{
    public class TransactionService : ITransactionService
    {
        public async Task<TrxRequestModel> GetValidTrx()
        {
            var trx = Utility.DeserializeFromFile<TrxRequestModel>("Data/TrxValid.json");

            return trx;
        }

        public bool AuthenticateTrx(TrxRequestModel model)
        {
            var pService = new PartnerService();
            var partner = pService.GetPartnerByNo(model.partnerrefno).Result;
            var modelPwd = model.partnerpassword;
            var partnerPwd = Utility.ConvertToBase64(partner.Password);
            var isAuthenticated = modelPwd.Equals(partnerPwd);

            return isAuthenticated;
        }


        public bool IsNotExpired(TrxRequestModel model)
        {
            var retBool = false;
            var curDateTime = DateTime.Now;
            var minDateTime = curDateTime.AddMinutes(-5);
            var maxDateTime = curDateTime.AddMinutes(5);


            if (model.timestamp >= minDateTime && model.timestamp <= minDateTime)
            {
                retBool = true;
            }
            return retBool;
        }
        public bool IsTotalAmountValid(TrxRequestModel model)
        {
            var retBool = false;
            var ttlAmt = model.totalamount;
            var itemsTtlAmt = model.items.Sum(i => i.qty * i.unitprice);
            if (ttlAmt == itemsTtlAmt)
            {
                retBool = true;
            }
            return retBool;
        }
        public List<string> IsRequiredParmValid(TrxRequestModel model)
        {
            var retBool = false;
            string[] requiredParm = new string[] { "xxx", "ggg", "partnerkey", "partnerrefno", "partnerpassword", "totalamount", "items", "timestamp", "sig" };
            var props = model.GetType().GetProperties().Select(p => p.Name).ToList();
            var missingParms = requiredParm.Except(props);
            if (missingParms.Count() == 0)
            {
                retBool = true;
            }
            return missingParms.ToList();
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
