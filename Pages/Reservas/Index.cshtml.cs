using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GestaoReservasUni.Data;
using GestaoReservasUni.Models;

namespace Gest_oEquipamentos.Pages.Reservas
{
    public class IndexModel : PageModel
    {
        private readonly GestaoReservasUni.Data.ApplicationDbContext _context;

        public IndexModel(GestaoReservasUni.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Reserva> Reserva { get;set; } = default!;

        public async Task OnGetAsync()
        {
            // Agora trazemos os Itens da Reserva e, para cada item, incluímos o Equipamento correspondente
            Reserva = await _context.Reservas
                .Include(r => r.ItensReserva)
                    .ThenInclude(i => i.Equipamento)
                .ToListAsync();
        }
    }
}