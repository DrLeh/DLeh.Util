using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLeh.Util.Extensions
{
    public static class ExceptionExtensions
    {
        public static Exception GetInnerMostException(this Exception ex)
        {
            Exception inex = ex;
            while (inex.InnerException != null)
            {
                inex = inex.InnerException;
            }
            return inex;
        }
    }
}
