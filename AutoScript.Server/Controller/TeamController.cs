using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoScript.Server
{
    public class TeamController : ControllerBase
    {
        public TeamController(HubConnection connection) : base(connection)
        {

        }

        public override void Start(Account account)
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Start(List<Account> accounts)
        {
            throw new NotImplementedException();
        }

        public override void Stop(Account account)
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
        public override void Stop(List<Account> accounts)
        {
            throw new NotImplementedException();
        }
    }
}
