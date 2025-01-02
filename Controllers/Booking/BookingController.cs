using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers.Booking;

[Route("/booking")]
public class BookingController : Controller
{
    [HttpGet]
    public IActionResult Booking()
    {
        return View();
    }
}