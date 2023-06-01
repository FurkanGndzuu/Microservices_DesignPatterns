using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaStateMachine_WorkerService;
using SagaStateMachine_WorkerService.Models;
using Shared.Consts;
using System.Reflection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext , services) =>
    {
        services.AddHostedService<Worker>();

        services.AddMassTransit(cfg =>
        {
            cfg.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>().EntityFrameworkRepository(opt =>
            {
                opt.AddDbContext<DbContext, OrderSagaDbContext>((provider, builder) =>
                {
                    builder.UseSqlServer(hostContext.Configuration.GetConnectionString("SqlServer") , m =>
                    {
                        m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                    });
                });
            });

            cfg.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(configuration =>
            {
                configuration.Host(hostContext.Configuration["RabbitMQ:Host"], "/", opt =>
                {
                    opt.Username(hostContext.Configuration["RabbitMQ:username"]);
                    opt.Password(hostContext.Configuration["RabbitMQ:password"]);   
                });

                configuration.ReceiveEndpoint(RabbitMQSetttings.SagaOrder, e =>
                {
                    e.ConfigureSaga<OrderStateInstance>(provider);
                });
            }));

        });
       services.AddOptions<MassTransitHostOptions>();

    })
    .Build();

await host.RunAsync();
