using FourtitudeAsiaAPITest.Models;

namespace FourtitudeAsiaAPITest.Interfaces
{
    public interface ITransactionService
    {
        Task<TrxRequestModel> GetValidTrx();
        bool AuthenticateTrx(TrxRequestModel model);

        bool IsNotExpired(TrxRequestModel model);

        bool IsTotalAmountValid(TrxRequestModel model);

        List<string> IsRequiredParmValid(TrxRequestModel model);

        decimal GetDiscount(TrxRequestModel model);
    }
}
