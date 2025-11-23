using System;
using System.Text;

namespace MLOOP2_L7
{
    public class Candidate
    {
        // Клас для зберігання кандидату

        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Education { get; set; }
        public LevelOfLanguage EnglishLevel { get; set; }
        public LevelOfLanguage GermanLevel { get; set; }
        public LevelOfLanguage FrenchLevel { get; set; }
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

            if (EnglishLevel != LevelOfLanguage.zero)
            {
                sb.Append("Англійська: " + EnglishLevel.ToString());
            }

            if (GermanLevel != LevelOfLanguage.zero)
            {
                if (sb.Length != 0)
                {
                    sb.Append( ", ");
                }
                sb.Append("Німецька: " + GermanLevel.ToString());
            }

            if (FrenchLevel != LevelOfLanguage.zero)
            {
                if (sb.Length != 0)
                {
                    sb.Append(", ");
                }
                sb.Append("Французька: " + FrenchLevel.ToString());
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