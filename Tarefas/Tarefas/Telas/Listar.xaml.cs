using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarefas.Banco;
using Tarefas.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tarefas.Telas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Listar : ContentPage
    {
        ObservableCollection<Tarefa> lista;
        public Listar()
        {
            InitializeComponent();
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    lista = new ObservableCollection<Tarefa>(await new TarefaDB().PesquisarAsync(DateTime.Now));
                });
                CVlistaTarefas.ItemsSource = lista;
            });
            MessagingCenter.Subscribe<Listar, Tarefa>(this, "OnTarefaCadastrada", (sender, tarefa) =>
            {
                if(lista != null)
                    lista.Add(tarefa);
            });

        }

        private void btn_AbrirCadastro(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Cadastrar());
        }

        private void btn_Visualizar(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Telas.Visualizar());
        }
    }
}