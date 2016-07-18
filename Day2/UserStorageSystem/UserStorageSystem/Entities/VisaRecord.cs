using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageSystem.Entities
{
    [Serializable()]
    public struct VisaRecord
    {
        public string CountryName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
