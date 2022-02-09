using System;
using System.Collections.Generic;
using System.Text;
using PermissionsN5.Core.Interfaces;
using PermissionsN5.Core.DTO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Nest;
using Confluent.Kafka;

namespace PermissionsN5.Infraestructure.External
{
    public class Utils : IUtils
    {
        private readonly ILogger<Utils> _logger;
        private readonly IElasticClient _clientElastic;
        private readonly ProducerConfig _producerConfig;

        public Utils(ILogger<Utils> logger, IElasticClient clientElastic, ProducerConfig producerConfig)
        {
            _logger = logger;
            _clientElastic = clientElastic;
            _producerConfig = producerConfig;
        }

        public async Task SendToElastic(PermissionsDTO elasticObj)
        {
            _logger.LogInformation("[ELASTICSEARCH] => OBJETO PERMISSIONS = {0}", JsonSerializer.Serialize(elasticObj));

            var existsResponse = await _clientElastic.DocumentExistsAsync<PermissionsDTO>(elasticObj);

            // verifica si el documento existe, si existe lo actualiza, si no lo inserta
            if (existsResponse.IsValid && existsResponse.Exists)
            {
                _logger.LogInformation("[ELASTICSEARCH] => OBJETO EXISTENTE, SERA ACTUALIZADO");
                var updateResponse = await _clientElastic.UpdateAsync<PermissionsDTO>(elasticObj, u => u.Doc(elasticObj));

                if (!updateResponse.IsValid)
                    _logger.LogInformation("[ELASTICSEARCH] => OBJETO NO PUDO SER ACTUALIZADO");
            }
            else 
            {
                _logger.LogInformation("[ELASTICSEARCH] => OBJETO NO EXISTENTE, SERA INSERTADO");
                var insertResponse = await _clientElastic.IndexDocumentAsync(elasticObj);

                if (!insertResponse.IsValid)
                    _logger.LogInformation("[ELASTICSEARCH] => OBJETO NO PUDO SER INSERTADO");
            }                            
        }

        public async Task SendToKafka(KafkaDTO kafkaObj)
        {
            _logger.LogInformation("[KAFKA] => ID = {0} - NOMBRE OPERACION = {1}", kafkaObj.id, kafkaObj.nameOperation);
            string jsonKafka = JsonSerializer.Serialize(kafkaObj);
            using(var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {
                await producer.ProduceAsync("N5PERMISSIONS", new Message<Null, string> { Value = jsonKafka });
                producer.Flush(TimeSpan.FromSeconds(2));
            }
        }
    }
}
