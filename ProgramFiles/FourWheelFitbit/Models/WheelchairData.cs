using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace FourWheelFitbit.Models
{
    public class WheelchairData
    {
        public DataTable wheelchairDataTable = new DataTable();

        public WheelchairData(string inputString)
        {
            wheelchairDataTable.Columns.AddRange(new DataColumn[4] { new DataColumn("x", typeof(Double)),
                new DataColumn("y", typeof(Double)),
                new DataColumn("z", typeof(Double)),
                new DataColumn("time",typeof(Double))
            });

            foreach (string row in inputString.Split('\n'))
            {
                if (!string.IsNullOrEmpty(row))
                {
                    wheelchairDataTable.Rows.Add();
                    int i = 0;

                    // Execute a loop over the columns.  
                    foreach (string cell in row.Split(','))
                    {
                        var doubleCell = Convert.ToDouble(cell);
                        wheelchairDataTable.Rows[wheelchairDataTable.Rows.Count - 1][i] = doubleCell;
                        i++;
                    }

                    // Check to make sure all four columns are filled for each row
                    if (i != 4) throw new Exception($"Input data did not match specified format {Environment.StackTrace}");
                }
            }
        }
    }
}
