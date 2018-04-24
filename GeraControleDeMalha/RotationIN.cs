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

        public RotationIN montaRotation(DateTime dtVoo, string subflt, int rot, int nVoo, string origem, DateTime std, DateTime sta,
                                        string destino, DateTime blkt, string srvc)
        {
            RotationIN _rotRet = new RotationIN();

            DateTime teste = DateTime.Parse("24/04/2018 00:12:00");
            DateTime hrVer = new Horarios().dtTObsb(teste);

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
