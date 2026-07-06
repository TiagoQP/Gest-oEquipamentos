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
    public class DeleteModel : PageModel
    {
        private readonly GestaoReservasUni.Data.ApplicationDbContext _context;

        public DeleteModel(GestaoReservasUni.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reserva Reserva { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.ItensReserva)
                    .ThenInclude(i => i.Equipamento)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva is not null)
            {
                Reserva = reserva;
                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Carrega a reserva com seus itens e equipamentos incluídos
            var reserva = await _context.Reservas
                .Include(r => r.ItensReserva)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva != null)
            {
                // Devolve o estoque de todos os equipamentos do carrinho
                foreach (var item in reserva.ItensReserva)
                {
                    var equipamento = await _context.Equipamentos.FindAsync(item.EquipamentoId);
                    if (equipamento != null)
                    {
                        equipamento.QuantidadeDisponivel += item.Quantidade;
                    }
                }

                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}