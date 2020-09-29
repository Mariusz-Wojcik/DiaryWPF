
namespace Diary.Models.Domains
{
    public class Rating
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }

        public Student Student { get; set; }

    }
}

