using System;
using System.Collections.Generic;
using System.Text;

namespace YPCode.CAML2SiteDesigns.Core.Entities
{
    public abstract class SiteAction
    {
        public abstract string Verb { get; }
    }

    public abstract class ActionWithSubActions : SiteAction
    {
        public SiteAction[] Subactions { get; set; }
    }

    public class CreateSPListSiteAction : ActionWithSubActions
    {
        public override string Verb => "createSPList";
        public string ListName { get; set; }
        public int TemplateType { get; set; }
    }

    public class SetTitleSiteAction : SiteAction
    {
        public override string Verb => "setTitle";
        public string Title { get; set; }
    }

    public class SetDescriptionSiteAction : SiteAction
    {
        public override string Verb => "setDescription";
        public string Description { get; set; }
    }

    public class AddSPFieldXmlSiteAction : SiteAction
    {
        public override string Verb => "addSPFieldXml";
        public string SchemaXml { get; set; }
    }
}
