using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Diagnostics;

namespace GeraControleDeMalha
{
    class Input
    {
        private List<RotationIN> ddsRotation(DateTime dtInicio, DateTime dtFim) /// colocar parâmetros para limitar consulta por período.
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
                    string _sqlCMD = "SELECT SCHEDOPS.LEG.Day_Of_Origin, SCHEDOPS.LEG.Ac_owner, SCHEDOPS.LEG.Ac_SubType, SCHEDOPS.LEG.AC_LOGICAL_NO, SCHEDOPS.LEG.Fn_Number, SCHEDOPS.LEG.Dep_AP_Sched, "
                                             + "to_char(SCHEDOPS.LEG.Dep_Sched_DT, 'HH24:MI:SS') STD, to_char(SCHEDOPS.LEG.Arr_Sched_DT, 'HH24:MI:SS') STA, SCHEDOPS.LEG.Arr_AP_sched, "
                                             + "SCHEDOPS.LEG.Flight_TM, SCHEDOPS.LEG.Leg_Type "
                                   + "FROM SCHEDOPS.LEG WHERE ((SCHEDOPS.LEG.leg_state <> 'CNL' AND SCHEDOPS.LEG.leg_type <> 'Z') AND (SCHEDOPS.LEG.Day_Of_Origin >= '" + dtInicio.Date.ToString("dd/MM/yyyy") + "' and SCHEDOPS.LEG.Day_Of_Origin <= '" + dtFim.Date.ToString("dd/MM/yyyy") + "')) "
                                             + "ORDER BY SCHEDOPS.LEG.Ac_SubType, SCHEDOPS.LEG.ac_logical_no, SCHEDOPS.LEG.day_of_origin ASC";

                    OracleCommand cmd = new OracleCommand(_sqlCMD, schCONN);
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader dr = cmd.ExecuteReader();
                    Int32 cntReg = 0;
                    while (dr.Read())
                    {
                        try
                        {
                            RotationIN tmpDados = new RotationIN();
                            tmpDados.DtVoo = DateTime.Parse(dr[0].ToString());
                            tmpDados.Subfleet = dr[2].ToString();
                            tmpDados.Rot = int.Parse(dr[3].ToString());
                            tmpDados.NVoo = (dr[4].ToString().Trim() != "") ? int.Parse(dr[4].ToString().Trim()) : 0;
                            tmpDados.Origem = dr[5].ToString().Trim() ;
                            DateTime stdHR = (dr[6].ToString().Trim() == "24:00") ? DateTime.Parse("00:00") : DateTime.Parse(dr[6].ToString().Trim());
                            DateTime staHR = (dr[7].ToString().Trim() == "24:00") ? DateTime.Parse("00:00") : DateTime.Parse(dr[7].ToString().Trim());
                            tmpDados.Std = tmpDados.DtVoo.Date + stdHR.TimeOfDay;
                            tmpDados.Sta = tmpDados.DtVoo.Date + staHR.TimeOfDay;
                            tmpDados.Destino = dr[8].ToString().Trim();
                            tmpDados.Blkt = new DateTime(TimeSpan.FromSeconds(int.Parse(dr[9].ToString().Trim())).Ticks);
                            tmpDados.SrvcTP = dr[10].ToString().Trim();
                            lstRET.Add(tmpDados);
                            cntReg++;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Erro ao Ler Registro");
                            new GravaLog().GravaACT(string.Format("Erro ao ler o registro : {0} - Erro : {1}", cntReg.ToString(), ex.Message));
                        }
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
        private List<RotationIN> csvRotation()
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
            return _tmpIN;
        }
        public List<RotationIN> _carregaROT(DateTime dtInicio = default(DateTime), DateTime dtFim = default(DateTime))
        {
            ///fazer validação de parâmetro
            dtInicio = new DateTime(DateTime.Now.Ticks);
            dtFim = dtInicio.AddDays(31);

            ///selecionar tipo de carregamento, se vis CSV ou BD
            List<RotationIN> _retRotation = new List<RotationIN>();
            ///carregamento pelo BD
          //  List<RotationIN> ddsCarregados = ddsRotation(dtInicio, dtFim);
            ///carregamento pelo CSV
            List<RotationIN> ddsCarregados = _carregaROT(dtInicio, dtFim);
            try
            {
                int _anoMIN = ddsCarregados.Select(x => x.DtVoo.Year).Min();
                Horarios _cnvHorarios = new Horarios(_anoMIN);
                foreach (RotationIN _tpROT in ddsCarregados.OrderBy(x => x.Rot).ThenBy(d => d.DtDEPstdLOC).GroupBy(g => new { g.DtVoo, g.Rot })) /// ver possibilidade de blocar por grupo
                {

                    DateTime dtBSBdep = _cnvHorarios.dtTObsb(_tpROT.Std);
                    DateTime dtBSBarr = _cnvHorarios.dtTObsb(_tpROT.Sta);
                    DateTime dtLOCdep = _cnvHorarios.dtTOloc(_tpROT.Std, _tpROT.Origem);
                    DateTime dtLOCarr = _cnvHorarios.dtTOloc(_tpROT.Sta, _tpROT.Destino);


                    RotationIN _nvRot = new RotationIN().montaRotation(_tpROT.DtVoo, _tpROT.Subfleet, _tpROT.Rot, _tpROT.NVoo, _tpROT.Origem, _tpROT.Std, _tpROT.Sta,
                                                               dtBSBdep, dtBSBarr, dtLOCdep, dtLOCarr, _tpROT.Destino, _tpROT.Blkt, _tpROT.SrvcTP, _anoMIN);

                    _retRotation.Add(_nvRot);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao processar Rotation " + ex.Message);
            }
            return _retRotation;

        }
    }
}
