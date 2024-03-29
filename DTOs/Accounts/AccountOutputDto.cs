﻿namespace API.DTOs.Accounts
{
    public class AccountOutputDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public string Token { get; set; }
        public IList<string> Roles { get; set; }
    }
}