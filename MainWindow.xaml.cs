using System;
using System.Windows;
using System.Windows.Input;

namespace MLOOP2_L7
{
    public partial class MainWindow : Window
    {
        private CandidateService candidateService;

        public MainWindow()
        {
            InitializeComponent();
            candidateService = new CandidateService();
            LoadCandidates();
        }

        private void LoadCandidates()
        {
            var candidates = candidateService.GetAllCandidates();
            candidatesDataGrid.ItemsSource = candidates;
            candidatesCountLabel.Text = "Всього кандидатів: " + candidates.Count;
            statusLabel.Text = "Завантажено " + candidates.Count + " кандидатів";
        }

        private void AddCandidate_Click(object sender, RoutedEventArgs e)
        {
            CandidateEditWindow editWindow = new CandidateEditWindow(candidateService, null, -1);
            editWindow.Owner = this;
            editWindow.ShowDialog();
            LoadCandidates();
        }

        private void ViewCandidate_Click(object sender, RoutedEventArgs e)
        {
            if (candidatesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Виберіть кандидата для перегляду!", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Candidate selected = (Candidate)candidatesDataGrid.SelectedItem;
            CandidateViewWindow viewWindow = new CandidateViewWindow(selected);
            viewWindow.Owner = this;
            viewWindow.ShowDialog();
        }

        private void EditCandidate_Click(object sender, RoutedEventArgs e)
        {
            if (candidatesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Виберіть кандидата для редагування!", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int index = candidatesDataGrid.SelectedIndex;
            Candidate selected = (Candidate)candidatesDataGrid.SelectedItem;

            CandidateEditWindow editWindow = new CandidateEditWindow(candidateService, selected, index);
            editWindow.Owner = this;
            editWindow.ShowDialog();
            LoadCandidates();
        }

        private void DeleteCandidate_Click(object sender, RoutedEventArgs e)
        {
            if (candidatesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Виберіть кандидата для видалення!", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Candidate selected = (Candidate)candidatesDataGrid.SelectedItem;
            MessageBoxResult result = MessageBox.Show(
                "Ви дійсно хочете видалити кандидата " + selected.FullName + "?",
                "Підтвердження видалення",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                int index = candidatesDataGrid.SelectedIndex;
                candidateService.DeleteCandidate(index);
                LoadCandidates();
                statusLabel.Text = "Кандидата видалено";
            }
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            string education = "";
            if (filterEducation.SelectedItem != null)
            {
                education = ((System.Windows.Controls.ComboBoxItem)filterEducation.SelectedItem).Content.ToString();
            }

            string language = "";
            if (filterLanguage.SelectedItem != null)
            {
                language = ((System.Windows.Controls.ComboBoxItem)filterLanguage.SelectedItem).Content.ToString();
            }

            int? minExperience = null;
            int expValue;
            if (int.TryParse(filterExperience.Text, out expValue))
            {
                minExperience = expValue;
            }

            bool? hasComputerSkills = null;
            if (filterComputerSkills.IsChecked == true)
            {
                hasComputerSkills = true;
            }

            bool? hasRecommendations = null;
            if (filterRecommendations.IsChecked == true)
            {
                hasRecommendations = true;
            }

            var filtered = candidateService.FilterCandidates(education, hasComputerSkills, hasRecommendations, minExperience, language);
            candidatesDataGrid.ItemsSource = filtered;
            statusLabel.Text = "Знайдено " + filtered.Count + " кандидатів за фільтром";
        }

        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            filterEducation.SelectedIndex = 0;
            filterLanguage.SelectedIndex = 0;
            filterExperience.Text = "";
            filterComputerSkills.IsChecked = false;
            filterRecommendations.IsChecked = false;
            LoadCandidates();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadCandidates();
        }

        private void CandidatesDataGrid_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (candidatesDataGrid.SelectedItem != null)
            {
                Candidate selected = (Candidate)candidatesDataGrid.SelectedItem;
                CandidateViewWindow viewWindow = new CandidateViewWindow(selected);
                viewWindow.Owner = this;
                viewWindow.ShowDialog();
            }
        }
    }
}