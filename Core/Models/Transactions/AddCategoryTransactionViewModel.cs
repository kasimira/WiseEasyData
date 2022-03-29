using System.ComponentModel.DataAnnotations;

namespace Core.Models.Transactions
{
    public class AddCategoryTransactionViewModel
    {

        [Required, StringLength(200)]
        public string Name { get; set; }
    }
}
