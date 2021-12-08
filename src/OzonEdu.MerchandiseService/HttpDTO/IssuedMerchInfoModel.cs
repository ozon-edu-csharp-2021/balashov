using System;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.Models
{
    public class IssuedMerchInfoModel
    {
        public long Id { get; }

        public long EmployeeId { get; }
        public DateTime IssueDate { get; }
        
        public List<long> MerchItems { get; }
        
        public IssuedMerchInfoModel(long itemId, long employeeId, DateTime issueDate, List<long> merchItems)
        {
            Id = itemId;
            EmployeeId = employeeId;
            IssueDate = issueDate;
            MerchItems = merchItems;
        }
    }
}