using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class CarContext : DbContext
{
    public CarContext(DbContextOptions<CarContext> options) : base(options) { }

    public DbSet<Car> Cars { get; set; }
}
