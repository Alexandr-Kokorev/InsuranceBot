using InsuranceBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InsuranceBot.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<ExtractedField> ExtractedFields { get; set; }
    public DbSet<Policy> Policies { get; set; }
    public DbSet<ConversationLog> Conversations { get; set; }
    public DbSet<ErrorLog> Errors { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<PolicyEvent> PolicyEvents { get; set; }
}