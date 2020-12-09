using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

//Solved with Christoph Maureder

var factory = new HotelContextFactory();
using var dbContext = factory.CreateDbContext();

if (args[0] == "add")
{
    await AddData();
}
else if (args[0] == "query")
{
    await QueryData();
}

async Task AddData()
{
    HotelSpecial dogFriendly, organicFood, spa, sauna, indoorPool, outdoorPool;
    await dbContext.HotelSpecials.AddRangeAsync(new[]
    {
        dogFriendly = new HotelSpecial() 
        {
            Specials = Specials.DogFriendly,
        },
        organicFood = new HotelSpecial() 
        {
            Specials = Specials.OrganicFood,
        },
        spa = new HotelSpecial() 
        {
            Specials = Specials.Spa,
        },
        sauna = new HotelSpecial() 
        {
            Specials = Specials.Sauna,
        },
        indoorPool = new HotelSpecial()
        {
            Specials = Specials.IndoorPool,
        },
        outdoorPool = new HotelSpecial()
        {
            Specials = Specials.OutdoorPool,
        }
    });
    await dbContext.SaveChangesAsync();

    RoomType singleRoom3x10, doubleRoom10x15, singleRoom10x15, doubleRoom25x30, juniorSuites5x45, honeymoonSuite1x100;
    await dbContext.RoomTypes.AddRangeAsync(new[]
    {
        singleRoom3x10 = new RoomType()
        {
            Title = "Single Room 10qm",
            NummberOfAvailableRooms = 3,
            Size = 10,
            IsDisabilityAccessible = false,
        },
        doubleRoom10x15 = new RoomType()
        {
            Title = "Double Room 15qm",
            NummberOfAvailableRooms = 10,
            Size = 15,
            IsDisabilityAccessible = false,
        },singleRoom10x15 = new RoomType()
        {
            Title = "Single Room 15qm",
            NummberOfAvailableRooms = 10,
            Size = 15,
            IsDisabilityAccessible = true,
        },doubleRoom25x30 = new RoomType()
        {
            Title = "Double Room 30qm",
            NummberOfAvailableRooms = 25,
            Size = 30,
            IsDisabilityAccessible = true,
        },juniorSuites5x45 = new RoomType()
        {
            Title = "Junior Suite 45qm",
            NummberOfAvailableRooms = 5,
            Size = 45,
            IsDisabilityAccessible = true,
        },honeymoonSuite1x100 = new RoomType()
        {
            Title = "Honeymoon Suite 100qm",
            NummberOfAvailableRooms = 1,
            Size = 100,
            IsDisabilityAccessible = true,
        },
    });
    await dbContext.SaveChangesAsync();


    Price p40, p60, p70, p120, p190, p300;
    await dbContext.Prices.AddRangeAsync(new[]
    {
        p40 = new Price()
        {
            PriceEUR = 40,
            RoomType = singleRoom3x10,
        },
        p60 = new Price()
        {
            PriceEUR = 60,
            RoomType = doubleRoom10x15,
        },p70 = new Price()
        {
            PriceEUR = 70,
            RoomType = singleRoom10x15,
        },p120 = new Price()
        {
            PriceEUR = 120,
            RoomType = doubleRoom25x30,
        },p190 = new Price()
        {
            PriceEUR = 190,
            RoomType = juniorSuites5x45,
        },p300 = new Price()
        {
            PriceEUR = 300,
            RoomType = honeymoonSuite1x100,
        },
    });
    await dbContext.SaveChangesAsync();

    Hotel PensionMarianne, GrandHotelGoldenerHirsch;
    await dbContext.Hotels.AddRangeAsync(new[]
    {
        PensionMarianne = new Hotel()
        {
            Name = "Pension Marianne",
            Address = "Am Hausberg 17, 1234 Irgendwo",
            RoomTypes = {singleRoom3x10, doubleRoom10x15},
            Specials = { dogFriendly, organicFood },
        },
        GrandHotelGoldenerHirsch = new Hotel()
        {
            Name = "Grand Hotel Goldener Hirsch",
            Address = "Im stillen Tal 42, 4711 Schönberg",
            RoomTypes = {singleRoom10x15, doubleRoom25x30, juniorSuites5x45, honeymoonSuite1x100},
            Specials = { spa, sauna, indoorPool, outdoorPool },
        }
    });
    await dbContext.SaveChangesAsync();
}
async Task QueryData()
{

}


#region Model
enum Specials
{
    Spa,
    Sauna,
    DogFriendly,
    IndoorPool,
    OutdoorPool,
    BikeRental,
    ECarChargingStation,
    VegetarianCuisine,
    OrganicFood
}
class Hotel
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = "";
    [MaxLength(100)]
    public string Address { get; set; } = "";
    public List<HotelSpecial> Specials { get; set; } = new();
    public List<RoomType> RoomTypes { get; set; } = new();
}
class HotelSpecial
{
    public int Id { get; set; }
    public Specials Specials { get; set; }
    public Hotel Hotel { get; set; }
}
class RoomType
{
    public int Id { get; set; }
    [MaxLength(75)]
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; }
    public int Size { get; set; }
    public bool IsDisabilityAccessible { get; set; }
    public int NummberOfAvailableRooms { get; set; }
    public Hotel Hotel { get; set; }
    public int HotelId { get; set; }
    public Price Price { get; set; }
}
class Price
{
    public int Id { get; set; }
    public RoomType RoomType { get; set; }
    public int RoomTypeId { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    [Column(TypeName = "decimal(8, 2)")]
    public decimal PriceEUR { get; set; }
}
#endregion

#region Context
class HotelContext : DbContext
{
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<HotelSpecial> HotelSpecials { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Price> Prices { get; set; }
#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
    public HotelContext(DbContextOptions<HotelContext> options) : base(options) { }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
}
class HotelContextFactory : IDesignTimeDbContextFactory<HotelContext>
{
    public HotelContext CreateDbContext(string[]? args = null)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var optionsBuilder = new DbContextOptionsBuilder<HotelContext>();
        optionsBuilder
            // Uncomment the following line if you want to print generated
            // SQL statements on the console.
            //.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

        return new HotelContext(optionsBuilder.Options);
    }
}
#endregion