using DryIoc;

namespace Bootstrapper
{
    public interface IModule
    {
        //Register Module
        void Load(IRegistrator builder);
    }
}
