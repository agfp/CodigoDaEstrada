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
    public sealed partial class Carro : Page
    {
        private Questoes _questoes;
        private int _current = 0;
        private Button[] _navigationButtons;

        public Carro()
        {
            this.InitializeComponent();
            _navigationButtons = new Button[] {
                btnNavegacao1, btnNavegacao2, btnNavegacao3, btnNavegacao4, btnNavegacao5, btnNavegacao6, btnNavegacao7, btnNavegacao8, btnNavegacao9, btnNavegacao10, 
                btnNavegacao11, btnNavegacao12, btnNavegacao13, btnNavegacao14, btnNavegacao15, btnNavegacao16, btnNavegacao17, btnNavegacao18, btnNavegacao19, btnNavegacao20, 
                btnNavegacao21, btnNavegacao22, btnNavegacao23, btnNavegacao24, btnNavegacao25, btnNavegacao26, btnNavegacao27, btnNavegacao28, btnNavegacao29, btnNavegacao30
            };
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            _questoes = (Questoes)e.Parameter;
            if (_questoes.EstáTerminado)
            {
                btnRespostaA.Click -= new RoutedEventHandler(ButtonResposta_Click);
                btnRespostaB.Click -= new RoutedEventHandler(ButtonResposta_Click);
                btnRespostaC.Click -= new RoutedEventHandler(ButtonResposta_Click);
                tbkTimer.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                
                SolidColorBrush verde = new SolidColorBrush(Colors.Green);
                SolidColorBrush vermelho = new SolidColorBrush(Colors.Red);

                for (int i = 0; i < _questoes.NumPerguntas; i++)
                {
                    _navigationButtons[i].Background = _questoes.EstáCorreto(i) ? verde : vermelho;
                }
                CarregarPergunta();
            }
            else
            {
                await _questoes.Carregar();
                CarregarPergunta();
                RunTimer();
            }
        }

        private void ButtonResposta_Click(object sender, RoutedEventArgs e)
        {
            Button botao = (Button)sender;
            int resposta;
            if (botao.Content.ToString() == "A")
            {
                resposta = 0;
            }
            else if (botao.Content.ToString() == "B")
            {
                resposta = 1;
            }
            else
            {
                resposta = 2;
            }
            _questoes.Lista.ElementAt(_current).Resposta = resposta;
            _navigationButtons[_current].Background = new SolidColorBrush(Colors.Green);
            _current++;
            if (_current == _questoes.NumPerguntas) _current = 0;
            CarregarPergunta();

        }

        private void ButtonNavegacao_Click(object sender, RoutedEventArgs e)
        {
            DestacarNavegacao();
            _current = Convert.ToInt16(((Button)sender).Content.ToString()) - 1;
            CarregarPergunta();
        }

        private void btnTerminar_Click(object sender, RoutedEventArgs e)
        {
            Terminar();
        }

        private async void RunTimer()
        {
            DateTime time = DateTime.MinValue.AddMinutes(30);
            TimeSpan ts = TimeSpan.FromSeconds(1);
            while (time != DateTime.MinValue)
            {
                tbkTimer.Text = time.ToString("mm:ss");
                await Task.Delay(ts);
                time = time.Subtract(ts);
                if (_questoes.EstáTerminado) return;
            }
            Terminar();
        }

        private void CarregarPergunta()
        {
            tbkPergunta.Text = _questoes.Lista.ElementAt(_current).Pergunta;
            tbkResposta1.Text = _questoes.Lista.ElementAt(_current).Opcoes[0].Value;
            tbkResposta2.Text = _questoes.Lista.ElementAt(_current).Opcoes[1].Value;
            if (_questoes.Lista.ElementAt(_current).Opcoes.Count() == 3)
            {
                btnRespostaC.Visibility = Windows.UI.Xaml.Visibility.Visible;
                tbkResposta3.Text = _questoes.Lista.ElementAt(_current).Opcoes[2].Value;
            }
            else
            {
                btnRespostaC.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                tbkResposta3.Text = String.Empty;
            }

            
            BitmapImage imagem = new BitmapImage();
            imagem.UriSource = new Uri("ms-appx:/Imagens/" + _questoes.Lista.ElementAt(_current).Imagem);
            imgPergunta.Source = imagem;
            btnRespostaA.Background = btnRespostaB.Background = btnRespostaC.Background = new SolidColorBrush(Colors.Transparent);
            _navigationButtons[_current].Background = new SolidColorBrush(Colors.Blue);
            DestacarOpcoes();
        }

        private void DestacarOpcoes()
        {
            SolidColorBrush verde = new SolidColorBrush(Colors.Green);
            SolidColorBrush vermelho = new SolidColorBrush(Colors.Red);

            Button[] respostas = new Button[] { btnRespostaA, btnRespostaB, btnRespostaC };

            if (_questoes.EstáTerminado)
            {
                if (_questoes.EstáCorreto(_current))
                {
                    respostas[_questoes.Lista.ElementAt(_current).Resposta].Background = verde;
                }
                else
                {
                    respostas[_questoes.Lista.ElementAt(_current).Resposta].Background = vermelho;
                    respostas[_questoes.QualÉCorreta(_current)].Background = verde;
                }
            }
            else
            {
                if (_questoes.Lista.ElementAt(_current).Resposta != -1)
                {
                    respostas[_questoes.Lista.ElementAt(_current).Resposta].Background = verde;
                }
            }
        }

        private void DestacarNavegacao()
        {
            SolidColorBrush transparente = new SolidColorBrush(Colors.Transparent);
            SolidColorBrush verde = new SolidColorBrush(Colors.Green);
            SolidColorBrush vermelho = new SolidColorBrush(Colors.Red);

            if (_questoes.EstáTerminado)
            {
                _navigationButtons[_current].Background =
                    _questoes.EstáCorreto(_current) ? verde : vermelho;

            }
            else
            {
                _navigationButtons[_current].Background =
                    _questoes.Lista.ElementAt(_current).Resposta != -1 ? verde : transparente;
                
            }
        }

        private void Terminar()
        {
            _questoes.EstáTerminado = true;
            Frame.Navigate(typeof(Resultado), _questoes);
        }
    }
}
