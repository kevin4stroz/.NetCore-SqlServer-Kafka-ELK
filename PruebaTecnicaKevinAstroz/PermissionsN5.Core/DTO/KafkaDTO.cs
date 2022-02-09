using System;
using System.Collections.Generic;
using System.Text;

namespace PermissionsN5.Core.DTO
{
    public class KafkaDTO
    {
        public KafkaDTO(string nOperation)
        {
            nameOperation = nOperation;
            id = Guid.NewGuid();
        }
        public string nameOperation { get; set; }
        public Guid id { get; set; }
    }
}
