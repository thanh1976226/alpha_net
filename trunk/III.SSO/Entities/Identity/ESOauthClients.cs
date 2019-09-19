﻿using System;
using System.Collections.Generic;

namespace Host.Entities
{
    public partial class ESOauthClient
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUris { get; set; }
        public string PostLogoutRedirectUris { get; set; }
        public int? AccessTokenLifetime { get; set; }
        public string GrantTypes { get; set; }
        public string AllowedScopes { get; set; }
        public bool? RequireConsent { get; set; }
        public string LogoutUri { get; set; }
        public string AllowedCorsOrigins { get; set; }
        public bool? AllowAccessTokensViaBrowser { get; set; }


    }
}
