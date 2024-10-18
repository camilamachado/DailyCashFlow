### ADR 7: Escolha do SQL Server como Banco de Dados

- **Status**: Decisão Tomada, Implantação realizada
  
- **Contexto**: O banco de dados precisa garantir consistência, integridade e segurança, enquanto lida com uma carga potencialmente alta de consultas e gravações, principalmente em um cenário de expansão. O banco também deve se integrar de maneira eficiente com o Entity Framework e suportar funcionalidades essenciais, como migrações automáticas e segurança de dados.
  
- **Decisão**: O SQL Server foi escolhido como banco de dados principal. Essa escolha se baseia em:
  - A afinidade com o stack .NET, oferecendo um bom suporte ao Entity Framework para mapeamento objeto-relacional.
  - Capacidade de lidar com grandes volumes de dados.

- **Consequências**:
  - O SQL Server oferece várias opções de otimização, como índices avançados, partições e planos de consulta eficientes, essenciais para processar grandes volumes de dados financeiros de maneira eficaz.
  - Além disso, oferece suporte a operações complexas e otimizações necessárias para consultas frequentes e relatórios de balanço diário.
  - A "equipe" tem expertise no uso do SQL Server, o que reduz a curva de aprendizado e acelera o processo de desenvolvimento.
