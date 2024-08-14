using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransacaoFinanceira
{
    class Program
    {
        static void Main(string[] args)
        {
            var transacoes = new[]
            {
                new Transacao(1, "09/09/2023 14:15:00", 938485762, 2147483649, 150),
                new Transacao(2, "09/09/2023 14:15:05", 2147483649, 210385733, 149),
                new Transacao(3, "09/09/2023 14:15:29", 347586970, 238596054, 1100),
                new Transacao(4, "09/09/2023 14:17:00", 675869708, 210385733, 5300),
                new Transacao(5, "09/09/2023 14:18:00", 238596054, 674038564, 1489),
                new Transacao(6, "09/09/2023 14:18:20", 573659065, 563856300, 49),
                new Transacao(7, "09/09/2023 14:19:00", 938485762, 2147483649, 44),
                new Transacao(8, "09/09/2023 14:19:01", 573659065, 675869708, 150)
            };

            
            var orderedTransacoes = transacoes.OrderBy(t => DateTime.Parse(t.DateTime)).ToArray();

            var repositorio = new RepositorioContas();
            var executor = new ExecutorTransacao(repositorio);

            foreach (var transacao in orderedTransacoes)
            {
                executor.Transferir(transacao);
            }

           
            Console.WriteLine("Estado final das contas:");
            foreach (var conta in repositorio.GetContas())
            {
                Console.WriteLine($"Conta {conta.NumeroConta}: Saldo {conta.Saldo}");
            }
        }
    }

    public class Transacao
    {
        public int CorrelationId { get; }
        public string DateTime { get; }
        public long ContaOrigem { get; }
        public long ContaDestino { get; }
        public decimal Valor { get; }

        public Transacao(int correlationId, string dateTime, long contaOrigem, long contaDestino, decimal valor)
        {
            CorrelationId = correlationId;
            DateTime = dateTime;
            ContaOrigem = contaOrigem;
            ContaDestino = contaDestino;
            Valor = valor;
        }
    }

    public interface IRepositorioContas
    {
        Conta? GetConta(long numeroConta); 
        void AtualizarConta(Conta conta);
        IEnumerable<Conta> GetContas();
    }

    public class RepositorioContas : IRepositorioContas
    {
        private readonly ConcurrentDictionary<long, Conta> _contas;

        public RepositorioContas()
        {
            _contas = new ConcurrentDictionary<long, Conta>
            {
                [938485762] = new Conta(938485762, 180),
                [347586970] = new Conta(347586970, 1200),
                [2147483649] = new Conta(2147483649, 0),
                [675869708] = new Conta(675869708, 4900),
                [238596054] = new Conta(238596054, 478),
                [573659065] = new Conta(573659065, 787),
                [210385733] = new Conta(210385733, 10),
                [674038564] = new Conta(674038564, 400),
                [563856300] = new Conta(563856300, 1200)
            };
        }

        public Conta? GetConta(long numeroConta)
        {
            _contas.TryGetValue(numeroConta, out var conta);
            return conta;
        }

        public void AtualizarConta(Conta conta)
        {
            _contas[conta.NumeroConta] = conta;
        }

        public IEnumerable<Conta> GetContas()
        {
            return _contas.Values;
        }
    }

    public class Conta
    {
        public long NumeroConta { get; }
        public decimal Saldo { get; private set; }

        public Conta(long numeroConta, decimal saldo)
        {
            NumeroConta = numeroConta;
            Saldo = saldo;
        }

        public bool Debitar(decimal valor)
        {
            if (Saldo >= valor)
            {
                Saldo -= valor;
                return true;
            }
            return false;
        }

        public void Creditar(decimal valor)
        {
            Saldo += valor;
        }
    }

    public class ExecutorTransacao
    {
        private readonly IRepositorioContas _repositorio;

        public ExecutorTransacao(IRepositorioContas repositorio)
        {
            _repositorio = repositorio;
        }

        public void Transferir(Transacao transacao)
        {
            var contaOrigem = _repositorio.GetConta(transacao.ContaOrigem);
            var contaDestino = _repositorio.GetConta(transacao.ContaDestino);

            if (contaOrigem == null || contaDestino == null)
            {
                Console.WriteLine($"Transação número {transacao.CorrelationId} foi cancelada: Conta não encontrada.");
                return;
            }

            if (!contaOrigem.Debitar(transacao.Valor))
            {
                Console.WriteLine($"Transação número {transacao.CorrelationId} foi cancelada por falta de saldo.");
                return;
            }

            contaDestino.Creditar(transacao.Valor);

            _repositorio.AtualizarConta(contaOrigem);
            _repositorio.AtualizarConta(contaDestino);

            Console.WriteLine($"Transação número {transacao.CorrelationId} foi efetivada com sucesso! Novos saldos: Conta Origem: {contaOrigem.Saldo} | Conta Destino: {contaDestino.Saldo}");
        }
    }
}
