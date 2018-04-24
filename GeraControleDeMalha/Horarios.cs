using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeraControleDeMalha
{
    class Horarios
    {
        private DateTime _iniVerao;
        private DateTime _fimVerao;

        public Horarios() { List<Bases> _ddsBases = new Bases()._carregaXMLbases(ConfigurationManager.AppSettings.Get("AUX_BASES")); }
        public DateTime dtTObsb(DateTime dtUTC, string orig, string dest)
        {
            
            return dtUTC;
        }
        public DateTime dtTOloc(DateTime dtUTC, string orig, string dest)
        {
            

            return dtUTC;

        }
        public DateTime iniVERAO(DateTime dtIN)
        {

            return dtIN;
        }
        public void calcVERAO(DateTime dtIN)
        {










            //return dtIN;
        }

    }
}
