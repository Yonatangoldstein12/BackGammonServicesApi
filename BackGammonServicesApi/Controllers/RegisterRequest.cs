using System.ComponentModel.DataAnnotations;

namespace BackGammonServicesApi.Controllers;

public class RegisterRequest
{
    [Required]
    public string UserName { get; set; }= string.Empty;
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }=string.Empty;
}
