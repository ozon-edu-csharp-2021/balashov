﻿using System;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.Models
{
    public class IssuedMerchInfoModel
    {
        public long Id { get; }

        public long PersonId { get; }
        public DateTime IssueDate { get; }
        
        public List<long> MerchItems { get; }
        
        public IssuedMerchInfoModel(long itemId, long personId, DateTime issueDate, List<long> merchItems)
        {
            Id = itemId;
            PersonId = personId;
            IssueDate = issueDate;
            MerchItems = merchItems;
        }
    }
}