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
            //Test data
            List<ResultSet> testResults = new List<ResultSet>();

            testResults.Add(new ResultSet(
                new DateTime(2018, 11, 21, 11, 0, 0),
                new DateTime(2018, 11, 21, 11, 0, 3),
                false
            ));

            testResults.Add(new ResultSet(
                new DateTime(2018, 11, 21, 11, 0, 3),
                new DateTime(2018, 11, 21, 11, 0, 5),
                true
            ));

            testResults.Add(new ResultSet(
                new DateTime(2018, 11, 21, 11, 0, 5),
                new DateTime(2018, 11, 21, 11, 0, 6),
                false
            ));

            testResults.Add(new ResultSet(
                new DateTime(2018, 11, 21, 11, 0, 6),
                new DateTime(2018, 11, 21, 11, 0, 14),
                true
            ));

            testResults.Add(new ResultSet(
                new DateTime(2018, 11, 21, 11, 0, 14),
                new DateTime(2018, 11, 21, 11, 0, 20),
                false
            ));

            return testResults;
        }
    }
}
