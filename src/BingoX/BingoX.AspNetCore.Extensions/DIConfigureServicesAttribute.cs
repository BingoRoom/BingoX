namespace BingoX.AspNetCore.Extensions
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class DIConfigureServicesAttribute : System.Attribute
    {
        public DIConfigureServicesAttribute(int order)
        {
            Order = order;

        }
        public int Order { get; private set; }
    }
}
