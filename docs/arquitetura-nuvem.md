
# Arquitetura de Computação em Nuvem - PetVille

## 1. Objetivo

A computação em nuvem tem como objetivo permitir que a plataforma PetVille possa ser acessada pela internet, mantendo sua API e seu banco de dados disponíveis em uma infraestrutura centralizada.

## 2. Arquitetura proposta

A arquitetura utiliza os seguintes serviços da AWS:

- Amazon EC2: hospedagem da API PetVille.
- Amazon RDS: gerenciamento do banco de dados MySQL.
- Amazon S3: armazenamento de imagens e arquivos.
- AWS Backup: realização de cópias de segurança.
- AWS IAM: gerenciamento de permissões e acessos.

## 3. Funcionamento

O aplicativo PetVille realiza solicitações para a API por meio da internet. A API processa essas solicitações e realiza operações no banco de dados MySQL hospedado no Amazon RDS.

Os arquivos e imagens poderão ser armazenados no Amazon S3, enquanto as cópias de segurança poderão ser gerenciadas pelo AWS Backup.

## 4. Segurança

A arquitetura prevê:

- Comunicação protegida por HTTPS.
- Controle de acesso por permissões.
- Banco de dados sem acesso público direto.
- Proteção das credenciais de conexão.
- Cópias de segurança periódicas.

## 5. Considerações sobre custos

A arquitetura foi desenvolvida como uma proposta acadêmica. Não foram contratados serviços de hospedagem ou infraestrutura paga para sua implementação.

O projeto utiliza atualmente o banco MySQL e a API desenvolvida em .NET. A migração para os serviços de nuvem dependerá de uma futura configuração e implantação.

## 6. Conclusão

A utilização da computação em nuvem poderá melhorar a disponibilidade, a escalabilidade e a organização da infraestrutura da PetVille, permitindo futuras expansões da plataforma.
