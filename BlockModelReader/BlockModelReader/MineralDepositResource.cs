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
            context.Response.SendResponse("Successfuly Mineral Deposit Added");
            MineralDepositsEnvironment.SerializePersistentData();
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/mineral_deposits/")]
        public IHttpContext GetMineralDeposits(IHttpContext context)
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


        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/mineral_deposits/[id]")]
        public IHttpContext GetMineralDeposit(IHttpContext context)
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



        /* [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/load_settings")]
         public IHttpContext LoadSettings(IHttpContext context)
         {
             try
             {
                 context.Response.ContentType = ContentType.JSON;
                 dynamic payload = JsonConvert.DeserializeObject(context.Request.Payload);
                 string weightFormula = payload.weigth_formula;
                 FileReader.SetWeightExpression(weightFormula);
                 FileReader.SetOreNames(payload.ore_names.ToObject<string[]>());
                 FileReader.SetOreExpressions(payload.ore_formulas.ToObject<string[]>());
                 FileReader.ClearData();
                 context.Response.SendResponse("OK");

             }
             catch (Exception e)
             {
                 throw e;
             }
             return context;
         }
         [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/load_sheet")]
         public IHttpContext LoadSheet(IHttpContext context)
         {
                 context.Response.ContentType = ContentType.JSON;
                 dynamic payload = JsonConvert.DeserializeObject(context.Request.Payload);
                 double[][] dataSegment = payload.data.ToObject<double[][]>();
                 foreach (double[] d in dataSegment)
                 {
                     FileReader.AddData(d);
                 }
                 if (payload.last == "true")
                 {
                     context.Response.SendResponse("Finished");
                     MineralDeposit.SetBlocks(FileReader.ReadList());
                     Console.WriteLine("CHECK");
                 }
                 else
                 {
                     context.Response.SendResponse("OK");
                 }

             return context;
         }*/
     }
}
