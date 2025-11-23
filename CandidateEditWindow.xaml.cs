using System;
using System.Windows;
using System.Windows.Controls;

namespace MLOOP2_L7
{
    public partial class CandidateEditWindow : Window
    {
        private CandidateService candidateService;
        private Candidate existingCandidate;
        private int candidateIndex;
        private bool isEditMode;

        public CandidateEditWindow(CandidateService service, Candidate candidate, int index)
        {
            InitializeComponent();
            candidateService = service;
            existingCandidate = candidate;
            candidateIndex = index;
            isEditMode = candidate != null;

            if (isEditMode)
            {
                headerText.Text = "✏️ Редагувати кандидата";
                LoadCandidateData();
            }
        }

        private void LoadCandidateData()
        {
            fullNameBox.Text = existingCandidate.FullName;
            birthDatePicker.SelectedDate = existingCandidate.BirthDate;

            for (int i = 0; i < educationCombo.Items.Count; i++)
            {
                ComboBoxItem item = (ComboBoxItem)educationCombo.Items[i];
                if (item.Content.ToString() == existingCandidate.Education)
                {
                    educationCombo.SelectedIndex = i;
                    break;
                }
            }

            SetLanguageCombo(englishCombo, existingCandidate.EnglishLevel);
            SetLanguageCombo(germanCombo, existingCandidate.GermanLevel);
            SetLanguageCombo(frenchCombo, existingCandidate.FrenchLevel);

            experienceBox.Text = existingCandidate.WorkExperience.ToString();
            computerSkillsCheck.IsChecked = existingCandidate.HasComputerSkills;
            recommendationsCheck.IsChecked = existingCandidate.HasRecommendations;
        }

        private void SetLanguageCombo(ComboBox combo, string level)
        {
            if (string.IsNullOrEmpty(level))
            {
                combo.SelectedIndex = 0;
                return;
            }

            for (int i = 0; i < combo.Items.Count; i++)
            {
                ComboBoxItem item = (ComboBoxItem)combo.Items[i];
                if (item.Content.ToString() == level)
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
            combo.SelectedIndex = 0;
        }

        private string GetComboValue(ComboBox combo)
        {
            if (combo.SelectedItem == null)
            {
                return "";
            }
            return ((ComboBoxItem)combo.SelectedItem).Content.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            Candidate candidate = new Candidate();
            candidate.FullName = fullNameBox.Text.Trim();
            candidate.BirthDate = birthDatePicker.SelectedDate.Value;
            candidate.Education = GetComboValue(educationCombo);
            candidate.EnglishLevel = GetComboValue(englishCombo);
            candidate.GermanLevel = GetComboValue(germanCombo);
            candidate.FrenchLevel = GetComboValue(frenchCombo);
            candidate.WorkExperience = int.Parse(experienceBox.Text);
            candidate.HasComputerSkills = computerSkillsCheck.IsChecked == true;
            candidate.HasRecommendations = recommendationsCheck.IsChecked == true;

            if (isEditMode)
            {
                candidate.ApplicationDate = existingCandidate.ApplicationDate;
                candidateService.UpdateCandidate(candidateIndex, candidate);
                MessageBox.Show("Дані кандидата оновлено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                candidate.ApplicationDate = DateTime.Now;
                candidateService.AddCandidate(candidate);
                MessageBox.Show("Кандидата успішно додано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            this.DialogResult = true;
            this.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(fullNameBox.Text))
            {
                ShowError("Введіть П.І.Б. кандидата!");
                fullNameBox.Focus();
                return false;
            }

            if (fullNameBox.Text.Trim().Length < 5)
            {
                ShowError("П.І.Б. повинно містити мінімум 5 символів!");
                fullNameBox.Focus();
                return false;
            }

            if (!birthDatePicker.SelectedDate.HasValue)
            {
                ShowError("Виберіть дату народження!");
                birthDatePicker.Focus();
                return false;
            }

            DateTime birthDate = birthDatePicker.SelectedDate.Value;
            int age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
            {
                age--;
            }

            if (age < 18)
            {
                ShowError("Кандидат повинен бути старше 18 років!");
                birthDatePicker.Focus();
                return false;
            }

            if (age > 70)
            {
                ShowError("Вік кандидата не може перевищувати 70 років!");
                birthDatePicker.Focus();
                return false;
            }

            if (birthDate > DateTime.Now)
            {
                ShowError("Дата народження не може бути в майбутньому!");
                birthDatePicker.Focus();
                return false;
            }

            if (educationCombo.SelectedItem == null)
            {
                ShowError("Виберіть освіту!");
                educationCombo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(experienceBox.Text))
            {
                ShowError("Введіть стаж роботи!");
                experienceBox.Focus();
                return false;
            }

            int experience;
            if (!int.TryParse(experienceBox.Text, out experience))
            {
                ShowError("Стаж роботи повинен бути числом!");
                experienceBox.Focus();
                return false;
            }

            if (experience < 0)
            {
                ShowError("Стаж роботи не може бути від'ємним!");
                experienceBox.Focus();
                return false;
            }

            if (experience > 50)
            {
                ShowError("Стаж роботи не може перевищувати 50 років!");
                experienceBox.Focus();
                return false;
            }

            if (experience > age - 18)
            {
                ShowError("Стаж роботи не може бути більшим за можливий робочий вік!");
                experienceBox.Focus();
                return false;
            }

            return true;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}