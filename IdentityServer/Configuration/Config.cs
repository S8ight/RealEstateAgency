using System.Security.Claims;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using IdentityServer4.Test;
using ApiResource = IdentityServer4.Models.ApiResource;
using Client = IdentityServer4.Models.Client;
using IdentityResource = IdentityServer4.Models.IdentityResource;
using Secret = IdentityServer4.Models.Secret;

namespace IdentityServer.Configuration;

public class Config
{
    public static IEnumerable<ApiResource> GetApiResources()
    {
        return new List<ApiResource>
        {
            new("resourceName", "resourceName")
            {
                Scopes = {new Scope("apiScope")}
            }
        };
    }

    public static IEnumerable<Client> GetClients()
    {
        return new[]
        {
            // for public api
            new Client
            {
                ClientId = "secret_userClient_id",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "ReadData" }
            },
            new Client
            {
                ClientId = "secret_adminClient_id",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "UpdateData" }
            }
        };
    }
    
    public static IEnumerable<IdentityResource> GetIdentityResources =>
        new [] 
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = "role",
                UserClaims = new List<string> {"role"}
            }
        };

}