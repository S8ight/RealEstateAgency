﻿using System.Text.Json.Serialization;

namespace AuthorizationService.BLL.DTO.Response;

public class AuthenticateResponse
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime DateOfBirthd { get; set; }
    public DateTime Created { get; set; }
    public byte[]? Photo { get; set; }
    public bool IsVerified { get; set; }
    
    public string JwtToken { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }
}