using CityInfo.API.Entities;

namespace CityInfo.API.DbContext;

using Microsoft.EntityFrameworkCore;

public class CityInfoContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<PointOfInterest> PointsOfInterest { get; set; }

    public CityInfoContext(DbContextOptions<CityInfoContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .HasData(
                new City("New York City")
                {
                    Id = 1,
                    Description = "The one with that big park."
                },
                new City("Antwerp")
                {
                    Id = 2,
                    Description = "The one with the cathedral that was never really finished."
                },
                new City("Paris")
                {
                    Id = 3,
                    Description = "The one with that big tower."
                },
                new City("London")
                {
                    Id = 4,
                    Description = "The capital of England and the United Kingdom."
                },
                new City("Tokyo")
                {
                    Id = 5,
                    Description = "The capital of Japan."
                },
                new City("Dubai")
                {
                    Id = 6,
                    Description = "A city in the United Arab Emirates."
                }

                
                
                );

        modelBuilder.Entity<PointOfInterest>()
            .HasData(
                new PointOfInterest("Central Park")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "The most visited urban park in the United States."
                },
                new PointOfInterest("Empire State Building")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "A 102-story skyscraper located in Midtown Manhattan."
                },
                new PointOfInterest("Cathedral")
                {
                    Id = 3,
                    CityId = 2,
                    Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
                },
                new PointOfInterest("Antwerp Central Station")
                {
                    Id = 4,
                    CityId = 2,
                    Description = "The the finest example of railway architecture in Belgium."
                },
                new PointOfInterest("Eiffel Tower")
                {
                    Id = 5,
                    CityId = 3,
                    Description =
                        "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel."
                },
                new PointOfInterest("The Louvre")
                {
                    Id = 6,
                    CityId = 3,
                    Description = "The world's largest museum."
                }
            );
        base.OnModelCreating(modelBuilder);
    }
}

/*
 *
 * SAMPLE DATA
 *("London", "The capital of England and the United Kingdom.", new[]
       {
           ("Big Ben", "The nickname for the Great Bell of the clock at the Palace of Westminster."),
           ("London Eye", "A giant Ferris wheel on the South Bank of the River Thames.")
       }),
   ("Tokyo", "The capital of Japan.", new[]
       {
           ("Tokyo Tower", "A communications and observation tower in the Shiba-koen district of Minato."),
           ("Shibuya Crossing", "The busiest pedestrian crossing in the world.")
       }),
   ("Dubai", "A city in the United Arab Emirates.", new[]
       {
           ("Burj Khalifa", "The tallest structure and building in the world since its topping out in 2009."),
           ("Palm Jumeirah", "A man-made archipelago in Dubai.")
       }),
        ("Rome", "The capital city of Italy.", new[]
                  {
                      ("Colosseum", "An ancient amphitheater in the center of the city."),
                      ("Vatican Museums", "Art and Christian museums in Vatican City.")
                  }),
              ("Sydney", "The largest city in Australia.", new[]
                  {
                      ("Sydney Opera House", "A multi-venue performing arts centre."),
                      ("Sydney Harbour Bridge", "A heritage-listed steel through arch bridge across Sydney Harbour.")
                  }),
              ("Beijing", "The capital of the People's Republic of China.", new[]
                  {
                      ("Great Wall of China", "A series of fortifications made of stone, brick, tamped earth, wood, and other materials."),
                      ("Forbidden City", "A palace complex in central Beijing.")
                  }),
              ("Moscow", "The capital and largest city of Russia.", new[]
                  {
                      ("Red Square", "A city square in Moscow."),
                      ("Saint Basil's Cathedral", "A church in Red Square in Moscow.")
                  }),
              ("Berlin", "The capital and largest city of Germany.", new[]
                  {
                      ("Brandenburg Gate", "An 18th-century neoclassical monument."),
                      ("Berlin Wall", "A guarded concrete barrier that physically and ideologically divided Berlin.")
                  }),
              ("Madrid", "The capital of Spain.", new[]
                  {
                      ("Prado Museum", "The main Spanish national art museum."),
                      ("Royal Palace of Madrid", "The official residence of the Spanish royal family.")
                  }),
              ("Buenos Aires", "The capital and largest city of Argentina.", new[]
                  {
                      ("Teatro Colón", "An opera house in Buenos Aires."),
                      ("Obelisco de Buenos Aires", "A national historic monument and icon of Buenos Aires.")
                  }),
              ("Cape Town", "A port city on South Africa’s southwest coast.", new[]
                  {
                      ("Table Mountain", "A flat-topped mountain forming a prominent landmark."),
                      ("Robben Island", "An island in Table Bay.")
                  }),
              ("Istanbul", "A major city in Turkey.", new[]
                  {
                      ("Hagia Sophia", "A former Greek Orthodox Christian patriarchal cathedral."),
                      ("Blue Mosque", "A historic mosque in Istanbul.")
                  }),
              ("Bangkok", "The capital of Thailand.", new[]
                  {
                      ("Grand Palace", "A complex of buildings at the heart of Bangkok."),
                      ("Wat Arun", "A Buddhist temple in Bangkok.")
                  }),
              ("Rio de Janeiro", "A huge seaside city in Brazil.", new[]
                  {
                      ("Christ the Redeemer", "An Art Deco statue of Jesus Christ."),
                      ("Sugarloaf Mountain", "A peak situated in Rio de Janeiro.")
                  }),
              ("Mexico City", "The capital of Mexico.", new[]
                  {
                      ("Zócalo", "The common name of the main square in central Mexico City."),
                      ("Chapultepec", "One of the largest city parks in the Western Hemisphere.")
                  }),
              ("Seoul", "The capital of South Korea.", new[]
                  {
                      ("Gyeongbokgung Palace", "The main royal palace of the Joseon dynasty."),
                      ("N Seoul Tower", "A communication and observation tower.")
                  }),
              ("Mumbai", "The capital city of the Indian state of Maharashtra.", new[]
                  {
                      ("Gateway of India", "An arch monument built during the 20th century."),
                      ("Chhatrapati Shivaji Maharaj Terminus", "A historic railway station.")
                  }),
              ("Singapore", "A global financial hub in Southeast Asia.", new[]
                  {
                      ("Marina Bay Sands", "An integrated resort fronting Marina Bay."),
                      ("Gardens by the Bay", "A nature park spanning 101 hectares.")
                  }),
 * 
 */


// SNIPPET TO CREATE 100 CITIES AND RANDOM POINTS OF INTEREST
// modelBuilder.Entity<City>()
//     .HasData(
//         Enumerable.Range(1, 100).Select(id => new City($"City {id}")
//         {
//             Id = id,
//             Description = $"Description for City {id}"
//         }).ToArray()
//     );
//
// modelBuilder.Entity<PointOfInterest>()
//     .HasData(
//         Enumerable.Range(1, 500).Select(id => new PointOfInterest($"Point Of Interest {id}")
//         {
//             Id = id,
//             CityId = (id - 1) / 5 + 1, // Distribute points of interest over cities
//             Description = $"Description for Point Of Interest {id}"
//         }).ToArray()
//     );