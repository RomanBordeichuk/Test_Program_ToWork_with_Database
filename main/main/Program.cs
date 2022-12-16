using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Runtime.InteropServices;

//-----------------------------------------------------

bool executing = true;
while (executing)
{
    Console.WriteLine("-------------------------------------------------");
    Console.WriteLine("Choose the operation:");
    string instructions = Console.ReadLine();

    switch (instructions)
    {
        case "add":
            addField();
            break;
        case "delete":
            deleteField();
            break;
        case "show":
            showDataBase();
            break;
        case "exit":
            executing = false;
            break;
        default:
            Console.WriteLine("Incorrect instruction");
            break;
    }
}

// addField()
// showDataBase();
// deleteField();

void addField()
{
    Console.Write("Enter type: ");
    string type = Console.ReadLine();

    Console.Write("Enter the cost: ");
    int cost = Convert.ToInt32(Console.ReadLine());

    Console.Write("Enter the number: ");
    int count = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("");

    Push(type, cost, count);
}

void deleteField()
{
    Console.WriteLine("Enter Id of the field: ");
    int id = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("");

    using (ApplicationContext marketDB = new ApplicationContext())
    {
        bool found = false;

        foreach (Product p in marketDB.Products.ToList())
        {
            if (p.id == id)
            {
                found = true;
                Console.WriteLine("Object has succesfully deleted");
                Console.WriteLine($"Type: {p.type}");
                Console.WriteLine($"Cost: {p.cost}");
                Console.WriteLine($"Count: {p.count}");
                
                marketDB.Remove(p);
                marketDB.SaveChanges();
                break;
            }
        }
        if(!found) Console.WriteLine("Object has not found");
    }
}

void showDataBase()
{
    Console.WriteLine("Executing...");
    using (ApplicationContext marketDB = new ApplicationContext())
    {
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine($"|id  |type          |cost        |count         |");

        foreach (Product product in marketDB.Products.ToList())
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"| {product.id}  | {product.type} | {product.cost} | {product.count} |");
        }
        Console.WriteLine("-------------------------------------------------");
    };
}

void Push(string type, int cost, int count)
{
    using (ApplicationContext marketDB = new ApplicationContext())
    {
        Product product = new Product(type, cost, count);
        marketDB.Add(product);
        marketDB.SaveChanges();

        Console.WriteLine("Objects succesfully pushed");    
        product.showProduct();
    };
}

public class Product
{
    public int id { set; get; }
    public string type { set; get; }
    public int cost { set; get; }
    public int count { set; get; }

    public Product()
    {
        this.id = 1;
        this.type = "book";
        this.cost = 100;
        this.count = 10;
    }
    public Product(string type, int cost, int count)
    {
        this.type = type;
        this.cost = cost;
        this.count = count;
    }
    public void showProduct()
    {
        Console.WriteLine($"Id: {this.id}\n" +
            $"Type: {this.type}\n" +
            $"Cost: {this.cost}\n" +
            $"Count: {this.count}\n");
    }
}

public class ApplicationContext : DbContext
{
    public DbSet<Product> Products { set; get; } = null!;

    public ApplicationContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=market",
            new MySqlServerVersion(new Version(8, 0, 31))); 
    }
}