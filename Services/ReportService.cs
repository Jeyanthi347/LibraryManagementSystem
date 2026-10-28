using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using System.Collections.Generic;

namespace LibraryManagementSystem.Services
{
    public class ReportService
    {
        private readonly ReportRepository reportRepository;

        public ReportService()
        {
            reportRepository = new ReportRepository();
        }

        // Get the book inventory report
        public List<BookViewRow> GetInventoryReport()
        {
            return reportRepository.GetInventoryReport();
        }

        // Get the complete borrow history report
        public List<BorrowViewRow> GetBorrowHistoryReport()
        {
            return reportRepository.GetBorrowHistoryReport();
        }

        // Get the overdue books report
        public List<BorrowViewRow> GetOverdueReport()
        {
            return reportRepository.GetOverdueReport();
        }

        // Get the fine report
        public List<FineViewRow> GetFineReport()
        {
            return reportRepository.GetFineReport();
        }
    }
}