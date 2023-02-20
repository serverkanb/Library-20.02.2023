using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess.Contexts
{
    public class LibraryContextFactory : IDesignTimeDbContextFactory<LibraryContext>
    {
        public LibraryContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlServer("server=LAPTOP-1IHFPICF;database=Library-2;trusted_connection=true;multipleactiveresultsets=true;trustservercertificate=true;"
);
            

            return new LibraryContext(optionsBuilder.Options); 
        }
    }
    
    
}
