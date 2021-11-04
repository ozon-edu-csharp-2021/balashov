using System;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate
{
    public class MerchandizeRequest : Entity
    {
        public MerchRequestStatus Status { get; private set; }

        public long HRManagerId { get; }

        public PhoneNumber HRManagerContactPhone { get; }

        public long EmployeeId { get; private set; }

        public PhoneNumber EmployeeContactPhone { get; private set; }

        public MerchPack RequestedMerchPack { get; private set; }

        public Size Size { get; private set; }

        public MerchandizeRequest(int hrManagerId, PhoneNumber hrManagerContactPhone, MerchPack requestedMerchPack, DateTime data)
        {
            HRManagerId = hrManagerId;
            HRManagerContactPhone = hrManagerContactPhone;

            RequestedMerchPack = requestedMerchPack;

            Status =new MerchRequestStatus(MerchRequestStatusType.Draft, data);
        }

        public bool AddEmployeeInfo(int employeeId, PhoneNumber employeeContactPhone, Size size, DateTime data)
        {
            EmployeeId = employeeId;
            EmployeeContactPhone = employeeContactPhone;
            Size = size;
            
            Status = new MerchRequestStatus(MerchRequestStatusType.Created, data);
            return true;
        }

        public bool SetAssigned(DateTime data)
        {
            if (Status.Status != MerchRequestStatusType.Created)
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.Assigned, data);
            return true;
        }

        public bool SetInProgress(DateTime data)
        {
            if (Status.Status != MerchRequestStatusType.Assigned)
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.InProgress, data);
            return true;
        }

        public bool SetDone(DateTime data)
        {
            if (Status.Status != MerchRequestStatusType.InProgress)
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.Done, data);
            return true;
        }
    }
}
