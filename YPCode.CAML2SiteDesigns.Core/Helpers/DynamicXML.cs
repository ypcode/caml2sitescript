using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace YPCode.CAML2SiteDesigns.Core.Helpers
{
    public class DynamicXml : DynamicObject
    {
        XElement _root;
        private DynamicXml(XElement root)
        {
            _root = root;
        }

        public static DynamicXml Parse(string xmlString)
        {
            XElement root = XDocument.Parse(xmlString).Root;
            return new DynamicXml(root);
        }

        public static DynamicXml Load(string filename)
        {
            XElement root = XDocument.Load(filename).Root;
            return new DynamicXml(root);
        }

        public static DynamicXml Load(Stream xmlStream)
        {
            XElement root = XDocument.Load(xmlStream).Root;
            return new DynamicXml(root);
        }

        public override string ToString()
        {

            return _root?.ToString() ?? "";
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            var att = _root.Attribute(binder.Name);
            if (att != null)
            {
                result = att.Value;
                return true;
            }

            var nodes = _root.Elements(binder.Name);
            if (nodes.Count() > 1)
            {
                result = nodes.Select(n => (n.HasElements || n.HasAttributes) ? (object)new DynamicXml(n) : n.Value).ToList();
                return true;
            }

            var node = _root.Element(binder.Name);
            if (node != null)
            {
                result = node.HasElements || node.HasAttributes ? (object)new DynamicXml(node) : node.Value;
                return true;
            }

            return true;
        }
    }
}
