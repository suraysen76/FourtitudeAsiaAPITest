using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;
using System.Text;

namespace FourtitudeAsiaAPITest.Models
{
    public class TrxRequsetItemModel
    {
        public string partneritemref { get; set; }
        public string name { get; set; }
        public int qty { get; set; }
        public int unitprice { get; set; }
    }

    public class TrxRequestModel
    {
        public string partnerkey { get; set; }
        public string partnerrefno { get; set; }
        public string partnerpassword { get; set; }
        public int totalamount { get; set; }
        public List<TrxRequsetItemModel> items { get; set; }
        public DateTime timestamp { get; set; }
        public string sig { get; set; }
    }
}
