using System;
using System.Collections.Generic;

#nullable disable

namespace RhymeBinder.Models
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserRoles = new HashSet<AspNetUserRole>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
            TextGroups = new HashSet<TextGroup>();
            TextHeaderCreatedByNavigations = new HashSet<TextHeader>();
            TextHeaderLastModifiedByNavigations = new HashSet<TextHeader>();
            TextHeaderLastReadByNavigations = new HashSet<TextHeader>();
            TextRecords = new HashSet<TextRecord>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual ICollection<TextGroup> TextGroups { get; set; }
        public virtual ICollection<TextHeader> TextHeaderCreatedByNavigations { get; set; }
        public virtual ICollection<TextHeader> TextHeaderLastModifiedByNavigations { get; set; }
        public virtual ICollection<TextHeader> TextHeaderLastReadByNavigations { get; set; }
        public virtual ICollection<TextRecord> TextRecords { get; set; }
    }
}
