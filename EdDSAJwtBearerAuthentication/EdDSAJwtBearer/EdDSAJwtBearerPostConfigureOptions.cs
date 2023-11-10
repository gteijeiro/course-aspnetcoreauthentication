using Microsoft.Extensions.Options;

namespace EdDSAJwtBearer
{
    public class EdDSAJwtBearerPostConfigureOptions : IPostConfigureOptions<EdDSAJwtBearerOptions>
    {
        public void PostConfigure(string name, EdDSAJwtBearerOptions options)
        {
            if (options.ValidateIssuer && string.IsNullOrWhiteSpace(options.ValidIssuer))
            {
                throw new InvalidOperationException(EdDSAJwtBearerErros.ValidIssuerRequired);
            }

            if (options.ValidateAudience && string.IsNullOrWhiteSpace(options.ValidAudience))
            {
                throw new InvalidOperationException(EdDSAJwtBearerErros.ValidAudienceRequired);
            }
        }
    }
}
