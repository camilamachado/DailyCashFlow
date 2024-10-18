### ADR 2: Uso do Kubernetes para Escalabilidade

- **Status**: Decisão Tomada, Implementação Pendente

- **Contexto**: A arquitetura precisa ser escalável para suportar picos de 50 requisições por segundo, além de garantir disponibilidade e fácil recuperação em caso de falhas.

- **Decisão**: Kubernetes foi escolhido para gerenciar a escalabilidade dos microsserviços. Ele permite o autoescalonamento de pods com base em métricas de utilização de recursos.

- **Consequências**:
  - O sistema pode escalar horizontalmente, adicionando novos pods quando necessário.
  - Balanceamento de carga automático entre os serviços.
  - Kubernetes reinicia automaticamente os pod's em caso de falha, garantindo resiliência.
  