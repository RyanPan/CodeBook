namespace AspNetIdentityDemo.DAL
{
    public enum Grade
    {
        A,B,C,D,E
    }

    public class Course
    {
        public int CourseID { get; set; }
        public string Title { get; set; }
        public Grade? Grade { get; set; }
    }
}