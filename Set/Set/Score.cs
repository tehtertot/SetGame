using SQLite;
using System;
using System.Collections.ObjectModel;

namespace Set
{
    public class Score
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int NumSets { get; set; }

        public string Type { get; set; }

        public int OrderBy { get; set; }

        public string ScoreDisplay { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
