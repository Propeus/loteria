using Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace Service
{
    /// <summary>
    /// Serviço para a entidade <see cref="Sorteios"/>
    /// </summary>
    public class SorteioService : GenericoService<SorteioRepository>
    {
        private int NumerosQuantidade { get; set; } = 6;
        private int NumeroMinimo { get; set; } = 0;
        private int NumeroMaximo { get; set; } = 60;


        ApostasRepository apostasRepository { get; set; }

        /// <summary>
        /// Instacia do serviço criando um novo contexto para o repositório
        /// </summary>
        public SorteioService()
        {
            apostasRepository = new ApostasRepository(Repository.RepositoryFactory);
        }
        /// <summary>
        /// Instacia do serviço utilizando um contexto já existente
        /// </summary>
        /// <param name="repositoryFactory">Contexto de algum repositório ja instanciado</param>
        public SorteioService(DbContext repositoryFactory) : base(repositoryFactory)
        {
            apostasRepository = new ApostasRepository(Repository.RepositoryFactory);

        }

        /// <summary>
        /// Insere um novo sorteio
        /// </summary>
        /// <param name="sorteios">Modelo preenchido</param>
        /// <returns><see cref="Sorteios"/></returns>
        public Sorteios InserirSorteio(Sorteios sorteios)
        {
            ValidarRegraNegocio(sorteios);
            using (TransactionScope scope = new TransactionScope())
            {
                Repository.Inserir(sorteios);
                var aux = apostasRepository.RecuperarVarios(x => x.IdSorteio == null && x.DataAposta <= sorteios.DataSorteio);
                foreach (var item in aux)
                {
                    item.Sorteios = sorteios;
                }
                VerificarGanhadores(aux);
                apostasRepository.EditarLote(aux);
                scope.Complete();
            }
            return sorteios;
        }

        /// <summary>
        /// Obtem um conjunto de <see cref="Sorteios"/> utilizando o mês do sorteio.
        /// </summary>
        /// <param name="mes">Mês do sorteio</param>
        /// <returns><see cref="Sorteios"/></returns>
        public List<Sorteios> RecuperarSorteiosPorMes(int mes)
        {
            if (mes > 12 && mes < 1)
                throw new ArgumentOutOfRangeException("Mes", mes, "O valor do mês deve ser entre 1 (Janeiro) a 12 (Dezembro).");

            return Repository.RecuperarPorMes(mes).ToList();
        }
        /// <summary>
        /// Obtem um conjunto de <see cref="Sorteios"/> utilizando o ano do sorteio.
        /// </summary>
        /// <param name="ano">Ano do sorteio</param>
        /// <returns><see cref="Sorteios"/></returns>
        public List<Sorteios> RecuperarSorteiosPorAno(int ano)
        {
            return Repository.RecuperarPorAno(ano).ToList();
        }
        /// <summary>
        /// Obtem um conjunto de <see cref="Sorteios"/> utilizando o mes e ano do sorteio.
        /// </summary>
        /// <param name="mes">Mês do sorteio</param>
        /// <param name="ano">Ano do sorteio</param>
        /// <returns><see cref="Sorteios"/></returns>
        public List<Sorteios> RecuperarSorteiosPorMesAno(int mes, int ano)
        {
            if (mes > 12 && mes < 1)
                throw new ArgumentOutOfRangeException("Mes", mes, "O valor do mês deve ser entre 1 (Janeiro) a 12 (Dezembro).");

            return Repository.RecuperarPorMesAno(mes, ano).ToList();
        }

        private void ValidarRegraNegocio(Sorteios sorteios)
        {
            if (sorteios.NumeroSorteioExibicao == null)
                throw new Exception("É obrigatório preencher o numeros");

            if (PossuiNumerosRepetidos(sorteios))
                throw new Exception("Não pode haver numeros repetidos");

            if (PossuiNumerosForaIntervalo(sorteios, NumeroMinimo, NumeroMaximo))
                throw new Exception("Os valores deve ser entre 0 e 60");

            if (!PossuiQuantidadeNumeros(sorteios, NumerosQuantidade))
                throw new Exception($"Deve ser preenchido somente {NumerosQuantidade} valores");
        }

        private bool PossuiQuantidadeNumeros(Sorteios sorteios, int qtdNumeros)
        {
            var numeros = sorteios.NumeroSorteioExibicao?.Split('-');
            return (numeros.Length == qtdNumeros);
        }
        private bool PossuiNumerosRepetidos(Sorteios sorteios)
        {
            var numeros = sorteios.NumeroSorteioExibicao?.Split('-');
            foreach (var item in numeros)
            {
                if (numeros.Count(x => x == item) > 1)
                    return true;
            }
            return false;

        }
        private bool PossuiNumerosForaIntervalo(Sorteios sorteios, int min, int max)
        {
            var numeros = sorteios.NumeroSorteioExibicao?.Split('-');
            foreach (var item in numeros)
            {
                if (Convert.ToInt32(item) > max || min > Convert.ToInt32(item))
                    return true;
            }
            return false;
        }
        private Sorteios SortearValor()
        {
            var resultado = Helper.Helper.SortearValores(NumerosQuantidade, NumeroMinimo, NumeroMaximo);
            int numero = Convert.ToInt32(string.Join("", resultado));
            string numeroExbicao = string.Join("-", resultado);

            Sorteios sorteo = new Sorteios
            {
                DataSorteio = DateTime.Now,
                NumeroSorteio = numero,
                NumeroSorteioExibicao = numeroExbicao
            };

            return sorteo;
        }
        private Apostas AvaliarAcertos(Apostas aposta)
        {
            string[] numerosSorteio = aposta.Sorteios?.NumeroSorteioExibicao.Split('-');
            if (numerosSorteio == null)
                return aposta;

            aposta.ApostaResultados = new ApostaResultados();

            string[] numerosAposta = aposta.NumeroApostaExibicao.Split('-');

            LinkedList<int> numerosAcertos = new LinkedList<int>();

            for (int i = 0; i < numerosSorteio.Length; i++)
            {
                if (numerosSorteio[i] == numerosAposta[i])
                {
                    aposta.ApostaResultados.Acertos++;
                    numerosAcertos.AddLast(Convert.ToInt32(numerosAposta[i]));
                }
            }

            aposta.ApostaResultados.NumerosAcertados = string.Join("-", numerosAcertos.ToArray());
            return aposta;
        }
        private IQueryable<Apostas> VerificarGanhadores(IQueryable<Apostas> resultados)
        {
            foreach (var item in resultados)
            {
                AvaliarAcertos(item);
            }
            var ganhadores = resultados.Where(x => x.ApostaResultados.Acertos >= 4);
            for (int i = 4; i <= 6; i++)
            {
                int ganhadoresptn = ganhadores.Count(x => x.ApostaResultados.Acertos == i);
                foreach (var resutado in resultados)
                {

                    if (resutado.ApostaResultados.Acertos == 4)
                    {
                        resutado.ApostaResultados.ValorPremio = ((resutado.Sorteios.ValorPremio / 100) * 10) / ganhadoresptn;
                    }
                    if (resutado.ApostaResultados.Acertos == 5)
                    {
                        resutado.ApostaResultados.ValorPremio = ((resutado.Sorteios.ValorPremio / 100) * 20) / ganhadoresptn;
                    }
                    if (resutado.ApostaResultados.Acertos == 6)
                    {
                        resutado.ApostaResultados.ValorPremio = ((resutado.Sorteios.ValorPremio / 100) * 70) / ganhadoresptn;
                    }


                }
            }

            return resultados;
        }
    }
}
