using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application.ViewModels.Categories
{
    public class SaveCategoryViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Esta valor es requerido.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Esta valor es requerido.")]
        public string Description { get; set; }
    }
}
