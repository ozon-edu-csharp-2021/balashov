using System;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate
{
    public class MerchandiseRequest : Entity
    {
        public MerchRequestStatus Status { get; private set; }

        public long HRManagerId { get; }

        public PhoneNumber HRManagerContactPhone { get; }

        public long EmployeeId { get; private set; }

        public PhoneNumber EmployeeContactPhone { get; private set; }

        public MerchPack RequestedMerchPack { get; private set; }

        public Size Size { get; private set; }

        public MerchandiseRequest(int hrManagerId, PhoneNumber hrManagerContactPhone, MerchPack requestedMerchPack, Date data)
        {
            HRManagerId = hrManagerId;
            HRManagerContactPhone = hrManagerContactPhone;

            RequestedMerchPack = requestedMerchPack;

            Status =new MerchRequestStatus(MerchRequestStatusType.Draft, data);
        }

        public bool AddEmployeeInfo(int employeeId, PhoneNumber employeeContactPhone, Size size, Date date)
        {
            EmployeeId = employeeId;
            EmployeeContactPhone = employeeContactPhone;
            Size = size;
            
            Status = new MerchRequestStatus(MerchRequestStatusType.Created, date);
            return true;
        }

        public bool SetAssigned(Date date)
        {
            if (Status.Status != MerchRequestStatusType.Created)
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.Assigned, date);
            return true;
        }

        public bool SetInProgress(Date date)
        {
            if (Status.Status != MerchRequestStatusType.Assigned)
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.InProgress, date);
            return true;
        }

        public bool SetDone(Date date)
        {
            if (Status.Status != MerchRequestStatusType.InProgress)
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.Done, date);
            return true;
        }
    }
}
