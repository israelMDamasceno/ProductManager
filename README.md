# ProductManager

Projeto desenvolvido como parte do processo seletivo para a vaga na empresa Robbu.

Recursos necessários para rodar o projeto

 .Net 8
 SQL SERVER
 
Necessário para executar o projeto

- Necessário ter um banco local((localdb)\\MSSQLLocalDB) , ou alterar a string de conexão do servidor que deseja utilizar, apenas no appSettings
- Executar o comando : Update-Dabase

Após Rodar o Projeto
 - Para executar o crud de produtos utilizar a controller Auth e gerar um token:
 - Chave para Gerar o token: Robbu
 - É criado token do tipo Bearer

Para criar o projeto utilizei um pouco de cada um dos itens abaixo.
 - CQRS
 - Clean Architecture
 - Clean Code
 - Conceitos Solid, como por exemplo Dependency Inversion Principle e Single Responsiblity Principle
 - Entity Framework Core
 - Mediator
 - Token JTW



