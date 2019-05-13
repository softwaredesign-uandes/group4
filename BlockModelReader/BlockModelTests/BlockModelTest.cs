using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockModelReader;
using System.Linq;

namespace BlockModelTests
{
    [TestClass]
    public class BlockModelTest
    {
        private double totalWeight;
        private Dictionary<string, double> totalMineralWeights;
        private List<Block> GenerateBlocks()
        {
            List<Block> blockList = new List<Block>();
            int id = 0;
            totalWeight = 0;
            totalMineralWeights = new Dictionary<string, double>
            {
                ["Au"] = 0,
                ["Cu"] = 0,
            };
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        double weight = 100 * (j + 1);
                        totalWeight += weight;
                        Dictionary<string, double> grades = new Dictionary<string, double>
                        {
                            ["Au"] = (i + 1) * 0.1,
                            ["Cu"] = (k + 1) * 0.1
                        };
                        totalMineralWeights["Au"] += weight*(i + 1) * 0.1;
                        totalMineralWeights["Cu"] += weight*(k + 1) * 0.1;
                        Block block = new Block(id, i, j, k, weight, grades);
                        blockList.Add(block);
                        id++;
                    }
                }
            }
            return blockList;
        }

        [TestMethod]
        public void Test_ReBlock()
        {
            BlockModel blockModel = new BlockModel();
            List<Block> blockList = GenerateBlocks();
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

            CollectionAssert.AreEqual(result, blockModel.GetBlocks());
        }

        [TestMethod]
        public void Test_CalculateEmptyClusterWeight()
        {
            List<Block> blocks = new List<Block>();
            BlockModel blockModel = new BlockModel();
            double weight = blockModel.TotalWeight(blocks);
            Assert.AreEqual(0, weight);
        }

        [TestMethod]
        public void Test_TotalWeight()
        {
            List<Block> blocks = GenerateBlocks();
            BlockModel blockModel = new BlockModel();
            double weight = blockModel.TotalWeight(blocks);
            Assert.AreEqual(totalWeight, weight);
        }

        [TestMethod]
        public void Test_EmptyBlockModelMineralWeight()
        {
            BlockModel blockModel = new BlockModel();
            List<Block> blocks = new List<Block>();
            Dictionary<string, double> mineralWeights = blockModel.MineralWeights(blocks);
            Dictionary<string, double> emptyMineralWeights = new Dictionary<string, double>();
            CollectionAssert.AreEquivalent(emptyMineralWeights, mineralWeights);
        }

        [TestMethod]
        public void Test_MineralWeights()
        {
            BlockModel blockModel = new BlockModel();
            List<Block> blocks = GenerateBlocks();
            blockModel.SetBlocks(blocks);
            Dictionary<string, double> mineralWeights = blockModel.MineralWeights(blocks);
            CollectionAssert.AreEquivalent(totalMineralWeights, mineralWeights);
        }

        [TestMethod]
        public void Test_ClusterGrades()
        {
            BlockModel blockModel = new BlockModel();
            List<Block> blocks = GenerateBlocks();
            Dictionary<string, double> mineralWeights = 
                blockModel.CalculateClusterGrades(blocks, totalWeight);
            totalMineralWeights["Au"] /= totalWeight;
            totalMineralWeights["Cu"] /= totalWeight;
            totalMineralWeights["Au"] = Math.Round(totalMineralWeights["Au"], 6);
            totalMineralWeights["Cu"] = Math.Round(totalMineralWeights["Cu"], 6);
            CollectionAssert.AreEquivalent(totalMineralWeights, mineralWeights);
        }

        [TestMethod]
        public void Test_AirBlockPercentage()
        {
            BlockModel blockModel = new BlockModel();
            List<Block> blocks = GenerateBlocks();
            double airPercentage = blockModel.AirBlocksPercentage(blocks);
            Assert.AreEqual(0, airPercentage);
        }
    }
}
