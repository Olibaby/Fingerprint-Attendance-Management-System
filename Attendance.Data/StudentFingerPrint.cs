using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Data
{
    public class StudentFingerPrint
    {
        public int StudentFingerPrintId { get; set; }
        public string LeftThumb { get; set; } //0
        public string LeftIndex { get; set; } //1
        public string LeftMiddle { get; set; } //2
        public string LeftRing { get; set; } //3
        public string LeftLittle { get; set; } //4

        public string RightThumb { get; set; } //5
        public string RightIndex { get; set; } //6
        public string RightMiddle { get; set; } //7
        public string RightRing { get; set; } //8
        public string RightLittle { get; set; } //9
        public Student Student { get; set; }
    }
}
