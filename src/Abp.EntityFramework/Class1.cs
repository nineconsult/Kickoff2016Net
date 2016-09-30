using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Events.Bus.Handlers;

namespace Abp
{
    class Class1 : Abp.Domain.Entities.IMayHaveTenant, Abp.Domain.Entities.IMustHaveTenant
    {
        public int? TenantId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        int IMustHaveTenant.TenantId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }

    class Class2 : IEventHandler, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
