# Exchanging external Tokens (Google, Twitter, Facebook) with IdentityServer access tokens using an extension grant

#### How to exchange external tokens for IdentityServer access token ?
* Request authentication using the provider's native libary.
* Exchange external token with IdentityServer token.

You can change **Provider** to **Facebook , Google and Twitter**

If the user's email is not retrieved from the provider, identity server returns the user object associated with the access token from the given 
provider.


![Alt text](https://github.com/waqaskhan540/IdentityServerExternalAuth/blob/master/screenshots/id1.PNG?raw=true "")

Make another request to **connect/token** , this time with an extra **email** parameter.

![Alt text](https://github.com/waqaskhan540/IdentityServerExternalAuth/blob/master/screenshots/id2.PNG?raw=true "")



