using Core.CustomAttributes;
using Infrastructure.Data.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Transactions
{
    public class AddTransactionViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [IsFile(ErrorMessage = "Please select only Supported Files. Flite must by maximum 10mb.")]
        public IFormFile? File { get; set; }

        [Required, StringLength(200)]
        public string CategoryTransactions { get; set; }

        [StringLength(700)]
        public string? Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [EnumDataType(typeof(Currency))]
        public string Currency { get; set; }

        [Required]
        [EnumDataType(typeof(TransactionType))]
        public string TransactionType { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
