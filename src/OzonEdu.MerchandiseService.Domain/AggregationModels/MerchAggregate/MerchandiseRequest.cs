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

        public Email EmployeeEmail { get; private set; }

        public MerchPack RequestedMerchPack { get; private set; }

        public Size Size { get; private set; }

        public MerchandiseRequest(long hrManagerId, MerchPack requestedMerchPack, Date data)
        {
            HRManagerId = hrManagerId;
            
            RequestedMerchPack = requestedMerchPack;

            Status = new MerchRequestStatus(MerchRequestStatusType.Draft, data);
        }

        public MerchandiseRequest(int hrManagerId, MerchPack requestedMerchPack, MerchRequestStatusType statusType, Date data)
        {
            HRManagerId = hrManagerId;

            RequestedMerchPack = requestedMerchPack;

            Status = new MerchRequestStatus(statusType, data);
        }

        public MerchandiseRequest AddEmployeeInfoFromDB(Email employeeEmail, Size size)
        {
            if (employeeEmail is null)
                return this;

            EmployeeEmail = employeeEmail;
            Size = size;
            return this;
        }

        public MerchandiseRequest AddEmployeeInfo(Email employeeEmail, Size size)
        {
            EmployeeEmail = employeeEmail ?? throw new Exception("Некорректный id сотрудника! Невозможно добавить данные в заявку.");
            Size = size;
            return this;
        }
        
        public MerchandiseRequest SetManagerId(long managerId)
        {
            if (managerId <= 0)
                throw new Exception("Некорректный id HR-менеджера! Невозможно добавить данные в заявку.");
            HRManagerId = managerId;
            return this;
        }

        #region Управление статусом заявки 
        public bool SetCreated(Date date)
        {
            if (!Status.Status.Equals(MerchRequestStatusType.Draft))
                return false;

            if (EmployeeEmail is null)
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.Created, date);
            
            AddDomainEvent(new MerchRequestCreatedDomainEvent(this));
            return true;
        }

        public bool SetAssigned(Date date)
        {
            if (!Status.Status.Equals(MerchRequestStatusType.Created))
                return false;

            if (!UniversalCheck())
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.Assigned, date);

            AddDomainEvent(new MerchRequestAssignedDomainEvent(this));
            return true;
        }

        public bool SetReserved(Date date)
        {
            if (!Status.Status.Equals(MerchRequestStatusType.Assigned))
                return false;

            if (!UniversalCheck())
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.Reserved, date);

            AddDomainEvent(new MerchReservedOnStockDomainEvent(this));
            return true;
        }

        public bool SetDone(Date date)
        {
            if (!Status.Status.Equals(MerchRequestStatusType.Reserved))
                return false;

            if (!UniversalCheck())
                return false;

            Status = new MerchRequestStatus(MerchRequestStatusType.Done, date);

            AddDomainEvent(new MerchRequestDoneDomainEvent(this));
            return true;
        }

        private bool UniversalCheck()
        {
            if (HRManagerId <= 0)
                return false;

            if (EmployeeEmail is null)
                return false;

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
