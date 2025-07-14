using System;
using System.Threading.Tasks;
using InsuranceBot.Domain.Entities;
using InsuranceBot.Domain.Interfaces.Services;
using InsuranceBot.Infrastructure.Data;

namespace InsuranceBot.Infrastructure.Services;

public class AuditLogService(AppDbContext db) : IAuditLogService
{
    public async Task LogAsync(long userId, string action, string state)
    {
        db.AuditLogs.Add(new AuditLog
            { UserId = userId, Action = action, State = state, Timestamp = DateTime.UtcNow });
        await db.SaveChangesAsync();
    }
}