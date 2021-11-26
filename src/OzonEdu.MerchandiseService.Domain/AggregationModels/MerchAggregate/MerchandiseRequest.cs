using System;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Events;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate
{
    public class MerchandiseRequest : Entity
    {
        public MerchRequestStatus Status { get; private set; }

        public long HRManagerId { get; private set; }

        public PhoneNumber HRManagerContactPhone { get; private set; }

        public long EmployeeId { get; private set; }

        public PhoneNumber EmployeeContactPhone { get; private set; }

        public MerchPack RequestedMerchPack { get; private set; }

        public Size Size { get; private set; }

        public MerchandiseRequest(long hrManagerId, MerchPack requestedMerchPack, Date data)
        {
            HRManagerId = hrManagerId;
            
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

        public MerchandiseRequest AddEmployeeInfo(long employeeId, PhoneNumber employeeContactPhone, Size size)
        {
            if (employeeId <= 0)
                throw new Exception("Некорректный id сотрудника! Невозможно добавить данные в заявку.");
            
            EmployeeId = employeeId;
            EmployeeContactPhone = employeeContactPhone;
            Size = size;
            return this;
        }
        
        public MerchandiseRequest AddManagerInfo(long managerId, PhoneNumber managerContactPhone)
        {
            if (managerId <= 0)
                throw new Exception("Некорректный id HR-менеджера! Невозможно добавить данные в заявку.");
            HRManagerId = managerId;
            HRManagerContactPhone = managerContactPhone;
            return this;
        }

        #region Управление статусом заявки 
        public bool SetCreated(Date date)
        {
            if (!Status.Status.Equals(MerchRequestStatusType.Draft))
                return false;

            if (EmployeeId <= 0)
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.Created, date);

            AddDomainEvent(new MerchRequestCreatedDomainEvent(this));
            return true;
        }

        public bool SetAssigned(Date date)
        {
            if (!Status.Status.Equals(MerchRequestStatusType.Created))
                return false;

            if (HRManagerId <= 0)
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.Assigned, date);

            AddDomainEvent(new MerchRequestAssignedDomainEvent(this));
            return true;
        }

        public bool SetReserved(Date date)
        {
            if (!Status.Status.Equals(MerchRequestStatusType.Assigned))
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.Reserved, date);

            AddDomainEvent(new MerchReservedOnStockDomainEvent(this));
            return true;
        }

        public bool SetDone(Date date)
        {
            if (!Status.Status.Equals(MerchRequestStatusType.Reserved))
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.Done, date);

            AddDomainEvent(new MerchRequestDoneDomainEvent(this));
            return true;
        }
        #endregion

        public MerchandiseRequest SetId(long id)
        {
            Id = id;
            return this;
        }
    }
}
