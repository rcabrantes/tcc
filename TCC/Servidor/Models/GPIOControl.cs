using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Servidor.Models
{
    public class GPIOControl
    {
        public List<GPIOPort> Ports { get; private set; } = new List<GPIOPort>();

        public GPIOControl()
        {
            for (int i = 1; i <= 4; i++)
            {
                Ports.Add(new GPIOPort { PortStatus = true });
            }
        }
    }
}