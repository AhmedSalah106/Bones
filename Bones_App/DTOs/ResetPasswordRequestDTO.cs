﻿namespace Bones_App.DTOs
{
    public class ResetPasswordRequestDTO
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }
}
