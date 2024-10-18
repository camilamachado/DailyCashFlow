### ADR 9: Recuperação de Falhas com RabbitMQ e NServiceBus

- **Status**: Decisão Tomada, Implementação realizada
  
- **Contexto**: Sistema utiliza mensageria assíncrona para consolidar balanços diários. A comunicação entre microsserviços precisa ser resiliente a falhas temporárias e falhas de rede.
  
- **Decisão**:  NServiceBus foi configurado para gerenciar o processo de recuperação de mensagens com falha. Foi implementada uma política de recuperação com tentativas imediatas e tentativas com atraso.

- **Consequências**:
  - Em caso de falha temporária, o sistema tenta reprocessar a mensagem imediatamente até o limite configurado.
  - Se todas as tentativas imediatas falharem, o sistema aplicará tentativas com atraso, espaçando-as por intervalos definidos.
  - A resiliência do sistema aumenta significativamente, garantindo que falhas transitórias não interrompam o processamento das transações.
  - O uso do NServiceBus para recoverability reduz a necessidade de intervenções manuais e garante a entrega final das mensagens, minimizando o risco de perda de dados.
