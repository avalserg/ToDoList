﻿namespace Users.Service.Dtos
{
    public class UpdateUserPasswordDto
    {
        public int Id { get; set; }
        public string PasswordHash { get; set; } = default!;
    }
}
