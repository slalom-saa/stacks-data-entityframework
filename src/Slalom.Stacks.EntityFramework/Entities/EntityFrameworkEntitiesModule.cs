using System.Data.Entity;
using System.Linq;
using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Slalom.Stacks.EntityFramework.Entities
{
    public class EntityFrameworkEntitiesModule : Module
    {
        private readonly Stack _stack;
        private readonly EntityFrameworkOptions _options;

        public EntityFrameworkEntitiesModule(Stack stack, EntityFrameworkOptions options)
        {
            _stack = stack;
            _options = options;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new EntityContext(_options)).AsImplementedInterfaces();
        }
    }
}