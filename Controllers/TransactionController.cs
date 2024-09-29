using FourtitudeAsiaAPITest.Interfaces;
using FourtitudeAsiaAPITest.Models;
using FourtitudeAsiaAPITest.Services;
using FourtitudeAsiaAPITest.Utilities;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FourtitudeAsiaAPITest.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TransactionController : Controller
    {
        private ITransactionService _trxService;
        private static readonly ILog log = LogManager.GetLogger(typeof(TransactionController));
        public  TransactionController(ITransactionService trxService)
        {
            _trxService=trxService;
        }


        [HttpGet]
        public Object submittrxmessage()
        {

            var continueValidation= true;
            var responseStr=String.Empty;
            var trx = _trxService.GetValidTrx().Result;


            while (continueValidation)
            {
                continueValidation= _trxService.AuthenticateTrx(trx);
                continueValidation = _trxService.IsNotExpired(trx);
                continueValidation = _trxService.IsTotalAmountValid(trx);
                if(_trxService.IsRequiredParmValid(trx).Count() == 0)
                {
                    continueValidation = true;
                }
                else
                {
                    continueValidation =false;
                }
            }
            //var isAuthenticated = _trxService.AuthenticateTrx(trx);
            //var isNotExpired = _trxService.IsNotExpired(trx);
            //var isTotalAmtValid = _trxService.IsTotalAmountValid(trx);
            //var isReqParmValid = _trxService.IsRequiredParmValid(trx);
            var discount = _trxService.GetDiscount(trx);
            if (!continueValidation)
            {                
                var obj = new SuccessResponseModel();
                obj.result = 1;
                obj.totalamount = trx.totalamount;
                obj.totaldiscount =(decimal) (discount/100 * obj.totalamount);
                obj.finalamount= (int) obj.totalamount- (int) obj.totaldiscount;
                responseStr=Utility.Serialize(obj);
                log.Info("Trx received :" + Utility.Serialize(trx));
                log.Info("Succrss :" + responseStr);
            }
            else
            {
                var obj = new ErrorResponseModel();
                obj.result = 0;
                obj.resultmessage = "Access Denied!";
                obj.resultmessagedescription= "Unauthorized partner or Signature Mismatch";
                responseStr = Utility.Serialize(obj);
                log.Error("Trx received :" + Utility.Serialize(trx));
                log.Error("Error :" + responseStr);
            }
            
            return responseStr;
        }
    }
}
