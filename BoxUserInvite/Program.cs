﻿using System;
using Box.V2;
using Box.V2.Config;
using Box.V2.JWTAuth;
using Box.V2.Models;
using Microsoft.Extensions.Configuration;

namespace BoxUserInvite
{
    class Program
    {
        

        static void Main(string[] args)
        {
            string UserEmail = "sampleemail@abc.edu";
            var appConfig = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            var configJson = appConfig["BoxConfigJson"];            
            Console.WriteLine($"Creating a Box Admin Client");
            var config = BoxConfig.CreateFromJsonString(configJson);
            var auth = new BoxJWTAuth(config);
            var adminToken = auth.AdminToken();
            var boxClient = auth.AdminClient(adminToken);
            Console.WriteLine($"Created a Box Admin Client");
            InviteUser(UserEmail, config, boxClient);
        }

        private static void InviteUser(string UserEmail, IBoxConfig config, BoxClient boxClient)
        {
            Console.WriteLine($"🔰 Initiate User invite...");
            var inviteEmail = new BoxActionableByRequest() { Login = UserEmail };
            var inviteId = new BoxRequestEntity() { Id = config.EnterpriseId, Type = new BoxType() };
            boxClient.UsersManager.InviteUserToEnterpriseAsync(new BoxUserInviteRequest() { Enterprise = inviteId, ActionableBy = inviteEmail });
            Console.WriteLine($"🏁 User invited to enterprise account");
        }
    }
}
