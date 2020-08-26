namespace BingoX.AspNetCore
{
    public interface ICurrentUser
    {
        object UserID { get; }
        string Name { get; }
        string Role { get; }
        string Account { get; }
    }
     
}
