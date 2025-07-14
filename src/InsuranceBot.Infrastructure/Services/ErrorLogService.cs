using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceBot.Domain.Entities;
using InsuranceBot.Domain.Interfaces.Services;
using InsuranceBot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InsuranceBot.Infrastructure.Services;

public class ErrorLogService(AppDbContext db) : IErrorLogService
{
    public async Task LogAsync(ErrorLog err)
    {
        db.Errors.Add(err);
        await db.SaveChangesAsync();
    }

    public async Task<string> GetRecentErrorsAsync(int count = 5)
    {
        List<ErrorLog> recent = await db.Errors.OrderByDescending(e => e.Timestamp).Take(count).ToListAsync();
        return string.Join("\n---\n", recent.Select(e => $"{e.Timestamp}: {e.Message}"));
    }
}