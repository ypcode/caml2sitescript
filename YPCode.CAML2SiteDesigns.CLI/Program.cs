using Fclp;
using System;
using System.IO;
using YPCode.CAML2SiteDesigns.Core;

namespace YPCode.CAML2SiteDesigns.CLI
{

    class ListSchemaConvertToSiteScriptArgs
    {
        public string SchemaFilePath { get; set; }

        public string OutputFilePath { get; set; }

        public static void Setup(FluentCommandLineParser parser, Action<ListSchemaConvertToSiteScriptArgs> execute)
        {
            var convertListSchemaCommand = parser.SetupCommand<ListSchemaConvertToSiteScriptArgs>("convert-list-schema").OnSuccess(args => execute(args));
            convertListSchemaCommand.Setup(args => args.SchemaFilePath).As('s', "schema")
                .Required()
                .WithDescription("The source schema xml file to convert")
                .UseForOrphanArguments();

            convertListSchemaCommand.Setup(args => args.OutputFilePath).As('o', "output")
                .WithDescription("The specified output file");
        }
    }



    class Program
    {
        static void Main(string[] args)
        {
            FluentCommandLineParser parser = new FluentCommandLineParser();

            ListSchemaConvertToSiteScriptArgs.Setup(parser, ConvertListSchema);

            parser.Parse(args);
        }

        static void ConvertListSchema(ListSchemaConvertToSiteScriptArgs args)
        {
            try
            {
                string outputFilePath = args.OutputFilePath ?? "site-script.json";
                using (var fs = File.OpenWrite(outputFilePath))
                {
                    var siteScript = CAMLSchemaToSiteScriptConverter.ConvertListSchema(args.SchemaFilePath);
                    SiteScriptJsonSerializer.Serialize(siteScript, fs);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("An error occured while converting List Schema to Site Script");
                Console.Error.Write(ex.ToString());
            }
        }
    }
}
