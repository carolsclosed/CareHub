using CareHub.Data;
using Microsoft.AspNetCore.SignalR;

namespace CareHub.Services.WebSockts;

public class ComentarioHub : Hub
{
    private readonly ApplicationDbContext _context;
}