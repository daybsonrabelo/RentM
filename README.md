# **RentM**

RentM é uma aplicação para gerenciar o aluguel de motos, permitindo o cadastro de entregadores, motos e locações, com regras específicas para devoluções e penalidades.

---

## **Dependências**

Certifique-se de que as seguintes ferramentas e bibliotecas estão instaladas no seu ambiente:

### **Ferramentas Necessárias**
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0): Para compilar e executar o projeto.
- [PostgreSQL](https://www.postgresql.org/): Banco de dados utilizado pela aplicação.
- [Git](https://git-scm.com/): Para clonar o repositório.

### **Pacotes NuGet**
Os pacotes NuGet utilizados no projeto incluem:
- **Entity Framework Core**:
  - `Microsoft.EntityFrameworkCore`
  - `Npgsql.EntityFrameworkCore.PostgreSQL` (para suporte ao PostgreSQL)
  - `Microsoft.EntityFrameworkCore.Tools`
- **xUnit** (para testes):
  - `xunit`
  - `Moq`

---

## **Como Clonar o Projeto**

1. Abra o terminal ou prompt de comando.
2. Navegue até o diretório onde deseja clonar o projeto.
3. Execute o seguinte comando: git clone https://github.com/seu-usuario/rentm.git
4. Navegue até o diretório do projeto: cd rentm

   
---

## **Configuração do Banco de Dados**

1. Certifique-se de que o PostgreSQL está em execução.
2. Crie um banco de dados chamado `RentMDb` no PostgreSQL. Você pode fazer isso usando o terminal do PostgreSQL ou uma ferramenta como o **pgAdmin**:
```
CREATE DATABASE RentMDb;
```
3. Atualize a string de conexão no arquivo `appsettings.json` do projeto `RentM.API`:
```
"ConnectionStrings": { "DefaultConnection": "Host=localhost;Port=5432;Database=RentMDb;Username=seu_usuario;Password=sua_senha" }
```

   Substitua:
   - `localhost` pelo endereço do seu servidor PostgreSQL, se necessário.
   - `seu_usuario` pelo nome de usuário do PostgreSQL.
   - `sua_senha` pela senha do usuário.

---

## **Executando as Migrations**

1. Abra o terminal na raiz do projeto.
2. Navegue até o diretório do projeto `RentM.API`:
```
cd src/RentM.API
```
3. Execute o comando para criar o banco de dados e aplicar as migrations:
```
dotnet ef database update
```
Se o comando `dotnet ef` não estiver disponível, instale a ferramenta Entity Framework CLI:
```
dotnet tool install --global dotnet-ef
```

---

## **Executando o Projeto**

1. Navegue até o diretório do projeto `RentM.API`:
```
cd src/RentM.API
```
2. Execute o projeto:
```
dotnet run
```

3. A API estará disponível em: `https://localhost:5001` ou `http://localhost:5000`.

---
   
   

   
