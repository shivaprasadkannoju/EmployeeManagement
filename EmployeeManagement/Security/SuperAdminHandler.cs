using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Security
{
	public class SuperAdminHandler : AuthorizationHandler<ManageAdminsRolesAndClaimsRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
			ManageAdminsRolesAndClaimsRequirement requirement)
		{
			if( context.User.IsInRole("Super Admin"))
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}
