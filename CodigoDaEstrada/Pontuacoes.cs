using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace CodigoDaEstrada
{
    public class Pontuacoes
    {
        public class Pontuacao
        {
            public string Data { get; set; }
            public string Categoria { get; set; }
            public int Pontos { get; set; }
            [XmlIgnore]
            public SolidColorBrush Cor
            {
                get
                {
                    if ((Categoria == "Carro" && Pontos <= 3) ||
                        (Categoria == "Mota" && Pontos <= 1) ||
                        (Categoria == "Pesados" && Pontos <= 2))
                    {

                        return new SolidColorBrush(Colors.LightGreen);
                    }
                    else
                    {
                        return new SolidColorBrush(Colors.Red);
                    }
                }
            }
        }

        private static ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

        public static List<Pontuacao> Historico
        {
            get
            {
                if (_localSettings.Values.ContainsKey("Pontuacao") && !String.IsNullOrEmpty(_localSettings.Values["Pontuacao"].ToString()))
                {
                    string xml = _localSettings.Values["Pontuacao"].ToString();
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(xml);
                    MemoryStream stream = new MemoryStream(buffer);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Pontuacao>));
                    stream.Position = 0;
                    List<Pontuacao> listaPontuacao = (List<Pontuacao>)serializer.Deserialize(stream);
                    stream.Dispose();
                    return listaPontuacao.OrderByDescending(p => p.Data).ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        public static void Salvar(string categoria, int pontos)
        {
            Pontuacao pontuacao = new Pontuacao
            {
                Data = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                Categoria = categoria,
                Pontos = pontos
            };

            List<Pontuacao> historico = Historico;
            if (historico != null)
            {
                historico.Add(pontuacao);
            }
            else
            {
                historico = new List<Pontuacao> { pontuacao };
            }
            XmlSerializer serializer = new XmlSerializer(typeof(List<Pontuacao>));
            MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, historico);
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            string xml = reader.ReadToEnd();
            stream.Dispose();
            reader.Dispose();

            if (_localSettings.Values.ContainsKey("Pontuacao"))
            {
                _localSettings.Values["Pontuacao"] = xml;
            }
            else
            {
                _localSettings.Values.Add("Pontuacao", xml);
            }
        }

        public static void Limpar()
        {
            _localSettings.Values.Remove("Pontuacao");
        }
    }
}
