using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace QuickAdmin.DAO
{
	public class QuickAdminDbContextDesignFactory : IDesignTimeDbContextFactory<QuickAdminDbContext>
	{
		public QuickAdminDbContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<QuickAdminDbContext>();
			optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=QuickAdmin;Persist Security Info=True;User ID=sa;Password=abc123!@#");

			return new QuickAdminDbContext(optionsBuilder.Options);
		}
	}

}
