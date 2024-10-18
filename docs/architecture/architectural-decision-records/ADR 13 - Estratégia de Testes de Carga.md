### ADR 13 - Estratégia de Testes de Carga

- **Status**: Decisão Tomada, Implementação Pendente
  
- **Contexto**: Para garantir que o sistema seja capaz de suportar o volume de requisições esperadas, é importante implementar uma estratégia de testes de carga. Esses testes ajudarão a avaliar o desempenho do sistema sob condições de estresse e a identificar limitações antes que se tornem problemas em produção.
  
- **Decisão**: Será realizado um teste de carga para validar que o sistema pode suportar 50 requisições por segundo.

- **Consequências**:
  - O teste de carga ajudará a identificar gargalos de desempenho e a capacidade do sistema, garantindo que ele atenda às expectativas de uso.
  - A realização de testes de carga regulares permitirá que a equipe avalie continuamente o desempenho do sistema em resposta a alterações no código e na infraestrutura.
  