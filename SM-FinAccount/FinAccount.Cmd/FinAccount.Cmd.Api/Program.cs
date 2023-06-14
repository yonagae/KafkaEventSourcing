
using Confluent.Kafka;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using MongoDB.Bson.Serialization;
using Post.Cmd.Api.Commands;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Config;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Handlers;
using Post.Cmd.Infrastructure.Producers;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;
using Post.Common.Events;

var builder = WebApplication.CreateBuilder(args);

BsonClassMap.RegisterClassMap<BaseEvent>();
BsonClassMap.RegisterClassMap<NewFinAccountEvent>();
BsonClassMap.RegisterClassMap<DebitFinAccountEvent>();
BsonClassMap.RegisterClassMap<CreditFinAccountEvent>();
BsonClassMap.RegisterClassMap<AddTransactionTypeEvent>();

// Add services to the container.
builder.Services.Configure<CosmosDBConfig>(builder.Configuration.GetSection(nameof(CosmosDBConfig)));
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer, EventProducer>();
builder.Services.AddScoped<IEventStore, EventStore>();

// register command handler methods

var dispatcher = new CommandDispatcher();
builder.Services.AddScoped<IEventSourcingHandler<FinAccountAggregate>, FinAccountEventSourcingHandler>();
builder.Services.AddScoped<IFinAccountCommandHandler, FinAccountCommandHandler>();
var finAccounntCommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<IFinAccountCommandHandler>();
dispatcher.RegisterHandler<NewFinAccountCommand>(finAccounntCommandHandler.HandleAsync);
dispatcher.RegisterHandler<DebitFinAccountCommand>(finAccounntCommandHandler.HandleAsync);
dispatcher.RegisterHandler<CreditFinAccountCommand>(finAccounntCommandHandler.HandleAsync);
dispatcher.RegisterHandler<AddTransactionTypeCommand>(finAccounntCommandHandler.HandleAsync);


builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
