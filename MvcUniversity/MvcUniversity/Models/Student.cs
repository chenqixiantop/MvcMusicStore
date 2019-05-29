using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcUniversity.Models
{
    public class Student
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50,ErrorMessage ="长度不能超过50")]
        [Display(Name="用户名")]
        public string UserName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }

        //导航属性通常定义为virtual，以便它们可以充分利用 Entity Framework 的特定功能，如延迟加载。
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}