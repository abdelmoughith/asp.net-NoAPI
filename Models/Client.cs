using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication_Client.Models;

[Table("Client")]
public class Client
{
    // data for auth
    [Key]
    public int Id { get; set; }
    
    [Required] // Not null
    [MaxLength(100)]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Password { get; set; }

    [Required]
    public Role role { get; set; }
    
    // all data about client
    [Required]
    public string Nom { get; set; }
    [Required]
    public string Prenom { get; set; }
    [Required]
    public int Age { get; set; }
    [Required]
    public string CIN { get; set; }
    [Required]
    public string Telephone { get; set; }
}