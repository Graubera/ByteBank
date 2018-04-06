using ByteBank.Core.Model;
using ByteBank.Core.Repository;
using ByteBank.Core.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ByteBank.View
{
    public partial class MainWindow : Window
    {
        private readonly ContaClienteRepository r_Repositorio;
        private readonly ContaClienteService r_Servico;

        public MainWindow()
        {
            InitializeComponent();

            r_Repositorio = new ContaClienteRepository();
            r_Servico = new ContaClienteService();
        }

        private void BtnProcessar_Click(object sender, RoutedEventArgs e)
        {
            TaskScheduler UITask = TaskScheduler.FromCurrentSynchronizationContext();
            BtnProcessar.IsEnabled = false;

            IEnumerable<ContaCliente> contas = r_Repositorio.GetContaClientes();
            //List<string> resultado = new List<string>();

            AtualizarView(new List<string>(), TimeSpan.Zero);

            DateTime inicio = DateTime.Now;
            ConsolidarContas(contas)
                .ContinueWith(task =>
                {
                    var fim = DateTime.Now;
                    var resultado = task.Result;
                    AtualizarView(resultado, fim - inicio);
                    BtnProcessar.IsEnabled = true;
                //}, UITask)
                //.ContinueWith(task =>
                //{
                //    BtnProcessar.IsEnabled = true;
                }, UITask);

            //Task[] contasTarefas = contas.Select(conta =>
            //{
            //    return Task.Factory.StartNew(() =>
            //    {
            //        string resultadoConta = r_Servico.ConsolidarMovimentacao(conta);
            //        resultado.Add(resultadoConta);
            //    });
            //}).ToArray();

            //Task.WhenAll(contasTarefas)
            //    .ContinueWith(task =>
            //    {
            //        DateTime fim = DateTime.Now;

            //        AtualizarView(resultado, fim - inicio);
            //    }, UITask)
            //    .ContinueWith(task =>
            //    {
            //        BtnProcessar.IsEnabled = true;
            //    }, UITask);
            Debug.Write("EEI");

        }

        private Task<List<string>> ConsolidarContas(IEnumerable<ContaCliente> contas)
        {
            var resultado = new List<string>();

            var tasks = contas.Select(conta =>
            {
                return Task.Factory.StartNew(() =>
                {
                    var contaResultado = r_Servico.ConsolidarMovimentacao(conta);
                    resultado.Add(contaResultado);
                });
            });

            return Task.WhenAll(tasks).ContinueWith(t =>
            {
                return resultado;
            });
        }
        private void AtualizarView(List<String> result, TimeSpan elapsedTime)
        {
            string tempoDecorrido = $"{ elapsedTime.Seconds }.{ elapsedTime.Milliseconds} segundos!";
            string mensagem = $"Processamento de {result.Count} clientes em {tempoDecorrido}";

            LstResultados.ItemsSource = result;
            TxtTempo.Text = mensagem;
        }
    }
}
//  CODE USED FOR TESTING
//var contasTotal = contas.Count() / 4;
//var contas_parte1 = contas.Take(contasTotal);
//var contas_parte2 = contas.Skip(contasTotal).Take(contasTotal);
//var contas_parte3 = contas.Skip(contasTotal * 2).Take(contasTotal);
//var contas_parte4 = contas.Skip(contasTotal * 3);



//Thread thread_pt1 = new Thread(() =>
//{
//    foreach (var conta in contas_parte1)
//    {
//        var resultadoConta = r_Servico.ConsolidarMovimentacao(conta);
//        resultado.Add(resultadoConta);
//    }
//});

//Thread thread_pt2 = new Thread(() =>
//{
//    foreach (var conta in contas_parte2)
//    {
//        var resultadoConta = r_Servico.ConsolidarMovimentacao(conta);
//        resultado.Add(resultadoConta);
//    }
//});

//Thread thread_pt3 = new Thread(() =>
//{
//    foreach (var conta in contas_parte3)
//    {
//        var resultadoConta = r_Servico.ConsolidarMovimentacao(conta);
//        resultado.Add(resultadoConta);
//    }
//});

//Thread thread_pt4 = new Thread(() =>
//{
//    foreach (var conta in contas_parte4)
//    {
//        var resultadoConta = r_Servico.ConsolidarMovimentacao(conta);
//        resultado.Add(resultadoConta);
//    }
//});

//thread_pt1.Start();
//            thread_pt2.Start();
//            thread_pt3.Start();
//            thread_pt4.Start();
//            while (thread_pt1.IsAlive || thread_pt2.IsAlive || thread_pt3.IsAlive || thread_pt4.IsAlive)
//            {
//                Thread.Sleep(150);
//            }
