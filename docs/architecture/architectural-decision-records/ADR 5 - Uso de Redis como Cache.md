### ADR 4: Uso de Redis como Cache

- **Status**: Decisão Tomada, Implementação Pendente

- **Contexto**: Para otimizar o desempenho e reduzir a carga sobre o banco de dados em consultas de dados repetitivas (como relatórios diários), é necessário um mecanismo de cache.

- **Decisão**: O Redis foi escolhido como ferramenta de cache para armazenar resultados de consultas frequentes, como o relatório de transações diárias.

- **Consequências**:
  - Redução de tempo de resposta ao buscar dados frequentemente acessados.
  - Menor carga no banco de dados, melhorando a escalabilidade geral do sistema.
  - Redis é uma ferramenta robusta e amplamente utilizada para caching distribuído.
  