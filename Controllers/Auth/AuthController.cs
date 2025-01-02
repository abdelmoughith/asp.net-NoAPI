using System.ComponentModel.DataAnnotations;
using Authentication_Client.Data;
using Authentication_Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Client.Controllers.Auth;

//[ApiController]
//[Route("/auth")]
[Route("/")]
public class AuthController : Controller
{
    //private readonly JwtTokenService _jwtTokenService;

    private readonly JwtTokenService _jwtTokenService;
    private readonly AppDbContext _context;
    private readonly ILogger<AuthController> _logger;
    
    public AuthController(JwtTokenService jwtTokenService, AppDbContext context, ILogger<AuthController> logger)
    {
        _jwtTokenService = jwtTokenService;
        _context = context;
        _logger = logger;
    }
    public IActionResult Index()
    {
        // Retrieve the token from the cookies
        var token = Request.Cookies["token"];
        if (string.IsNullOrEmpty(token))
        {
            TempData["Error"] = "You need to log in.";
            TempData["fullname"] = null;
            return View();
        }

        // Extract claims from the token
        var claimsPrincipal = _jwtTokenService.ExtractClaimsFromToken(token);
        if (claimsPrincipal == null)
        {
            TempData["Error"] = "Invalid token.";
            return View();
        }

        // Retrieve the "fullname" claim
        var fullNameClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "fullname");
        if (fullNameClaim != null)
        {
            TempData["fullname"] = fullNameClaim.Value;
        }

        return View();
    }

    
    [HttpGet("auth/login")]
    public IActionResult Login()
    {
        return View(); // Render the login form (Views/Auth/Login.cshtml)
    }
    /*
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] ClientLoginRequest loginRequest)
    {
        // Validate the user (replace this with actual database validation)
        //var client = ValidateClient(loginRequest.Email, loginRequest.Password);
        var client = _context.Clients.FirstOrDefault(c => c.Email == loginRequest.Email);
        
        if (client == null)
            return Unauthorized("Invalid email or password.");
        if (HashPasswordHelper.VerifyPassword(loginRequest.Password, client.Password))
        {
            var token = _jwtTokenService.GenerateToken(client);
            return Ok(new { Token = token });    
        }

        return Unauthorized("Invalid password.");

    }
    */
    
    
    [HttpPost("auth/login")]
    public async Task<IActionResult> Login(ClientLoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return View(loginRequest); // Return the form with validation errors
        }

        var client = _context.Clients.FirstOrDefault(c => c.Email == loginRequest.Email);

        if (client == null || !HashPasswordHelper.VerifyPassword(loginRequest.Password, client.Password))
        {
            TempData["Error"] = "Invalid email or password.";
            return View(loginRequest);
        }

        
        // Generate the JWT token
        
        var token = _jwtTokenService.GenerateToken(client);

        // Set the token as a cookie
        Response.Cookies.Append("token", token, new CookieOptions
        {
            HttpOnly = true, // Prevents JavaScript access for security
            Secure = true, // Ensures the cookie is only sent over HTTPS
            SameSite = SameSiteMode.Strict, // Adjust based on your cross-origin requirements
            Expires = DateTimeOffset.Now.AddDays(7) // Token expiration time
        });
        
        // On successful login, you can set a session or redirect
        TempData["SuccessLogin"] = "Login successful!";
        return RedirectToAction("Index", "Auth");
    }

    private Client ValidateClient(string email, string password)
    {
        // Simulate a database check
        if (email == "test@example.com" && password == "Password123")
        {
            return new Client { Id = 1, Email = email, Password = password };
        }
        return null;
    }

    [HttpGet("auth/register")]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost("auth/register")]
    public async Task<IActionResult> Register(ClientRegisterRequest request)
    {
        string errorMissingField = validate_input(request);
        if (!string.IsNullOrEmpty(errorMissingField))
        {
            TempData["Success"] = errorMissingField;
            return View(request);
        }
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["Success"] = string.Join(", ", errors);
            return View(request);
        }
        /*
        if (!ModelState.IsValid)
        {
            TempData["Success"] = "Not valid";
            return View(request); // Return the form with validation errors
        }
        */

        if (_context.Clients.Any(c => c.Email == request.Email))
        {
            ModelState.AddModelError("Email", "Email is already in use.");
            return View(request);
        }

        // Create new client
        var client = new Client
        {
            Email = request.Email,
            Password = HashPasswordHelper.HashPassword(request.Password),
            role = Role.CLIENT, // Default role
            Nom = request.Nom,
            Prenom = request.Prenom,
            Age = request.Age,
            CIN = request.CIN,
            Telephone = request.Telephone,
        };

        // Save to database
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Registration successful!";
        return RedirectToAction("Login");
    }


    
    [Authorize(Roles = "ADMIN")]
    [HttpGet("admin")]
    public IActionResult test1()
    {
        return Ok("You are ADMIN");
    }
    [Authorize(Roles = "CLIENT, ADMIN")]
    [HttpGet("client")]
    public IActionResult test2()
    {
        return Ok("You are CLIENT");
    }

    private string validate_input(ClientRegisterRequest request)
    {
        if (string.IsNullOrEmpty(request.Email))
        {
            return "Email cannot be null or empty.";
        }
        if (string.IsNullOrEmpty(request.Password))
        {
            return "Password cannot be null or empty.";
        }
        if (string.IsNullOrEmpty(request.Nom))
        {
            return "Nom cannot be null or empty.";
        }

        if (string.IsNullOrEmpty(request.Prenom))
        {
            return "Prenom cannot be null or empty.";
        }

        if (string.IsNullOrEmpty(request.Password))
        {
            return "Password cannot be null or empty.";
        }

        if (request.Age <= 0)
        {
            return "Age must be a positive number.";
        }

        if (string.IsNullOrEmpty(request.CIN))
        {
            return "CIN cannot be null or empty.";
        }

        if (string.IsNullOrEmpty(request.Email))
        {
            return "Email cannot be null or empty.";
        }

        if (string.IsNullOrEmpty(request.Telephone))
        {
            return "Telephone cannot be null or empty.";
        }

        return null;
    }

}

public class ClientLoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}

public class ClientRegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    public string Nom { get; set; }

    [Required]
    public string Prenom { get; set; }

    [Required]
    [Range(1, 120)]
    public int Age { get; set; }

    [Required]
    public string CIN { get; set; }

    [Required]
    [Phone]
    public string Telephone { get; set; }
}

