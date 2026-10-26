# TrabalhoRaizesDoNordeste

API REST para uma rede de lanchonetes "Raízes do Nordeste" que está em expansão. A API possui cadastro de unidades, categorias de produtos, produtos, estoque por unidade, promoções,
cadastro de usuários com seus perfis de acesso, geração de token, pedidos por canal (APP, TOTEM, BALCÃO, etc) pagamento simulado (mock) para poder criar pagamento recusado, resposta de pagamento com retorno 
payload, programada de fidelidade onde poderá acumular ou resgatar pontos, termos LGPD, estoque de movimentação registrando entrada e saída de produtos, 
itens do pedido solicitado verificando se possui estoque suficiente e log auditória. Todos possuindo permissão de acesso para cada tipo de perfil de usuário.

Projeto de atividade prática multidisciplinar da faculdade UNINTER - Trilha escolhida (Back-End) 

------

# Requisitos: (linguagem, versão, banco, dependências)

Linguagem de programação escolhida para realização do projeto = C#.
Plataforma = .NET 10 SDK (net10.0)
Framework = ASP.NET Core Web API
ORM = Entity Framework Core 10
Banco de dados = SQLite (arquivo local)
Autenticação JWT (Bearer Token)
Documentação  Swagger OpenAPI (Swashbuckle) 
Testes Postman (coleção `.json`), arquivos `.http` e swagger.

-- Pacotes NuGet usados: --

Pacote                                                       Versão
`Microsoft.AspNetCore.Authentication.JwtBearer`              10.0.13 
`Microsoft.AspNetCore.OpenApi`                               10.0.12
`Microsoft.EntityFrameworkCore`                              10.0.12
`Microsoft.EntityFrameworkCore.Design`                       10.0.12
`Microsoft.EntityFrameworkCore.Sqlite`                       10.0.12
`Swashbuckle.AspNetCore.Swagger`                             10.3.1
`Swashbuckle.AspNetCore.SwaggerGen`                          10.3.1
`Swashbuckle.AspNetCore.SwaggerUI`                           10.3.1

 -- Ferramentas --

.NET 10SDK (https://dotnet.microsoft.com/download)
Ferramenta do EF Core (para as migrations):
  ```bash
  dotnet tool install --global dotnet-ef
  ```
Visual Studio 2022
Postman (Para criação da coleção e prints para evidências) - RaizesDoNordeste.postman_collection.json
Swagger (https://localhost:7209/swagger/) - swagger.json

-- Estrutura do projeto --

TrabalhoRaizesDoNordeste/
|-API/
  |-Controllers                                   -> endpoints
|-Application/
  |-DTOs                                          -> objetos de entrada e retorno da API
  |-Services                                      -> regras de negócio
|-Context/                                        -> AppDbContext
|-Domain/
  |-Enums                                         -> Enums
  |-Models                                        -> Models (entidades)
|-Http/                                           -> requisições de teste por tabela (.http)
|-Infrastructure/
  |-Repository                                    -> acesso ao banco (EF Core)
  |-Migrations                                    -> migrations do EF Core
|-RaizesDoNordeste.db                             -> banco SQLite já populado
|-RaizesDoNordeste.postman_collection.json        -> coleção de testes do Postman
|-swagger.json                                    -> documentação OpenAPI exportada
            

------

# Como configurar variáveis de ambiente (.env.example + instruções)

As configurações ficam no arquivo "appsettings.json". O arquivo ".env.example" na raiz mostra as variáveis no formato de variável ambiente.

Variável                                                  para que serve                                                                Exemplo

`Jwt__Key`                             Chave secreta que assina o token (em base64, com no mínimo 32 bytes)                     `troque-por-uma-chave-base64`
`Jwt__Issuer`                          Quem emite o token                                                                       `RaizesDoNordeste`
`Jwt__Audience`                        Para quem o token é emitido                                                              `RaizesDoNordesteClient`
`Jwt__ExpirationMinutes`               Validade do token em minutos                                                             `60`

O banco SQLite não usa nenhuma variável, o arquivo "RaizesDoNordeste.db" fica na raíz do projeto

-- Como usar -- 

1 - Usar o "appsettings.json" que já vem no projeto. Não precisa fazer nada.
2 - (variáveis de ambiente): copie o ".env.example", preencha os valores e defina as variáveis no terminal antes de rodar a API. O ASP.NET Core lê automaticamente variáveis com 
__ (dois underlines) e susbtituem o que está no "appsettings.json".

OBS: A Jwt__Key nunca deve ficar no repositório, por conta da segurança do projeto, mas a chave do "appsettings.json" está visivel para facilitar a execução local do trabalho. 

# Como instalar dependências

Na pasta do projeto ( onde fica o ".csproj")

bash 
dotnet restore


# Como criar banco e executar migrations (e seed, se houver)

O banco é SQLlite e fica em um arquivo local

Criar o banco e aplicar as migrations
bash 
dotnet ef database update

Este comando cria o arquivo do banco com todas as tabelas.

------

O projeot não possui seed automático. Os dados de testes são foram cadastrados pelos arquivos da pasta "Http/" e pelo "Postman": Segui uma ordem para criação:

1 - Categorias.http
2 - Produtos.http
3 - UnidadesEstabelecimento.http
4 - Usuarios.http
5 - EstoqueUnidade.http
6 - PromocoesCampanhas.http
7 - Pedidos.http
8 - Pagamentos.http

As tabelas "Fidelidade.http", "PontosFidelidade", "ItensPedido", "EstoqueMovimentacao" e "LogAuditoria" são criadas mediante a outras tabelas, essas tabelas apenas 
consultam os dados gerados pelos pedidos, pagamentos e usuarios

O arquivo de banco RaizesDoNordeste.db já esta populado e incluso no repositório. Com esse aquivo, é possível rodar a API e testar direito. Caso queira criar dados novos, 
na coleção "Postman" deixei preenchido com dados caso queria criar. 

# Como iniciar a API

bash
dotnet run --launch-profile https

Ou no Visual Studio, selecione "https" e aperte o F5

API:
HTTPS: https://localhost:7209/swagger/
HTTP: http://localhost:5044/swagger/

Caso na primeira execução, se o navegador reclamar do certificado, rode:

bash
dotnet dev-certs https --trust

# Como acessar a documentação (Swagger/OpenAPI)

Com a API rodando, acesse:

https://localhost:7209/swagger/

Cada endpoint mostra os parâmetros, o corpo da requisição os possíveis códigos de retorno (200, 201, 400, 401, 403, 404 e 409).

O arquivo OpenAPI também está salvo em `swagger.json` na raiz do repositório e pode ser aberto em [editor.swagger.io](https://editor.swagger.io) sem rodar a API.

Para testar rotas protegidas no Swagger:

1. Execute `POST /api/Auth/login` com email e senha.
2. Copie o token retornado.
3. Clique em **Authorize** (cadeado no topo da página), cole o token e confirme.

LEMBRANDO: Para visualização, criação, alteração e deletar/desativar de alguns métodos é necessário que o perfil corresponda.

-- O que cada perfil tem acesso --

PERFIL                                         ACESSO

CLIENTE                       criar, alterar e cancelar os PRÓPRIOS pedidos; pagar; consultar o próprio saldo de pontos; alterar o próprio cadastro 
ATENDENTE                     pedidos de balcão; alterar status (PRONTO / ENTREGUE); consultar produtos, estoque, promoções e pagamentos 
COZINHA                       consultar pedidos e itens; alterar status do pedido 
GERENTE / ADMIN               cadastros (unidades, categorias, produtos, estoque, promoções, funcionários), visualização de listas, buscas por ID e por nome, relatórios auditoria e movimentações. 

OBS: O cadastro de "CLIENTE" é aberto (não precisa de token). O "CARDÁPIO" da unidade é público.

------

-- Autenticação e perfis em POSTMAN --

O login gera um ""token Jwt"", que deve ser enviado nas demais requisições no header: Authorization: Bearer {token}

O token vale pelo tempo que configurei em `Jwt__ExpirationMinutes`, com 60 minutos. Sem token, a API devolve o erro 401. Com um perfil sem permissão, devolve o erro 403.

em LOGIN

POST:
https://localhost:7209/api/Auth/login?email={email}&senha={senha}

Já deixei criado e populado 6 POST com email e senha de cada perfil da unidade 4, para geração do Token e faciliar o acesso.

PERFIL                    NOME                                 EMAIL                                    SENHA

GERENTE                 Carlos Albuquerque            carlos.albuquerque@example.com                Teste@123456
ADMIN                   Fernanda Cavalcanti           fernanda.cavalcanti@example.com               Teste@123456
ATENDENTE               Juliana Barros                juliana.barros@example.com                    Teste@123456
COZINHA                 Severino Lima                 severino.lima@example.com                     Teste@123456
CLIENTE                 Neusa Bacci                   neusa.bacci@example.com                       Teste@123456
CLIENTE                 Marcos Vieira                 marcos.vieira@example.com                     Marcos@2026

LEMBRANDO: Para visualização, criação, alteração e deletar/desativar de alguns métodos é necessário que o perfil corresponda.

-- O que cada perfil tem acesso --

PERFIL                                         ACESSO

CLIENTE                       criar, alterar e cancelar os PRÓPRIOS pedidos; pagar; consultar o próprio saldo de pontos; alterar o próprio cadastro 
ATENDENTE                     pedidos de balcão; alterar status (PRONTO / ENTREGUE); consultar produtos, estoque, promoções e pagamentos 
COZINHA                       consultar pedidos e itens; alterar status do pedido 
GERENTE / ADMIN               cadastros (unidades, categorias, produtos, estoque, promoções, funcionários), visualização de listas, buscas por ID e por nome, relatórios auditoria e movimentações. 

OBS: O cadastro de "CLIENTE" é aberto (não precisa de token). O "CARDÁPIO" da unidade é público.

-- Pagamento simulado (mock) --

O pagamento NÃO usa um gateway real. O próprio sistema simula a resposta do gateway e devolve um payload com o ID da transação, o status, o código e a mensagem para o canal do pedido.

Deixei na coleção 3 tipos de pedidos diferentes para criação (pedido para acumular pontos, pedido para resgatar pontos e pedido para gerar um pagamento RECUSADO). Para ver a evolução correta 
e visualizar os pontos em Fidelidade, siga esta ordem:

Pedido 1º:
Gere o pedido de ACÚMULO de pontos e, em Pagamentos, gere o pagamento desse pedido. Assim que o pedido é pago, a API retorna o payload com o ID da transação e o código, e o status do pedido vai automaticamente 
para "EMPREPARO". Também são gerados os pontos conforme o valor do pedido, e em Fidelidade é possível consultar o "meu-saldo" com a pontuação.

# Regras de fidelidade: a cada R$ 10,00 pagos, a cliente ganha 1 ponto. Só participa quem é CLIENTE e aceitou a LGPD.

Pedido 2º:
Gere o pedido com RESGATE de pontos e, em Pagamentos, gere o pagamento com resgate (`?usarPontos=true`). Assim que o pedido é pago, a API retorna o payload com o ID da transação e o código, o status do pedido vai 
automaticamente para "EMPREPARO", e o saldo de pontos acumulados é usado como desconto no valor do pagamento.

# Regras de fidelidade: pontos como desconto (1 ponto = R$ 1,00)

Pedido 3º
Gere o pedido de RECUSA e pague com os dados que já deixei preenchidos em Pagamentos (`?simularRecusa=true`). Assim que o pagamento é recusado, a API retorna o payload com o ID da transação e o código, e o status do 
pedido vai automaticamente para "CANCELADO".

# Como rodar os testes

Os testes são "manuais", pela coleção do Postman e pelos arquivos `.http`. O projeto não tem testes automatizados.

1. Abra o Postman, clique em "Import" e selecione o arquivo `RaizesDoNordeste.postman_collection.json`, que está na raiz do repositório.
2. Não é preciso environment: as variáveis ficam na própria coleção (aba "Variables"):
   - `baseUrl` = `https://localhost:7209`
   - `token` = preenchido automaticamente pelo login
3. Com a API rodando, abra a pasta "Login" e execute o login do perfil desejado (por exemplo, "GerarToken(Gerente)"). O token é salvo automaticamente na variável `{{token}}`.
4. Execute as requisições das demais pastas. Todas usam o `{{token}}` da coleção.

Para testar com outro perfil, rode o login correspondente (Admin, Atendente, Cozinha, Cliente Neusa ou Cliente Marcos): o `{{token}}` é substituído e passa a valer para todas as requisições.

A requisição "GerarToken(Invalido)" mostra o cenário de erro: login com senha errada, que retorna 401, conforme solicitado.

-- Fluxos principais testados --

Usuarios -> cadastra perfil, dados e senha -> inclui na unidade -> se aceitar a LGPD e a fidelidade, a fidelidade é criada automaticamente -> cria auditoria
Unidade (COMPLETA/REDUZIDA) -> inclui estoque por unidade cadastrada
Acúmulo de pontos -> pedido -> baixa do estoque -> movimentação SAÍDA -> pagamento aceito -> EMPREPARO -> PRONTO -> ENTREGUE -> cria auditoria -> Fidelidade (meu-saldo) com os pontos acumulados
Resgate de pontos -> pedido -> baixa do estoque -> movimentação SAÍDA -> mostra o valor com desconto -> pagamento com `usarPontos=true` -> EMPREPARO -> PRONTO -> ENTREGUE -> cria auditoria -> Fidelidade (meu-saldo) com os pontos resgatados
Pagamento recusado -> pedido -> baixa do estoque -> movimentação SAÍDA -> pagamento com `simularRecusa=true` -> cria auditoria -> pedido cancelado -> repor estoque -> movimentação ENTRADA.

-- Códigos de Retorno -- 

CÓDIGO                 RETORNO
200              Sucesso
201              Registro criado
400              Dado inválido ou campo obrigatório faltando
401              Sem token, token vencido ou login inválido
403              Perfil sem permissão, ou cliente tentando acessar dado de outra pessoa
404              Registro não encontrado
409              Regra de negócio violada (ex: pedido já pago, pedido não esta pendente)