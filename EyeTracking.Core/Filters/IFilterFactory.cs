using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.Filters
{
    public class IFilterFactory
    {


        public static IEnumerable<IFilter> GetAllFilters()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => !x.IsInterface && typeof(IFilter).IsAssignableFrom(x))
                .Select(x => (IFilter)Activator.CreateInstance(x));
        }
    }
}
