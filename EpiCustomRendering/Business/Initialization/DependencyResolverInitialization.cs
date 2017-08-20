using System;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EpiCustomRendering.Business.Rendering;
using EpiCustomRendering.Business.Rendering.Conventions;
using EpiCustomRendering.Helpers;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.TypeRules;

namespace EpiCustomRendering.Business.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class DependencyResolverInitialization : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Container.Configure(ConfigureContainer);

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(context.Container));
        }

        private static void ConfigureContainer(ConfigurationExpression container)
        {
            //Swap out the default ContentRenderer for our custom
            container.For<IContentRenderer>().Use<ErrorHandlingContentRenderer>();
            container.For<ContentAreaRenderer>().Use<ConventionalContentAreaRenderer>();
            container.ForSingletonOf<IConventionApplier>().Use<ConventionApplier>();
            container.Scan(scan =>
            {
                scan.AssemblyContainingType<ITagBuilderConvention>();
                scan.AddAllTypesOf<ITagBuilderConvention>();
                scan.Convention<SingletonConvention<ITagBuilderConvention>>();
            });

            //Implementations for custom interfaces can be registered here.
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }
    }

    public class SingletonConvention<TPluginFamily> : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract || !type.CanBeCreated() || !type.AllInterfaces().Contains(typeof(TPluginFamily)))
                return;

            registry.ForSingletonOf(typeof(TPluginFamily)).Use(type);
        }
    }
}
