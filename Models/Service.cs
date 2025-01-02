using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication_Client.Models;

[Table("Service")]
public class Service
{
    [Key]
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Description { get; set; }
    public double Prix { get; set; }
}