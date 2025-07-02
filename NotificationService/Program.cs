using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService.Models;
using NotificationService.Queue;
using NotificationService.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // RabbitMQ mesaj tüketicisi
        services.AddSingleton<NotificationHandler>();
        services.AddHostedService<RabbitMQConsumer>();

        // Otel odalarını HTTP ile sorgulayan servis
        services.AddHttpClient<IRoomQueryService, RoomQueryService>();

        // Bildirim gönderici (şu an console mock)
        services.AddSingleton<INotificationSender, ConsoleNotificationSender>();

        // E-posta gönderici
        services.AddScoped<IEmailSender, EmailSenderService>();

        // SMTP ayarlarını bağla
        services.Configure<SmtpSettings>(context.Configuration.GetSection("SmtpSettings"));

        // Gece 3'te çalışan kapasite kontrol servisimiz
        services.AddHostedService<LowCapacityCheckerService>();
    })
    .Build();

await host.RunAsync();
