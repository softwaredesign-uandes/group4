using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlockModelReader
{
    [RestResource]
    public class BlockModelResource
    {

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/block_models/")]
        public IHttpContext GetBlockModels(IHttpContext context)
        {
            dynamic jsonResponse = new JObject();
            JObject[] block_models = MineralDepositsEnvironment.GetBlockModels().Select(blockModel =>
            {
            dynamic block_model = new JObject();
            block_model.id = blockModel.GetId();
            return block_model as JObject;
            }).ToArray();
            jsonResponse.block_models = new JArray(block_models);
            string responseString = jsonResponse.ToString();
            context.Response.ContentType = ContentType.JSON;
            context.Response.SendResponse(responseString);
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/block_models/[id]")]
        public IHttpContext GetBlockModel(IHttpContext context)
        {
            int id = int.Parse(context.Request.PathParameters["id"]);
            context.Response.ContentType = ContentType.JSON;
            BlockModel blockModel = MineralDepositsEnvironment.GetBlockModel(id);
            dynamic jsonResponse = new JObject();
            jsonResponse.block_model = new JObject();
            jsonResponse.block_model.id = id;
            jsonResponse.block_model.total_weight = blockModel.GetTotalWeight(blockModel.GetBlocks());
            jsonResponse.block_model.air_blocks_percentage = blockModel.CalculateAirBlocksPercentage(blockModel.GetBlocks());

            Dictionary<string, double> mineralWeights = blockModel.CalculateMineralWeights(blockModel.GetBlocks());
            jsonResponse.block_model.mineral_weights = new JArray();
            foreach(string key in mineralWeights.Keys)
            {
                JObject mineral_weight = JObject.Parse("{\"" + key + "\": " + mineralWeights[key].ToString() + " }");
                jsonResponse.block_model.mineral_weights.Add(mineral_weight);
            }

            string responseString = jsonResponse.ToString();
            context.Response.ContentType = ContentType.JSON;
            context.Response.SendResponse(responseString);
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/block_models/")]
        public IHttpContext CreateBlockModel(IHttpContext context)
        {
            context.Response.ContentType = ContentType.JSON;
            dynamic payload = JsonConvert.DeserializeObject(context.Request.Payload);
            if (payload.block_model.x_indices != null)
            {
                int[] xIndices = payload.block_model.x_indices.ToObject<int[]>();
                int[] yIndices = payload.block_model.y_indices.ToObject<int[]>();
                int[] zIndices = payload.block_model.z_indices.ToObject<int[]>();
                double[] weights = payload.block_model.z_indices.ToObject<double[]>();
                Dictionary<string, double[]> grades = payload.block_model.grades.ToObject<Dictionary<string, double[]>>();
                int[] blockIterations = Enumerable.Range(0, xIndices.Length).ToArray();
                List<IReblockable> blocks = blockIterations.Select(i =>
                {
                    IReblockable block = new Block(xIndices[i], yIndices[i], zIndices[i], weights[i]);
                    foreach (string key in grades.Keys)
                    {
                        ((Block)block).SetGrade(key, grades[key][i]);
                    }
                    return block;
                }).ToList();
                BlockModel blockModel = new BlockModel();
                blockModel.SetBlocks(blocks);
                MineralDepositsEnvironment.AddBlockModel(blockModel);
                context.Response.SendResponse("Successfuly Added Block Model");
            }
            else if(payload.block_model.base_block_model_id != null)
            {
                int blockModelId = payload.block_model.base_block_model_id.ToObject<int>();
                int reblockX = payload.block_model.reblock_x.ToObject<int>();
                int reblockY = payload.block_model.reblock_y.ToObject<int>();
                int reblockZ = payload.block_model.reblock_z.ToObject<int>();
                BlockModel blockModel = new BlockModel();
                blockModel.SetBlocks(MineralDepositsEnvironment.GetBlockModel(blockModelId).ReBlock(reblockX, reblockY, reblockZ, true));
                MineralDepositsEnvironment.AddBlockModel(blockModel);
                context.Response.SendResponse("Successfuly Added Reblocked Block Model");
            }
            MineralDepositsEnvironment.SerializePersistentData();
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/block_models/[id]/blocks")]
        public IHttpContext GetBlocks(IHttpContext context)
        {
            int id = int.Parse(context.Request.PathParameters["id"]);
            context.Response.ContentType = ContentType.JSON;
            BlockModel blockModel = MineralDepositsEnvironment.GetBlockModel(id);
            dynamic jsonResponse = new JObject();
            JObject[] blocks = blockModel.GetBlocks().Select(block =>
            {
                int[] coordinates = block.GetCoordinates();
                dynamic j_block = new JObject();
                j_block.id = block.GetId();
                j_block.x_index = coordinates[0];
                j_block.y_index = coordinates[1];
                j_block.z_index = coordinates[2];
                j_block.weight = block.GetWeight();
                Dictionary<string, double> grades = block.GetGrades();
                j_block.grades = new JArray();
                foreach (string key in grades.Keys)
                {
                    JObject grade = JObject.Parse("{\"" + key + "\": " + grades[key].ToString() + " }");
                    j_block.grades.Add(grade);
                }
                return j_block as JObject;
            }).ToArray();
            jsonResponse.blocks = new JArray(blocks);
            string responseString = jsonResponse.ToString();
            context.Response.ContentType = ContentType.JSON;
            context.Response.SendResponse(responseString);
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/block_models/[id]/blocks/[block_id]")]
        public IHttpContext GetBlock(IHttpContext context)
        {
            int id = int.Parse(context.Request.PathParameters["id"]);
            int blockId = int.Parse(context.Request.PathParameters["block_id"]);
            context.Response.ContentType = ContentType.JSON;
            BlockModel blockModel = MineralDepositsEnvironment.GetBlockModel(id);
            dynamic jsonResponse = new JObject();
            IReblockable block = blockModel.SimpleIdQuery(blockId);
            int[] coordinates = block.GetCoordinates();
            jsonResponse.block = new JObject();
            jsonResponse.block.id = block.GetId();
            jsonResponse.block.x_index = coordinates[0];
            jsonResponse.block.y_index = coordinates[1];
            jsonResponse.block.z_index = coordinates[2];
            jsonResponse.block.weight = block.GetWeight();
            Dictionary<string, double> grades = block.GetGrades();
            jsonResponse.block.grades = new JArray();
            foreach (string key in grades.Keys)
            {
                JObject grade = JObject.Parse("{\"" + key + "\": " + grades[key].ToString() + " }");
                jsonResponse.block.grades.Add(grade);
            }
            string responseString = jsonResponse.ToString();
            context.Response.ContentType = ContentType.JSON;
            context.Response.SendResponse(responseString);
            return context;
        }

    }
}
