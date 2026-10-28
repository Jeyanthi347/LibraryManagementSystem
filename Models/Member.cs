using System;

namespace LibraryManagementSystem.Models
{
    public class Member
    {
        public int MemberId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string? Phone { get; set; }

        public DateOnly MembershipStartDate { get; set; }

        public DateOnly MembershipExpiryDate { get; set; }

        public bool IsActive { get; set; }

        // Default constructor
        public Member()
        {
            FullName = string.Empty;
            Email = string.Empty;
            Phone = null;
            MembershipStartDate = DateOnly.FromDateTime(DateTime.Today);
            MembershipExpiryDate = DateOnly.FromDateTime(DateTime.Today.AddYears(1));
            IsActive = true;
        }

        // Parameterized constructor
        public Member(
            int memberId,
            string fullName,
            string email,
            string? phone,
            DateOnly membershipStartDate,
            DateOnly membershipExpiryDate,
            bool isActive)
        {
            MemberId = memberId;
            FullName = fullName;
            Email = email;
            Phone = phone;
            MembershipStartDate = membershipStartDate;
            MembershipExpiryDate = membershipExpiryDate;
            IsActive = isActive;
        }
    }
}