using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace BlockModelReader
{
    public static class FileReader
    {
        public static List<Block> ReadFile(BlockModel blockModel, string weightExpression, string[] gradeNames, string[] gradeExpressions, string path)
        {
            List<Block> result = new List<Block>();
            string unmutableWeigthExpression = weightExpression;
            string[] unmutableGradeExpressions = gradeExpressions;
            foreach (string line in File.ReadLines(path, Encoding.UTF8))
            {
                string mutableWeightExpression = (string)unmutableWeigthExpression.Clone();
                string[] mutableGradeExpressions = (string[])unmutableGradeExpressions.Clone();
                string[] lineValues = line.Split(' ');
                int id = int.Parse(lineValues[0]);
                int xCoordinate = int.Parse(lineValues[1]);
                int yCoordinate = int.Parse(lineValues[2]);
                int zCoordinate = int.Parse(lineValues[3]);
                double weight = SolveExpression(lineValues, unmutableWeigthExpression);
                Dictionary<string, double> grades = new Dictionary<string, double>();
                for(int i = 0; i < gradeExpressions.Length; i++)
                {
                    grades[gradeNames[i]] = SolveExpression(lineValues, mutableGradeExpressions[i]); 
                }
                //Dictionary<string, double> additionalData = new Dictionary<string, double>();
                //additionalData["proc_profit"] = double.Parse(lineValues[7]);
                result.Add(new Block(id, xCoordinate, yCoordinate, zCoordinate, weight, grades));
            }
            return result;
        }

        public static double SolveExpression(string[] lineValues, string expression)
        {
            string returnExpression = expression;
            for (int i = 0; i < 26; i++)
            {
                char currentLetter = (char)(i + 97);
                if (expression.Contains(currentLetter))
                {
                    returnExpression = returnExpression.Replace(currentLetter.ToString(), lineValues[4 + i]);
                }
            }
            return Evaluate(returnExpression);
        }


        public static List<Block> ReadMarvinFile(string path)
        {
            List<Block> result = new List<Block>();
            foreach (string line in File.ReadLines(path, Encoding.UTF8))
            {
                string[] lineValues = line.Split(' ');
                int id = int.Parse(lineValues[0]);
                int xCoordinate = int.Parse(lineValues[1]);
                int yCoordinate = int.Parse(lineValues[2]);
                int zCoordinate = int.Parse(lineValues[3]);
                double weight = double.Parse(lineValues[4]);
                Dictionary<string, double> grades = new Dictionary<string, double>();
                grades["Au"] = double.Parse(lineValues[5]) / 10000;
                grades["Cu"] = double.Parse(lineValues[6]);
                Dictionary<string, double> additionalData = new Dictionary<string, double>();
                additionalData["proc_profit"] = double.Parse(lineValues[7]);
                result.Add(new Block(id, xCoordinate, yCoordinate, zCoordinate, weight, grades, additionalData));
            }
            return result;
        }

        public static List<Block> ReadZuckSmallFile(string path)
        {
            List<Block> result = new List<Block>();
            foreach (string line in File.ReadLines(path, Encoding.UTF8))
            {
                string[] lineValues = line.Split(' ');
                int id = int.Parse(lineValues[0]);
                int xCoordinate = int.Parse(lineValues[1]);
                int yCoordinate = int.Parse(lineValues[2]);
                int zCoordinate = int.Parse(lineValues[3]);
                double rockWeight = double.Parse(lineValues[6]);
                double oreWeight = double.Parse(lineValues[7]);
                double weight = rockWeight + oreWeight;
                Dictionary<string, double> grades = new Dictionary<string, double>();
                grades["Ore"] = oreWeight / (oreWeight + rockWeight);
                Dictionary<string, double> additionalData = new Dictionary<string, double>();
                additionalData["cost"] = double.Parse(lineValues[4]);
                additionalData["value"] = double.Parse(lineValues[5]);
                additionalData["rock_tonnes"] = rockWeight;
                additionalData["ore_tonnes"] = oreWeight;
                result.Add(new Block(id, xCoordinate, yCoordinate, zCoordinate, weight, grades, additionalData));
            }
            return result;
        }

        private static double Evaluate(string expression)
        {
            DataTable table = new DataTable();
            table.Columns.Add("expression", typeof(string), expression);
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            return double.Parse((string)row["expression"]);
        }
    }
}
