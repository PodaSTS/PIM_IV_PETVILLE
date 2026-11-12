
# Computação em Nuvem e DevOps - PetVille

## 1. Objetivo

Implementar práticas de Computação em Nuvem e DevOps no projeto PetVille, buscando melhorar a organização, a integração e a qualidade do desenvolvimento da API.

## 2. Tecnologias utilizadas

- .NET 10: desenvolvimento da API.
- GitHub: versionamento e gerenciamento do código.
- GitHub Actions: automação do processo de integração contínua.
- Entity Framework Core: acesso e gerenciamento dos dados.
- MySQL: banco de dados relacional.
- TiDB Cloud: alternativa de banco de dados em nuvem.

## 3. Integração Contínua (CI)

Foi criado um workflow no GitHub Actions chamado "DevOps - PetVille API".

O processo é executado automaticamente quando ocorrem alterações na branch main ou quando uma Pull Request é aberta para essa branch.

As etapas automatizadas são:

1. Baixar o código do repositório.
2. Configurar o ambiente .NET 10.
3. Restaurar as dependências da API.
4. Compilar a API.
5. Executar os testes automatizados.

## 4. Testes automatizados

Foram implementados cinco testes para verificar funcionalidades da API:

- Verificação de funcionamento da API.
- Verificação de acesso ao perfil sem autenticação.
- Teste de cadastro de clientes.
- Teste de login com senha incorreta.
- Teste de login com token e acesso ao perfil.

Os testes utilizam um banco de dados em memória, evitando a necessidade de conectar a um banco MySQL real durante sua execução.

## 5. Computação em Nuvem

A aplicação possui uma estrutura de configuração para utilização de banco de dados MySQL local ou TiDB Cloud.

As configurações de conexão em nuvem são protegidas por meio do mecanismo de Data Protection do .NET.

## 6. Resultados

O pipeline de integração contínua foi executado com sucesso no GitHub Actions, incluindo a compilação da API e a execução dos cinco testes automatizados.

## 7. Considerações finais

A implementação permite automatizar a validação do código, identificar erros durante o desenvolvimento e melhorar a confiabilidade do projeto PetVille.

O processo de implantação automática (CD) não faz parte das etapas implementadas nesta versão.
