Employee Management System
Visão Geral do Projeto
Este projeto é um sistema de gerenciamento de funcionários, desenvolvido como parte de um desafio técnico. A solução consiste em uma API RESTful em .NET 8 e um banco de dados SQL Server, ambos orquestrados com Docker Compose.

A arquitetura da solução foi projetada para seguir os princípios de Clean Architecture e SOLID, garantindo uma clara separação de responsabilidades. Isso resulta em um código mais limpo, fácil de manter e escalável.

Funcionalidades Implementadas
CRUD Completo de Funcionários: Implementação das operações básicas de criar, ler, atualizar e excluir.

Validação de Regras de Negócio:

Idade Mínima: A validação na entidade de domínio garante que um funcionário deve ter no mínimo 18 anos.

Permissões Hierárquicas: Um usuário não pode criar outro com um nível de permissão superior ao seu.

Unicidade: Validação para evitar o cadastro de funcionários com o mesmo DocNumber ou Email.

Deleção Segura (Soft Delete): A exclusão de um funcionário não o remove fisicamente do banco de dados, mas o marca como inativo (IsActive = false).

Testes Unitários: Cobertura de testes unitários para as regras de negócio críticas na camada de Aplicação.

Containerização: Utilização de Docker e Docker Compose para orquestrar e isolar os serviços da aplicação (API e Banco de Dados), facilitando o desenvolvimento e a implantação.

Tecnologias Utilizadas
Backend:

C# / .NET 8

ASP.NET Core Web API

Entity Framework Core

Swagger/OpenAPI (para documentação interativa da API)

Banco de Dados:

SQL Server

Ferramentas:

Docker e Docker Compose

xUnit e Moq (para testes unitários)

Pré-requisitos
Para executar este projeto localmente, você precisa ter o seguinte software instalado:

.NET 8.0 SDK

Docker Desktop

Um editor de código (como Visual Studio Code ou Visual Studio)

Primeiros Passos
1. Clonar o Repositório
git clone <URL_DO_SEU_REPOSITORIO>
cd EmployeeManagement

2. Configurar Variáveis de Ambiente
Crie um arquivo chamado .env na raiz da sua solução (EmployeeManagement/) com o seguinte conteúdo. Importante: Substitua a senha por uma senha forte de sua escolha. Este arquivo é lido automaticamente pelo Docker Compose.

# .env file for Docker Compose

DB_SA_PASSWORD=MinhaSenhaSuperSecreta!
DB_NAME=EmployeeDb
DB_PORT=1433
API_PORT=8080
FRONTEND_PORT=3000

3. Executar a Aplicação com Docker Compose
Com o Docker Desktop em execução e o arquivo .env configurado, você pode iniciar todos os serviços da aplicação com um único comando:

docker-compose up --build -d

--build: Garante que as imagens Docker da sua API e frontend sejam reconstruídas a partir dos Dockerfiles caso haja alterações.

-d: Executa os containers em segundo plano (detached mode).

4. Acessar o Sistema
Após a execução do comando docker-compose up, o sistema estará acessível nos seguintes endereços:

API (Swagger UI): http://localhost:8080/swagger

Frontend: http://localhost:3000

5. Parar a Aplicação
Para parar e remover todos os containers criados pelo Docker Compose, use:

docker-compose down

Estrutura do Projeto
A solução é organizada em projetos que representam as camadas da arquitetura:

EmployeeManagement.Api: Camada de apresentação (Controllers, DTOs de request/response).

EmployeeManagement.Application: Camada de aplicação (Serviços de aplicação, lógica de negócio e DTOs).

EmployeeManagement.Domain: Camada de domínio (Entidades de negócio, regras de domínio e interfaces de repositório).

EmployeeManagement.Infrastructure: Camada de infraestrutura (Implementações de repositórios, acesso a dados com Entity Framework Core).

EmployeeManagement.Application.Tests: Projeto de testes unitários para a camada de Aplicação.

Testes Unitários
Para executar os testes unitários do projeto, navegue até a pasta do projeto de teste e use o comando:

cd EmployeeManagement.Application.Tests/
dotnet test

Licença
Este projeto está licenciado sob a Licença MIT.