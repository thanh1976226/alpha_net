﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESEIM.Models
{
    public class ClientPostLogoutRedirectUri
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string PostLogoutRedirectUri { get; set; }

        public virtual Client Clients { get; set; }
    }
}
