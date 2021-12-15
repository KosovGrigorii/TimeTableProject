using TimetableApplication;
using TimetableDomain;
using Ninject;

namespace UserInterface.Models
{
    public static class ApplicationConfigurator
    {
        public static Configurator AppConfigurator { get; }
        
        static ApplicationConfigurator()
        {
            var appContainer = new KernelConfiguration();
            appContainer.Bind<IInputParser>().To<XlsxInputParser>();
            appContainer.Bind<ITimetableMaker>().To<GeneticAlgorithm>();
            appContainer.Bind<OutputFormatter>().To<XlsxOutputFormatter>();
            var readonlyAppKernel = appContainer.BuildReadonlyKernel();
            AppConfigurator = readonlyAppKernel.Get<Configurator>();
        }
    }
}