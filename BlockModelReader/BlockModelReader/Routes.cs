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


namespace BlockModelReader
{
    [RestResource]
    public class Routes
    {
        [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/load_settings")]
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
                    BlockAssembler.SetBlocks(FileReader.ReadList());
                    Console.WriteLine("CHECK");
                }
                else
                {
                    context.Response.SendResponse("OK");
                }

            return context;
            }
    }
}
