using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Trabalho.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly ClientDbContext _context;

    public ClientesController(ClientDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetClients()
    {
        return await _context.Clients.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetClient(int id)
    {
        return await _context.Clients.FindAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult<Client>> CreateClient(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(int id, Client client)
    {
        if (id != client.Id)
        {
            return BadRequest();
        }

        _context.Entry(client).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return NotFound();
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("endereco")]
    public async Task<ActionResult<IEnumerable<Client>>> GetClientsByEndereco(string endereco)
    {
        var clients = await _context.Clients.Where(c => c.Endereco.Contains(endereco)).ToListAsync();
        return clients;
    }
    
    [HttpGet("cidade")]
    public async Task<ActionResult<IEnumerable<Client>>> GetClientsByCidade(string cidade)
    {
        var clients = await _context.Clients.Where(c => c.Cidade == cidade).ToListAsync();
        return clients;
    }
}


public class Client
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Endereco { get; set; }
    public string Cidade {get; set; }
}

public class ClientDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=trabalho_api.db");
    }
}