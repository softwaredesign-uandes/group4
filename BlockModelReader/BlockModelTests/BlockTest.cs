using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockModelReader;
using System.Collections.Generic;

namespace BlockModelTests
{
    [TestClass]
    public class BlockTest
    {
        [TestMethod]
        public void Test_SetGrade()
        {
            Dictionary<string, double> grades = new Dictionary<string, double>
            {
                ["Au"] = 0.005678,
                ["Cu"] = 0.00463
            };
            Block block = new Block(0, 0, 0, 0, 6.78, grades);
            block.SetGrade("Au", 0.03);
            Assert.AreEqual(block.GetGrades()["Au"], 0.03);
        }

        [TestMethod]
        public void Test_GetId()
        {
            List<Block> blocks = new List<Block>();
            for (int i = 0; i < 5; i++)
            {
                Block block = new Block(i, i, i + 1, i + 2, 200);
                blocks.Add(block);
            }
            int thirdBlockId = blocks[2].GetId();
            Assert.AreEqual(thirdBlockId, 2);
        }

        [TestMethod]
        public void Test_GetCoordinates()
        {
            List<Block> blocks = new List<Block>();
            for (int i = 0; i < 5; i++)
            {
                Block block = new Block(i, i + 3, i + 1, i + 2, 200);
                blocks.Add(block);
            }
            int[] secondBlockCoordinates = blocks[1].GetCoordinates();
            int[] expectedResult = new int[] { 4, 2, 3 };
            CollectionAssert.AreEqual(secondBlockCoordinates, expectedResult);
        }

        public void Test_GetWeight()
        {
            Block block = new Block(1, 1, 1, 1, 6.76);
            double weight = block.GetWeight();
            Assert.AreEqual(weight, 6.76);
        }

        [TestMethod]
        public void Test_GetGrades()
        {
            List<Block> blocks = new List<Block>();
            for (int i = 0; i < 5; i++)
            {
                Dictionary<string, double> grades = new Dictionary<string, double>
                {
                    ["Au"] = (i + 1) * 0.56,
                    ["Cu"] = (i + 3) * 0.34
                };
                Block block = new Block(i, i + 3, i + 1, i + 2, 200, grades);
                blocks.Add(block);
            }

            Dictionary<string, double> lastBlockGrades = blocks[4].GetGrades();
            Dictionary<string, double> expectedGrades = new Dictionary<string, double>
            {
                ["Au"] = 5*0.56,
                ["Cu"] = 7*0.34
            };
            CollectionAssert.AreEquivalent(lastBlockGrades, expectedGrades);
        }      
    }
}
