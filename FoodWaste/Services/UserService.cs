using System.Security.Claims;

namespace FoodWaste.Services
{
    public class UserService : IUserService
    {
        public int GetCurrentUserId(ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            return new int();
        }

    }
}
