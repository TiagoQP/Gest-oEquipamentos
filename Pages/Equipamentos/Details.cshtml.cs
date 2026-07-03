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
    public class DetailsModel : PageModel
    {
        private readonly GestaoReservasUni.Data.ApplicationDbContext _context;

        public DetailsModel(GestaoReservasUni.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
