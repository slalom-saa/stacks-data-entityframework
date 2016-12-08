using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slalom.Stacks.Data.EntityFramework;

namespace Slalom.Stacks.Configuration
{
    public static class ApplicationContainerExtensions
    {
        public static ApplicationContainer UseEntityFrameworkSearch(this ApplicationContainer instance)
        {
            instance.RegisterModule(new EntityFrameworkSearchModule());
            return instance;
        }

        public static ApplicationContainer UseEntityFrameworkSearch(this ApplicationContainer instance, Action<EntityFrameworkSearchOptions> options)
        {
            instance.RegisterModule(new EntityFrameworkSearchModule(options));
            return instance;
        }
    }
}
