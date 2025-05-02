using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CareHub.Models { 
    [PrimaryKey(nameof(IdUtil),nameof(IdPost))]
    
    public class Up {
        [ForeignKey("id_util")]
        public int IdUtil { get; set; }
        
        [ForeignKey("id_post")]
        public int IdPost { get; set; }
        
    }
    
}