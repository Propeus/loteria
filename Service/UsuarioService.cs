using Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Service
{
    public class UsuarioService
    {

        public UsuarioService()
        {
            usuarioRepository = new UsuariosRepository();
        }
        public UsuarioService(DbContext repositoryFactory)
        {
            usuarioRepository = new UsuariosRepository(repositoryFactory);
        }

        public UsuariosRepository usuarioRepository { get; private set; }

        public Usuarios Login(string login, string senha)
        {
            senha = Hash(senha);
            var usuario = usuarioRepository.Recuperar(x => x.Usuario == login && x.Senha == senha);
            return usuario;

        }
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
                usuarioRepository.Inserir(usuario);
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
