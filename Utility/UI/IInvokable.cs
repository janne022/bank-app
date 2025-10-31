using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.UI
{
    /// <summary>
    /// Interface that helps implement the Invoke method so that it can be used across multiple Invokable classes.
    /// </summary>
    public interface IInvokable
    {
        void Invoke(params object[] args);
    }
}
