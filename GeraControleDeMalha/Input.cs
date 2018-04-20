using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace GeraControleDeMalha
{
    class Input
    {
        public List<RotationIN> ddsRotation() /// colocar parâmetros para limitar consulta por período.
        {
            try
            {
                string oraCONN = ConfigurationManager.ConnectionStrings["SchedOPS"].ConnectionString;
                OracleConnection schCONN = new OracleConnection(oraCONN);
                schCONN.Open();


            }
            catch (Exception ex)
            {
                new GravaLog().GravaACT("Erro ao Abrir acesso ao banco SCHEDops");
            }








            List<RotationIN> lstRET = new List<RotationIN>();


            return lstRET;
        }


        public List<RotationIN> csvRotation()
        {
            string _arqCSV = "";
            string pstIN = ConfigurationManager.AppSettings.Get("IN");
            try
            {
                Console.WriteLine("Lendo Pasta de Entrada !");
                DirectoryInfo diretorio = new DirectoryInfo(pstIN);
                String[] Arquivos = Directory.GetFiles(pstIN);
                foreach (FileInfo arq in diretorio.GetFiles())
                {
                    if (arq.Extension.ToString().ToLower() == ".csv")
                    {
                        _arqCSV = arq.FullName;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nErro ao Ler Arquivo CSV de Entrada ! \n Erro -> {0}", ex.ToString());
                Environment.Exit(1);
            }
            List<RotationIN> lstRET = new List<RotationIN>();
            try
            {
                using (StreamReader sr = new StreamReader(_arqCSV))
                {
                    string line;
                    sr.ReadLine();
                    while ((line = sr.ReadLine()) != null)
                    {
                        try
                        {
                            String[] lstRotation = line.Split(';');
                            DateTime dtVoo = DateTime.Parse(lstRotation[0]);
                            string subflt = lstRotation[1].Trim().Substring(3, 3);
                            int rot = int.Parse(lstRotation[2]);
                            int nVoo = (lstRotation[3].Trim() != "") ? int.Parse(lstRotation[3].Trim()) : 0;
                            string origem = lstRotation[4];
                            DateTime stdHR = DateTime.Parse(lstRotation[5]);
                            DateTime staHR = DateTime.Parse(lstRotation[6]);
                            DateTime std = dtVoo.Date + stdHR.TimeOfDay;
                            DateTime sta = dtVoo.Date + staHR.TimeOfDay;
                            string destino = lstRotation[7];
                            DateTime blkt = DateTime.Parse(lstRotation[8]);
                            string srvc = lstRotation[9];
                            lstRET.Add(new RotationIN().montaRotation(dtVoo, subflt, rot, nVoo, origem, std, sta, destino, blkt, srvc));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Erro ao ler o registro : " + line);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return lstRET;
        }
    }
}
