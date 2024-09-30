using FourtitudeAsiaAPITest.Models;

namespace FourtitudeAsiaAPITest.Interfaces
{
    public interface ITransactionService
    {
        Task<TrxRequestModel> GetValidTrx();
        ResponseModel AuthenticateTrx(TrxRequestModel model);

        ResponseModel IsNotExpired(TrxRequestModel model);

        ResponseModel IsTotalAmountValid(TrxRequestModel model);

        ResponseModel IsRequiredParmValid(TrxRequestModel model);

        decimal GetDiscount(TrxRequestModel model);
    }
}
