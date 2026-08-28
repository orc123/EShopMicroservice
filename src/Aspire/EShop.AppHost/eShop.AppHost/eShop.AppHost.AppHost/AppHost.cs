var builder = DistributedApplication.CreateBuilder(args);

// 1. Backing Services (Containers)
var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var calalogDb = postgres.AddDatabase("CatalogDb");
var basketDb = postgres.AddDatabase("BasketDb");

var redis = builder.AddRedis("distributedcache");

var rabbitmq = builder.AddRabbitMQ("messagebroker")
    .WithManagementPlugin();

var sqlServer = builder.AddSqlServer("sqlserver");

var orderDb = sqlServer.AddDatabase("OrderDb");

// 2. Microservice
var discountGerpc = builder.AddProject<Projects.Discount_gRPC>("discount-grpc");

var catalogApi = builder.AddProject<Projects.Catalog_API>("catalog-api")
    .WithReference(calalogDb, connectionName: "Database");

var basketApi = builder.AddProject<Projects.Basket_API>("basket-api")
    .WithReference(basketDb, connectionName: "Database")
    .WithReference(redis, connectionName: "Redis")
    .WithReference(discountGerpc)
    .WithReference(rabbitmq);

var orderingApi = builder.AddProject<Projects.Ordering_API>("ordering-api")
    .WithReference(orderDb, connectionName: "Database")
    .WithReference(rabbitmq);

var yarpapigateway = builder.AddProject<Projects.YarpApiGateway>("yarpapigateway")
    .WithReference(catalogApi)
    .WithReference(basketApi)
    .WithReference(orderingApi);

var shoppingWeb = builder.AddProject<Projects.Shopping_Web>("shopping-web")
    .WithReference(yarpapigateway)
     .WithEnvironment("ApiSettings__GatewayAddress", yarpapigateway.GetEndpoint("https"));

builder.Build().Run();
