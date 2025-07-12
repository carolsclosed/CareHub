using System.ComponentModel.DataAnnotations.Schema; 
using Microsoft.EntityFrameworkCore; 

namespace CareHub.Models { 

    /// <summary>
    /// modelo dos upvotes
    /// </summary>
    [PrimaryKey(nameof(IdUtil),nameof(IdPost))]
    public class Up
    {
        
        [ForeignKey(nameof(Utilizador))]
        public int IdUtil { get; set; }    
        
        [ForeignKey(nameof(Post))]
        public int IdPost { get; set; }
        
        public Utilizadores Utilizador { get; set; } 
        public Posts Post { get; set; } 
    }
}