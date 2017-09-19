### External Authentication with IdentityServer4
> Exchange Facebook,Google and Twitter access token for an IdentityServer4 access token using ResourceOwner flow.

#### How to exchange external tokens for IdentityServer access token ?
Make the following request to IdentityServer's **connect/token** endpoint.

You can change **Provider** to **Facebook , Google and Twitter**

If the user's email is not retrieved from the provider, identity server returns the user object associated with the access token from the given 
provider.


![Alt text](https://github.com/waqaskhan540/IdentityServerExternalAuth/blob/master/screenshots/id1.PNG?raw=true "")

Make another request to **connect/token** , this time with an extra **email** parameter.

![Alt text](https://github.com/waqaskhan540/IdentityServerExternalAuth/blob/master/screenshots/id2.PNG?raw=true "")



