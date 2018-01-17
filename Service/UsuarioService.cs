using Repository;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;

namespace Service
{
    /// <summary>
    /// Serviço para a entidade <see cref="Usuarios"/>
    /// </summary>
    public class UsuarioService : GenericoService<UsuariosRepository>
    {

        /// <summary>
        /// Instacia do serviço criando um novo contexto para o repositório
        /// </summary>
        public UsuarioService()
        {

        }
        /// <summary>
        /// Instacia do serviço utilizando um contexto já existente
        /// </summary>
        /// <param name="repositoryFactory">Contexto de algum repositório ja instanciado</param>
        public UsuarioService(DbContext repositoryFactory) : base(repositoryFactory)
        {

        }

        /// <summary>
        /// Recupera o Usuaro caso login e senha estajam corretos
        /// </summary>
        /// <param name="login">Login do usuario</param>
        /// <param name="senha">Senha do usuario</param>
        /// <returns>Retorna <see cref="Usuarios"/> caso login e senha estejam corretos.</returns>
        public Usuarios Login(string login, string senha)
        {
            senha = Hash(senha);
            var usuario = Repository.Recuperar(x => x.Usuario == login && x.Senha == senha);
            return usuario;

        }
        /// <summary>
        /// Registra um novo usuario no banco
        /// </summary>
        /// <param name="login">Login do usuario</param>
        /// <param name="senha">Senha do usuario</param>
        /// <param name="e_mail">E-mail do usuario</param>
        /// <returns></returns>
        public Usuarios Registrar(string login, string senha, string e_mail)
        {
            Usuarios usuario = new Usuarios
            {
                Detalhes = new Detalhes
                {
                    E_Mail = e_mail
                },
                Senha = Hash(senha),
                ConfirmarSenha = Hash(senha),
                Usuario = login,
                TipoConta = IdcTipoConta.Usuario
            };
            using (TransactionScope scope = new TransactionScope())
            {
                Repository.Inserir(usuario);
                scope.Complete();
            }
            return usuario;
        }

        private string Hash(string valor)
        {
            //Criando hash da senha
            var r = HashAlgorithm.Create("SHA256").ComputeHash(Encoding.UTF8.GetBytes(valor));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in r)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }
    }
}
