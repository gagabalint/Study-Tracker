using SQLite;
using StudyTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyTracker.Data
{
    internal class StudyTrackerDatabase : IStudyTrackerDatabase
    {
        readonly SQLiteAsyncConnection database;
        readonly string dbPath;

        public StudyTrackerDatabase()
        {
             dbPath = Path.Combine(FileSystem.AppDataDirectory, "studytracker.db3");
            database= new SQLiteAsyncConnection(dbPath);
            //if (File.Exists(dbPath))
            //{
            //    File.Delete(dbPath);
            //}
    
            database = new SQLiteAsyncConnection(dbPath);

            database.CreateTableAsync<Subject>().Wait();
            database.CreateTableAsync<Grade>().Wait();
            database.CreateTableAsync<Material>().Wait();
        }
     

        public Task<List<Subject>> GetSubjectsAsync()
        {
            return database.Table<Subject>().ToListAsync();
        }

        public Task<Subject> GetSubjectAsync(int id)
        {
            return database.Table<Subject>().Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveSubjectAsync(Subject subject)
        {
            if (subject.Id != 0)
            {
                return database.UpdateAsync(subject);
            }
            else
            {
                return database.InsertAsync(subject);
            }
        }

        public async Task<int> DeleteSubjectAsync(Subject subject)
        {
            var grades = await GetGradesForSubjectAsync(subject.Id);
            if (grades.Count > 0)
            { 
                foreach (var grade in grades) {await database.DeleteAsync(grade); }
            }
            var materials = await GetMaterialsForSubjectAsync(subject.Id);
            if (materials.Count > 0)
            {
                foreach (var material in materials)
                {
                    if (!string.IsNullOrEmpty(material.PictureUri) && File.Exists(material.PictureUri))
                    {
                        try
                        {
                            File.Delete(material.PictureUri);
                        }
                        catch (Exception ex)
                        {

                            //TODO
                        }

                    }
                    await database.DeleteAsync(material);
                }

            }

            return await database.DeleteAsync(subject);
        }

        public Task<List<Grade>> GetGradesForSubjectAsync(int subjectId)
        {
            return database.Table<Grade>().Where(g => g.SubjectId == subjectId).ToListAsync();
        }

        public Task<int> SaveGradeAsync(Grade grade)
        {
            if (grade.Id != 0)
            {
                return database.UpdateAsync(grade);
            }
            else
            {
                return database.InsertAsync(grade);
            }
        }

        public Task<int> DeleteGradeAsync(Grade grade)
        {
            return database.DeleteAsync(grade);
        }

        public Task<List<Material>> GetMaterialsForSubjectAsync(int subjectId)
        {
            return database.Table<Material>().Where(m => m.SubjectId == subjectId).ToListAsync();
        }

        public Task<int> SaveMaterialAsync(Material material)
        {
            if (material.Id != 0)
            {
                return database.UpdateAsync(material);
            }
            else
            {
                return database.InsertAsync(material);
            }
        }

        public Task<int> DeleteMaterialAsync(Material material)
        {
            return database.DeleteAsync(material);
        }
    }
}
