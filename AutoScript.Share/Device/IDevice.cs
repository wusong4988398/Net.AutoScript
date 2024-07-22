using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoScript.Share
{
    public interface IDevice
    {
        int Hwnd { get; set; }
        string Title { get; set; }
        int Pid { get; set; }
        string Port { get; set; }
        string IP { get; set; }
        HubConnection Connection { get; set; }
    }
}