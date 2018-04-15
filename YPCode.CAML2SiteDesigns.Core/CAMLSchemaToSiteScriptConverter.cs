using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using YPCode.CAML2SiteDesigns.Core.Entities;
using YPCode.CAML2SiteDesigns.Core.Helpers;

namespace YPCode.CAML2SiteDesigns.Core
{
    public class CAMLSchemaToSiteScriptConverter
    {
        public static SiteScript ConvertListSchema(string filePath)
        {
            dynamic document = DynamicXml.Load(filePath);

            return ConvertListSchema(document);
        }
        public static SiteScript ConvertListSchema(Stream xmlStream)
        {
            dynamic document = DynamicXml.Load(xmlStream);

            return ConvertListSchema(document);
        }
        public static SiteScript ConvertListSchema(DynamicXml document)
        {
            dynamic listSchema = document;
            var resultScript = new SiteScript()
            {
                Actions = new SiteAction[]
             {
                    new CreateSPListSiteAction()
                    {
                        ListName = listSchema.Title,
                        TemplateType = int.Parse(listSchema.ServerTemplate as string),
                        Subactions = GetCreateListSubActions(document)
                    }
             },
                Bindata = new object(),
                Version = 1
            };

            return resultScript;
        }

        private static string[] ExcludedFields = new string[]
        {
            "Title",
            "ContentType",
            "Edit",
            "LinkTitleNoMenu",
            "LinkTitle",
            "DocIcon",
            "_IsRecord",
            "ComplianceAssetId",
            "ID",
            "Modified",
            "Created",
            "Author",
            "Editor",
            "Attachments",
            "ItemChildCount",
            "FolderChildCount",
            "_UIVersionString",
            "_ComplianceFlags",
            "_ComplianceTag",
            "_ComplianceTagWrittenTime",
            "_ComplianceTagUserId",
            "AppAuthor",
            "AppEditor"
        };

        private static SiteAction[] GetCreateListSubActions(DynamicXml xmlDoc)
        {
            dynamic listSchema = xmlDoc;
            List<SiteAction> siteActionsList = new List<SiteAction>();

            // Set Title
            siteActionsList.Add(new SetTitleSiteAction()
            {
                Title = listSchema.Title,
            });

            // Set Description
            siteActionsList.Add(new SetDescriptionSiteAction()
            {
                Description = listSchema.Description,
            });

            dynamic fields = listSchema.Fields.Field;
            foreach (var field in fields)
            {
                string xmlString = field.ToString();
                if (!string.IsNullOrEmpty(xmlString))
                {
                    // Exclude Hidden fields
                    if ((field.Hidden == null || field.Hidden != "TRUE") && !ExcludedFields.Any(f => field.Name == f))
                    {
                        siteActionsList.Add(new AddSPFieldXmlSiteAction()
                        {
                            SchemaXml = xmlString
                        });
                    }
                }
            }

            return siteActionsList.ToArray();
        }
    }
}