using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Set
{
    public class ScoreDatabase
    {
        readonly SQLiteAsyncConnection database;

        public ScoreDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Score>().Wait();
        }

        public Task<List<Score>> GetAllScoresAsync()
        {
            return database.Table<Score>().ToListAsync();
        }

        public Task<List<Score>> GetTimedScoresAsync()
        {
            return database.Table<Score>().OrderBy(s => s.NumSets).Where(s => s.Type=="timed").ToListAsync();
        }

        public Task<List<Score>> GetSpeedScoresAsync()
        {
            return database.Table<Score>().OrderByDescending(s => s.NumSets).Where(s => s.Type == "speed").ToListAsync();
        }

        public Task<Score> GetMaxScore()
        {
            return database.Table<Score>().OrderByDescending(s => s.NumSets).FirstOrDefaultAsync();
        }

        public Task<int> SaveScore(Score s)
        {
            if (s.Id != 0)
            {
                return database.UpdateAsync(s);
            }
            else
            {
                return database.InsertAsync(s);
            }
        }

        public Task<int> DeleteScore(Score s)
        {
            return database.DeleteAsync(s);
        }
    }
}
