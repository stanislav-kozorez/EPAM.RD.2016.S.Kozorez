using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageSystem.Entities
{
    [DataContract]
    [Serializable()]
    public struct VisaRecord
    {
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public DateTime From { get; set; }
        [DataMember]
        public DateTime To { get; set; }
    }
}
