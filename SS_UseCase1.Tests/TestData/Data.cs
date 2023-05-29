using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_UseCase1.Tests.TestData
{
    internal class Data{

        private static string file = @"./TestData/CountryData.json";      

        public static string GetData() {            
            return File.ReadAllText(file);        
        }
    }
}
