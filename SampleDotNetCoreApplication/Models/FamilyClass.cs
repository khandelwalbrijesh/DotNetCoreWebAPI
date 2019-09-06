using Newtonsoft.Json;
using System;
using SampleDotNetCoreApplication.View;

namespace SampleDotNetCoreApplication.Models
{

    public class Family
    {
        public string FamilyId { get; private set; }

        public int NumberOfFamilyMembers { get; set; }

        public string FamilyName { get; set; }
        public string HeadOfFamily { get; set; }

        public string LadyOfFamily { get; set; }

        public string FirstSonOfFamily { get; set; }

        public string FirstDaughterOFFamily { get; set;}

        public Family(FamilyView family)
        {
            this.FamilyId = Guid.NewGuid().ToString();
            this.HeadOfFamily = family.HeadOfFamily;
            this.FamilyName = family.FamilyName;
            this.LadyOfFamily = family.LadyOfFamily;
            this.FirstDaughterOFFamily = (family.FirstDaughterOFFamily != null) ? family.FirstDaughterOFFamily : null;
            this.FirstSonOfFamily= (family.FirstSonOfFamily != null) ? family.FirstSonOfFamily : null;
            this.NumberOfFamilyMembers = family.NumberOfFamilyMembers;
        }
        public Family()
        {
            this.FamilyId = Guid.NewGuid().ToString();
        }

        public void UpdateFamily(FamilyView family)
        {
            this.HeadOfFamily = family.HeadOfFamily;
            this.FamilyName = family.FamilyName;
            this.LadyOfFamily = family.LadyOfFamily;
            this.FirstDaughterOFFamily = (family.FirstDaughterOFFamily != null) ? family.FirstDaughterOFFamily : null;
            this.FirstSonOfFamily = (family.FirstSonOfFamily != null) ? family.FirstSonOfFamily : null;
            this.NumberOfFamilyMembers = family.NumberOfFamilyMembers;
        }

        public override string ToString()
        {
            string format = "";
            format += String.Format("HeadOfFamily : {0}", this.HeadOfFamily);
            format += String.Format("LadyOfFamily : {0}", this.LadyOfFamily);
            format += String.Format("FamilyId : {0}", this.FamilyId);
            format += String.Format("FirstSonOfFamily : {0}", this.FirstSonOfFamily);
            format += String.Format("FirstDaughterOFFamily : {0}", this.FirstDaughterOFFamily);
            format += String.Format("NumberOfFamilyMembers : {0}", this.NumberOfFamilyMembers);

            return format;
        }
    }
}
