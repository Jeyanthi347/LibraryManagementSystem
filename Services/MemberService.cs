using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Services
{
    public class MemberService
    {
        private readonly MemberRepository memberRepository;

        public MemberService()
        {
            memberRepository = new MemberRepository();
        }

        // Get all members
        public List<Member> GetAllMembers()
        {
            return memberRepository.GetAll();
        }

        // Get a member by ID
        public Member? GetMemberById(int memberId)
        {
            if (memberId <= 0)
            {
                throw new ArgumentException(
                    "Member ID must be greater than zero.");
            }

            return memberRepository.GetById(memberId);
        }

        // Add a new member
        public int AddMember(Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            ValidateMember(member);

            if (member.MembershipExpiryDate <
                member.MembershipStartDate)
            {
                throw new ArgumentException(
                    "Membership expiry date cannot be before the start date.");
            }

            member.IsActive = true;

            return memberRepository.Add(member);
        }

        // Update an existing member
        public bool UpdateMember(Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            if (member.MemberId <= 0)
            {
                throw new ArgumentException(
                    "Member ID must be greater than zero.");
            }

            ValidateMember(member);

            if (member.MembershipExpiryDate <
                member.MembershipStartDate)
            {
                throw new ArgumentException(
                    "Membership expiry date cannot be before the start date.");
            }

            if (memberRepository.GetById(member.MemberId) == null)
            {
                throw new InvalidOperationException(
                    "Member not found.");
            }

            return memberRepository.Update(member);
        }

        // Deactivate a member
        public bool DeactivateMember(int memberId)
        {
            if (memberId <= 0)
            {
                throw new ArgumentException(
                    "Member ID must be greater than zero.");
            }

            if (memberRepository.GetById(memberId) == null)
            {
                throw new InvalidOperationException(
                    "Member not found.");
            }

            return memberRepository.Deactivate(memberId);
        }

        // Validate member details
        private void ValidateMember(Member member)
        {
            member.FullName = member.FullName.Trim();

            if (string.IsNullOrWhiteSpace(member.FullName))
            {
                throw new ArgumentException(
                    "Member name cannot be empty.");
            }

            if (member.FullName.Length > 100)
            {
                throw new ArgumentException(
                    "Member name cannot exceed 100 characters.");
            }

            member.Email = member.Email.Trim();

            if (string.IsNullOrWhiteSpace(member.Email))
            {
                throw new ArgumentException(
                    "Email cannot be empty.");
            }

            if (member.Email.Length > 150 ||
                !member.Email.Contains('@') ||
                !member.Email.Contains('.'))
            {
                throw new ArgumentException(
                    "Please enter a valid email address.");
            }

            member.Phone = member.Phone.Trim();

            if (member.Phone.Length != 10)
            {
                throw new ArgumentException(
                    "Phone number must contain exactly 10 digits.");
            }

            foreach (char digit in member.Phone)
            {
                if (!char.IsDigit(digit))
                {
                    throw new ArgumentException(
                        "Phone number must contain only digits.");
                }
            }
        }
    }
}