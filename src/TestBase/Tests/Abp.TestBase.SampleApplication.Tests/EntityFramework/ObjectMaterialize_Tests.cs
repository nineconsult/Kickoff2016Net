﻿using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.TestBase.SampleApplication.Crm;
using Abp.Timing;

namespace Abp.TestBase.SampleApplication.Tests.EntityFramework
{
    public class ObjectMaterialize_Tests : SampleApplicationTestBase
    {
        private readonly IRepository<Company> _companyRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ObjectMaterialize_Tests()
        {
            _companyRepository = Resolve<IRepository<Company>>();
            _unitOfWorkManager = Resolve<IUnitOfWorkManager>();

            if (RandomHelper.GetRandomOf(1, 2) == 1)
            {
                Clock.Provider = new LocalClockProvider();
            }
            else
            {
                Clock.Provider = new UtcClockProvider();
            }
        }

        //Note: The code below is cancelled since Effort does not work well with ObjectMaterialized event
        //[Fact] 
        public void DateTime_Kind_Propert_Should_Be_Normalized_On_Ef_ObjectMaterialition()
        {
            //using (var uow = _unitOfWorkManager.Begin())
            //{
            //    var companies = _companyRepository.GetAll().Include(c => c.Branches).ToList();

            //    foreach (var company in companies)
            //    {
            //        company.CreationTime.Kind.ShouldBe(Clock.Kind);


            //        //company.BillingAddress.CreationTime.Kind.ShouldBe(Clock.Kind);
            //        //company.BillingAddress.LastModifier.ModificationTime.Value.Kind.ShouldBe(Clock.Kind);

            //        //company.ShippingAddress.CreationTime.Kind.ShouldBe(Clock.Kind);
            //        //company.ShippingAddress.LastModifier.ModificationTime.Value.Kind.ShouldBe(Clock.Kind);

            //        //company.Branches.ForEach(branch =>
            //        //{
            //        //    branch.CreationTime.Kind.ShouldBe(Clock.Kind);
            //        //});
            //    }

            //    uow.Complete();
            //}
        }
    }
}