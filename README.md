# loteria

## Sum�rio
* Requisitos
* Instala��o
* Publica��o
* Usuarios
* Observa��es

## Requisitos
* Visual Studio 2015 ou superior.
* SQL Server Express LocalDB 2014 ou superior.
* Gerenciador de servi�os de informa��es da internet(IIS) 7 ou superior

## Instala��o
* Clone o reposit�rio usando o visual studio.
* Compile a solu��o
* Utilizando o perfil de publica��o pr� configurado publique o projeto.

## Publica��o
* V� em Gerenciador de servi�os de informa��es da internet
* Crie um novo site com o nome "loteria"
* Configure o caminho fisico para [Local do seu repositorio]\Loterica\bin\Release\PublishOutput.
* Adicione a conta de usuario do windows no caminho fisico.
* No Pools de aplicativos clique em "loteria" > Configura��es avan�adas... > Modelo de processo > Identidade. Insira novamete a conta de usuario do windows

## Usuarios
* O banco de dados da aplicacao vem acompanhado com dois usuarios, "sistema" e "usuario" (sem aspas), ambas possuem a senha 123456

## Observa��es
* O perfil de publica��o est� configurado para publicar o projeto na pasta [Local do seu repositorio]\Loterica\bin\Release\PublishOutput.
* Caso ocorra o erro 50 do MSSQLLocalDB consulte os links
 * https://stackoverflow.com/questions/11278114/enable-remote-connections-for-sql-server-express-2012
 * https://www.codeproject.com/questions/1154848/error-local-database-runtime-error-occurred-when-p
 * https://forum.imasters.com.br/topic/480651-resolvido%C2%A0erro-50-sql-server-x-visual-studio-2012/
* Caso o problema persista inicie a aplica��o pelo Debug do visual studio.