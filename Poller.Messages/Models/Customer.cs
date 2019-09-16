using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poller.Messages.Models
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string CustomerName { get; set; }

        public string CustomerType { get; set; }

        public int Status { get; set; }

        public override string ToString()
        {
            return $"{Id}.{CustomerName}.{CustomerType}.{Status}";
        }
    }
}
