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
        
        [TestMethod]
        public void Test_ReBlock()
        {
            BlockModel blockModel= new BlockModel();
            List<Block> blockList = new List<Block>();
            int id = 0;
            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        double weight = 100 * (j+1);
                        Dictionary<string, double> grades = new Dictionary<string, double>
                        {
                            ["Au"] = (i+1) * 0.1,
                            ["Cu"] = (k+1) * 0.1
                        };
                        Block block = new Block(id, i, j, k, weight, grades);
                        blockList.Add(block);
                        id++;
                    }
                }
            }
            blockModel.SetBlocks(blockList);

            List<Block> result = new List<Block>();
            Dictionary<string, double> g1 = new Dictionary<string, double>
            {
                ["Au"] = (100 * 0.1 + 100 * 0.1 + 100 * 0.2 + 100 * 0.2 + 200 * 0.1 + 200 * 0.1 + 200 * 0.2 + 200 * 0.2) / 1200,
                ["Cu"] = (100 * 0.1 + 100 * 0.1 + 100 * 0.2 + 100 * 0.2 + 200 * 0.1 + 200 * 0.1 + 200 * 0.2 + 200 * 0.2) / 1200
            };
            Dictionary<string, double> g2 = new Dictionary<string, double>
            {
                ["Au"] = (100 * 0.1 + 100 * 0.2 + 200 * 0.1 + 200 * 0.2) / 600,
                ["Cu"] = (100 * 0.3 + 100 * 0.3 + 200 * 0.3 + 200 * 0.3) / 600
            };
            Dictionary<string, double> g3 = new Dictionary<string, double>
            {
                ["Au"] = (300 * 0.1 + 300 * 0.2 + 300 * 0.1 + 300 * 0.2) / 1200,
                ["Cu"] = (300 * 0.1 + 300 * 0.2 + 300 * 0.1 + 300 * 0.2) / 1200
            };
            Dictionary<string, double> g4 = new Dictionary<string, double>
            {
                ["Au"] = (300 * 0.1 + 300 * 0.2) / 600,
                ["Cu"] = (300 * 0.3 + 300 * 0.3) / 600
            };
            Dictionary<string, double> g5 = new Dictionary<string, double>
            {
                ["Au"] = (100 * 0.3 + 100 * 0.3 + 200 * 0.3 + 200 * 0.3) / 600,
                ["Cu"] = (100 * 0.1 + 100 * 0.2 + 200 * 0.1 + 200 * 0.2) / 600
            };
            Dictionary<string, double> g6 = new Dictionary<string, double>
            {
                ["Au"] = (100 * 0.3 + 200 * 0.3) / 300,
                ["Cu"] = (100 * 0.3 + 200 * 0.3) / 300
            };
            Dictionary<string, double> g7 = new Dictionary<string, double>
            {
                ["Au"] = (300 * 0.3 + 300 * 0.3) / 600,
                ["Cu"] = (300 * 0.1 + 300 * 0.2) / 600
            };
            Dictionary<string, double> g8 = new Dictionary<string, double>
            {
                ["Au"] = (300 * 0.3) / 300,
                ["Cu"] = (300 * 0.3) / 300
            };

            result.Add(new Block(0, 0, 0, 0, 1200, g1));
            result.Add(new Block(1, 0, 0, 1, 600, g2));
            result.Add(new Block(2, 0, 1, 0, 1200, g3));
            result.Add(new Block(3, 0, 1, 1, 600, g4));
            result.Add(new Block(4, 1, 0, 0, 600, g5));
            result.Add(new Block(5, 1, 0, 1, 300, g6));
            result.Add(new Block(6, 1, 1, 0, 600, g7));
            result.Add(new Block(7, 1, 1, 1, 300, g8));

            blockModel.ReBlock(2, 2, 2);

            CollectionAssert.AreEquivalent(result, blockModel.GetBlocks());
        }
    }
}
