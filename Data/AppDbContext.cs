using Authentication_Client.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Authentication_Client.Data;

public class AppDbContext: DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Chambre> chambres { get; set; }
    
    public DbSet<Facture> factures { get; set; }
    public DbSet<Reservation> reservations { get; set; }
    public DbSet<Service> services { get; set; }
    

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    // instead of having ENUM colomns as number we will change as strings value (ADMIN...)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>()
            .Property(c => c.role)
            .HasConversion(new EnumToStringConverter<Role>());

        // base.OnModelCreating(modelBuilder);
        
        base.OnModelCreating(modelBuilder);

        // Optionally seed initial data (for migrations)
        modelBuilder.Entity<Chambre>().HasData(
            new Chambre { Id = 1, Numero = 101, Type = ChambreType.VIP, PrixParNuit = 200, Disponibilite = true },
            new Chambre { Id = 2, Numero = 102, Type = ChambreType.NORMAL, PrixParNuit = 100, Disponibilite = true },
            new Chambre { Id = 3, Numero = 103, Type = ChambreType.TRIPLET, PrixParNuit = 150, Disponibilite = true },
            new Chambre { Id = 4, Numero = 104, Type = ChambreType.NORMAL, PrixParNuit = 110, Disponibilite = true },
            new Chambre { Id = 5, Numero = 105, Type = ChambreType.VIP, PrixParNuit = 250, Disponibilite = true },
            new Chambre { Id = 6, Numero = 106, Type = ChambreType.TRIPLET, PrixParNuit = 140, Disponibilite = true },
            new Chambre { Id = 7, Numero = 107, Type = ChambreType.NORMAL, PrixParNuit = 120, Disponibilite = true },
            new Chambre { Id = 8, Numero = 108, Type = ChambreType.VIP, PrixParNuit = 230, Disponibilite = true },
            new Chambre { Id = 9, Numero = 109, Type = ChambreType.TRIPLET, PrixParNuit = 160, Disponibilite = true },
            new Chambre { Id = 10, Numero = 110, Type = ChambreType.NORMAL, PrixParNuit = 105, Disponibilite = true }
        );
    }

}