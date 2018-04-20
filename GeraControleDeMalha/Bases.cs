using System.Collections.Generic;
using System.Xml;

namespace GeraControleDeMalha
{
    class Bases
    {
        private string icao;
        private string iata;
        private string uf;
        private string codigo;
        private string difBSB;
        private string difUTC;
        private string difBSBver;
        private string difUTCver;
        public Bases() { }
        public string Icao
        {
            get
            {
                return icao;
            }

            set
            {
                icao = value;
            }
        }
        public string Iata
        {
            get
            {
                return iata;
            }

            set
            {
                iata = value;
            }
        }
        public string Uf
        {
            get
            {
                return uf;
            }

            set
            {
                uf = value;
            }
        }
        public string Codigo
        {
            get
            {
                return codigo;
            }

            set
            {
                codigo = value;
            }
        }
        public string DifBSB
        {
            get
            {
                return difBSB;
            }

            set
            {
                difBSB = value;
            }
        }
        public string DifUTC
        {
            get
            {
                return difUTC;
            }

            set
            {
                difUTC = value;
            }
        }
        public string DifBSBver
        {
            get
            {
                return difBSBver;
            }

            set
            {
                difBSBver = value;
            }
        }
        public string DifUTCver
        {
            get
            {
                return difUTCver;
            }

            set
            {
                difUTCver = value;
            }
        }
        public List<Bases> _carregaXMLbases(string arqXML)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(arqXML);
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/data-set/record");
            List<Bases> retdds = new List<Bases>();
            foreach (XmlNode node in nodes)
            {
                Bases _base = new Bases();
                _base.Icao = node.SelectSingleNode("ICAO").InnerText;
                _base.Iata = node.SelectSingleNode("IATA").InnerText;
                _base.Uf = node.SelectSingleNode("UF").InnerText;
                _base.Codigo = node.SelectSingleNode("Codigo").InnerText;
                _base.DifBSB = node.SelectSingleNode("InBSB").InnerText;
                _base.DifUTC = node.SelectSingleNode("InUTC").InnerText;
                _base.DifBSBver = node.SelectSingleNode("InBSBV").InnerText;
                _base.DifUTCver = node.SelectSingleNode("InUTCV").InnerText;
                retdds.Add(_base);
            }
            return retdds;
        }
    }
}
