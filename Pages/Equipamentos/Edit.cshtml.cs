using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestaoReservasUni.Data;
using GestaoReservasUni.Models;

namespace Gest_oEquipamentos.Pages.Equipamentos
{
    public class EditModel : PageModel
    {
        private readonly GestaoReservasUni.Data.ApplicationDbContext _context;

        public EditModel(GestaoReservasUni.Data.ApplicationDbContext context)
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

            var equipamento =  await _context.Equipamentos.FirstOrDefaultAsync(m => m.Id == id);
            if (equipamento == null)
            {
                return NotFound();
            }
            Equipamento = equipamento;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Equipamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipamentoExists(Equipamento.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool EquipamentoExists(int id)
        {
            return _context.Equipamentos.Any(e => e.Id == id);
        }
    }
}
