using System;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.Models
{
    public class IssuingMerchRequestDto
    {
        public long Id { get; }

        public long PersonId { get; }
        public DateTime IssueDate { get; }
        
        public List<long> MerchItemsIds { get; }
        
        public IssuingMerchRequestDto(long itemId, long personId, DateTime issueDate, List<long> merchItemsIds)
        {
            Id = itemId;
            PersonId = personId;
            IssueDate = issueDate;
            MerchItemsIds = merchItemsIds;
        }
    }
}