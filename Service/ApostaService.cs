using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Repository;
using Service.Helper;
using Service.Model;

namespace Service
{
    public class ApostaService : GenericoService<ApostasRepository>
    {

        private int NumerosQuantidade { get; set; } = 6;
        private int NumeroMinimo { get; set; } = 0;
        private int NumeroMaximo { get; set; } = 60;


        SorteioService sorteioService { get; set; }

        public ApostaService()
        {
            sorteioService = new SorteioService(Repository.RepositoryFactory);
        }
        public ApostaService(DbContext repositoryFactory) : base(repositoryFactory)
        {
            sorteioService = new SorteioService(repositoryFactory);
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
            var resultado = Repository.RecuperarVarios(x => x.NumeroAposta == numeroaposta).Include(x => x.Usuarios);
            return resultado.Select(x => x.Usuarios).ToList();
        }
        public List<Apostas> RecuperarApostas(Usuarios usuario)
        {
            return Repository.RecuperarPorUsuario(usuario.Id).Include(x => x.Sorteios).Include(x => x.ApostaResultados).ToList();
        }
        public List<Apostas> RecuperarApostasPorMes(Usuarios usuario, int mes)
        {
            if (mes > 12 && mes < 1)
                throw new ArgumentOutOfRangeException("Mes", mes, "O valor do mês deve ser entre 1 (Janeiro) a 12 (Dezembro).");

            return Repository.RecuperarPorUsuario(usuario.Id).Where(x => x.DataAposta.Month == mes).Include(x => x.ApostaResultados).ToList();
        }
        public List<Apostas> RecuperarApostasPorAno(Usuarios usuario, int ano)
        {
            return Repository.RecuperarPorUsuario(usuario.Id).Where(x => x.DataAposta.Year == ano).Include(x => x.ApostaResultados).ToList();
        }
        public List<Apostas> RecuperarApostasPorMesAno(Usuarios usuario, int mes, int ano)
        {
            if (mes > 12 && mes < 1)
                throw new ArgumentOutOfRangeException("Mes", mes, "O valor do mês deve ser entre 1 (Janeiro) a 12 (Dezembro).");

            return Repository.RecuperarPorUsuario(usuario.Id).Where(x => x.DataAposta.Month == mes && x.DataAposta.Year == ano).Include(x => x.ApostaResultados).ToList();
        }
        public List<Apostas> RecuperarPorAcertos(Usuarios usuario, int acertos)
        {
            return RecuperarApostas(usuario).Where(x => x.ApostaResultados.Acertos == acertos).ToList();
        }
        //public List<ApostaResultados> RecuperarResultados(Usuarios usuario)
        //{

        //    var apostas = RecuperarApostas(usuario);
        //    return= apostas.Select(x => x.ApostaResultados).ToList();

        //}
        //public List<ApostaResultados> RecuperarResultadosPorMes(Usuarios usuario, int mes)
        //{

        //    var apostas = RecuperarApostasPorMes(usuario, mes);
        //    return apostas.Select(x => x.ApostaResultados).ToList();
        //}
        //public List<ResultadoAposta> RecuperarResultadosPorAno(Usuarios usuario, int ano)
        //{
        //    var apostas = RecuperarApostasPorAno(usuario, ano);
        //    List<ResultadoAposta> resultados = apostas.Select(x => new ResultadoAposta { Aposta = x, Sorteio = x.Sorteios }).ToList();
        //    return VerificarGanhadores(resultados);
        //}
        //public List<ResultadoAposta> RecuperarResultadosPorMesAno(Usuarios usuario, int mes, int ano)
        //{

        //    var apostas = RecuperarApostasPorMesAno(usuario, mes, ano);
        //    List<ResultadoAposta> resultados = apostas.Select(x => new ResultadoAposta { Aposta = x, Sorteio = x.Sorteios }).ToList();
        //    return VerificarGanhadores(resultados);
        //}




    }

}
