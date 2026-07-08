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

        public IList<Reserva> Reserva { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? BuscarProfessor { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? BuscarSala { get; set; }

        // Como o Status no seu Reserva.cs é string, o filtro também deve ser string!
        [BindProperty(SupportsGet = true)]
        public string? BuscarStatus { get; set; }

        public async Task OnGetAsync()
        {
            var consulta = _context.Reservas
                .Include(r => r.ItensReserva)
                    .ThenInclude(i => i.Equipamento)
                .AsQueryable();

            if (!string.IsNullOrEmpty(BuscarProfessor))
            {
                consulta = consulta.Where(r => r.NomeProfessorBeneficiario.ToLower().Contains(BuscarProfessor.ToLower()));
            }

            if (!string.IsNullOrEmpty(BuscarSala))
            {
                consulta = consulta.Where(r => r.Sala.ToLower().Contains(BuscarSala.ToLower()));
            }

            if (!string.IsNullOrEmpty(BuscarStatus))
            {
                consulta = consulta.Where(r => r.Status == BuscarStatus);
            }

            Reserva = await consulta.OrderByDescending(r => r.Data).ToListAsync();
        }
    }
}