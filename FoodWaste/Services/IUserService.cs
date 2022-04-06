using System.Security.Claims;

namespace FoodWaste.Services
{
    public interface IUserService
    {
        int GetCurrentUserId(ClaimsPrincipal user);
    }
}
