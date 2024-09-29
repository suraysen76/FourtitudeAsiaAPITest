using FourtitudeAsiaAPITest.Interfaces;
using FourtitudeAsiaAPITest.Models;
using FourtitudeAsiaAPITest.Utilities;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FourtitudeAsiaAPITest.Services
{
    public class PartnerService : IPartnerService
    {

        public async Task<PartnersModel> GetPartnerByNo(string partnerNo)
        {
            var jsonStr = File.ReadAllText("Data/PartnersInfo.json");
            var partners = JsonConvert.DeserializeObject<List<PartnersModel>>(jsonStr);

            return partners.Where(p => p.PartnerNo == partnerNo).FirstOrDefault();
        }
        public async Task<List<PartnersModel>> GetPartners()
        {           
            var partners = Utility.DeserializeFromFile<List<PartnersModel>>("Data/PartnersInfo.json");
            
            return partners;
        }
    }
}
