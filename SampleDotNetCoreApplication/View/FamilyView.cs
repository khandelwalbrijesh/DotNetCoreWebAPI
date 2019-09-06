using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleDotNetCoreApplication.View
{
    public class FamilyView
    {
        [JsonProperty(Required = Required.Always)]
        public int NumberOfFamilyMembers { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string HeadOfFamily { get; set; }


        [JsonProperty(Required = Required.Always)]
        public string FamilyName { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string LadyOfFamily { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string FirstSonOfFamily { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string FirstDaughterOFFamily { get; set; }

        public override string ToString()
        {
            string format = "";
            format += String.Format("HeadOfFamily : {0}", this.HeadOfFamily);
            format += String.Format("LadyOfFamily : {0}", this.LadyOfFamily);
            format += String.Format("FirstSonOfFamily : {0}", this.FirstSonOfFamily);
            format += String.Format("FirstDaughterOFFamily : {0}", this.FirstDaughterOFFamily);
            format += String.Format("NumberOfFamilyMembers : {0}", this.NumberOfFamilyMembers);

            return format;
        }
    }
}
