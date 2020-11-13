namespace BingoX.AspNetCore
{
    public interface ICurrentUser
    {
        object NameIdentifier { get; }
        string Name { get; }
        string Role { get; }

        System.Security.Claims.Claim[] Claims { get; }
    }

   


}
