using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CareHub.Models { 
    [PrimaryKey(nameof(id_util),nameof(id_post))]
    
    public class Up {
        [ForeignKey("id_util")]
        public int id_util { get; set; }
        
        [ForeignKey("id_post")]
        public int id_post { get; set; }
        
        //public ICollection<Comentarios> ListaComentarios { get; set; }
    }
    
}