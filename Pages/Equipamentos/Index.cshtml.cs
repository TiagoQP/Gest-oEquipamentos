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
    public class IndexModel : PageModel
    {
        private readonly GestaoReservasUni.Data.ApplicationDbContext _context;

        public IndexModel(GestaoReservasUni.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Equipamento> Equipamento { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Equipamento = await _context.Equipamentos.ToListAsync();
        }
    }
}
