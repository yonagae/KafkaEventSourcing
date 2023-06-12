using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities;

[Table("TransactionType")]
public class TransactionTypeEntity
{
    [Key]
    public Guid Id { get; set; }
    public Guid FinAccountId { get; set; }
    public string Name { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual FinAccountEntity FinAccount { get; set; }
}
