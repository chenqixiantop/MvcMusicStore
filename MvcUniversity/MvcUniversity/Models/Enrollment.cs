using System.ComponentModel.DataAnnotations;

namespace MvcUniversity.Models
{
    //枚举
    public enum Grade
    {
        A,B,C,D,F
    }
    //导航属性
    public class Enrollment
    {
        public int EnrollmentID { get; set; }

        public int CourseID { get; set; }
        public int StudentID { get; set; }

        //评级为 null 是不同于评级为零，null 表示一个等级未知或者尚未分配。
        [DisplayFormat(NullDisplayText="No grade")]
        public Grade? Grade { get; set; }

        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}