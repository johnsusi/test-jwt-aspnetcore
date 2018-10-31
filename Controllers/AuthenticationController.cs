using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Controllers
{

  [Route("/authentication")]
  [AllowAnonymous]
  public class AuthenticationController : Controller
  {


    [HttpGet("login")]
    public IActionResult Login(string username, string password)
    {
      if (username == "demo" && password == "demo")
      {
        var machineList = new string[1000];
        for (int i = 0;i < machineList.Length;++i)
          machineList[i] = "890000" + (i+1).ToString("d4");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Hello, World, asdflakfjdlkasjlkfjalskfjlaksjflkajslkfjlaksjflkajslfkjalsjflkajlkjflkdasjlfkjaslkjflaksjldfkjalsfdkjlaskjflkajslfkjlkajf"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokens = machineList.ToDictionary(m => m, m =>
          new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
//            issuer: "localhost",
//            audience: "localhost",
            claims:  new[]
            {
              new Claim("ma.ne", m)
            },
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds)
        ));

        return Ok(new
        {
          tokens
        });
      }
      return BadRequest("Not authorized");
    }

    public IActionResult Refresh()
    {

      return Ok();
    }

  }

}