using System;
using System.Threading.Tasks;
using Hangfire;
using AppGestorResumo.Services; // Ajuste conforme o namespace correto
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main(string[] args)
    {
        // Configuração do Hangfire para usar armazenamento em memória
        var services = new ServiceCollection();
        services.AddHangfire(x => x.UseInMemoryStorage());
        services.AddTransient<ResumoService>();
        services.AddTransient<EmailService>();

        // Inicializa Hangfire
        var serviceProvider = services.BuildServiceProvider();
        var hangfireServer = new BackgroundJobServer();
        
        // Job para gerar o resumo
        RecurringJob.AddOrUpdate<ResumoService>(
            "resumo-diario", 
            async (service) =>
            {
                var resumo = await service.GerarResumoAsync();
                var emailService = serviceProvider.GetRequiredService<EmailService>();
                await emailService.EnviarEmailAsync("gerente@empresa.com", "Resumo Diário", resumo);

                // Aqui você coloca o que deseja mostrar no CMD
                Console.WriteLine("Resumo gerado e email enviado:");
                Console.WriteLine(resumo); // Ou qualquer outra informação relevante
            }, 
            Cron.Daily); // Você pode ajustar o cronograma do job conforme necessário

        // Roda a aplicação e garante que o CMD fique aberto
        Console.WriteLine("A aplicação está em execução...");
        Console.WriteLine("Pressione qualquer tecla para sair.");
        Console.ReadKey();

        // Finaliza o servidor Hangfire quando o usuário pressionar qualquer tecla
        hangfireServer.Dispose();
    }
}
