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

        public Task<int> DeleteSubjectAsync(Subject subject)
        {
            return database.DeleteAsync(subject);
        }

        // --- Grade implementáció ---
        public Task<List<Grade>> GetGradesForSubjectAsync(int subjectId)
        {
            // Lekérdezzük az összes jegyet, ami az adott tantárgy ID-hoz tartozik
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

        // --- Material implementáció ---
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
