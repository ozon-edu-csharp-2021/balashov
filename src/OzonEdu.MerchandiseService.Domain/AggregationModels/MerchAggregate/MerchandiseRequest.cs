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

            Status = new MerchRequestStatus(MerchRequestStatusType.Draft, data);
        }

        public MerchandiseRequest(int hrManagerId, PhoneNumber hrManagerContactPhone, MerchPack requestedMerchPack, MerchRequestStatusType statusType, Date data)
        {
            HRManagerId = hrManagerId;
            HRManagerContactPhone = hrManagerContactPhone;

            RequestedMerchPack = requestedMerchPack;

            Status = new MerchRequestStatus(statusType, data);
        }

        public MerchandiseRequest AddEmployeeInfoFromDB(int? employeeId, PhoneNumber employeeContactPhone, Size size)
        {
            if (employeeId is null)
                return this;

            EmployeeId = (int)employeeId;
            EmployeeContactPhone = employeeContactPhone;
            Size = size;
            return this;
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
            if (!Status.Status.Equals(MerchRequestStatusType.Created))
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.Assigned, date);
            return true;
        }

        public bool SetInProgress(Date date)
        {
            if (!Status.Status.Equals(MerchRequestStatusType.Assigned))
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.InProgress, date);
            return true;
        }

        public bool SetDone(Date date)
        {
            if (!Status.Status.Equals(MerchRequestStatusType.InProgress))
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.Done, date);
            return true;
        }

        public MerchandiseRequest SetId(int id)
        {
            Id = id;
            return this;
        }
    }
}
