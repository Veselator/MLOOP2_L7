using System;
using System.Collections.Generic;
using System.Linq;

namespace MLOOP2_L7
{
    public class CandidateService
    {
        private static readonly string FilePath = "candidates.borys";
        private List<Candidate> candidates;

        public CandidateService()
        {
            LoadCandidates();
        }

        private void LoadCandidates()
        {
            try
            {
                candidates = BorysSerializingManager.Deserialize<Candidate>(FilePath);

                if (candidates == null)
                {
                    candidates = new List<Candidate>();
                }
            }
            catch
            {
                candidates = new List<Candidate>();
            }
        }

        private void SaveCandidates()
        {
            BorysSerializingManager.Serialize(candidates, FilePath);
        }

        public void AddCandidate(Candidate candidate)
        {
            candidates.Add(candidate);
            SaveCandidates();
        }

        public void UpdateCandidate(int index, Candidate candidate)
        {
            if (index >= 0 && index < candidates.Count)
            {
                candidates[index] = candidate;
                SaveCandidates();
            }
        }

        public void DeleteCandidate(int index)
        {
            if (index >= 0 && index < candidates.Count)
            {
                candidates.RemoveAt(index);
                SaveCandidates();
            }
        }

        public List<Candidate> GetAllCandidates()
        {
            return new List<Candidate>(candidates);
        }

        public List<Candidate> FilterCandidates(string education, bool? hasComputerSkills,
            bool? hasRecommendations, int? minExperience, string language)
        {
            IEnumerable<Candidate> filtered = candidates;

            if (!string.IsNullOrEmpty(education) && education != "Всі")
            {
                filtered = filtered.Where(c => c.Education == education);
            }

            if (hasComputerSkills.HasValue)
            {
                filtered = filtered.Where(c => c.HasComputerSkills == hasComputerSkills.Value);
            }

            if (hasRecommendations.HasValue)
            {
                filtered = filtered.Where(c => c.HasRecommendations == hasRecommendations.Value);
            }

            if (minExperience.HasValue && minExperience.Value > 0)
            {
                filtered = filtered.Where(c => c.WorkExperience >= minExperience.Value);
            }

            if (!string.IsNullOrEmpty(language) && language != "Всі")
            {
                filtered = filtered.Where(c =>
                {
                    if (language == "Англійська")
                    {
                        return c.EnglishLevel != "Не володію" && !string.IsNullOrEmpty(c.EnglishLevel);
                    }
                    if (language == "Німецька")
                    {
                        return c.GermanLevel != "Не володію" && !string.IsNullOrEmpty(c.GermanLevel);
                    }
                    if (language == "Французька")
                    {
                        return c.FrenchLevel != "Не володію" && !string.IsNullOrEmpty(c.FrenchLevel);
                    }
                    return true;
                });
            }

            return filtered.ToList();
        }

        public int GetCandidatesCount()
        {
            return candidates.Count;
        }
    }
}