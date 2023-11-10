namespace EdDSAJwtBearer
{
    public static class EdDSAJwtBearerErros
    {
        public const string ValidIssuerRequired = "ValidIssuer is required when ValidateIssuer is true";
        public const string ValidAudienceRequired = "ValidAudience is required when ValidateAudience is true";

        public const string InvalidToken = "(001) Invalid Bearer authentication token";
        public const string InvalidIssuer = "(002) Invalid Issuer";
        public const string InvalidAudience = "(003) Invalid Audience";
        public const string ExpireToken = "(004) Token has expired";
    }
}
