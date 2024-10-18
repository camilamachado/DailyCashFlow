### ADR 6: Escolha de EntityFramework como ORM

- **Status**: Decisão Tomada, Implementação realizada
  
- **Contexto**: O sistema precisa de um banco de dados relacional para armazenar as transações e os balanços diários. Também é necessário um controle eficaz de migrações.
  
- **Decisão**: O Entity Framework foi escolhido como ORM para facilitar a interação com o banco de dados e gerenciar as migrações. Apenas o **DailyCashFlow.WebApi** aplica migrações, enquanto **DailyCashFlow.Auth** e **DailyCashFlow.Processor** apenas utilizam o banco.

- **Consequências**:
  - O Entity Framework facilita o mapeamento entre o banco e as entidades do sistema, melhorando a produtividade do desenvolvimento.
  - O controle de migrações é centralizado para evitar conflitos e garantir que a estrutura do banco esteja sempre atualizada.