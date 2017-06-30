using SQLite;
using System;

namespace Set
{
    public class Score
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int NumSets { get; set; }

        public string Type { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
