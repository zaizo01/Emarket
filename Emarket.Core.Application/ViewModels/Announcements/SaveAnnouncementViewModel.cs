using Emarket.Core.Application.ViewModels.Categories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emarket.Core.Application.ViewModels.Announcements
{
    public class SaveAnnouncementViewModel
    {

        public int Id { get; set; } = 0;
        
        [Required(ErrorMessage = "Este valor es requerido.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Este vaor es requerido.")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Este vaor es requerido.")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Este vaor es requerido.")]
        public double Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe colocar la categoria del producto")]
        public int CategoryId { get; set; }

        public List<CategoryViewModel>? Categories { get; set; }
        [Required(ErrorMessage = "Este vaor es requerido.")]
        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }
    }
}
