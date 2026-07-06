using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestaoReservasUni.Data;
using GestaoReservasUni.Models;
using Microsoft.EntityFrameworkCore;

namespace Gest_oEquipamentos.Pages.Reservas
{
    public class CreateModel : PageModel
    {
        private readonly GestaoReservasUni.Data.ApplicationDbContext _context;

        public CreateModel(GestaoReservasUni.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // Carrega os equipamentos para preencher o select da tela
            ViewData["EquipamentosLista"] = new SelectList(_context.Equipamentos.Where(e => e.QuantidadeDisponivel > 0), "Id", "Nome");
            return Page();
        }

        [BindProperty]
        public Reserva Reserva { get; set; } = default!;

        // Esta propriedade receberá os itens do carrinho enviados pelo formulário
        [BindProperty]
        public List<ItemReserva> ItensCarrinho { get; set; } = new List<ItemReserva>();

     // ADICIONADO: [FromForm] List<ItemReserva> itensCarrinho direto no parâmetro do método
public async Task<IActionResult> OnPostAsync([FromForm] List<ItemReserva> itensCarrinho)
{
    // Se o BindProperty falhar, nós injetamos manualmente o parâmetro recebido do formulário
    if (itensCarrinho != null && itensCarrinho.Any())
    {
        ItensCarrinho = itensCarrinho;
    }

    ModelState.Remove("ItensCarrinho");
    ModelState.Remove("Reserva.ItensReserva");

    // Forçamos a limpeza de erros dos itens do carrinho para o ModelState não travar o IsValid
    foreach (var key in ModelState.Keys.Where(k => k.Contains("ItensCarrinho") || k.Contains("ItensReserva")).ToList())
    {
        ModelState.Remove(key);
    }

    if (!ModelState.IsValid)
    {
        ViewData["EquipamentosLista"] = new SelectList(_context.Equipamentos.Where(e => e.QuantidadeDisponivel > 0), "Id", "Nome");
        return Page();
    }

    if (ItensCarrinho == null || !ItensCarrinho.Any())
    {
        ModelState.AddModelError(string.Empty, "Você precisa adicionar pelo menos um equipamento ao carrinho.");
        ViewData["EquipamentosLista"] = new SelectList(_context.Equipamentos.Where(e => e.QuantidadeDisponivel > 0), "Id", "Nome");
        return Page();
    }

    // ... restante do código do OnPostAsync continua exatamente igual abaixo ...

    // Inicializa a lista de itens da reserva para evitar nulos
    Reserva.ItensReserva = new List<ItemReserva>();

    // Validar estoque e associar itens à reserva
    foreach (var item in ItensCarrinho)
    {
        var equip = await _context.Equipamentos.FindAsync(item.EquipamentoId);
        if (equip == null || equip.QuantidadeDisponivel < item.Quantidade)
        {
            ModelState.AddModelError(string.Empty, $"Estoque insuficiente para o equipamento: {equip?.Nome ?? "Desconhecido"}.");
            ViewData["EquipamentosLista"] = new SelectList(_context.Equipamentos.Where(e => e.QuantidadeDisponivel > 0), "Id", "Nome");
            return Page();
        }

        // Deduz do estoque disponível
        equip.QuantidadeDisponivel -= item.Quantidade;

        // Adiciona o item diretamente na lista relacionada da reserva
        Reserva.ItensReserva.Add(new ItemReserva 
        { 
            EquipamentoId = item.EquipamentoId, 
            Quantidade = item.Quantidade 
        });
    }

    // Salva a reserva mãe e o EF Core já salvará os filhos automaticamente
    _context.Reservas.Add(Reserva);
    await _context.SaveChangesAsync();

    return RedirectToPage("./Index");
}
    }
}