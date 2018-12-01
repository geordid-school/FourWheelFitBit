using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using FourWheelFitbit.Models;

namespace FourWheelFitbit.AlgorithmAnalysis
{
    public class WheelchairMovementAnalyzer
    {
        //Jon's code goes here
        //for now it just returns bunk data so we can test the api
        // This should return a string, the controller can JSONify it
        public List<ResultSet> AnalyzeDataTable(DataTable wheelchairData)
        {
            wheelchairData.Columns.Add("Jerk", typeof(Double)); 
            wheelchairData.Columns.Add("State", typeof(string)); 
            wheelchairData.Columns.Add("Duration", typeof(Double));
            Double x, px, y, py, z, pz, t, pt = 0;
            bool move = false;
            List<ResultSet> testResults = new List<ResultSet>();
            Int64 moveTime = 0;
            Int64 stillTime = 0;


            for (int i = 1; i < wheelchairData.Rows.Count; i++)
            {
                x = Convert.ToDouble(wheelchairData.Rows[i]["x"]);
                px = Convert.ToDouble(wheelchairData.Rows[i - 1]["x"]);
                y = Convert.ToDouble(wheelchairData.Rows[i]["y"]);
                py = Convert.ToDouble(wheelchairData.Rows[i - 1]["y"]);
                z = Convert.ToDouble(wheelchairData.Rows[i]["z"]);
                pz = Convert.ToDouble(wheelchairData.Rows[i - 1]["z"]);
                t = Convert.ToDouble(wheelchairData.Rows[i]["time"]);
                pt = Convert.ToDouble(wheelchairData.Rows[i - 1]["time"]);

                wheelchairData.Rows[i]["Jerk"] = Math.Sqrt(((x - px) * (x - px)) + ((y - py) * (y - py)) + ((z - pz) * (z - pz)));
                wheelchairData.Rows[i]["State"] = (Convert.ToDouble(wheelchairData.Rows[i]["Jerk"]) > 0.2 ? "Moving" : "Still");
                wheelchairData.Rows[i]["Duration"] = t - pt;
                if (Convert.ToString(wheelchairData.Rows[i]["State"]) == "Moving")
                {
                    moveTime += Convert.ToInt64(wheelchairData.Rows[i]["Duration"]);
                }
                else
                {
                    stillTime += Convert.ToInt64(wheelchairData.Rows[i]["Duration"]);
                }
            }

            DateTime start, end;
            //Not sure if conversion is completly correct, possible timezone issue?
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            start = epoch.AddMilliseconds(Convert.ToInt64(wheelchairData.Rows[0]["time"]));
            for (int i = 2; i < wheelchairData.Rows.Count; i++)
            {
                if (wheelchairData.Rows[i]["State"] != wheelchairData.Rows[i - 1]["State"])
                {
                    end = epoch.AddMilliseconds(Convert.ToInt64(wheelchairData.Rows[i]["time"]));
                    move = Convert.ToString(wheelchairData.Rows[i - 1]["State"]) == "Moving" ? true : false;
                    testResults.Add(new ResultSet(start, end, move));
                    start = epoch.AddMilliseconds(Convert.ToInt64(wheelchairData.Rows[i]["time"]));
                }

                if (i == (wheelchairData.Rows.Count - 1) && wheelchairData.Rows[i]["State"] == wheelchairData.Rows[i - 1]["State"])
                {
                    end = epoch.AddMilliseconds(Convert.ToInt64(wheelchairData.Rows[i]["time"]));
                    move = Convert.ToString(wheelchairData.Rows[i]["State"]) == "Moving" ? true : false;
                    testResults.Add(new ResultSet(start, end, move));
                }
            }

            return testResults;
        }
    }
}
