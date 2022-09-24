using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppRestaurante.ViewModel
{
    public class ItemViewModel
    {
        public int ItemId { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "*{0} é Obrigatório")]
        public string ItemName { get; set; }
        
        [DisplayName("Valor")]
        [Required(ErrorMessage = "*{0} é Obrigatório")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "O valor informado não é valido")]
        public decimal ItemPrice  { get; set; }

        [DisplayName("Categoria")]
        [Required(ErrorMessage = "*{0} é Obrigatório")]
        public string ItemCategory { get; set; }
    }
}