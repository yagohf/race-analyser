# race-analyser
Repositório para armazenamento dos artefatos relacionados ao teste técnico solicitado pela empresa [**Gympass**](https://www.gympass.com).

Esse projeto consiste em uma aplicação que possibilita submeter arquivos com resultados de corridas para análise. Uma vez analisados, os resultados ficam disponíveis para consulta pública.

## Estrutura de pastas
- A pasta [backend](./src/backend) contém uma API desenvolvida em [ASP.NET Core 2.2](https://docs.microsoft.com/pt-br/aspnet/core/?view=aspnetcore-2.2), utilizando o [Swagger](https://swagger.io/) como ferramenta para documentar API's, e o [Entity Framework](https://docs.microsoft.com/pt-br/ef/core/) como ORM para acesso a um banco [SQL Server](https://docs.microsoft.com/pt-br/sql/sql-server/sql-server-technical-documentation?view=sql-server-2017). O framework de testes unitários utilizado foi o [MSTest](https://docs.microsoft.com/pt-br/dotnet/core/testing/unit-testing-with-mstest). A aplicação foi desenvolvida em camadas totalmente independentes, facilitando assim a elaboração de testes unitários.
- A pasta [db](./src/database) contém os scripts que devem ser executados em uma base SQL Server para que a aplicação funcione. Os scripts já encontram-se na ordem correta de execução. Existe inclusive um script que cria um usuário para utilização em testes, já com uma massa de dados (Usuário: **gympass** / Senha: **123mudar**). Recomenda-se o uso da versão 2014 ou superior do SQL Server.
- A pasta [frontend](./src/frontend) contém uma aplicação cliente desenvolvida em [Angular 7](https://angular.io/).

## Ajustes necessários para executar o projeto
- Substituir a connection string no arquivo [appsettings.json](./src/backend/Yagohf.Gympass.RaceAnalyser.Api/appsettings.json) pela conexão do seu banco de dados.
- Substituir o caminho para o arquivo de exemplo de upload no arquivo [appsettings.json](./src/backend/Yagohf.Gympass.RaceAnalyser.Api/appsettings.json), na linha 8. Sugiro apontar para o caminho no qual encontra-se o arquivo [EXAMPLE.txt](./src/simulations/EXAMPLE.txt).
- Substituir a URL da API no arquivo [environment.ts](./src/frontend/src/environments/environment.ts) pela URL em que sua API encontra-se rodando.

## Executando a aplicação
- Você precisará ter instalado em sua máquina o SDK e o runtime do .NET Core 2.2, que podem ser obtidos [**aqui**](https://dotnet.microsoft.com/download/dotnet-core/2.2).
- Além disso, precisará ter instalado o Node.js e o NPM (gerenciador de pacotes), que podem ser obtidos [**aqui**](https://nodejs.org/en/).
- Uma vez instalados, navegue até a raiz da aplicação e execute os seguintes comandos:

```
# Navegar até a pasta onde estão os fontes
cd src

# Instalar o Angular CLI
npm install -g @angular/cli

# Restaurar pacotes para rodar a aplicação como um todo
npm install

# Navegar até a pasta do frontend
cd frontend

# Restaurar pacotes do frontend
npm install

# Voltar à raíz da aplicação
cd ..

# Executar backend e frontend simultaneamente
npm start
```
- Para acessar o frontend rodando, acesse [**http://localhost:4200**](http://localhost:4200). Para o backend, [**http://localhost:57856**](http://localhost:57856).
- **Atente-se** que para subir um arquivo para processamento será necessário estar logado. Por isso, utilize o usuário **gympass** e a senha **123mudar**, como mencionado anteriormente. 
