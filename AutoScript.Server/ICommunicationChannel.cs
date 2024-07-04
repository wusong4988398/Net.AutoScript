using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Server
{
    public interface ICommunicationChannel
    {
        void Notify(string action);
        string GetCurrentAction();
    }
}
