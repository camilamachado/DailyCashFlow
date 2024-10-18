### ADR 1: Escolha de Microsserviços como Arquitetura

- **Status**: Decisão Tomada, Implementação realizada
  
- **Contexto**: O projeto precisa lidar com diferentes responsabilidades, como autenticação, transações financeiras e processamento assíncrono de dados.
  
- **Decisão**: Optou-se pela arquitetura de microsserviços para dividir as responsabilidades em serviços isolados: DailyCashFlow.Auth, DailyCashFlow.WebApi e DailyCashFlow.Processor.

- **Consequências**:
  - Cada serviço pode ser escalado de forma independente, o que facilita a manutenção e melhora a disponibilidade.
  - O sistema se torna mais resiliente, pois falhas em um serviço não afetam os outros.
  - A comunicação entre os microsserviços é gerida de forma assíncrona via RabbitMQ.