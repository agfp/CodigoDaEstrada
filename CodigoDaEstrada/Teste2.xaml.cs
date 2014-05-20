using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class Teste : Page
    {
        public Teste()
        {
            this.InitializeComponent();

            CarregarQuestoes();

        }

        public async void CarregarQuestoes()
        {
            StorageFolder sf = Package.Current.InstalledLocation;
            StorageFile sf2 = await sf.GetFileAsync("Perguntas.xml");
            string bla = await FileIO.ReadTextAsync(sf2, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            XElement xml = XElement.Parse(bla);

            List<Questao> questoes = new List<Questao>();

            foreach (var x in xml.Element("Carros").Elements("Questao"))
            {
                questoes.Add(new Questao
                {
                    Pergunta = x.Element("Pergunta").Value,
                    RespostaCorreta = x.Element("Resposta").Value,
                    RespostaIncorreta1 = x.Elements("Errado").ElementAt(0).Value,
                    RespostaIncorreta2 = x.Elements("Errado").ElementAt(1).Value
                });
            }

        }

        class Questao
        {
            public string Pergunta { get; set; }
            public string RespostaCorreta { get; set; }
            public string RespostaIncorreta1 { get; set; }
            public string RespostaIncorreta2 { get; set; }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
