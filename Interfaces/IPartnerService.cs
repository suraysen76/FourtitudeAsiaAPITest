using FourtitudeAsiaAPITest.Models;

namespace FourtitudeAsiaAPITest.Interfaces
{
    public interface IPartnerService
    {
        Task<List<PartnersModel>> GetPartners();

        Task<PartnersModel> GetPartnerByNo(string partnerNo);
    }
}
