using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AForge.FrameProcessing.FrameProviders
{
    public static class FrameProviderFactory
    {

        public static IEnumerable<IFrameProvider> GetAllFrameProviders()
        {
            var types = Assembly.GetExecutingAssembly()
                .GetTypes();
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x != typeof(IFrameProvider) && typeof(IFrameProvider).IsAssignableFrom(x))
                .Select(x => (IFrameProvider)Activator.CreateInstance(x));
        }

    }
}
