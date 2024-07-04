using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Server
{
    public interface IController
    {
        void Start(Account account);
        void Stop(Account account);
        void Start(List<Account> accounts);

        void Start();
        void Stop();

        void Stop(List<Account> accounts);
    }
}
