using System.Threading.Tasks;
using PermissionsN5.Core.DTO;

namespace PermissionsN5.Core.Interfaces
{
    public interface IUtils
    {
        Task SendToKafka(KafkaDTO kafkaObj);
        Task SendToElastic(PermissionsDTO elasticObj);
    }
}
