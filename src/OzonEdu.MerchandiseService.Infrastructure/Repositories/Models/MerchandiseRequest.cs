using System;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchandiseService.Infrastructure.Repositories.Models
{
    class MerchandiseRequest
    {
        public int Id { get; set; }
        
        public int MerchRequestStatusId { get; set; }
        public int HrManagerId { get; set; }
        public int? EmployeeId { get; set; }
        public int? PackTitleId { get; set; }
        public int? ClothingSizeId { get; set; }
        public DateTime Date { get; set; }
    }
}