using EyeTracking.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Core.TransformProcessors
{
    public interface ITransformProcessor
    {
        Complex[][] Apply(Complex[][] input);
    }
}
