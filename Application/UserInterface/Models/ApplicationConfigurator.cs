using System;
using TimetableApplication;
using TimetableDomain;
using Ninject;

namespace UserInterface.Models
{
    public static class ApplicationConfigurator
    {
        public static Configurator Configurator { get; }
        public static Type FilterType { get; }
        
        static ApplicationConfigurator()
        {
            var container = new KernelConfiguration();
            container.Bind<IInputParser>().To<XlsxInputParser>();
            container.Bind<ITimetableMaker>().To<GeneticAlgorithm>();
            container.Bind<OutputFormatter>().To<XlsxOutputFormatter>();
            var readonlyKernel = container.BuildReadonlyKernel();
            FilterType = readonlyKernel.Get<Filter>().GetType();
            Configurator = readonlyKernel.Get<Configurator>();
        }
    }
}