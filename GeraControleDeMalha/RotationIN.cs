using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace GeraControleDeMalha
{
    class RotationIN : Voo
    {
        private DateTime dtDEPstdLOC;
        private DateTime dtARRstaLOC;
        private DateTime dtDEPstdBSB;
        private DateTime dtARRstaBSB;
        private DateTime gt;
        private double thilho;
        private DateTime mgt;
        private int temDLY;
        private int temWKY;

        public RotationIN() { }
        public RotationIN(RotationIN arqIN)
        {
            this.DtDEPstdLOC = arqIN.DtDEPstdLOC;
            this.DtDEPstdBSB = arqIN.DtDEPstdBSB;
            this.DtARRstaLOC = arqIN.DtARRstaLOC;
            this.DtARRstaBSB = arqIN.DtARRstaBSB;
            this.Gt = arqIN.Gt;
            this.Thilho = arqIN.Thilho;
            this.Mgt = arqIN.Mgt;
        }
        public DateTime DtDEPstdLOC
        {
            get
            {
                return dtDEPstdLOC;
            }

            set
            {
                dtDEPstdLOC = value;
            }
        }
        public DateTime DtARRstaLOC
        {
            get
            {
                return dtARRstaLOC;
            }

            set
            {
                dtARRstaLOC = value;
            }
        }
        public DateTime DtDEPstdBSB
        {
            get
            {
                return dtDEPstdBSB;
            }

            set
            {
                dtDEPstdBSB = value;
            }
        }
        public DateTime DtARRstaBSB
        {
            get
            {
                return dtARRstaBSB;
            }

            set
            {
                dtARRstaBSB = value;
            }
        }
        public DateTime Gt
        {
            get
            {
                return gt;
            }

            set
            {
                gt = value;
            }
        }
        public double Thilho
        {
            get
            {
                return thilho;
            }

            set
            {
                thilho = value;
            }
        }
        public DateTime Mgt
        {
            get
            {
                return mgt;
            }

            set
            {
                mgt = value;
            }
        }
        public int TemDLY
        {
            get
            {
                return temDLY;
            }

            set
            {
                temDLY = value;
            }
        }
        public int TemWKY
        {
            get
            {
                return temWKY;
            }

            set
            {
                temWKY = value;
            }
        }

        public RotationIN montaRotation(DateTime dtVoo, string subflt, int rot, int nVoo, string origem, DateTime std, DateTime sta,
                                        DateTime _dtStdBSB, DateTime _dtStaBSB, DateTime _dtStdLOC, DateTime _dtStaLOC, 
                                        string destino, DateTime blkt, string srvc, int _anoMinVER)
        {
            
            RotationIN _rotRet = new RotationIN();
           _rotRet.DtVoo = dtVoo;
            _rotRet.Subfleet = subflt;
            _rotRet.Rot = rot;
            _rotRet.NVoo = nVoo;
            _rotRet.Origem = origem;
            _rotRet.Std = std;
            _rotRet.Sta = sta;
            _rotRet.Destino = destino;
            _rotRet.Blkt = blkt;
            _rotRet.SrvcTP = srvc;
            _rotRet.DtDEPstdBSB = _dtStdBSB;
            _rotRet.DtARRstaBSB = _dtStaBSB;
            _rotRet.DtDEPstdLOC = _dtStdLOC;
            _rotRet.DtARRstaLOC = _dtStaLOC;
            _rotRet.Mgt = (ConfigurationManager.AppSettings.Get("BASES_40MIN").IndexOf(_rotRet.Destino,0) > 0) ? DateTime.Parse("00:40:00") : DateTime.Parse("00:30:00");
            return _rotRet;
        }
    }
    class TotRotation
    {
        private double totDaily;
        private double totWeek;

        public double TotDaily
        {
            get
            {
                return totDaily;
            }

            set
            {
                totDaily = value;
            }
        }
        public double TotWeek
        {
            get
            {
                return totWeek;
            }

            set
            {
                totWeek = value;
            }
        }
    }
}
