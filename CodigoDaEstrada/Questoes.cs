using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Storage;

namespace CodigoDaEstrada
{
    class Questoes
    {
        public class Questao
        {
            public string Pergunta { get; set; }
            public List<XElement> Opcoes { get; set; }
            public string Imagem { get; set; }
            public int Resposta { get; set; }
        }

        private List<Questao> _questoes = new List<Questao>();
        public List<Questao> Lista { get { return _questoes; } }
        public int NumPerguntas { get; set; }
        public string Categoria { get; set; }
        public int MinimoPontos { get { return (int)Math.Floor(0.9 * this.NumPerguntas); } }
        public bool EstáTerminado { get; set; }

        public Questoes(string categoria, int numPerguntas)
        {
            this.Categoria = categoria;
            this.NumPerguntas = numPerguntas;
        }

        public async Task<bool> Carregar()
        {
            StorageFolder folder = Package.Current.InstalledLocation;
            StorageFile file = await folder.GetFileAsync("Perguntas.xml");
            string fileContent = await FileIO.ReadTextAsync(file, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            XElement xml = XElement.Parse(fileContent);

            foreach (XElement e in xml.Element(this.Categoria).Elements("Questao"))
            {
                Questao q = new Questao
                {
                    Pergunta = e.Element("Pergunta").Value,
                    Imagem = e.Element("Imagem").Value,
                    Resposta = -1
                };
                if (e.Elements("Errado").Count() == 1)
                {
                    q.Opcoes = (new List<XElement> {
                                    e.Element("Resposta"),
                                    e.Elements("Errado").ElementAt(0)}).OrderBy(a => Guid.NewGuid()).ToList();
                }
                else
                {
                    q.Opcoes = (new List<XElement> {
                                    e.Element("Resposta"),
                                    e.Elements("Errado").ElementAt(0),
                                    e.Elements("Errado").ElementAt(1) }).OrderBy(a => Guid.NewGuid()).ToList();
                }
                _questoes.Add(q);
            }
            _questoes = _questoes.OrderBy(a => Guid.NewGuid()).ToList();

            return true;
        }

        public int CalculaPontuacao()
        {
            return (from q in _questoes
                    where (from o in q.Opcoes
                           where o.Name.LocalName == "Resposta"
                           select q.Opcoes.IndexOf(o) == q.Resposta).Single()
                    select q).Count();
        }

        public bool EstáCorreto(int i)
        {
            return (from o in _questoes[i].Opcoes
                    where o.Name.LocalName == "Resposta"
                    select _questoes[i].Opcoes.IndexOf(o) == _questoes[i].Resposta).Single();
        }

        public int QualÉCorreta(int i)
        {
            return (from o in _questoes[i].Opcoes
                    where o.Name.LocalName == "Resposta"
                    select _questoes[i].Opcoes.IndexOf(o)).Single();
        }
    }
}
