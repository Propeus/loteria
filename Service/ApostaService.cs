using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using Repository;

namespace Service
{
    /// <summary>
    /// Serviço para a entidade <see cref="Apostas"/>
    /// </summary>
    public class ApostaService : GenericoService<ApostasRepository>
    {

        private int NumerosQuantidade { get; set; } = 6;
        private int NumeroMinimo { get; set; } = 0;
        private int NumeroMaximo { get; set; } = 60;


        SorteioService sorteioService { get; set; }

        /// <summary>
        /// Instacia do serviço criando um novo contexto para o repositório
        /// </summary>
        public ApostaService()
        {
            sorteioService = new SorteioService(Repository.RepositoryFactory);
        }
        /// <summary>
        /// Instacia do serviço utilizando um contexto já existente
        /// </summary>
        /// <param name="repositoryFactory">Contexto de algum repositório ja instanciado</param>
        public ApostaService(DbContext repositoryFactory) : base(repositoryFactory)
        {
            sorteioService = new SorteioService(repositoryFactory);
        }

        /// <summary>
        /// Insere uma nova aposta aleatória para o usuario
        /// </summary>
        /// <param name="usuario">Usario que irá fazer a aposta</param>
        /// <returns><see cref="Apostas"/></returns>
        public Apostas InserirApostaAleatorio(Usuarios usuario)
        {
            int[] numeros = Helper.Helper.SortearValores(NumerosQuantidade, NumeroMinimo, NumeroMaximo);

            Apostas apostas = new Apostas
            {
                DataAposta = DateTime.Now,
                NumeroAposta = Convert.ToInt32(string.Join("", numeros)),
                NumeroApostaExibicao = string.Join("-", numeros),
                IdUsuario = usuario.Id
            };
            using (TransactionScope scope = new TransactionScope())
            {
                Repository.Inserir(apostas);
                scope.Complete();
            }
            return apostas;
        }
        /// <summary>
        /// Insere uma nova aposta aleatória para o usuario
        /// </summary>
        /// <param name="usuario">Usario que irá fazer a aposta</param>
        /// <param name="numeros">Numeros sortiados ou escolidos pelo usuario.</param>
        /// <returns><see cref="Apostas"/></returns>
        public Apostas InserirAposta(Usuarios usuario, int[] numeros)
        {
            Apostas aposta = new Apostas
            {
                DataAposta = DateTime.Now,
                NumeroAposta = Convert.ToInt32(string.Join("", numeros)),
                NumeroApostaExibicao = string.Join("-", numeros),
                IdUsuario = usuario.Id
            };

            ValidarRegraNegocio(aposta);
            using (TransactionScope scope = new TransactionScope())
            {
                Repository.Inserir(aposta);
                scope.Complete();
            }

            return aposta;
        }
        /// <summary>
        /// Insere uma nova aposta aleatória para o usuario
        /// </summary>
        /// <param name="aposta">Modelo preenchido</param>
        /// <returns><see cref="Apostas"/></returns>
        public Apostas InserirAposta(Apostas aposta)
        {
            ValidarRegraNegocio(aposta);

            using (TransactionScope scope = new TransactionScope())
            {
                Repository.Inserir(aposta);
                scope.Complete();
            }
            return aposta;
        }



        public List<Usuarios> RecuperarUsuariosApostas(int[] numeros)
        {
            int numeroaposta = Convert.ToInt32(string.Concat("", numeros));
            var resultado = Repository.RecuperarVarios(x => x.NumeroAposta == numeroaposta).Include(x => x.Usuarios).Include(x => x.Sorteios);
            return resultado.Select(x => x.Usuarios).ToList();
        }
        /// <summary>
        /// Obtem um conjunto de <see cref="Apostas"/> utilizando o <see cref="Usuarios.Id"/>.
        /// </summary>
        /// <param name="usuario">Objeto <see cref="Usuarios"/> com o Id</param>
        /// <returns><see cref="Apostas"/></returns>
        public List<Apostas> RecuperarApostas(Usuarios usuario)
        {
            return Repository.RecuperarPorUsuario(usuario.Id).Include(x => x.Sorteios).Include(x => x.ApostaResultados).Include(x => x.Sorteios).ToList();
        }
        /// <summary>
        /// Obtem um conjunto de <see cref="Apostas"/> utilizando o mês da aposta.
        /// </summary>
        /// <param name="usuario">Objeto <see cref="Usuarios"/> com o Id</param>
        /// <param name="mes">Mês da aposta</param>
        /// <returns><see cref="Apostas"/></returns>
        public List<Apostas> RecuperarApostasPorMes(Usuarios usuario, int mes)
        {
            if (mes > 12 && mes < 1)
                throw new ArgumentOutOfRangeException("Mes", mes, "O valor do mês deve ser entre 1 (Janeiro) a 12 (Dezembro).");

            return Repository.RecuperarPorUsuario(usuario.Id).Where(x => x.DataAposta.Month == mes).Include(x => x.ApostaResultados).Include(x => x.Sorteios).ToList();
        }
        /// <summary>
        /// Obtem um conjunto de <see cref="Apostas"/> utilizando o ano da aposta.
        /// </summary>
        /// <param name="usuario">Objeto <see cref="Usuarios"/> com o Id</param>
        /// <param name="ano">Ano da aposta</param>
        /// <returns><see cref="Apostas"/></returns>
        public List<Apostas> RecuperarApostasPorAno(Usuarios usuario, int ano)
        {
            return Repository.RecuperarPorUsuario(usuario.Id).Where(x => x.DataAposta.Year == ano).Include(x => x.ApostaResultados).Include(x => x.Sorteios).ToList();
        }
        /// <summary>
        /// Obtem um conjunto de <see cref="Apostas"/> utilizando o mês e ano da aposta.
        /// </summary>
        /// <param name="usuario">Objeto <see cref="Usuarios"/> com o Id</param>
        /// <param name="ano">Ano da aposta</param>
        /// <param name="mes">Mês da aposta</param>
        /// <returns><see cref="Apostas"/></returns>
        public List<Apostas> RecuperarApostasPorMesAno(Usuarios usuario, int mes, int ano)
        {
            if (mes > 12 && mes < 1)
                throw new ArgumentOutOfRangeException("Mes", mes, "O valor do mês deve ser entre 1 (Janeiro) a 12 (Dezembro).");

            return Repository.RecuperarPorUsuario(usuario.Id).Where(x => x.DataAposta.Month == mes && x.DataAposta.Year == ano).Include(x => x.ApostaResultados).Include(x=> x.Sorteios).ToList();
        }
        /// <summary>
        /// Obtem um conjunto de <see cref="Apostas"/> utilizando a quantidade de acertos da aposta.
        /// </summary>
        /// <param name="usuario">Objeto <see cref="Usuarios"/> com o Id</param>
        /// <param name="acertos">Quantidade de acertos</param>
        /// <returns><see cref="Apostas"/></returns>
        public List<Apostas> RecuperarPorAcertos(Usuarios usuario, int acertos)
        {
            return RecuperarApostas(usuario).Where(x => x.ApostaResultados != null && x.ApostaResultados.Acertos == acertos).ToList();
        }

        private void ValidarRegraNegocio(Apostas aposta)
        {
            if (aposta.NumeroApostaExibicao == null)
                throw new Exception("É obrigatório preencher o numeros");

            if (PossuiNumerosRepetidos(aposta))
                throw new Exception("Não pode haver numeros repetidos");

            if (PossuiNumerosForaIntervalo(aposta, NumeroMinimo, NumeroMaximo))
                throw new Exception("Os valores deve ser entre 0 e 60");

            if (!PossuiQuantidadeNumeros(aposta, NumerosQuantidade))
                throw new Exception($"Deve ser preenchido somente {NumerosQuantidade} valores");
        }

        private bool PossuiQuantidadeNumeros(Apostas aposta, int qtdNumeros)
        {
            var numeros = aposta.NumeroApostaExibicao?.Split('-');
            return (numeros.Length == qtdNumeros);
        }
        private bool PossuiNumerosRepetidos(Apostas aposta)
        {
            var numeros = aposta.NumeroApostaExibicao?.Split('-');
            foreach (var item in numeros)
            {
                if (numeros.Count(x => x == item) > 1)
                    return true;
            }
            return false;

        }
        private bool PossuiNumerosForaIntervalo(Apostas aposta, int min, int max)
        {
            var numeros = aposta.NumeroApostaExibicao?.Split('-');
            foreach (var item in numeros)
            {
                if (Convert.ToInt32(item) > max || min > Convert.ToInt32(item))
                    return true;
            }
            return false;
        }
    }

}
