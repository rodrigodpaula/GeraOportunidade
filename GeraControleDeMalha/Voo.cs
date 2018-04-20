using System;

namespace GeraControleDeMalha
{
    class Voo
    {
        private int nVoo;
        private DateTime dtVoo;
        private string origem;
        private string destino;
        private DateTime std;
        private DateTime sta;
        private string subfleet;
        private string srvcTP;
        private int rot;
        private DateTime blkt;
        public Voo() { }
        public int NVoo
        {
            get
            {
                return nVoo;
            }

            set
            {
                nVoo = value;
            }
        }
        public DateTime DtVoo
        {
            get
            {
                return dtVoo;
            }

            set
            {
                dtVoo = value;
            }
        }
        public string Origem
        {
            get
            {
                return origem;
            }

            set
            {
                origem = value;
            }
        }
        public string Destino
        {
            get
            {
                return destino;
            }

            set
            {
                destino = value;
            }
        }
        public DateTime Std
        {
            get
            {
                return std;
            }

            set
            {
                std = value;
            }
        }
        public DateTime Sta
        {
            get
            {
                return sta;
            }

            set
            {
                sta = value;
            }
        }
        public string Subfleet
        {
            get
            {
                return subfleet;
            }

            set
            {
                subfleet = value;
            }
        }
        public string SrvcTP
        {
            get
            {
                return srvcTP;
            }

            set
            {
                srvcTP = value;
            }
        }
        public int Rot
        {
            get
            {
                return rot;
            }

            set
            {
                rot = value;
            }
        }
        public DateTime Blkt
        {
            get
            {
                return blkt;
            }

            set
            {
                blkt = value;
            }
        }
    }
}
