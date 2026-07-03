using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GestaoReservasUni.Data;
using GestaoReservasUni.Models;

namespace Gest_oEquipamentos.Pages.Equipamentos
{
    public class DeleteModel : PageModel
    {
        private readonly GestaoReservasUni.Data.ApplicationDbContext _context;

        public DeleteModel(GestaoReservasUni.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Equipamento Equipamento { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipamento = await _context.Equipamentos.FirstOrDefaultAsync(m => m.Id == id);

            if (equipamento is not null)
            {
                Equipamento = equipamento;

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

            var equipamento = await _context.Equipamentos.FindAsync(id);
            if (equipamento != null)
            {
                Equipamento = equipamento;
                _context.Equipamentos.Remove(Equipamento);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
