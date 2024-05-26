﻿namespace REA.AdvertSystem.DTOs.Request;

public class UserRequest
{
    public string Id { get; set; }
    public string FirstName { get; set; }
        
    public string LastName { get; set; }
        
    public string? Patronymic { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
}