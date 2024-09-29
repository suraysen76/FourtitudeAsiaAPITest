using FourtitudeAsiaAPITest.Models;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Newtonsoft.Json;

namespace FourtitudeAsiaAPITest.Utilities
{
    public static class Utility
    {

        public static string ConvertToBase64(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string DecodeBase64(string base64EncodedText)
        { 
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedText);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static T DeserializeFromFile<T>(string pathName)
        {
            var jsonStr = File.ReadAllText(pathName);
            var partners = JsonConvert.DeserializeObject<T>(jsonStr);

            return partners;
        }

        public static T Deserialize<T>(string jsonText)
        {
            var partners = JsonConvert.DeserializeObject<T>(jsonText);
            return partners;
        }

        public static string Serialize(object obj)
        {
            var str = JsonConvert.SerializeObject(obj);
            return str;
        }

        public static bool IsPrimeNumber(int number)
        {
            for (int i = 2; i <= number / 2; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
                
            }
            return true;
        }

        public static bool IsNumberEndsWith5(int number)
        {   
            var lastNum = number.ToString().Last();

            if (lastNum.Equals("5"))
            {
                return true;
            }
            return false;
        }
    }
}
