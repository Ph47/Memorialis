using Memorialis.Core.Identity;
using Memorialis.Core.Structures;
using Memorialis.Core.Sys.Settings;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Memorialis.Core.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Context context)
        {
            string url = "https://localhost:44300/";
            string echo = "/Echo";
            string clientId = "memorialis";
            string secret = "secret47";
            UriBuilder redirectBuilder = new UriBuilder(url);
            redirectBuilder.Path = echo;
//
//
//
//
//
//
//
//EE2AA081-760E-45D1-964F-73FA283A4A74
//E8035BC3-8628-4F23-AF5E-599FFF84E5E4
//4D83345B-AEDB-4E1F-8844-3E904EA4EE46


            InitSetting(context, new Guid("AA7FB51D-B02D-467C-9A33-A6447B79749C"), SettingGroup.Basic, true, "ProjectUrl", url);
            InitSetting(context, new Guid("FAA56156-C66D-4923-8D71-E4840AA6CC45"), SettingGroup.Basic, true, "EchoUrl", echo);
            InitSetting(context, new Guid("45995355-80A7-44DF-BF79-FEC6737C1C82"), SettingGroup.Basic, false, "RootPath", "/");
            InitSetting(context, new Guid("3E91DE3A-2A73-46C3-9FD9-D0CC02062284"), SettingGroup.Basic, true, "StaticFilesCacheTime", "86400");
            InitSetting(context, new Guid("4905F3B7-A1D2-4B79-A572-FBAE5C77B548"), SettingGroup.OAuth, true, "AuthorizeEndpointPath", "/Login");
            InitSetting(context, new Guid("9EF0B8B4-FE22-49A7-85C1-6F03DD0C6C61"), SettingGroup.OAuth, true, "TokenEndpointPath", "/Token");
            InitSetting(context, new Guid("6292EF3C-3183-4EF9-96F7-8C7C893A3BF0"), SettingGroup.OAuth, true, "ClientId", clientId);
            InitSetting(context, new Guid("5678B46D-386A-4227-9D8E-F7393FC7CC08"), SettingGroup.OAuth, false, "Secret", secret);


            context.Clients.AddOrUpdate<Client>(
                new Client()
                {
                    Id = Guid.Parse("1FB2E1D7-72C6-4817-BC92-6EE1A249F9A4"),
                    ClientId = clientId,
                    Secret = secret,
                    RedirectUrl = redirectBuilder.ToString()
                }
            );



            Guid adminId = Guid.Parse("F2D1A580-39FC-4524-B1F3-504B84D1CB1D");
            context.Users.AddOrUpdate<User>(
                new User()
                {
                    Id = adminId,
                    UserName = "Admin",
                    PasswordHash = "AJ9KH/q5wvKxAFKTJsx3h1dYhRxWh7CNnfv8NP9JvedBVj39a8qFD9zSVF1JnAZTEQ==",
                    Email = "ph@effetto.ru",
                    EmailConfirmed = true,
                    Phone = "+79082063823",
                    PhoneConfirmed = true,
                    FullName = new FullName()
                    {
                        Name = "Администратор",
                        Nick = "Admin",
                        Surename = "Системный"
                    }
                }
            );
            

            Guid adminRoleId = Guid.Parse("520A7664-1F78-4299-8055-2A9BCC74F420");
            context.Roles.AddOrUpdate<Role>(
                new Role()
                {
                    Id = adminRoleId,
                    Name = "Administrators",
                    Weight = 10
                }
            );
            
            context.SaveChanges();

            using (Context db = new Context())
            {
                User admin = db.Users.Single(u => u.Id == adminId);
                Role adminRole = db.Roles.Single(r => r.Id == adminRoleId);
                if (!admin.Roles.Contains(adminRole))
                {
                    admin.Roles.Add(adminRole);
                }
                db.SaveChanges();
            }
        }

        private void InitSetting(Context ctx, Guid id, SettingGroup group, bool isPublic, string name, string value)
        {
            if (ctx.Settings.SingleOrDefault(s => s.Id == id) == null)
            {
                ctx.Settings.Add(
                    new Setting()
                    {
                        Id = id,
                        Group = group,
                        Name = name,
                        Value = value,
                        IsPublic = isPublic
                    });
            }
        }
    }
}
