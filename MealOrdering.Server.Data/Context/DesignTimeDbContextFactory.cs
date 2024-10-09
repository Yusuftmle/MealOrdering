using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;



namespace MealOrdering.Server.Data.Context
{
    public class DesignTimeDbContextFactory: IDesignTimeDbContextFactory<MealOrderingDbContext>
    {
        public MealOrderingDbContext CreateDbContext(string[] args)
        {
            String connectionString = "Server=YUSUF\\YUSUF;Database=MealOrderingg;User Id=sa;Password=password1;TrustServerCertificate=True; Encrypt=false";

            var builder = new DbContextOptionsBuilder<MealOrderingDbContext>();

            builder.UseSqlServer(connectionString);

            return new MealOrderingDbContext(builder.Options);
        }
    }
}
