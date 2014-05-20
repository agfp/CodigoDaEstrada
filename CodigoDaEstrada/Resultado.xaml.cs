using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CodigoDaEstrada
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Resultado : Page
    {
        Questoes _questoes;

        public Resultado()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _questoes = (Questoes)e.Parameter;
            int pontos = _questoes.CalculaPontuacao();
            int errou = _questoes.NumPerguntas - pontos;

            Pontuacoes.Salvar(_questoes.Categoria, errou);

            tbkResultado2.Text = "Errou " + errou.ToString() + " perguntas";
            if (errou <= 3)
            {
                tbkResultado1.Text = "Passou";
                tbkResultado1.Foreground = tbkResultado2.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                tbkResultado1.Text = "Chumbou";
                tbkResultado1.Foreground = tbkResultado2.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void btnVerRespostas_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Carro), _questoes);
        }
    }
}
