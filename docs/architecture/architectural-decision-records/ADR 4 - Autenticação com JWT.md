### ADR 4: Autenticação com JWT no DailyCashFlow.Auth

- **Status**: Decisão Tomada, Implementação parcialemte realizada

- **Contexto**:  É necessário garantir que somente usuários autenticados possam acessar os dados registrados.

- **Decisão**: Implementou-se autenticação com tokens JWT no DailyCashFlow.Auth. Os tokens são gerados após o login e incluídos em todas as requisições subsequentes.

- **Consequências**:
  - Os serviços podem verificar a autenticidade do usuário de forma independente, sem necessidade de consulta contínua ao serviço de autenticação.
  - JWT proporciona segurança e performance, sendo amplamente utilizado em microsserviços.
  