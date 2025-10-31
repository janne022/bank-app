using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility.UI.Invokables
{
    /// <summary>
    /// Implements IInvokable and takes in a method that has 2 parameters and does not return anything.
    /// </summary>
    /// <typeparam name="T1">Any type of parameter</typeparam>
    /// <typeparam name="T2">Any type of parameter</typeparam>
    /// <param name="action">A reference to the method to run using a Action as generic delegate</param>
    public class Invokable<T1,T2>(Action<T1, T2> action) : IInvokable
    {
        private readonly Action<T1, T2> _action = action;

        public void Invoke(params object[] args) {
            _action((T1)args[0], (T2)args[1]);
        }
    }
}
