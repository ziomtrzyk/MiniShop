var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDb = postgres.AddDatabase("catalogdb");

builder.AddProject<Projects.Catalog_API>("catalog-api")
    .WithReference(catalogDb)
    .WaitFor(catalogDb);

builder.Build().Run();
