using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Models;
using System.Linq;

namespace test_jwt
{
  public class MachineAuthorizationHandler : AuthorizationHandler<AccessToMachineRequirement, Machine>
  {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessToMachineRequirement requirement, Machine resource)
    {
      if (!context.User.HasClaim(c => c.Type == "machine-list"))
      {
        return Task.CompletedTask;
      }

      var data = context.User.FindFirst(c => c.Type == "machine-list").Value;
      var list = JArray.Parse(data);
      if (list.FirstOrDefault(o => o.ToObject<int>() == resource.Id) != null)
      {
        context.Succeed(requirement);
      }

      return Task.CompletedTask;
    }
  }
}