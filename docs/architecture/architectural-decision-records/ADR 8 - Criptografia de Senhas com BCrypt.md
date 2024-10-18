### ADR 8: Criptografia de Senhas com BCrypt

- **Status**: Decisão Tomada, Implementação realizada
  
- **Contexto**: O projeto envolve o gerenciamento de usuários e transações financeiras, sendo essencial garantir que informações sensíveis, como senhas, estejam devidamente protegidas contra vazamentos. O uso de uma solução de hashing segura é essencial para garantir que, mesmo em caso de uma falha de segurança, as senhas armazenadas não possam ser facilmente descobertas ou revertidas para o formato original.
  
- **Decisão**: Foi decidido usar a biblioteca BCrypt para criptografar as senhas dos usuários.

- **Consequências**:
  - BCrypt é um algoritmo amplamente aceito para hashing de senhas, fornecendo uma camada adicional de segurança, tornando difícil para atacantes descriptografarem senhas armazenadas.
  - Ao contrário de algoritmos simples, como SHA ou MD5, o BCrypt é baseado no algoritmo Blowfish e foi projetado especificamente para a segurança de senhas. O fato de ser adaptativo significa que, com o tempo, a quantidade de iterações do algoritmo pode ser aumentada para acompanhar o crescimento do poder computacional, mantendo o algoritmo resistente a ataques de força bruta.
  - A biblioteca BCrypt.Net foi facilmente integrada ao sistema, possibilitando o uso de métodos simples para realizar o hashing de senhas no momento do cadastro ou alteração de senha.
