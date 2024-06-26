﻿using API.Entities.Movies;
using Microsoft.AspNetCore.Identity;

namespace API.Entities.Users
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public bool IsPrivate { get; set; } = false;
        public DateTime DateOfBirth { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public ICollection<AppUserRole> UserRoles { get; set; }
        public Avatar Avatar { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Watchlist> Watchlists { get; set; }

        // Điểm đóng góp
        public int ContributionPoints { get; set; }
        public ICollection<PointTransaction> PointTransactions { get; set; }
        public ICollection<Voucher> Vouchers { get; set; }
    }
}