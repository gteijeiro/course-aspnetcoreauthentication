using Microsoft.AspNetCore.Authentication;

namespace EdDSAJwtBearer
{
    public class EdDSAJwtBearerOptions : AuthenticationSchemeOptions
    {
        public string PublicSigningKey { get; set; }
        public bool ValidateIssuer { get; set; }
        public string ValidIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public string ValidAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool SaveToken { get; set; }

    }
}
