﻿namespace Customer_And_Orders.BAL.Models
{
    public class CreateUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
    }
}
