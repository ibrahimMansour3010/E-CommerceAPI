using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            IConfigurationRoot root = 
            new ConfigurationBuilder().SetBasePath
                (Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json")
                .Build();
            DbContextOptionsBuilder<Context> builder = new DbContextOptionsBuilder<Context>();
            builder.UseSqlServer(root.GetConnectionString("ECommerceSTR"));
            Context context = new Context(builder.Options);
            return context;
        }
    }
}
