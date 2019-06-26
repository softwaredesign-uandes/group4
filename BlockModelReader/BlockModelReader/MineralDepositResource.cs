using System;
using System.Collections.Generic;
using System.Linq;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlockModelReader
{
    [RestResource]
    public class MineralDepositResource
    {

        [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/mineral_deposits/")]
        public IHttpContext CreateMineralDeposit(IHttpContext context)
        {

                context.Response.ContentType = ContentType.JSON;
                dynamic payload = JsonConvert.DeserializeObject(context.Request.Payload);
                string name = payload.mineral_deposit.ToObject<Dictionary<string, string>>()["name"];
                MineralDepositsEnvironment.AddMineralDeposit(name);
                context.Response.SendResponse("Mineral Deposit Successfuly Added");
                MineralDepositsEnvironment.SerializePersistentData();
                return context;

        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/mineral_deposits/")]
        public IHttpContext GetMineralDeposits(IHttpContext context)
        {
            try
            {
                dynamic jsonResponse = new JObject();
                JObject[] mineral_deposits = MineralDepositsEnvironment.GetMineralDeposits().Select(mineralDeposit =>
                {
                    dynamic mineral_deposit = new JObject();
                    mineral_deposit.id = mineralDeposit.GetId();
                    mineral_deposit.name = mineralDeposit.GetName();
                    return mineral_deposit as JObject;
                }).ToArray();
                jsonResponse.mineral_deposits = new JArray(mineral_deposits);
                string responseString = jsonResponse.ToString();
                context.Response.ContentType = ContentType.JSON;
                context.Response.SendResponse(responseString);
                return context;
            }
            catch (Exception e)
            {
                context.Response.SendResponse(e.ToString());
                return context;
            }
        }


        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/mineral_deposits/[id]")]
        public IHttpContext GetMineralDeposit(IHttpContext context)
        {
            try
            {
                int id = int.Parse(context.Request.PathParameters["id"]);
                context.Response.ContentType = ContentType.JSON;
                MineralDeposit mineralDeposit = MineralDepositsEnvironment.GetMineralDeposit(id);
                dynamic jsonResponse = new JObject();
                jsonResponse.mineral_deposit = new JObject();
                jsonResponse.mineral_deposit.id = id;
                jsonResponse.mineral_deposit.name = mineralDeposit.GetName();
                JObject[] block_models = mineralDeposit.GetBlockModels().Select(blockModel =>
                {
                    dynamic mineral_deposit = new JObject();
                    mineral_deposit.id = blockModel.GetId();
                    return mineral_deposit as JObject;
                }).ToArray();
                jsonResponse.mineral_deposit.block_models = new JArray(block_models);
                string responseString = jsonResponse.ToString();
                context.Response.ContentType = ContentType.JSON;
                context.Response.SendResponse(responseString);
                return context;
            }
            catch (Exception e)
            {
                context.Response.SendResponse(e.ToString());
                return context;
            }
        }
     }
}
