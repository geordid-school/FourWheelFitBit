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
        public Int64 StillTime { get; set; }
        public Int64 MoveTime { get; set; }

        public List<ResultSet> AnalyzeDataTable(DataTable wheelchairData)
        {
            wheelchairData.Columns.Add("Jerk", typeof(Double));
            wheelchairData.Columns.Add("State", typeof(Int16)); // 1 = moving, 0 = still
            wheelchairData.Columns.Add("Duration", typeof(Double));
            Double xPosition;
            Double previousXPosition;
            Double yPosition;
            Double previousYPosition;
            Double zPosition;
            Double previousZPosition;
            Double time;
            Double previousTime = 0;
            bool move = false;
            List<ResultSet> testResults = new List<ResultSet>();
            MoveTime = 0;
            StillTime = 0;

            for (int i = 1; i < wheelchairData.Rows.Count; i++)
            {
                xPosition = Convert.ToDouble(wheelchairData.Rows[i]["x"]);
                previousXPosition = Convert.ToDouble(wheelchairData.Rows[i - 1]["x"]);
                yPosition = Convert.ToDouble(wheelchairData.Rows[i]["y"]);
                previousYPosition = Convert.ToDouble(wheelchairData.Rows[i - 1]["y"]);
                zPosition = Convert.ToDouble(wheelchairData.Rows[i]["z"]);
                previousZPosition = Convert.ToDouble(wheelchairData.Rows[i - 1]["z"]);
                time = Convert.ToDouble(wheelchairData.Rows[i]["time"]);
                previousTime = Convert.ToDouble(wheelchairData.Rows[i - 1]["time"]);

                wheelchairData.Rows[i]["Jerk"] = Math.Sqrt(((xPosition - previousXPosition) * (xPosition - previousXPosition)) + ((yPosition - previousYPosition) * (yPosition - previousYPosition)) + ((zPosition - previousZPosition) * (zPosition - previousZPosition)));
                wheelchairData.Rows[i]["State"] = (Convert.ToDouble(wheelchairData.Rows[i]["Jerk"]) > 0.2 ? 1 : 0);
                wheelchairData.Rows[i]["Duration"] = time - previousTime;

            }

            Double previousAverage = 0;
            Double forwardAverage = 0;
            Double movingAverage = 0;

            // Need data set of at least 23 to use moving average
            if (wheelchairData.Rows.Count >= 23)
            {
                for (int i = 1; i < 11; i++)
                {
                    forwardAverage = Convert.ToDouble((Convert.ToDouble(wheelchairData.Rows[i + 10]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i + 9]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i + 8]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i + 7]["Jerk"]) +
                        Convert.ToDouble(wheelchairData.Rows[i + 6]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i + 5]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i + 4]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i + 3]["Jerk"]) +
                        Convert.ToDouble(wheelchairData.Rows[i + 2]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i + 1]["Jerk"])) / 10);
                    wheelchairData.Rows[i]["State"] = forwardAverage >= 0.15 ? 1 : 0;

                    if (Convert.ToInt16(wheelchairData.Rows[i]["State"]) == 1)
                    {
                        MoveTime += Convert.ToInt64(wheelchairData.Rows[i]["Duration"]);
                    }
                    else
                    {
                        StillTime += Convert.ToInt64(wheelchairData.Rows[i]["Duration"]);
                    }
                }

                for (int i = 11; i < wheelchairData.Rows.Count - 10; i++)
                {
                    movingAverage = Convert.ToDouble((Convert.ToDouble(wheelchairData.Rows[i - 10]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i - 9]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i - 8]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i - 7]["Jerk"]) +
                        Convert.ToDouble(wheelchairData.Rows[i - 6]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i - 5]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i - 4]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i - 3]["Jerk"]) +
                        Convert.ToDouble(wheelchairData.Rows[i - 2]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i - 1]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i + 10]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i + 9]["Jerk"]) +
                        Convert.ToDouble(wheelchairData.Rows[i + 8]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i + 7]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i + 6]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i + 5]["Jerk"]) +
                        Convert.ToDouble(wheelchairData.Rows[i + 4]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i + 3]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i + 2]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i + 1]["Jerk"])) / 20);
                    wheelchairData.Rows[i]["State"] = movingAverage >= 0.15 ? 1 : 0;

                    if (Convert.ToInt16(wheelchairData.Rows[i]["State"]) == 1)
                    {
                        MoveTime += Convert.ToInt64(wheelchairData.Rows[i]["Duration"]);
                    }
                    else
                    {
                        StillTime += Convert.ToInt64(wheelchairData.Rows[i]["Duration"]);
                    }
                }

                for (int i = wheelchairData.Rows.Count - 10; i < wheelchairData.Rows.Count; i++)
                {
                    previousAverage = Convert.ToDouble((Convert.ToDouble(wheelchairData.Rows[i - 10]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i - 9]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i - 8]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i - 7]["Jerk"]) +
                        Convert.ToDouble(wheelchairData.Rows[i - 6]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i - 5]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i - 4]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i - 3]["Jerk"]) +
                        Convert.ToDouble(wheelchairData.Rows[i - 2]["Jerk"]) + Convert.ToDouble(wheelchairData.Rows[i - 1]["Jerk"])) / 10);
                    wheelchairData.Rows[i]["State"] = previousAverage >= 0.15 ? 1 : 0;

                    if (Convert.ToInt16(wheelchairData.Rows[i]["State"]) == 1)
                    {
                        MoveTime += Convert.ToInt64(wheelchairData.Rows[i]["Duration"]);
                    }
                    else
                    {
                        StillTime += Convert.ToInt64(wheelchairData.Rows[i]["Duration"]);
                    }
                }

            }

            DateTime start, end;
            // Not sure if conversion is completly correct, possible timezone issue?
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            start = epoch.AddMilliseconds(Convert.ToInt64(wheelchairData.Rows[0]["time"]));
            for (int i = 2; i < wheelchairData.Rows.Count; i++)
            {
                if (Convert.ToInt16(wheelchairData.Rows[i]["State"]) != Convert.ToInt16(wheelchairData.Rows[i - 1]["State"]))
                {
                    end = epoch.AddMilliseconds(Convert.ToInt64(wheelchairData.Rows[i]["time"]));
                    move = Convert.ToInt16(wheelchairData.Rows[i - 1]["State"]) == 1 ? true : false;
                    testResults.Add(new ResultSet(start, end, move));
                    start = epoch.AddMilliseconds(Convert.ToInt64(wheelchairData.Rows[i]["time"]));
                }

                if (i == (wheelchairData.Rows.Count - 1) && Convert.ToInt16(wheelchairData.Rows[i]["State"]) == Convert.ToInt16(wheelchairData.Rows[i - 1]["State"]))
                {
                    end = epoch.AddMilliseconds(Convert.ToInt64(wheelchairData.Rows[i]["time"]));
                    move = Convert.ToInt16(wheelchairData.Rows[i]["State"]) == 1 ? true : false;
                    testResults.Add(new ResultSet(start, end, move));
                }
            }

            return testResults;
        }
    }
}
