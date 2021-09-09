using System;
using System.Collections.Generic;
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
    public partial class Cadastrar : ContentPage
    {
        public Cadastrar()
        {
            InitializeComponent();
        }

        private void fechar_Modal(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void salvarTarefa(object sender, EventArgs e)
        {
            //TO-DO: Peagr dados e criar tarefa 
            Tarefa tarefa = new Tarefa();
            tarefa.Nome = Nome.Text;
            tarefa.Nota = Nota.Text;
            tarefa.Data = Data.Date;
            tarefa.HorarioFinal = HorarioFinal.Time;
            tarefa.HorarioInicial = HorarioFinal.Time;
            tarefa.Finalizado = false;

            //TO-DO: Validacao dos dados

            //TO-DO: Salvar tarefa no banco
            if(await new TarefaDB().CadastrarAsync(tarefa))
            {
                MessagingCenter.Send<Listar, Tarefa>(new Listar(), "OnTarefaCadastrada", tarefa);
                await Navigation.PopModalAsync();
            }

            //TO-DO: Retornar para listagem
        }
    }
}