using Microsoft.EntityFrameworkCore;
using SampleWebService.Data;
using SampleWebService.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL baðlantýsý
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.MapGet("/", () => "CRUD API Çalýþýyor");

// ------------------- PAGE CRUD -------------------
app.MapPost("/pages", async (Page page, AppDbContext db) =>
{
    if (page.JsonData != null)
        page.Data = JsonSerializer.Serialize(page.JsonData);

    db.Pages.Add(page);
    await db.SaveChangesAsync();
    return Results.Created($"/pages/{page.Id}", page);
});

app.MapPut("/pages/{id}", async (int id, Page input, AppDbContext db) =>
{
    var page = await db.Pages.FindAsync(id);
    if (page is null) return Results.NotFound();

    page.PageName = input.PageName;

    if (input.JsonData != null)
        page.Data = JsonSerializer.Serialize(input.JsonData);

    await db.SaveChangesAsync();
    return Results.Ok(page);
});

app.MapGet("/pages", async (AppDbContext db) =>
{
    var pages = await db.Pages.OrderByDescending(d => d.Id).ToListAsync();
    return pages.Select(p => new
    {
        p.Id,
        p.PageName,
        data = p.JsonData // JSON olarak dön
    });
});

app.MapGet("/pages/{id}", async (int id, AppDbContext db) =>
{
    var page = await db.Pages.FindAsync(id);
    if (page is null) return Results.NotFound();
    return Results.Ok(new
    {
        page.Id,
        page.PageName,
        data = page.JsonData
    });
});

// ------------------- PRODUCTS -------------------
app.MapGet("/products", async (AppDbContext db) =>
{
    var products = await db.Products
        .Include(p => p.ProductBrand)
        .Include(p => p.ProductGroup)
        .OrderByDescending(d => d.Id)
        .Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Image = p.Image,
            Price = p.Price,
            Description = p.Description,
            ProductBrandName = p.ProductBrand.Name,
            ProductGroupName = p.ProductGroup.Name
        }).ToListAsync();

    return Results.Ok(products);
});

app.MapGet("/products/{id}", async (int id, AppDbContext db) =>
{
    var p = await db.Products
        .Include(p => p.ProductBrand)
        .Include(p => p.ProductGroup)
        .Where(p => p.Id == id)
        .Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Image = p.Image,
            Price = p.Price,
            Description = p.Description,
            ProductBrandName = p.ProductBrand.Name,
            ProductGroupName = p.ProductGroup.Name
        }).FirstOrDefaultAsync();

    return p is null ? Results.NotFound() : Results.Ok(p);
});

app.MapPost("/products", async (Product product, AppDbContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
});

app.MapPut("/products/{id}", async (int id, Product input, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();

    product.Name = input.Name;
    product.Image = input.Image;
    product.Price = input.Price;
    product.Description = input.Description;
    product.ProductBrandId = input.ProductBrandId;
    product.ProductGroupId = input.ProductGroupId;

    await db.SaveChangesAsync();
    return Results.Ok(product);
});

app.MapDelete("/products/{id}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();

    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ------------------- PRODUCTGROUP CRUD -------------------
app.MapGet("/productgroups", async (AppDbContext db) => await db.ProductGroups.OrderByDescending(d => d.Id).ToListAsync());
app.MapGet("/productgroups/{id}", async (int id, AppDbContext db) => await db.ProductGroups.FindAsync(id));
app.MapPost("/productgroups", async (ProductGroup group, AppDbContext db) =>
{
    db.ProductGroups.Add(group);
    await db.SaveChangesAsync();
    return Results.Created($"/productgroups/{group.Id}", group);
});
app.MapPut("/productgroups/{id}", async (int id, ProductGroup input, AppDbContext db) =>
{
    var group = await db.ProductGroups.FindAsync(id);
    if (group is null) return Results.NotFound();

    group.Name = input.Name;
    await db.SaveChangesAsync();
    return Results.Ok(group);
});
app.MapDelete("/productgroups/{id}", async (int id, AppDbContext db) =>
{
    var group = await db.ProductGroups.FindAsync(id);
    if (group is null) return Results.NotFound();

    db.ProductGroups.Remove(group);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ------------------- PRODUCTBRAND CRUD -------------------
app.MapGet("/productbrands", async (AppDbContext db) => await db.ProductBrands.OrderByDescending(d => d.Id).ToListAsync());
app.MapGet("/productbrands/{id}", async (int id, AppDbContext db) => await db.ProductBrands.FindAsync(id));
app.MapPost("/productbrands", async (ProductBrand brand, AppDbContext db) =>
{
    db.ProductBrands.Add(brand);
    await db.SaveChangesAsync();
    return Results.Created($"/productbrands/{brand.Id}", brand);
});
app.MapPut("/productbrands/{id}", async (int id, ProductBrand input, AppDbContext db) =>
{
    var brand = await db.ProductBrands.FindAsync(id);
    if (brand is null) return Results.NotFound();

    brand.Name = input.Name;
    await db.SaveChangesAsync();
    return Results.Ok(brand);
});
app.MapDelete("/productbrands/{id}", async (int id, AppDbContext db) =>
{
    var brand = await db.ProductBrands.FindAsync(id);
    if (brand is null) return Results.NotFound();

    db.ProductBrands.Remove(brand);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ------------------- USERGROUP CRUD -------------------
app.MapGet("/usergroups", async (AppDbContext db) => await db.UserGroups.OrderByDescending(d => d.Id).ToListAsync());
app.MapGet("/usergroups/{id}", async (int id, AppDbContext db) => await db.UserGroups.FindAsync(id));
app.MapPost("/usergroups", async (UserGroup group, AppDbContext db) =>
{
    db.UserGroups.Add(group);
    await db.SaveChangesAsync();
    return Results.Created($"/usergroups/{group.Id}", group);
});
app.MapPut("/usergroups/{id}", async (int id, UserGroup input, AppDbContext db) =>
{
    var group = await db.UserGroups.FindAsync(id);
    if (group is null) return Results.NotFound();

    group.Name = input.Name;
    await db.SaveChangesAsync();
    return Results.Ok(group);
});
app.MapDelete("/usergroups/{id}", async (int id, AppDbContext db) =>
{
    var group = await db.UserGroups.FindAsync(id);
    if (group is null) return Results.NotFound();

    db.UserGroups.Remove(group);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ------------------- USERS -------------------
app.MapGet("/users", async (AppDbContext db) =>
{
    var users = await db.Users
        .Include(u => u.UserGroup)
        .OrderByDescending(d => d.Id)
        .Select(u => new UsersDto
        {
            Id = u.Id,
            Name = u.Name,
            Surname = u.Surname,
            Phone = u.Phone,
            Username = u.Username,
            UserGroupName = u.UserGroup.Name
        }).ToListAsync();

    return Results.Ok(users);
});

app.MapGet("/users/{id}", async (int id, AppDbContext db) =>
{
    var user = await db.Users
        .Include(u => u.UserGroup)
        .Where(u => u.Id == id)
        .Select(u => new UsersDto
        {
            Id = u.Id,
            Name = u.Name,
            Surname = u.Surname,
            Phone = u.Phone,
            Username = u.Username,
            UserGroupName = u.UserGroup.Name
        }).FirstOrDefaultAsync();

    return user is null ? Results.NotFound() : Results.Ok(user);
});

app.MapPost("/users", async (Users user, AppDbContext db) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/users/{user.Id}", user);
});

app.MapPut("/users/{id}", async (int id, Users input, AppDbContext db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null) return Results.NotFound();

    user.Name = input.Name;
    user.Surname = input.Surname;
    user.Phone = input.Phone;
    user.Username = input.Username;
    user.Password = input.Password;
    user.UserGroupId = input.UserGroupId;

    await db.SaveChangesAsync();
    return Results.Ok(user);
});

app.MapDelete("/users/{id}", async (int id, AppDbContext db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null) return Results.NotFound();

    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ------------------- CAMPAIGN CRUD -------------------
app.MapGet("/campaigns", async (AppDbContext db) => await db.Campaigns.OrderByDescending(d => d.Id).ToListAsync());

app.MapGet("/campaigns/{id}", async (int id, AppDbContext db) =>
    await db.Campaigns.FindAsync(id) is Campaign c
        ? Results.Ok(c)
        : Results.NotFound()
);

app.MapPost("/campaigns", async (Campaign campaign, AppDbContext db) =>
{
    db.Campaigns.Add(campaign);
    await db.SaveChangesAsync();
    return Results.Created($"/campaigns/{campaign.Id}", campaign);
});

app.MapPut("/campaigns/{id}", async (int id, Campaign input, AppDbContext db) =>
{
    var campaign = await db.Campaigns.FindAsync(id);
    if (campaign is null) return Results.NotFound();

    campaign.Name = input.Name;
    campaign.StartDate = input.StartDate;
    campaign.EndDate = input.EndDate;
    campaign.Description = input.Description;

    await db.SaveChangesAsync();
    return Results.Ok(campaign);
});

app.MapDelete("/campaigns/{id}", async (int id, AppDbContext db) =>
{
    var campaign = await db.Campaigns.FindAsync(id);
    if (campaign is null) return Results.NotFound();

    db.Campaigns.Remove(campaign);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.UseCors("AllowAll");

app.Run();
