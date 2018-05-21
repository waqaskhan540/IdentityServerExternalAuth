> Project Available as Nuget Package , [click here](https://github.com/waqaskhan540/identityserver-token-exchange)
# Exchanging external Tokens (Google, Twitter, Facebook,LinkedIn) with IdentityServer access tokens using an extension grant

## Supported providers

 - [x] Facebook
 - [x] LinkedIn
 - [x] Twitter
 - [x] Google
 - [ ] GitHub 
 
## How to exchange external tokens for IdentityServer access token ?
* Request authentication using the provider's native library.
* Exchange external token with IdentityServer token by making following request to IdentityServer.

```
POST connect/token
     
     client_id = [your_client_id]
     client_secret = [your_client_secret]
     scopes = [your_scopes]
     grant_type = external
     provider = facebook 
     external_token  = [facebook_access_token]
```
 * If user is already registered then IdentityServer will return the access token, otherwise it will send the user's data and prompt for an email parameter to be added, in this case make another request with an extra ```email``` parameter.
 
 ```
POST connect/token
     
     client_id = [your_client_id]
     client_secret = [your_client_secret]
     scopes = [your_scopes]
     grant_type = external
     provider = facebook 
     email = myemail@abc.com
     external_token  = [facebook_access_token]
```

You can change ```provider``` to ```Facebook``` , ```Google``` , ```Twitter``` and ```LinkedIn``` and provide respective token in the ```external_token``` parameter.

## How to setup an external provider

1. ##### Derive an interface from ```IExternalAuthProvider```

```csharp

public interface IMyCustomProvider : IExternalAuthProvider {
    Provider provider {get;}
}
```
2. ##### Add your provider to ```ProviderType``` enum

```csharp
public enum ProviderType {
  
  Facebook,
  Twitter,
  Google,
  MyCustomProvider
}
```
3. ##### Add provider info to ```ProviderDataSource```

```csharp

 public class ProviderDataSource
    {
        public static IEnumerable<Provider> GetProviders()
        {
            return new List<Provider>
            {
                new Provider
                {
                    ProviderId = 1,
                    Name = "Facebook",
                    UserInfoEndPoint = "https://graph.facebook.com/v2.8/me"
                },
                new Provider
                {
                    ProviderId = 2,
                    Name = "Google",
                    UserInfoEndPoint = "https://www.googleapis.com/oauth2/v2/userinfo"
                },
                 new Provider
                {
                    ProviderId = 3,
                    Name = "Twitter",
                    UserInfoEndPoint = "https://api.twitter.com/1.1/account/verify_credentials.json"
                },
                new Provider 
                {
                    ProviderId = 4,
                    Name="MyCustomProvider",
                    UserInfoEndPoint = "[url to end point which validates the token and returns user data]"
                }
            };
        }
    }

```

4. ##### Provide an implementation for ```IMyCustomProvider```

```csharp
public class MyCustomProvider : IMyCustomProvider {

private readonly HttpClient _httpClient;
public MyCustomProvider(HttpClient httpClient) {
  _httpClient = httpClient;
}

public Provider =>_providerRepository.Get()
                                    .FirstOrDefault(x => x.Name.ToLower() == ProviderType.MyCustomProvider.ToString().ToLower());
                                    
public JObject GetUserInfo(string accessToken) {

 var query = "[build your request according to your providers configuration]";
 
 var result = _httpClient.GetAsync(Provider.UserInfoEndPoint + query).Result;
            if (result.IsSuccessStatusCode)
            {
                var infoObject = JObject.Parse(result.Content.ReadAsStringAsync().Result);
                return infoObject;
            }
            return null;

}
}
```
5. ##### Bind ```IMyCustomProvider``` in ```ServiceCollectionExtensions```

```csharp
 public static IServiceCollection AddProviders(this IServiceCollection services)
        {
            services.AddTransient<IFacebookAuthProvider, FacebookAuthProvider>();
            services.AddTransient<ITwitterAuthProvider, TwitterAuthProvider>();
            services.AddTransient<IGoogleAuthProvider, GoogleAuthProvider>();
            services.AddTransient<IMyCustomProvider,MyCustomProvider>();
            return services;
        }
```

6. ##### Add ```MyCustomProvider``` to ```ExternalAuthenticationGrant```
```csharp
  providers = new Dictionary<ProviderType, IExternalAuthProvider>();
            providers.Add(ProviderType.Facebook, _facebookAuthProvider);
            providers.Add(ProviderType.Google, _googleAuthProvider);
            providers.Add(ProviderType.Twitter, _twitterAuthProvider);
            providers.Add(ProviderType.LinkedIn, _linkedAuthProvider);
            providers.Add(ProviderType.MyCustomProvider, _myCustomProvider);
```
7. ##### Make a request to IdentityServer using new provider

```
POST connect/token
     
     client_id = [your_client_id]
     client_secret = [your_client_secret]
     scopes = [your_scopes]
     grant_type = external
     provider = mycustomprovider 
     external_token  = [access_token_from_custom_provider]
```
