using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TruckApi.Infrastructure.Database.Entities;

public enum UserRole
{
    Admin,
    CompanyManager,
    CompanySupervisor,
    CompanyOperator,
}

[Table("users")]
[Index(nameof(CompanyId), Name = "users_company_id_idx")]
[Index(nameof(Role), Name = "users_role_idx")]
[Index(nameof(IsActive), Name = "users_is_active_idx")]
[Index(nameof(CompanyId), nameof(Role), Name = "users_company_id_role_idx")]
public class User
{
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("full_name")]
    public required string FullName { get; set; }

    [Column("whatsapp")]
    public required string Whatsapp { get; set; }

    [Column("password")]
    public string? Password { get; set; }

    [Column("document")]
    public string? Document { get; set; }

    [Column("age")]
    public int? Age { get; set; }

    [Column("role")]
    public UserRole Role { get; set; } = UserRole.CompanyOperator;

    [Column("picture")]
    public string? Picture { get; set; }

    [Column("company_id")]
    public string? CompanyId { get; set; }

    public Company? Company { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = false;

    [Column("timezone")]
    public string Timezone { get; set; } = "America/Sao_Paulo";

    [Column("accepted_terms_at")]
    public DateTimeOffset? AcceptedTermsAt { get; set; }

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
