using System.Security.Claims;

namespace EdDSAJwtBearer
{
    public class EdDSAJwtBearerServer
    {
        public EdDSAJwtBearerServerOptions EdDSAJwtBearerServerOptions { get; set; }

        public EdDSAJwtBearerServer()
        {

        }

        public EdDSAJwtBearerServer(EdDSAJwtBearerServerOptions edDSAJwtBearerServerOptions)
        {
            this.EdDSAJwtBearerServerOptions = edDSAJwtBearerServerOptions;
        }

        public string CreateToken(IEnumerable<Claim> claims,
            string[] roles, DateTime expires) => EdDSATokenHandler.CreateToker(EdDSAJwtBearerServerOptions.PrivateSigningKey,
                                                                               EdDSAJwtBearerServerOptions.Issuer,
                                                                               EdDSAJwtBearerServerOptions.Audience,
                                                                               claims, roles, expires);
    }
}
