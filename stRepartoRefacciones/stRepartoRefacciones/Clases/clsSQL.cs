using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ws = stRepartoRefacciones.wsCoden;

namespace stRepartoRefacciones
{
    class clsSQL
    {
        private static SqlConnection oConnection;
        private static SqlCommand oCommand;
        private static SqlDataReader oDataReader;
        private static SqlTransaction oTransaction;
        

        public static void Conectar(string sBD)
        {
            try
            {
                ws.wsCodenSoapClient objService = new ws.wsCodenSoapClient();
                List<string> lstCredentials = objService.GetCredentialsBD(sBD);
                oConnection = new SqlConnection(string.Format("Data Source = {0} ; Initial Catalog={1} ;User Id={2}; Password={3};",lstCredentials[0], lstCredentials[1], lstCredentials[2], lstCredentials[3]));
                oConnection.Open();
               // oTransaction = oConnection.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw new Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + " | "+ sBD + " | " + ex.Message );
            }
        }

        public static void Conectar(string sServer, string sBD, string sUser, string sPassword)
        {
            try
            {
                oConnection = new SqlConnection("Data Source=" + sServer + "; Initial Catalog=" + sBD + ";User Id=" + sUser + "; Password=" + sPassword + ";");
                oConnection.Open();
                // oTransaction = oConnection.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw new Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + " | " + sBD + " | " + sServer + " | "+ ex.Message);
            }
        }

        public static void Commando(string _sCommand)
        {
            try
            {
                oTransaction = oConnection.BeginTransaction();
                oCommand = new SqlCommand(_sCommand, oConnection);
                oCommand.Transaction = oTransaction;
                oDataReader = oCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                oTransaction.Rollback();
                throw new Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + " | " + ex.Message);
            }
        }

        public static void CerrarDataReader()
        {
            try
            {
                if (!oDataReader.IsClosed)
                    oDataReader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + " | " + ex.Message);
            }
        }

        public static void CerrarConexion()
        {
            try
            {
                oConnection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + " | " + ex.Message);
            }
        }

        public static void CerrarTodo()
        {
            try
            {
                if (!oDataReader.IsClosed)
                    oDataReader.Close();
                oConnection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + " | " + ex.Message);
            }
        }

        public static List<JObject> Resultado()
        {
            try
            {
                List<JObject> lstRespuesta = new List<JObject>();

                if (oDataReader.HasRows)
                {
                    //string sArreglo = "\"lst\":[";
                    while (oDataReader.Read())
                    {
                        string sJson = "{";
                        //sArreglo += "{";
                        for (int i = 0; i < oDataReader.FieldCount; i++)
                        {
                            //if (i == 19)
                            //    break;
                            //if (oDataReader.GetName(i) == "Mensaje" || oDataReader.GetName(i) == "Success")
                            //{
                                sJson += "\"" + oDataReader.GetName(i) + "\" : ";
                                sJson += Tipo((object)oDataReader[oDataReader.GetName(i)], oDataReader[i].GetType().Name) + ",";
                            //}
                            //else
                            //{
                            //    sArreglo += "\"" + oDataReader.GetName(i) + "\" : ";
                            //    sArreglo += Tipo((object)oDataReader[oDataReader.GetName(i)], oDataReader[i].GetType().Name) + ",";
                            //}     
                        }
                        //sArreglo = sArreglo.Remove(sArreglo.Length - 1);
                        //sArreglo += "}]";
                        //sJson += sArreglo;
                        sJson = (sJson[sJson.Length - 1] == ',') ? sJson.Remove(sJson.Length - 1) : sJson;
                        //sJson += sArreglo;
                        sJson += "}";
                        lstRespuesta.Add(JObject.Parse(sJson));

                    }
                }  
                CerrarDataReader();

              

                oTransaction.Commit();
                return lstRespuesta;
            }
            catch (Exception ex)
            {
                oTransaction.Rollback();
                throw new Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + " | " + ex.Message);
            }
        }
                  

        private static string Tipo(object _oValor, string _sTipo)
        {
            try
            {
                string sValAux = string.Empty;
                switch (_sTipo.ToUpper())
                {
                    case "STRING": sValAux = "\"" + ((string)_oValor).Replace("\\","/").Replace('"',' ') + "\""; break;
                    case "DATETIME": sValAux = "\"" + ((DateTime)_oValor).ToString("yyyy-MM-dd HH:mm:ss") + "\""; break;
                    case "BOOLEAN": sValAux = ((bool)_oValor).ToString().ToLower(); break;
                    case "INT32": sValAux = ((int)_oValor).ToString().ToLower(); break;
                    case "INT16": sValAux = Convert.ToInt16(_oValor).ToString();break;
                    case "DOUBLE": sValAux = ((double)_oValor).ToString().ToLower(); break;
                    case "DBNULL": sValAux = "null"; break;
                    case "TIMESPAN": sValAux = "\"" + _oValor.ToString() + "\""; break;
                    case "DECIMAL": sValAux = Convert.ToDecimal(_oValor).ToString(); break;
                    default: sValAux = "";   break;
                }
                return sValAux;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }



}
