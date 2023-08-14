using Microsoft.EntityFrameworkCore;

namespace BetwayAuthentication.DAL.Entities
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions options) : base(options)
		{
		}

		public virtual DbSet<User> Users { get; set; }
	}
}
