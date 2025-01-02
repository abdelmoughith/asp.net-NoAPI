using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication_Client.Models;
[Table("Chambre")]
public class Chambre
{
    [Key]
    public int Id { get; set; }
    public int Numero { get; set; }
    public ChambreType Type { get; set; }
    public double PrixParNuit { get; set; }
    public bool Disponibilite { get; set; }


    
}
public enum ChambreType
{
    VIP,
    NORMAL,
    TRIPLET
}