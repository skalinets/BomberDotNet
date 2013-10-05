using Ninject;
using Ninject.Extensions.Conventions;

namespace Bomberman.Api
{
    public class Bootstrapper
    {
        public Game GetGame()
        {
            var kernel = new StandardKernel();
            kernel.Bind(x =>
                        x.FromThisAssembly()
                         .SelectAllClasses()
                         .BindDefaultInterface());
            return kernel.Get<Game>();
        }
    }
}