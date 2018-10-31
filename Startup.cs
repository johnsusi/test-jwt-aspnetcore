using System;
using System.Text;
using Data;
using Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace test_jwt
{

  public class AccessToMachineRequirement : IAuthorizationRequirement {}

  public class Startup
  {

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(o =>
        {

          var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Hello, World, asdflakfjdlkasjlkfjalskfjlaksjflkajslkfjlaksjflkajslfkjalsjflkajlkjflkdasjlfkjaslkjflaksjldfkjalsfdkjlaskjflkajslfkjlkajf"));

          var tokenValidationParameters = new TokenValidationParameters
          {
            ValidateAudience = false,
            ValidateIssuer = false,
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
          };
          o.TokenValidationParameters = tokenValidationParameters;
          o.SaveToken = true;

        });

      services.AddAuthorization(o =>
      {
        o.AddPolicy("HasAccessToMachine", policy =>
        {
          policy.Requirements.Add(new AccessToMachineRequirement());
        });
      });
      services.AddSingleton<IAuthorizationHandler, MachineAuthorizationHandler>();
      services.AddTransient<IMachineRepository, MachineRepository>();

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseAuthentication();
      app.UseMvc();
    }
  }
}
