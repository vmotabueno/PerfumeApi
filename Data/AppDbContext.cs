using Microsoft.EntityFrameworkCore;
using PerfumeApi.Models;

namespace PerfumeApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Perfume> Perfumes => Set<Perfume>();
}