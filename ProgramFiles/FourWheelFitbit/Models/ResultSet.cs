using System;
namespace FourWheelFitbit.Models
{
    public class ResultSet
    {
        public DateTime StartTime
        {
            get; set;
        }

        public DateTime EndTime
        {
            get; set;
        }

        public bool IsMoving
        {
            get; set;
        }

        public ResultSet(DateTime start, DateTime end, bool moving)
        {
            StartTime = start;
            EndTime = end;
            IsMoving = moving;
        }
    }
}
