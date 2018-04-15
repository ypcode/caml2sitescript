using System;
using System.Collections.Generic;
using System.Text;

namespace YPCode.CAML2SiteDesigns.Core.Entities
{
    public class SiteScript
    {
        public SiteAction[] Actions { get; set; }

        public int Version { get; set; }

        public object Bindata { get; set; }
    }
}
