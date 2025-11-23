using System.Windows;
using System.Windows.Media;

namespace MLOOP2_L7
{
    public partial class CandidateViewWindow : Window
    {
        public CandidateViewWindow(Candidate candidate)
        {
            InitializeComponent();
            LoadCandidateData(candidate);
        }

        private void LoadCandidateData(Candidate candidate)
        {
            candidateNameHeader.Text = candidate.FullName;
            fullNameValue.Text = candidate.FullName;
            birthDateValue.Text = candidate.BirthDate.ToString("dd MMMM yyyy");
            ageValue.Text = candidate.GetAge().ToString() + " років";
            educationValue.Text = candidate.Education;
            languagesValue.Text = candidate.GetLanguagesInfo();
            experienceValue.Text = candidate.WorkExperience.ToString() + " років";
            applicationDateValue.Text = candidate.ApplicationDate.ToString("dd.MM.yyyy HH:mm");

            if (candidate.HasComputerSkills)
            {
                computerSkillsValue.Text = "Володію комп'ютером";
                computerSkillsValue.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                computerSkillsValue.Text = "Не володію комп'ютером";
                computerSkillsValue.Foreground = new SolidColorBrush(Colors.Gray);
            }

            if (candidate.HasRecommendations)
            {
                recommendationsValue.Text = "Є рекомендації";
                recommendationsValue.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                recommendationsValue.Text = "Немає рекомендацій";
                recommendationsValue.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}