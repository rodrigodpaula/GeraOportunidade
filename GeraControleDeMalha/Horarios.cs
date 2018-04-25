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
        private List<Bases> _ddsBases; 
        public Horarios() { }
        public Horarios(int _qAno) { _ddsBases = new Bases()._carregaXMLbases(ConfigurationManager.AppSettings.Get("AUX_BASES")); this.calcVERAO(_qAno); }
        public DateTime dtTObsb(DateTime dtUTC)
        {
            DateTime dtBSB = (dtUTC >= _iniVerao & dtUTC <= _fimVerao) ? dtUTC.AddHours(-2) : dtUTC.AddHours(-3);
            return dtBSB;
        }
        public DateTime dtTOloc(DateTime dtBSB, string _qBase)
        {
            int difTMP = 0;
            if (dtBSB < _fimVerao & dtBSB > _iniVerao)
            {
                difTMP = int.Parse(_ddsBases.Where(x => x.Iata == _qBase).Select(h => h.DifBSBver).FirstOrDefault());
            }
            else
            {
                difTMP = int.Parse(_ddsBases.Where(x => x.Iata == _qBase).Select(h => h.DifBSB).FirstOrDefault());
            }
            DateTime dtLoc = dtBSB.AddHours(difTMP);
            return dtLoc;

        }
        public DateTime iniVERAO(DateTime dtIN)
        {
            return _iniVerao;
        }
        public void calcVERAO(int _qAno)
        {
            DateTime _inidtBaseVER = new DateTime(_qAno, 10, 1);
            DateTime _fimdtBaseVER = new DateTime((_qAno+1), 02, 1);
            int _InidtSEM = ((int)_inidtBaseVER.DayOfWeek == 0) ? 7 : (int)_inidtBaseVER.DayOfWeek;
            switch (_InidtSEM)
            {
                case 1:
                    _iniVerao = _inidtBaseVER.AddDays(20);
                    break;
                case 2:
                    _iniVerao = _inidtBaseVER.AddDays(19);
                    break;
                case 3:
                    _iniVerao = _inidtBaseVER.AddDays(18);
                    break;
                case 4:
                    _iniVerao = _inidtBaseVER.AddDays(17);
                    break;
                case 5:
                    _iniVerao = _inidtBaseVER.AddDays(16);
                    break;
                case 6:
                    _iniVerao = _inidtBaseVER.AddDays(15);
                    break;
                case 7:
                    _iniVerao = _inidtBaseVER.AddDays(14);
                    break;
            }
            int _FimdtSEM = ((int)_fimdtBaseVER.DayOfWeek == 0) ? 7 : (int)_fimdtBaseVER.DayOfWeek;
            switch (_FimdtSEM)
            {
                case 1:
                    _fimVerao = _fimdtBaseVER.AddDays(20);
                    break;
                case 2:
                    _fimVerao = _fimdtBaseVER.AddDays(19);
                    break;
                case 3:
                    _fimVerao = _fimdtBaseVER.AddDays(18);
                    break;
                case 4:
                    _fimVerao = _fimdtBaseVER.AddDays(17);
                    break;
                case 5:
                    _fimVerao = _fimdtBaseVER.AddDays(16);
                    break;
                case 6:
                    _fimVerao = _fimdtBaseVER.AddDays(15);
                    break;
                case 7:
                    _fimVerao = _fimdtBaseVER.AddDays(14);
                    break;
            }
            int tstExA = (_qAno%19);
            int tstExb = (int)(_qAno/100);
            int tstExC = (_qAno%100);
            int tstExD = (int)(tstExb/4);
            int tstExE = (tstExb%4);
            int tstExF = (int)((tstExb+8)/25);
            int tstExG = (int)((tstExb-tstExF+1)/3);
            int tstExH = ((19*tstExA+tstExb-tstExD-tstExG+15)%30);
            int tstExI = (int)(tstExC/4);
            int tstExK = (tstExC%4);
            int tstExL = ((32+2*tstExE+2*tstExI-tstExH-tstExK)%7);
            int tstExM = (int)((tstExA+11*tstExH+22*tstExL)/451);
            int _qMes = (int)((tstExH+tstExL-7*tstExM+114)/31);
            int qDia = (((tstExH+tstExL-7*tstExM+114)%31)+1);
            DateTime pascoa = new DateTime(_qAno, _qMes, qDia);
            DateTime carnaval = pascoa.AddDays(- 49);
            if (_fimVerao == carnaval)
            {
                _fimVerao = _fimVerao.AddDays(7);
            }
            if (ConfigurationManager.AppSettings.Get("INI_VERAO") != "" & ConfigurationManager.AppSettings.Get("FIM_VERAO") != "")
            {
                _iniVerao = DateTime.Parse(ConfigurationManager.AppSettings.Get("INI_VERAO"));
                _fimVerao = DateTime.Parse(ConfigurationManager.AppSettings.Get("FIM_VERAO"));
            }
            else
            {
                _iniVerao = _iniVerao.AddHours(-3);
                _fimVerao = _fimVerao.AddHours(-2);
            }
        }

    }
}
