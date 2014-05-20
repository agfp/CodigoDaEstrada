using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace CodigoDaEstrada
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Mota : Page
    {
        private Questoes _questoes;
        private int _current = 0;
        private bool _finished = false;
        private Button[] _navigationButtons;

        public Mota()
        {
            this.InitializeComponent();
            _navigationButtons = new Button[] {
                btnNavegacao1, btnNavegacao2, btnNavegacao3, btnNavegacao4, btnNavegacao5, btnNavegacao6, btnNavegacao7, btnNavegacao8, btnNavegacao9, btnNavegacao10, 
                
            };
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //_questoes = new Questoes();
            //_questoes.NumPerguntas = 10;
            //await _questoes.Carregar(e.Parameter.ToString());
            //CarregarPergunta();
            //RunTimer();
        }

        private void ButtonResposta_Click(object sender, RoutedEventArgs e)
        {
            Button botao = (Button)sender;
            int resposta;
            if (botao.Content.ToString() == "A")
                resposta = 0;
            else if (botao.Content.ToString() == "B")
                resposta = 1;
            else
                resposta = 2;

            _questoes.Lista.ElementAt(_current).Resposta = resposta;
            _navigationButtons[_current].Background = new SolidColorBrush(Colors.Green);
            _current++;
            if (_current == _questoes.NumPerguntas) _current = 0;
            CarregarPergunta();

        }

        private void ButtonNavegacao_Click(object sender, RoutedEventArgs e)
        {
            Button botao = (Button)sender;
            if (_questoes.Lista.ElementAt(_current).Resposta != -1)
            {
                _navigationButtons[_current].Background = new SolidColorBrush(Colors.Green);
            }
            else
            {
                _navigationButtons[_current].Background = new SolidColorBrush(Colors.Transparent);
            }
            _current = Convert.ToInt16(botao.Content.ToString()) - 1;
            CarregarPergunta();
        }

        private void btnTerminar_Click(object sender, RoutedEventArgs e)
        {
            Terminar();
        }

        private async void RunTimer()
        {
            DateTime time = DateTime.MinValue.AddMinutes(10);
            TimeSpan ts = TimeSpan.FromSeconds(1);
            while (time != DateTime.MinValue)
            {
                tbkTimer.Text = time.ToString("mm:ss");
                await Task.Delay(ts);
                time = time.Subtract(ts);
                if (_finished) return;
            }
            Terminar();
        }

        private void CarregarPergunta()
        {
            tbkPergunta.Text = _questoes.Lista.ElementAt(_current).Pergunta;
            tbkResposta1.Text = _questoes.Lista.ElementAt(_current).Opcoes[0].Value;
            tbkResposta2.Text = _questoes.Lista.ElementAt(_current).Opcoes[1].Value;
            tbkResposta3.Text = _questoes.Lista.ElementAt(_current).Opcoes[2].Value;
            BitmapImage imagem = new BitmapImage();
            imagem.UriSource = new Uri("ms-appx:/Imagens/" + _questoes.Lista.ElementAt(_current).Imagem);
            imgPergunta.Source = imagem;
            btnRespostaA.Background = btnRespostaB.Background = btnRespostaC.Background = new SolidColorBrush(Colors.Transparent);
            if (_questoes.Lista.ElementAt(_current).Resposta != -1)
            {
                SolidColorBrush color = new SolidColorBrush(Colors.Green);
                if (_questoes.Lista.ElementAt(_current).Resposta == 0)
                {
                    btnRespostaA.Background = color;
                }
                else if (_questoes.Lista.ElementAt(_current).Resposta == 1)
                {
                    btnRespostaB.Background = color;
                }
                else
                {
                    btnRespostaC.Background = color;
                }
            }
            _navigationButtons[_current].Background = new SolidColorBrush(Colors.Blue);
        }

        private void Terminar()
        {
            _finished = true;
            Frame.Navigate(typeof(Resultado), _questoes);
        }
    }
}
