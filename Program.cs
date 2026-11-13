using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Backend.Services;
using Backend.Validators;
using backend.Validators;
using backend.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKeyedSingleton<IPeopleService, PeopleService>("peopleService");
builder.Services.AddKeyedSingleton<IPeopleService, People2Service>("people2Service");

builder.Services.AddKeyedSingleton<IRandomService, RandomService>("randomSingleton");
builder.Services.AddKeyedScoped<IRandomService, RandomService>("randomScoped");
builder.Services.AddKeyedTransient<IRandomService, RandomService>("randomTransient");

builder.Services.AddHttpClient<IPostService, PostService>(client =>
{
    var baseUrl = builder.Configuration["BaseUrlPost"]
        ?? throw new InvalidOperationException("The BaseUrlPost configuration value is required.");
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreConnection"));
});

builder.Services.AddScoped<IValidator<BeerInsertDto>, BeerInsertValidator>();
builder.Services.AddScoped<IValidator<BeerUpdateDto>, BeerUpdateValidator>();
builder.Services.AddScoped<IValidator<BrandInsertDto>, BrandInsertValidator>();
builder.Services.AddScoped<IValidator<BrandUpdateDto>, BrandUpdateValidator>();
builder.Services.AddScoped<IValidator<backend.DTOs.InventoryInsertDto>, InventoryInsertValidator>();
builder.Services.AddScoped<IValidator<backend.DTOs.InventoryUpdateDto>, InventoryUpdateValidator>();
builder.Services.AddScoped<IValidator<SupplierInsertDto>, SupplierInsertValidator>();
builder.Services.AddScoped<IValidator<SupplierUpdateDto>, SupplierUpdateValidator>();
builder.Services.AddScoped<IValidator<SaleInsertDto>, SaleInsertValidator>();
builder.Services.AddScoped<IValidator<SaleUpdateDto>, SaleUpdateValidator>();

builder.Services.AddKeyedScoped<ICommonService<BeerDto, BeerInsertDto, BeerUpdateDto>, BeerService>("beerService");
builder.Services.AddScoped<IRepository<Beer>, BeerRepository>();

builder.Services.AddKeyedScoped<BrandService>("brandService");
builder.Services.AddKeyedScoped<ICommonService<BrandDto, BrandInsertDto, BrandUpdateDto>, BrandService>("brandService");
builder.Services.AddScoped<IRepository<Brand>, BrandRepository>();
builder.Services.AddScoped<IRepository<Supplier>, SupplierRepository>();
builder.Services.AddScoped<IRepository<Sale>, SaleRepository>();
builder.Services.AddKeyedScoped<
    ICommonService<SupplierDto, SupplierInsertDto, SupplierUpdateDto>,
    SupplierService>("supplierService");
builder.Services.AddKeyedScoped<
    ICommonService<SaleDto, SaleInsertDto, SaleUpdateDto>,
    SaleService>("saleService");
builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
