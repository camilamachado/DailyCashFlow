### ADR 3: Uso do RabbitMQ com NServiceBus para Mensageria

- **Status**: Decisão Tomada, Implementação realizada

- **Contexto**: Os serviços precisam se comunicar de forma assíncrona para evitar bloqueios e permitir a recuperação de mensagens em caso de falhas temporárias.

- **Decisão**: RabbitMQ foi escolhido como broker de mensagens, juntamente com o NServiceBus para facilitar o gerenciamento de filas, reprocessamento e garantia de entrega.

- **Consequências**:
  - As mensagens de transações da DailyCashFlow.WebApi são processadas de forma assíncrona pelo DailyCashFlow.Processor.
  - O uso de NServiceBus permite retries automáticos em caso de falhas e oferece mecanismos de recoverability.
  