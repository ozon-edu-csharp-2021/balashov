using System;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.Models
{
    public class IssuingMerchResponseDto
    {
        public long PersonId { get; }
        
        public DateTime IssueDate { get; }
        
        public List<long> MerchItems { get; }
        
        public IssuingMerchResponseDto(long personId, DateTime issueDate, List<long> merchItems)
        {
            PersonId = personId;
            IssueDate = issueDate;
            MerchItems = merchItems;
        }
        
        public IssuingMerchResponseDto(IssuingMerchModel issuingMerch)
        {
            PersonId = issuingMerch.PersonId;
            IssueDate = issuingMerch.IssueDate;
            MerchItems = issuingMerch.MerchItems;
        }
    }
}