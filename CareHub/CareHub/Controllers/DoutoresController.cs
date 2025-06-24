

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CareHub.Data;
using CareHub.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CareHub.Controllers;

public class DoutoresController : Controller
{
    private readonly ApplicationDbContext _context;
    public DoutoresController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET
    public IActionResult Doutores()
    {
      // Inclui o utilizador associado ao doutor para obter os dados base
        var doutores = _context.Doutores
            .Include(d => d.Utilizador)  // Carrega os dados do utilizador associado
            .ToList();

        return View(doutores);
          
    }
}