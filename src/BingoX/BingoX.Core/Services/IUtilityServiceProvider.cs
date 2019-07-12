namespace BingoX.Services
{
    public interface IUtilityServiceProvider
    {

        IDateTimeService DateTimeService { get; }
    }

    public class UtilityServiceProvider : IUtilityServiceProvider
    {
        public UtilityServiceProvider() : this(DateTimeService.Default)
        {



        }
        public UtilityServiceProvider(DateTimeService dateTimeService)
        {

            DateTimeService = dateTimeService;

        }


        public DateTimeService DateTimeService { get; private set; }



        IDateTimeService IUtilityServiceProvider.DateTimeService { get { return DateTimeService; } }
    }
}
