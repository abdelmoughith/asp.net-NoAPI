using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication_Client.Models;

[Table("Reservation")]
public class Reservation
{
    [Key]
    public int Id { get; set; }

    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public string Etat { get; set; }
    public int ClientId { get; set; }
    [ForeignKey("ClientId")]
    public Client Client { get; set; }
    public int ChambreId { get; set; }
    [ForeignKey("ChambreId")]
    public Chambre Chambre { get; set; }

    public double Facture
    {
        get
        {
            return Chambre != null ? (DateFin - DateDebut).Days * Chambre.PrixParNuit : 0;
        }
    }
}
