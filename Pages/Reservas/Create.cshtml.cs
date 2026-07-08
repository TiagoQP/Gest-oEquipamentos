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

namespace Gest_oEquipamentos.Pages.Reservas
{
    public class CreateModel : PageModel
    {
        private readonly GestaoReservasUni.Data.ApplicationDbContext _context;

        public CreateModel(GestaoReservasUni.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        private void CarregarEquipamentos()
        {
            var equipamentos = _context.Equipamentos.OrderBy(e => e.Nome).ToList();
            ViewData["EquipamentosLista"] = new SelectList(equipamentos, "Id", "Nome");
        }

        public IActionResult OnGet()
        {
            CarregarEquipamentos();
            return Page();
        }

        [BindProperty]
        public Reserva Reserva { get; set; } = default!;

        [BindProperty]
        public List<int> ItemEquipamentoIds { get; set; } = new List<int>();

        [BindProperty]
        public List<int> ItemQuantidades { get; set; } = new List<int>();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                CarregarEquipamentos();
                return Page();
            }

            var novosItens = new List<ItemReserva>();
            for (int i = 0; i < ItemEquipamentoIds.Count; i++)
            {
                if (ItemEquipamentoIds[i] > 0 && ItemQuantidades[i] > 0)
                {
                    novosItens.Add(new ItemReserva
                    {
                        EquipamentoId = ItemEquipamentoIds[i],
                        Quantidade = ItemQuantidades[i]
                    });
                }
            }

            if (!novosItens.Any())
            {
                ModelState.AddModelError(string.Empty, "Selecione pelo menos um equipamento válido no seu carrinho.");
                CarregarEquipamentos();
                return Page();
            }

            // Busca as reservas ativas do mesmo dia e período para validação
            var reservasNoMesmoPeriodo = await _context.Reservas
                .Include(r => r.ItensReserva)
                .Where(r => r.Data.Date == Reserva.Data.Date && r.Periodo == Reserva.Periodo && r.Status != "Cancelada")
                .ToListAsync();

            foreach (var itemPretendido in novosItens)
            {
                var equipamento = await _context.Equipamentos.FindAsync(itemPretendido.EquipamentoId);
                if (equipamento == null) continue;

                // Identifica dinamicamente a coluna de estoque físico total
                var propEstoque = equipamento.GetType().GetProperty("Estoque") 
                                  ?? equipamento.GetType().GetProperty("QuantidadeDisponivel")
                                  ?? equipamento.GetType().GetProperty("QuantidadeEstoque")
                                  ?? equipamento.GetType().GetProperties().FirstOrDefault(p => p.PropertyType == typeof(int) && p.Name != "Id");

                int estoqueFisicoTotal = 0;
                if (propEstoque != null)
                {
                    estoqueFisicoTotal = (int)(propEstoque.GetValue(equipamento) ?? 0);
                }

                int quantidadeJaComprometida = reservasNoMesmoPeriodo
                    .SelectMany(r => r.ItensReserva)
                    .Where(i => i.EquipamentoId == itemPretendido.EquipamentoId)
                    .Sum(i => i.Quantidade);

                int estoqueDisponivelReal = estoqueFisicoTotal - quantidadeJaComprometida;

                if (itemPretendido.Quantidade > estoqueDisponivelReal)
                {
                    ModelState.AddModelError(string.Empty, 
                        $"Estoque insuficiente para '{equipamento.Nome}'. " +
                        $"Disponível neste período: {estoqueDisponivelReal} unidade(s). (Já reservados neste horário: {quantidadeJaComprometida}).");
                    
                    CarregarEquipamentos();
                    return Page();
                }

                Reserva.ItensReserva.Add(itemPretendido);
            }

            _context.Reservas.Add(Reserva);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}