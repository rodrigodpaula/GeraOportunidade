using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Linq;

namespace GeraControleDeMalha
{
    class Input
    {
        public List<RotationIN> ddsRotation() /// colocar parâmetros para limitar consulta por período.
        {
            List<RotationIN> lstRET = new List<RotationIN>();
            List<string> DadosNET = new List<string>();
            try
            {
                string oraCONN = ConfigurationManager.ConnectionStrings["SchedOPS"].ConnectionString;
                OracleConnection schCONN = new OracleConnection(oraCONN);
                schCONN.Open();
                try
                {
                    string _base = "CGH";
                    DateTime DIAhOJE = DateTime.Now.Date;
                    string _sqlCMD = "SELECT schops.TIME_ZONE, " +
                                            "schops.COUNTRY_CODE, " +
                                            "dstinfo.START_DATE_TIME, " +
                                            "dstinfo.END_DATE_TIME, " +
                                            "dstinfo.DIFF_LST_DST," +
                                            "timezone.DIFF_UTC_LST " +
                                     "FROM " +
                                            "SCHEDOPS.AP_BASICS schops " +
                                            "INNER JOIN NETBASE.DST_INFO dstinfo ON(schops.DST_ZONE_CODE = dstinfo.TIME_ZONE_CODE) " +
                                            "INNER JOIN NETBASE.TIME_ZONE timezone ON(schops.DST_ZONE_CODE = timezone.TIME_ZONE_CODE) " +
                                            "WHERE ((schops.IATA_AP_CODE = '" + _base + "') AND (dstinfo.START_DATE_TIME > '" +
                        DIAhOJE.ToString("dd/MM/yyyy") + "') AND (ROWNUM = 1)) ORDER BY dstinfo.START_DATE_TIME DESC";


                   string cmdTWO = "SELECT SCHEDOPS.LEG.Day_Of_Origin, SCHEDOPS.LEG.Ac_owner, SCHEDOPS.LEG.Ac_SubType, SCHEDOPS.LEG.AC_LOGICAL_NO, SCHEDOPS.LEG.Fn_Number, SCHEDOPS.LEG.Dep_AP_Sched, "
                                 + "to_char(SCHEDOPS.LEG.Dep_Sched_DT, 'HH:MM:SS') STD, to_char(SCHEDOPS.LEG.Arr_Sched_DT, 'HH:MM:SS') STA, SCHEDOPS.LEG.Arr_AP_sched, "
                                 + "SCHEDOPS.LEG.Flight_TM, SCHEDOPS.LEG.Leg_Type"
                                 + "FROM SCHEDOPS.LEG WHERE (SCHEDOPS.LEG.Day_Of_Origin > '01/03/2018') ORDER BY SCHEDOPS.LEG.Ac_SubType, SCHEDOPS.LEG.ac_logical_no, SCHEDOPS.LEG.day_of_origin ASC;";



                    OracleCommand cmd = new OracleCommand(_sqlCMD, schCONN);
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader dr = cmd.ExecuteReader();
                    Int32 columnCount = dr.FieldCount;
                    while (dr.Read())
                    {
                        string ddsRetACM = _base;
                        for (Int32 columnIndex = 0; columnIndex < columnCount; columnIndex++)
                        {
                            ddsRetACM = ddsRetACM + ";" + dr.GetValue(columnIndex).ToString();
                            ///Console.WriteLine(dr.GetName(columnIndex) + ": " + dr.GetValue(columnIndex).ToString());
                        }
                        DadosNET.Add(ddsRetACM);
                    }
                    dr.Dispose();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    new GravaLog().GravaACT(string.Format("Erro ao consultar o banco SCHEDops - Erro : {0}", ex.Message));
                }
                schCONN.Close();
            }
            catch (Exception ex)
            {
                new GravaLog().GravaACT(string.Format("Erro ao Abrir acesso ao banco SCHEDops - Erro : {0}", ex.Message));
            }
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
            List<RotationIN> _tmpIN = new List<RotationIN>();
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
                            RotationIN tmpDados = new RotationIN();
                            String[] lstRotation = line.Split(';');
                            tmpDados.DtVoo = DateTime.Parse(lstRotation[0]);
                            tmpDados.Subfleet = lstRotation[1].Trim().Substring(3, 3);
                            tmpDados.Rot = int.Parse(lstRotation[2]);
                            tmpDados.NVoo = (lstRotation[3].Trim() != "") ? int.Parse(lstRotation[3].Trim()) : 0;
                            tmpDados.Origem = lstRotation[4];
                            DateTime stdHR = (lstRotation[5].Trim() == "24:00") ? DateTime.Parse("00:00") : DateTime.Parse(lstRotation[5]);
                            DateTime staHR = (lstRotation[6].Trim() == "24:00") ? DateTime.Parse("00:00") : DateTime.Parse(lstRotation[6]);
                            tmpDados.Std = tmpDados.DtVoo.Date + stdHR.TimeOfDay;
                            tmpDados.Sta = tmpDados.DtVoo.Date + staHR.TimeOfDay;
                            tmpDados.Destino = lstRotation[7];
                            tmpDados.Blkt = DateTime.Parse(lstRotation[8]);
                            tmpDados.SrvcTP = lstRotation[9];
                            _tmpIN.Add(tmpDados);
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
            ///carrega lista completa do rotation - colocar em sub externa - mudar somente qual a carga
            int _anoMIN = _tmpIN.Select(x => x.DtVoo.Year).Min();
            Horarios _cnvHorarios = new Horarios(_anoMIN);
            foreach (RotationIN _tpROT in _tmpIN)
            {

                DateTime dtBSBdep = _cnvHorarios.dtTObsb(_tpROT.Std);
                DateTime dtBSBarr = _cnvHorarios.dtTObsb(_tpROT.Sta);
                DateTime dtLOCdep = _cnvHorarios.dtTOloc(_tpROT.Std, _tpROT.Origem);
                DateTime dtLOCarr = _cnvHorarios.dtTOloc(_tpROT.Sta, _tpROT.Destino);

                lstRET.Add(new RotationIN().montaRotation(_tpROT.DtVoo, _tpROT.Subfleet, _tpROT.Rot, _tpROT.NVoo, _tpROT.Origem, _tpROT.Std, _tpROT.Sta,
                                                           dtBSBdep, dtBSBarr, dtLOCdep, dtLOCarr,  _tpROT.Destino, _tpROT.Blkt, _tpROT.SrvcTP, _anoMIN));
            }
            return lstRET;
        }
    }
}
