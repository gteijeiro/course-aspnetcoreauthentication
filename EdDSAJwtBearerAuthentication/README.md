# EdDSA JWT authentication course example

This is an educational localhost example, not production-ready authentication.
The signing key in `SSOServer/appsettings.json` is a public sample key: anyone
can read it and sign tokens with it. Never trust it outside this local exercise.

Before adapting this code for a real deployment, generate a new key pair, supply
the private signing key through local .NET user secrets or an environment variable
(`JWT__PrivateSigningKey`), and configure consumers with the corresponding
public key. Do not commit the new private key or real credentials. Rotate any key
that was reused from this example; removing a file does not erase Git history.

The existing sample key is retained solely to keep the course runnable unchanged.
