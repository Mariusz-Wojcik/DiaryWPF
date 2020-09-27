using Diary.Models.Domains;
using Diary.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Diary.Models.Converters;
using Diary.Models;
using System.Windows;
using Diary.Properties;

namespace Diary
{
    public class Repository
    {
        public List<Group> GetGroups()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Groups.ToList();
            }
        }

        public List<StudentWrapper> GetStudents(int groupId)
        {
            using (var context = new ApplicationDbContext())

            {
                
                var students = context
                    .Students
                    .Include(x => x.Group)
                    .Include(x => x.Ratings)
                    .AsQueryable();

                if (groupId != 0)
                    students = students.Where(x => x.GroupId == groupId);

                return students.ToList().Select(x => x.ToWrapper()).ToList();
            }
        }

        public void DeleteStudent(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var studentToDelete = context.Students.Find(id);
                context.Students.Remove(studentToDelete);
                context.SaveChanges();
            }
        }

        public void UpdateStudent(StudentWrapper studentWrapper)
        {
            var student = studentWrapper.ToDao();
            var ratings = studentWrapper.ToRatingDao();

            using (var context = new ApplicationDbContext())
            {
                UpdateStudentProperties(context, student);

                var studentRatings = GetStudentRatings(context, student);



                UpdateRate(student, ratings, context, studentRatings, Subject.Math);
                UpdateRate(student, ratings, context, studentRatings, Subject.Physics);
                UpdateRate(student, ratings, context, studentRatings, Subject.Technology);
                UpdateRate(student, ratings, context, studentRatings, Subject.PolishLang);
                UpdateRate(student, ratings, context, studentRatings, Subject.ForeignLang);

                context.SaveChanges();
            };
        }
        private static List<Rating> GetStudentRatings(ApplicationDbContext context, Student student)
        {
            return context
                .Ratings
                .Where(x => x.StudentId == student.Id)
                .ToList();
        }

        private static void UpdateRate(Student student, List<Rating> newRatings, ApplicationDbContext context, List<Rating> studentsRatings, Subject subject)
        {
            var subRatings = studentsRatings
                .Where(x => x.SubjectId == (int)subject)
                .Select(x => x.Rate);

            var newSubRatings = newRatings
                .Where(x => x.SubjectId == (int)subject)
                .Select(x => x.Rate);

            var subRatingToDelete = subRatings.Except(newSubRatings).ToList();
            var subRatingToAdd = newSubRatings.Except(subRatings).ToList();

            subRatingToDelete.ForEach(x =>
            {
                var ratingToDelete = context.Ratings.First(y =>
                y.Rate == x &&
                y.StudentId == student.Id &&
                y.SubjectId == (int)subject);

                context.Ratings.Remove(ratingToDelete);
            });

            subRatingToAdd.ForEach(x =>
            {
                var ratingToAdd = new Rating
                {
                    Rate = x,
                    StudentId = student.Id,
                    SubjectId = (int)subject
                };

                context.Ratings.Add(ratingToAdd);
            });

        }

        private void UpdateStudentProperties(ApplicationDbContext context, Student student)
        {
            var studentToUpdate = context.Students.Find(student.Id);
            studentToUpdate.Activities = student.Activities;
            studentToUpdate.Comments = student.Comments;
            studentToUpdate.FirstName = student.FirstName;
            studentToUpdate.LastName = student.LastName;
            studentToUpdate.GroupId = student.GroupId;
        }

        public void AddStudent(StudentWrapper studentWprapper)
        {
            var student = studentWprapper.ToDao();
            var ratings = studentWprapper.ToRatingDao();
            using (var context = new ApplicationDbContext())
            {
                var dbStudent = context.Students.Add(student);
                ratings.ForEach(x =>
                {
                    x.StudentId = dbStudent.Id;
                    context.Ratings.Add(x);
                });
                context.SaveChanges();
            }
        }
    }
}

