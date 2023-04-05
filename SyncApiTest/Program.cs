var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("SqlLite"))
        .EnableSensitiveDataLogging());

builder.Services.AddGraphQLServer()
    .AddMutationConventions(applyToAllMutations: true)
    .RegisterDbContext<ApplicationDbContext>(DbContextKind.Pooled)
    .AddQueryType<Query>().AddProjections().AddFiltering().AddSorting()
    .AddMutationType<Mutation>();

builder.Services.AddTransient<IDogRepository,DogRepository>();
builder.Services.AddTransient<IOwnerRepository,OwnerRepository>();
builder.Services.AddTransient<ISyncService,SyncService>();
//builder.Services.AddTransient<ISortingEntities,SortingEntities>();
builder.Services.AddTransient<IPropertyDependencySorter, PropertyDependencySorter>();

 
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
    await using (var dbContext = dbContextFactory.CreateDbContext())
    {
        await dbContext.Database.MigrateAsync();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();

app.UseRouting();
app.MapGraphQL("/api/graphql");


app.Run();