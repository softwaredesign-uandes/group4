using System.Collections.Generic;
using System.Linq;

namespace BlockModelReader
{
    public class MineralDeposit
    {
        private List<BlockModel> blockModels;
        private readonly int id;
        private int blockModelIdCount;

        public MineralDeposit(int id)
        {
            this.id = id;
            blockModels = new List<BlockModel>();
        }

        public int GetId()
        {
            return id;
        }
        public void AddBlockModel()
        {
            blockModels.Add(new BlockModel(blockModelIdCount));
            blockModelIdCount++;
        }

        public void SetBlocks(int blockModelId,List<IReblockable> blocks)
        {
            GetBlockModel(blockModelId).SetBlocks(blocks);
        }

        public BlockModel GetBlockModel(int id)
        {
            return blockModels.First(i => i.GetId() == id);
        }
    }
}
