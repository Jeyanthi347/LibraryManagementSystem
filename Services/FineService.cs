using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Services
{
    public class FineService
    {
        private readonly FineRepository fineRepository;

        public FineService()
        {
            fineRepository = new FineRepository();
        }

        // Get all fine records
        public List<FineViewRow> GetAllFines()
        {
            return fineRepository.GetAll();
        }

        // Get a fine record by ID
        public Fine? GetFineById(int fineId)
        {
            if (fineId <= 0)
            {
                throw new ArgumentException(
                    "Fine ID must be greater than zero.");
            }

            return fineRepository.GetById(fineId);
        }

        // Pay a fine
        public bool PayFine(int fineId, decimal paymentAmount)
        {
            if (fineId <= 0)
            {
                throw new ArgumentException(
                    "Fine ID must be greater than zero.");
            }

            if (paymentAmount <= 0)
            {
                throw new ArgumentException(
                    "Payment amount must be greater than zero.");
            }

            Fine? fine = fineRepository.GetById(fineId);

            if (fine == null)
            {
                throw new InvalidOperationException(
                    "Fine record not found.");
            }

            if (fine.Status == "Paid")
            {
                throw new InvalidOperationException(
                    "This fine has already been paid.");
            }

            decimal remainingAmount =
                fine.FineAmount - fine.AmountPaid;

            if (paymentAmount > remainingAmount)
            {
                throw new ArgumentException(
                    "Payment cannot exceed the remaining fine amount.");
            }

            return fineRepository.PayFine(fineId, paymentAmount);
        }
    }
}