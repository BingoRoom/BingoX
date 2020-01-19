

namespace BingoX.EF
{

    public class DbEntityInterceptOptions
    {
        public DbEntityInterceptOptions()
        {
            Intercepts = new InterceptCollection();
           ;
        }
        public InterceptCollection Intercepts { get; private set; }
      
    }

}
