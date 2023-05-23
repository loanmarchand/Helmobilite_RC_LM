using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelmoBilite_RC_LM.Models
{
    public class HelmoBiliteRcLmContext : IdentityDbContext<Utilisateur>
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Camionneur> Camionneurs { get; set; }
        public DbSet<Dispatcher> Dispatchers { get; set; }
        public DbSet<Adresse> Adresses { get; set; }
        public DbSet<Livraison> Livraisons { get; set; }
        public DbSet<OrdreLivraison> OrdreLivraisons { get; set; }
        public DbSet<Camion> Camions { get; set; }

        public HelmoBiliteRcLmContext(DbContextOptions<HelmoBiliteRcLmContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Livraison>()
                .HasOne(d => d.LieuChargement)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Livraison>()
                .HasOne(d => d.LieuDechargement)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

        }


        public static class DataInitalizer
        {
            public static void SeedRole(RoleManager<IdentityRole> roleManager)
            {
                if (!roleManager.RoleExistsAsync("Client").Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = "Client";
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
                if (!roleManager.RoleExistsAsync("Camionneur").Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = "Camionneur";
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
                if (!roleManager.RoleExistsAsync("Dispatcher").Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = "Dispatcher";
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
                if (!roleManager.RoleExistsAsync("UtilisateurHelmo").Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = "UtilisateurHelmo";
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }

            /*public static async Task Seed(UserManager<Utilisateur> userManager)
            {
                if (userManager.Users.Count() != 0)
                {
                    return;
                }
                // Add Client
                var firstName = new Person().FirstName;
                var lastName = new Person().LastName;
                var adresse = new Adresse
                {
                    Numero = new Randomizer().Int(1, 1000),
                    Rue = new Person().Address.Street,
                    CodePostal = new Randomizer().Int(1000, 9999),
                    Ville = new Person().Address.City,
                    Pays = new Person().Address.State
                };
                var client = new Client
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = $"{firstName}.{lastName}@clientHelmo.be",
                    CompagnyAdress = adresse,
                    CompanyName = "Helmo",

                };
                var result = userManager.CreateAsync(client, "Helmo123!").Result;
                if (result.Succeeded)
                {
                   var result2= userManager.AddToRoleAsync(client, "Client").Result;
                }

                // Add Camionneur
                firstName = new Person().FirstName;
                lastName = new Person().LastName;
                var permis1 = new Permis
                {
                    Type = "C"
                };
                var permis2 = new Permis
                {
                    Type = "CE"
                };
                ICollection<Permis> permis = new List<Permis>();
                permis.Add(permis1);
                permis.Add(permis2);
                var camionneur = new Camionneur
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = $"{firstName}.{lastName}@camionneurHelmo.be",
                    Permis = permis,
                    Matricule = new Randomizer().Replace("??-???-??"),
                };
                result = userManager.CreateAsync(camionneur, "Helmo123!").Result;
                if (result.Succeeded)
                {
                    var result2 = userManager.AddToRoleAsync(camionneur, "Camionneur").Result;
                }

                    // Add Dispatcher
                firstName = new Person().FirstName;
                lastName = new Person().LastName;
                adresse = new Adresse
                {
                    Numero = new Randomizer().Int(1, 1000),
                    Rue = new Person().Address.Street,
                    CodePostal = new Randomizer().Int(1000, 9999),
                    Ville = new Person().Address.City,
                    Pays = new Person().Address.State
                };
                var dispatcher = new Dispatcher
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = $"{firstName}.{lastName}@dispatcherHelmo.be",
                    Matricule = new Randomizer().Replace("??-???-??"),
                    NiveauEtude = "Bachelier",
                };
                result = userManager.CreateAsync(dispatcher, "Helmo123!").Result;
                if (result.Succeeded)
                {
                    var result2 = userManager.AddToRoleAsync(dispatcher, "Dispatcher").Result;
                }

                // Add Admin
                firstName = new Person().FirstName;
                lastName = new Person().LastName;
                var admin = new Utilisateur
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = $"{firstName}.{lastName}@adminHelmo.be"
                };
                result = userManager.CreateAsync(admin, "Helmo123!").Result;
                if (result.Succeeded)
                {
                    var result2 = userManager.AddToRoleAsync(admin, "Admin").Result;
                }

            }*/
        }
    }

    
}