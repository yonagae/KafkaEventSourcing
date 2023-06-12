using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities;

[Table("BalanceByTransactionType")]
public class BalanceByTransactionTypeEntity
{ 
    public Guid FinAccountId { get; set; }
    public Guid TransactionTypeId { get; set; }
    public Decimal Balance { get; set; }


    [System.Text.Json.Serialization.JsonIgnore]
    [ForeignKey("FinAccountId")]
    public virtual FinAccountEntity FinAccount { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    [ForeignKey("TransactionTypeId")]
    public virtual TransactionTypeEntity TransactionType { get; set; }

}
