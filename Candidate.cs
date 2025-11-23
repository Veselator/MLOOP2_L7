using System;
using System.Text;

namespace MLOOP2_L7
{
    public class Candidate
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Education { get; set; }
        public string EnglishLevel { get; set; }
        public string GermanLevel { get; set; }
        public string FrenchLevel { get; set; }
        public bool HasComputerSkills { get; set; }
        public int WorkExperience { get; set; }
        public bool HasRecommendations { get; set; }
        public DateTime ApplicationDate { get; set; }

        public Candidate()
        {
            ApplicationDate = DateTime.Now;
        }

        public int GetAge()
        {
            int age = DateTime.Now.Year - BirthDate.Year;
            if (DateTime.Now.DayOfYear < BirthDate.DayOfYear)
            {
                age--;
            }
            return age;
        }

        public string GetLanguagesInfo()
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(EnglishLevel) && EnglishLevel != "Не володію")
            {
                sb.Append("Англійська: " + EnglishLevel);
            }

            if (!string.IsNullOrEmpty(GermanLevel) && GermanLevel != "Не володію")
            {
                if (sb.Length != 0)
                {
                    sb.Append( ", ");
                }
                sb.Append("Німецька: " + GermanLevel);
            }

            if (!string.IsNullOrEmpty(FrenchLevel) && FrenchLevel != "Не володію")
            {
                if (sb.Length != 0)
                {
                    sb.Append(", ");
                }
                sb.Append("Французька: " + FrenchLevel);
            }

            if (sb.Length == 0)
            {
                return "Не володію іноземними мовами";
            }

            return sb.ToString();
        }
    }

    public enum LevelOfLanguage
    {
        zero,
        A1,
        A2,
        B1,
        B2,
        C1,
        C2
    }
}