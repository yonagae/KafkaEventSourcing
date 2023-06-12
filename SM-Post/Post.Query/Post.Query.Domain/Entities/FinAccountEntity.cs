using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities;

[Table("FinAccount")]
public class FinAccountEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Owner { get; set; }
    public Decimal TotalBalance { get; set; }
    public virtual ICollection<BalanceByTransactionTypeEntity> Balances { get; set; }
}