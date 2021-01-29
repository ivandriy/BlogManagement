using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlogManagement.DataAccess.Models
{
    [Index(nameof(ThemeName), IsUnique = true)]
    public class Theme
    {
        public int ThemeId { get; set; }

        [Required]
        public string ThemeName { get; set; }
    }
}