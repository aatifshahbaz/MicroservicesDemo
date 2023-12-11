using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Setting
{
    public class RabbitMQSettings
    {
        public string Host { get; init; }
        public ushort? Port { get; init; }
        public string UserName { get; init; }
        public string Password { get; init; }
        public string? VHost { get; init; }
    }
}
