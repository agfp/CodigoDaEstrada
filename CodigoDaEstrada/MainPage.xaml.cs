using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            lvPontuacoes.ItemsSource = Pontuacoes.Historico;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnMota_Click(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(Mota), "Mota");
        }

        private void btnCarro_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Carro), new Questoes("Carro", 30));
        }

        private void btnPesados_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            Pontuacoes.Limpar();
            lvPontuacoes.ItemsSource = Pontuacoes.Historico;
        }
    }
}
