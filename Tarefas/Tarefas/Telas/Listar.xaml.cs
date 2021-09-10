using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tarefas.Banco;
using Tarefas.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tarefas.Telas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Listar : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Tarefa> _lista;
        public ObservableCollection<Tarefa> Lista
        {
            get
            {
                return _lista;
            }
            set
            {
                _lista = value;
                NotifyPropertyChanged("Lista");
            }
        }

        public Listar()
        {
            InitializeComponent();

            AtualizarDataCalendario(DateTime.Now);


            MessagingCenter.Subscribe<Listar, Tarefa>(this, "OnTarefaCadastrada", (sender, tarefa) =>
            {
                if (Lista != null)
                    Lista.Add(tarefa);
            });

        }

        private void btn_AbrirCadastro(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Cadastrar());
        }

        private void btn_Visualizar(object sender, EventArgs e)
        {
            var evento = (TappedEventArgs)e;
            var tarefa = (Tarefa)evento.Parameter;
            Navigation.PushAsync(new Telas.Visualizar(tarefa));
        }

        private async void excluir(object sender, EventArgs e)
        {
            bool pergunta = await DisplayAlert("Excluir", "Tem certeza que deseja excluir esse registro?", "Sim", "NÃO");
            if (pergunta)
            {
                //TO-DO: Excluir do banco
                var swipeItem = (SwipeItem)sender;
                Tarefa tarefa = (Tarefa)swipeItem.CommandParameter;
                var excluido = await new TarefaDB().ExcluirAsync(tarefa.Id);
                if (excluido)
                {
                    Lista.Remove(tarefa);
                }
                //TO-DO: Atualizar tarefa no banco
            }
        }

        private async void CheckBox_Action(object sender, CheckedChangedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            var label = checkbox.Parent.FindByName<Label>("lblTarefaDetalhe");
            if (label != null)
            {
                var tapGesture = (TapGestureRecognizer)label.GestureRecognizers[0];
                if (tapGesture != null)
                {
                    var tarefa = (Tarefa)tapGesture.CommandParameter;
                    if (tarefa != null)
                    { 
                        await new TarefaDB().AtualizarAsync(tarefa);
                        Tachado(label, tarefa.Finalizado);
                    }
                }
            }

        }

        

        private void AbrirCalendario(object sender, EventArgs e)
        {
            DPCalendario.Focus();

        }

        private void DateSelectAcation(object sender, DateChangedEventArgs e)
        {
            AtualizarDataCalendario(e.NewDate);
        }

        private void AtualizarDataCalendario(DateTime date)
        {
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    Lista = new ObservableCollection<Tarefa>(
                        await new TarefaDB().PesquisarAsync(date)
                    );
                    CVlistaTarefas.ItemsSource = Lista;
                    qtdTarefas.Text = Lista.Count.ToString();
                });
            });

            var idioma = CultureInfo.CurrentCulture;

            Dia.Text = date.Day.ToString();
            Mes.Text = PrimeiraLetraMaiuscula(date.ToString("MMMM", idioma).Substring(0, 3));
            DiaSemana.Text = PrimeiraLetraMaiuscula(idioma.DateTimeFormat.GetDayName(date.DayOfWeek).ToString());
        }

        private string PrimeiraLetraMaiuscula(string palavra)
        {
            return char.ToUpper(palavra[0]) + palavra.Substring(1);
        }

        private void Tachado(Label label, bool finalizado)
        {
            if (finalizado)
            {
                label.TextDecorations = TextDecorations.Strikethrough;
            }
            else
            {
                label.TextDecorations = TextDecorations.None;
            }
        }
    }
}