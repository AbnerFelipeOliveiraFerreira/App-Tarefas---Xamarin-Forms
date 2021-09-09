using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarefas.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tarefas.Telas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Visualizar : ContentPage
    {
        public Visualizar()
        {
            InitializeComponent();
        }

        public Visualizar(Tarefa tarefa)
        {
            InitializeComponent();
            BindingContext = tarefa;
        }
        private void btn_Voltar(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}