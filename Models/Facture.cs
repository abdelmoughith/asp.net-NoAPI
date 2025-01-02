using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication_Client.Models;

[Table("Facture")]
public class Facture
{
    [Key]
    public int Id { get; set; }

    public double MontantTotal { get; set; }
    public DateTime DatePaiement { get; set; }
    public string Etat { get; set; }
    public int ReservationId { get; set; }
    [ForeignKey("ReservationId")]
    public Reservation Reservation { get; set; }
}
