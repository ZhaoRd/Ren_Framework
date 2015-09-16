using System;
using System.Collections.Generic;
using System.Linq;

namespace Skymate
{
    using Skymate.TypeFinders;

    public class ApplicationStartup
    {
        public void Initialize()
        {
            var typeFinder = this.CreateTypeFinder();

            var registrarTypes = typeFinder.FindClassesOfType<IApplicationStartup>();
            var registrarInstances = new List<IApplicationStartup>();
            foreach (var type in registrarTypes)
            {
                registrarInstances.Add((IApplicationStartup)Activator.CreateInstance(type));
            }

            // sort
            registrarInstances = registrarInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            foreach (var startup in registrarInstances)
            {
                startup.Startup();
            }
        }

        protected virtual ITypeFinder CreateTypeFinder()
        {
            return new WebAppTypeFinder();
        }
    }
}
