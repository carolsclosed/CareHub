using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CareHub.Models { 
    [PrimaryKey(nameof(IdUtil),nameof(IdPost))]
    public class Up
    {
        // Foreign Keys
        [ForeignKey(nameof(Utilizador))]
        public int IdUtil { get; set; }    
        
        [ForeignKey(nameof(Post))]
        public int IdPost { get; set; }

        // Navigation properties
        public Utilizadores Utilizador { get; set; }
        public Posts Post { get; set; }
    }
}