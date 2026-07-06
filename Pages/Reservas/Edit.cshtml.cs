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
    public class EditModel : PageModel
    {
        private readonly GestaoReservasUni.Data.ApplicationDbContext _context;

        public EditModel(GestaoReservasUni.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reserva Reserva { get; set; } = default!;

        [BindProperty]
        public List<ItemReserva> ItensCarrinho { get; set; } = new List<ItemReserva>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva =  await _context.Reservas
                .Include(r => r.ItensReserva)
                    .ThenInclude(i => i.Equipamento)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }
            
            Reserva = reserva;

            // Alimenta a lista que será enviada para o JavaScript carregar na tabela do carrinho
            ItensCarrinho = reserva.ItensReserva.ToList();

            ViewData["EquipamentosLista"] = new SelectList(_context.Equipamentos, "Id", "Nome");
            return Page();
        }

   // ADICIONADO: [FromForm] List<ItemReserva> itensCarrinho direto no parâmetro
public async Task<IActionResult> OnPostAsync([FromForm] List<ItemReserva> itensCarrinho)
{
    // Se o BindProperty falhar, injetamos manualmente o parâmetro recebido do formulário
    if (itensCarrinho != null && itensCarrinho.Any())
    {
        ItensCarrinho = itensCarrinho;
    }

    // Remove as validações automáticas que travam o ModelState
    ModelState.Remove("ItensCarrinho");
    ModelState.Remove("Reserva.ItensReserva");

    // Força a limpeza de qualquer resquício de erro do carrinho no validador
    foreach (var key in ModelState.Keys.Where(k => k.Contains("ItensCarrinho") || k.Contains("ItensReserva")).ToList())
    {
        ModelState.Remove(key);
    }

    if (!ModelState.IsValid)
    {
        ViewData["EquipamentosLista"] = new SelectList(_context.Equipamentos, "Id", "Nome");
        return Page();
    }

    // 1. Busca a reserva original no banco trazendo os itens anteriores
    var reservaNoBanco = await _context.Reservas
        .Include(r => r.ItensReserva)
        .FirstOrDefaultAsync(r => r.Id == Reserva.Id);

    if (reservaNoBanco == null)
    {
        return NotFound();
    }

    // 2. Atualiza as propriedades básicas (Incluindo o novo STATUS que você alterou!)
    reservaNoBanco.AssistenteId = Reserva.AssistenteId;
    reservaNoBanco.NomeProfessorBeneficiario = Reserva.NomeProfessorBeneficiario;
    reservaNoBanco.Sala = Reserva.Sala;
    reservaNoBanco.Data = Reserva.Data;
    reservaNoBanco.Periodo = Reserva.Periodo;
    reservaNoBanco.Status = Reserva.Status;

    // 3. Devolve temporariamente o estoque antigo para recalcular limpo
    foreach (var itemAntigo in reservaNoBanco.ItensReserva)
    {
        var equip = await _context.Equipamentos.FindAsync(itemAntigo.EquipamentoId);
        if (equip != null)
        {
            equip.QuantidadeDisponivel += itemAntigo.Quantidade;
        }
    }

    // 4. Remove os itens antigos do relacionamento
    _context.ItensReserva.RemoveRange(reservaNoBanco.ItensReserva);
    reservaNoBanco.ItensReserva.Clear();

    if (ItensCarrinho == null || !ItensCarrinho.Any())
    {
        ModelState.AddModelError(string.Empty, "A reserva deve conter ao menos um equipamento.");
        ViewData["EquipamentosLista"] = new SelectList(_context.Equipamentos, "Id", "Nome");
        return Page();
    }

    // 5. Valida o estoque e insere os itens atuais vindos da tela
    foreach (var itemNovo in ItensCarrinho)
    {
        var equip = await _context.Equipamentos.FindAsync(itemNovo.EquipamentoId);
        if (equip == null || equip.QuantidadeDisponivel < itemNovo.Quantidade)
        {
            ModelState.AddModelError(string.Empty, $"Estoque insuficiente para o item '{equip?.Nome ?? "Desconhecido"}'.");
            ViewData["EquipamentosLista"] = new SelectList(_context.Equipamentos, "Id", "Nome");
            return Page();
        }

        // Deduz a quantidade do estoque disponível
        equip.QuantidadeDisponivel -= itemNovo.Quantidade;

        // Adiciona de volta ao banco vinculado à reserva
        reservaNoBanco.ItensReserva.Add(new ItemReserva
        {
            EquipamentoId = itemNovo.EquipamentoId,
            Quantidade = itemNovo.Quantidade
        });
    }

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!ReservaExists(Reserva.Id))
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

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}