using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppRestaurante.ViewModel
{
    public class TableViewModel
    {
        public int TableId { get; set; }
        [DisplayName("Descrição")]
        [Required(ErrorMessage ="O campo {0} é obrigatório")]
        public string TableDescription { get; set; }
    }
}