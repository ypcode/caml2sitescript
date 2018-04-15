using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YPCode.CAML2SiteDesigns.Core.Entities;

namespace YPCode.CAML2SiteDesigns.Core
{
    public class SiteScriptJsonSerializer
    {
        public static void Serialize(SiteScript siteScript, Stream targetStream)
        {
            string json = JsonConvert.SerializeObject(siteScript, Formatting.Indented, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var streamWriter = new StreamWriter(targetStream, Encoding.UTF8);
            streamWriter.Write(json);
            streamWriter.Flush();
        }
    }
}
