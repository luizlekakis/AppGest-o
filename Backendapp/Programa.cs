using Hangfire;
using Microsoft.EntityFrameworkCore;
using AppGestorResumo.Data;
using AppGestorResumo.Services;

var builder = WebApplication.CreateBuilder(args);

//Serviços
builder.Services.AddDbContext<SambaDbContext>();
builder.Services.AddTransient<ResumoService>();
builder.Services.AddTransient<EmailService>();

builder.Services.AddHangfire(x => x.UseInMemoryStorage());
builder.Services.AddHangfireServer();

var app = builder.Build();

//Painel do Hangfire
app.UseHangfireDashboard();


RecurringJob.AddOrUpdate<ResumoService>("resumo-diario", async (service) =>
{
    var resumo = await service.GerarResumoAsync();
    var emailService = app.Services.GetRequiredService<EmailService>();
    await emailService.EnviarEmailAsync("gerente@empresa.com", "Resumo Diário", resumo);
}, Cron.Daily);

app.Run();
