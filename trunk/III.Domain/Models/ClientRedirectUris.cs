﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESEIM.Models
{
    public class ClientRedirectUri
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string RedirectUri { get; set; }

        public virtual Client Clients { get; set; }
    }
}
