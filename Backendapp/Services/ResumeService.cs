using AppGestorResumo.Data;
using Microsoft.EntityFrameworkCore;

namespace AppGestorResumo.Services
{
    public class ResumoService
    {
        private readonly SambaDbContext _db;

        public ResumoService(SambaDbContext db)
        {
            _db = db;
        }

        public async Task<string> GerarResumoAsync()
        {
            var hoje = DateTime.Today;
            var tickets = await _db.Tickets
                .Where(t => t.Date >= hoje)
                .ToListAsync();

            var total = tickets.Sum(t => t.TotalAmount);
            var pedidos = tickets.Count;

            return $"📊 Resumo {hoje:dd/MM/yyyy}\n" +
                   $"Vendas: R$ {total:F2}\n" +
                   $"Pedidos: {pedidos}";
        }
    }
}
