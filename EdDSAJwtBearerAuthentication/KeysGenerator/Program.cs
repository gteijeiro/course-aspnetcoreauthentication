using EdDSAJwtBearer;

var EdDSAKeys = EdDSATokenHandler.CreateDerEncodeKeys();
Console.WriteLine("Private Key");
Console.WriteLine(EdDSAKeys.Private);
Console.WriteLine("Public Key:");
Console.WriteLine(EdDSAKeys.Public);
Console.WriteLine("Presiona <enter> para finalizar...");
Console.ReadLine();
