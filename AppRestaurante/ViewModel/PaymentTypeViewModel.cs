using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppRestaurante.ViewModel
{
    public class PaymentTypeViewModel
    {
        public int PaymentTypeId      { get; set; }
        [DisplayName("Tipo de pagamento")]
        [Required(ErrorMessage ="O campo {0} é obrigatório")]
        public string PaymentTypeName { get; set; }
    }
}