﻿using System;
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
            LblData.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void fechar_Modal(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void salvarTarefa(object sender, EventArgs e)
        {
 
            Tarefa tarefa = new Tarefa();
            tarefa.Nome = Nome.Text;
            tarefa.Nota = Nota.Text;
            tarefa.Data = Data.Date;
            tarefa.HorarioFinal = HorarioFinal.Time;
            tarefa.HorarioInicial = HorarioInicial.Time;
            tarefa.Finalizado = false;

            if (await ValidacaoAsync(tarefa))
            {
                bool certo = await new TarefaDB().CadastrarAsync(tarefa);
                if (certo)
                {
                    MessagingCenter.Send<Listar, Tarefa>(new Listar(), "OnTarefaCadastrada", tarefa);
                    await Navigation.PopModalAsync();
                }

            }
        }

        private async Task<bool> ValidacaoAsync(Tarefa tarefa)
        {
            bool validacao = true;
            if (tarefa.HorarioInicial > tarefa.HorarioFinal)
            {
                await DisplayAlert("Erro", "O horario inicial não pode ser maior que o horario de termino", "OK");
                validacao = false;
            }
            if (tarefa.Nome == null)
            {
                await DisplayAlert("Erro", "O nome da tarefa precisa ser preenchido!", "OK");
                validacao = false;
            }

            if (tarefa.Nome != null && tarefa.Nome.Length < 5)
            {
                await DisplayAlert("Erro", "O nome da tarefa precisa ter pelo menos 5 caracteres!", "OK");
                validacao = false;
            }

            return validacao;
        }

        private void Data_Unfocused(object sender, FocusEventArgs e)
        {
            LblData.Text = ((DatePicker)sender).Date.ToString("dd/MM/yyyy");
        }

        private void AcionarDatePicker(object sender, EventArgs e)
        {
            Data.Focus();
        }

        private void AcionarTimePicker(object sender, EventArgs e)
        {
            HorarioInicial.Focus();
        }

        private void HorarioInicial_Unfocused(object sender, FocusEventArgs e)
        {
            lblHorarioInicial.Text = ((TimePicker)sender).Time.ToString(@"hh\:mm");
            HorarioFinal.Focus();
        }

        private void HorarioFinal_Unfocused(object sender, FocusEventArgs e)
        {
            lblHorarioFinal.Text = ((TimePicker)sender).Time.ToString(@"hh\:mm");
        }
    }
}