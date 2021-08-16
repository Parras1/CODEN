using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stRepartoRefacciones
{
    class clsLogs
    {
        //@"C:\Metrica Projects\Scheduled Task\stReportesBI
        private static string sPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0,6)+@"\Log.log"; // @"C:\Metrica Projects\Scheduled Task\stReporteEdenred\Logs.log";
        private static string sPath1 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + @"\MM_";//@"C:\Metrica Projects\Scheduled Task\stReporteEdenred\MM_"  ;
        //private static string sPathErrores = @"C:\Desarrollo\ReportesEP\Errores.txt";
        //private static string sBulkPath = @"C:\Users\MMDev01\Documents\Métricamóvil\ReportesBI\stReportesBIV2\BCPFILE.txt";
        public static string sBaseDatos = "";
        public static void InsertLog(string sMessage)
        {
            try
            {
                //sPath = sPath.Remove(0, 6);
                //Si el archivo no existe se crea uno nuevo.
                if (!File.Exists(sPath))
                {
                    FileStream fs = File.Create(sPath);
                    fs.Close();
                }
                else //Si el archivo existe pero pesa mas de 10 MB lo limpiamos.
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(sPath);
                    long lTamaño = fileInfo.Length;
                    if (lTamaño == 10485760) //Si el archivo mide 10 megas lo vaciamos.
                        File.WriteAllText(sPath, string.Empty);
                }
                //Insertamos el nuevo registro log en el archivo.
                using (StreamWriter sw = File.AppendText(sPath))
                {
                    sw.WriteLine("[" + DateTime.Now + "] " + sMessage);
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string InsertReg(string sMessage, string sDateTime)//, string sVehiculo, int _iDiasAtras)
        {
            try
            {
                //sPath1 = sPath1.Remove(0, 7);
                string sArchivo = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + @"\ERMX-MM-" + sDateTime + ".csv";//sPath1 + DateTime.Now.AddDays(-_iDiasAtras).ToString("yyyy-MM-dd") + " _ " + sVehiculo + ".log";
                //Si el archivo no existe se crea uno nuevo.
                if (!File.Exists(sArchivo))
                {
                    FileStream fs = File.Create(sArchivo);
                    fs.Close();
                }
                else //Si el archivo existe pero pesa mas de 10 MB lo limpiamos.
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(sArchivo);
                    long lTamaño = fileInfo.Length;
                    if (lTamaño == 10485760) //Si el archivo mide 10 megas lo vaciamos.
                        File.WriteAllText(sArchivo, string.Empty);
                }
                //Insertamos el nuevo registro log en el archivo.
                using (StreamWriter sw = File.AppendText(sArchivo))
                {
                    sw.WriteLine(sMessage);
                    sw.Close();
                }
                return sArchivo;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
