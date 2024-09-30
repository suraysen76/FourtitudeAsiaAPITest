using FourtitudeAsiaAPITest.Interfaces;
using FourtitudeAsiaAPITest.Models;
using FourtitudeAsiaAPITest.Services;
using FourtitudeAsiaAPITest.Utilities;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;

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
            var responseModel = new ResponseModel() { result=1};
            var responseStr=String.Empty;
            var trx = _trxService.GetValidTrx().Result;


            responseModel = _trxService.AuthenticateTrx(trx);
            if (responseModel.result == 1)
            {
                responseModel = _trxService.IsTotalAmountValid(trx);
            }
            if (responseModel.result == 1)
            {
                responseModel = _trxService.IsNotExpired(trx);
            }
            if (responseModel.result == 1)
            {
                responseModel = _trxService.IsRequiredParmValid(trx);
            }          
           
            var discount = _trxService.GetDiscount(trx);
            if (responseModel.result == 1)
            {                
                var obj = new ResponseModel();
                obj.result = 1;
                obj.totalamount = trx.totalamount;
                obj.totaldiscount =(decimal) (discount/100 * obj.totalamount);
                obj.finalamount= (int) obj.totalamount- (int) obj.totaldiscount;
                
                responseStr =Utility.Serialize(obj);

                var jsonParsed = JObject.Parse(responseStr);

                jsonParsed.Properties()
                 .Where(attr => attr.Name == "resultmessage")
                 .First()
                 .Remove();
                jsonParsed.Properties()
                 .Where(attr => attr.Name == "resultmessagedescription")
                 .First()
                 .Remove();

                responseStr = jsonParsed.ToString();

                log.Info("Trx received :" + Utility.Serialize(trx));
                log.Info("Succrss :" + responseStr);
            }
            else
            {
                var obj = new ResponseModel();
                obj.result = 0;
                obj.resultmessage= responseModel.resultmessage;
                obj.resultmessagedescription = responseModel.resultmessagedescription;
                responseStr = Utility.Serialize(obj);
                var jsonParsed = JObject.Parse(responseStr);

                jsonParsed.Properties()
                 .Where(attr => attr.Name == "totalamount")
                 .First()
                 .Remove();
                jsonParsed.Properties()
                 .Where(attr => attr.Name == "totaldiscount")
                 .First()
                 .Remove();
                jsonParsed.Properties()
                 .Where(attr => attr.Name == "finalamount")
                 .First()
                 .Remove();

                responseStr = jsonParsed.ToString(); 
                log.Error("Trx received :" + Utility.Serialize(trx));
                log.Error("Error :" + responseStr);
            }
            
            return responseStr;
        }
    }
}
