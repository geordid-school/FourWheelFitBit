using System;
namespace FourWheelFitbit.Models
{
    public class ResultSet
    {
        public Double StartTime
        {
            get; set;
        }

        public Double EndTime
        {
            get; set;
        }

        public bool IsMoving
        {
            get; set;
        }

        public ResultSet(Double start, Double end, bool moving)
        {
            StartTime = start;
            EndTime = end;
            IsMoving = moving;
        }
    }
}
