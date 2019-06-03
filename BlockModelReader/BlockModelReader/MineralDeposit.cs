using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BlockModelReader
{
    [Serializable]
    public class MineralDeposit
    {
        private List<BlockModel> blockModels;
        private readonly int id;
        private string name;
        private static int mineralDepositIdCount;

        public static void SetIdCount(int idCount)
        {
            mineralDepositIdCount = idCount;
        }

        public static void SerializeIdCount()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("MineralDepositIdCount.dat", FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, mineralDepositIdCount);
            stream.Close();
        }

        public MineralDeposit(string name)
        {
            this.id = mineralDepositIdCount;
            this.name = name;
            blockModels = new List<BlockModel>();
            mineralDepositIdCount++;
        }

        public int GetId()
        {
            return id;
        }

        public string GetName()
        {
            return name;
        }

        public void AddBlockModel()
        {
            blockModels.Add(new BlockModel());
        }

        public void AddBlockModel(BlockModel blockModel)
        {
            blockModels.Add(blockModel);
        }

        public void SetBlocks(int blockModelId,List<IReblockable> blocks)
        {
            GetBlockModel(blockModelId).SetBlocks(blocks);
        }

        public BlockModel GetBlockModel(int id)
        {
            return blockModels.First(i => i.GetId() == id);
        }

        public List<BlockModel> GetBlockModels()
        {
            return blockModels;
        }
    }
}
