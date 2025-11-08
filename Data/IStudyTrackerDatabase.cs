using StudyTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyTracker.Data
{
    public interface IStudyTrackerDatabase
    {
        Task<List<Subject>> GetSubjectsAsync();
        Task<Subject> GetSubjectAsync(int id);
        Task<int> SaveSubjectAsync(Subject subject); 
        Task<int> DeleteSubjectAsync(Subject subject);

        Task<List<Grade>> GetGradesForSubjectAsync(int subjectId);
        Task<int> SaveGradeAsync(Grade grade);
        Task<int> DeleteGradeAsync(Grade grade);

        Task<List<Material>> GetMaterialsForSubjectAsync(int subjectId);
        Task<int> SaveMaterialAsync(Material material);
        Task<int> DeleteMaterialAsync(Material material);
    }
}
