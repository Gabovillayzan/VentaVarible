using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Reflection;
[assembly: AssemblyVersion("2.1.*")]
public partial class registroVentas : System.Web.UI.Page
{
    public ClaseSql SQL = new ClaseSql();
    public string version;
    protected void Page_Load(object sender, EventArgs e)
    {
        version = "V." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        ver.Text = version;
        if (HttpContext.Current.Session["Login"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            Usuario datosU = (Usuario)HttpContext.Current.Session["Login"];
            HttpContext.Current.Session["datosUser"] = datosU;
        }
    }
    protected void cerrarSession(object sender, EventArgs e)
    {
        HttpContext.Current.Session["login"] = null;
        HttpContext.Current.Session["datosUser"] = null;
        Session.Abandon();
        Session.Clear();
        Response.Redirect("Login.aspx");
    }
    protected void mensaje(string msj)
    {
        ClientScript.RegisterStartupScript(GetType(), "mensaje", "respuesta('" + msj + "');", true);
    }
    protected void ConsultarDatos(object sender, EventArgs e)
    {
        string regAño = años.Value.ToString();
        string regMes = meses.Value.ToString();
        string mes = años.Value.ToString() + "-" + meses.Value.ToString();



        if (subir.PostedFile.ContentLength > 0)
        {
            string nombre = Path.GetFileName(subir.PostedFile.FileName);

            string extension = Path.GetExtension(subir.PostedFile.FileName);
            string fn = System.IO.Path.GetFileName("adpExcel.xlsx");
            string file = Server.MapPath(@"~\files") + "\\" + fn;
            try
            {
                subir.PostedFile.SaveAs(file);
                string FilePath = file;
                if (Import_To_Grid(FilePath, extension, mes, regAño, regMes))
                {

                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        else
        {
            mensaje("No existe ningun archivo cargado");

        }
    }
    private bool Import_To_Grid(string FilePath, string Extension, string month, string regAño, string regMes)
    {
        string conStr = "";
        switch (Extension)
        {
            case ".xls": //Excel 97-03
                conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                break;
            case ".xlsx": //Excel 07
                conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                break;
        }
        conStr = String.Format(conStr, FilePath, "no");
        OleDbConnection connExcel = new OleDbConnection(conStr);
        OleDbCommand cmdExcel = new OleDbCommand();
        OleDbDataAdapter oda = new OleDbDataAdapter();
        DataTable dt = new DataTable();
        cmdExcel.Connection = connExcel;
        //Get the name of First Sheet
        connExcel.Open();
        DataTable dtExcelSchema;
        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        connExcel.Close();
        //Read Data from First Sheet
        cmdExcel.CommandText = "SELECT * From [" + SheetName + "A4:Q35]";
        oda.SelectCommand = cmdExcel;
        oda.Fill(dt);


        List<datosAlmacenados> list = new List<datosAlmacenados>();
        foreach (DataRow myDatarow in dt.Rows)
        {
            datosAlmacenados excelData = new datosAlmacenados();
            excelData.fecha = validarNoSeQue(myDatarow.ItemArray[0].ToString());
            excelData.FM_DEL = validarNoSeQue(myDatarow.ItemArray[1].ToString());
            excelData.FM_AL = validarNoSeQue(myDatarow.ItemArray[2].ToString());
            excelData.BM_DEL = validarNoSeQue(myDatarow.ItemArray[3].ToString());
            excelData.BM_AL = validarNoSeQue(myDatarow.ItemArray[4].ToString());
            excelData.V_DEL = validarNoSeQue(myDatarow.ItemArray[5].ToString());
            excelData.V_AL = validarNoSeQue(myDatarow.ItemArray[6].ToString());
            excelData.TB_DEL = validarNoSeQue(myDatarow.ItemArray[7].ToString());
            excelData.TB_AL = validarNoSeQue(myDatarow.ItemArray[8].ToString());
            excelData.TF_DEL = validarNoSeQue(myDatarow.ItemArray[9].ToString());
            excelData.TF_AL = validarNoSeQue(myDatarow.ItemArray[10].ToString());
            excelData.SOLES = validarNoSeQue(myDatarow.ItemArray[11].ToString());

            list.Add(excelData);
        }

        //var user = HttpContext.Current.Session["Login"].ToString();
        //string css = "<style type='text/css'> #tresultado table tr td[colspan='3']{ font-weight: bold;} #tresultado table{ border-spacing: 0px;} #tresultado th{ font-family:'Calibri',sans-serif; font-size:12pt; color:#385B83; font-weight:bold; background-color:#DEDFDE; height:30px; } #tresultado tbody td { border-bottom:#92D050 solid 1px; width: 10%; } </style>";
        string relacion = "<table> <thead> <tr> <th >TIPO DE ERROR</th> <th >DESDE</th> <th >HASTA</th> <th >COOR X,Y (excel)</th> </tr> </thead> <tbody>";
        relacion += validarCorrelacion(list) + "</body></table>";
        int cont = relacion.Length;
        if (cont <= 515)
        {
            RegistrarDatos(list, month, regAño, regMes);
            mensaje("Se registraron los datos correctamente");

            Thread.Sleep(10000);
            tablaResultado.InnerHtml = "";
            //EnvioCorreo(relacion, user);
        }
        else
        {

            var a = list;
            RegistrarDatos(list, month, regAño, regMes);
            guardarInconsistencia(relacion);
            //tablaResultado.InnerHtml = relacion;       no se mostrara mensaje de error! temp                
            //Thread.Sleep(15000);          si no se muestra mensaje de error la espera no es necesaria
            //tablaResultado.InnerHtml = "";   no existe ninguna inyeccion de html. No hay necesidad de limpiar
            mensaje("Se registraron los datos con algunas obs");
            //ArmarReporte(list);
        }


        //---------Fin test--------
        connExcel.Close();
        return false;
    }
    public string validarNoSeQue(string valor)
    {
        Regex soloNum = new Regex(@"\d");
        var valorExcel = valor;
        bool result = soloNum.IsMatch(valorExcel);
        if (!result)
        {
            return "0";

        }
        else
        {
            return valorExcel;

        }

    }
    public string validarRow(List<String[]> list)
    {
        //  int contador = 0; transformar en array! para saber en que ubicaciones esta el error
        var estado = "bien";
        for (int i = 0; i <= 31; i++)
        {
            if (estado == "bien")
            {
                var soles = list[i][11];
                var dolares = list[i][12];

                if (soles != "" || list[i][12] != "")
                {
                    for (var j = 0; j <= 10; j++)
                    {
                        if (list[i][j] == "0")
                        {
                            estado = "mal";
                            //podemos registrar en que fila esta mal
                        }
                    }
                }
                else if (soles == "" || dolares == "")
                {
                    for (var j = 0; j <= 10; j++)
                    {
                        if (list[i][j] != "")
                        {
                            estado = "mal";
                            //podemos registrar en que fila esta mal
                        }
                    }
                }
            }
        }
        return estado;
    }
    public void ArmarReporte(List<String[]> list)
    {
        string tbody = "";
        string tableHead = "<table id='TablaResultado' class='table table-hover table-bordered'><thead> <tr> <th rowspan='3'>Total por dia</th><th colspan='10'>Comprobante de pago</th><th colspan='2' rowspan='2'>Sub Total</th><th colspan='2' rowspan='2'>Igv</th><th colspan='2' rowspan='2'>Importe total</th> </tr> <tr> <th colspan='2'>Factura manual</th><th colspan='2'>Boleta Manual</th><th colspan='2'>Vales</th><th colspan='2'>Ticket-Boleta</th><th colspan='2'>Ticket-Factura</th> </tr> <tr> <th>Del</th><th>Al</th><th>Del</th><th>Al</th><th>Del</th><th>Al</th><th>Del</th><th>Al</th><th>Del</th><th>Al</th><th>Soles</th><th>Dolares</th><th>Soles</th><th>Dolares</th><th>Soles</th><th>Dolares</th> </tr> </thead><tbody id='cuerpoTabla'>";
        for (int i = 0; i <= 30; i++)
        {
            tbody += "<tr>" + SoloNumero(list[i][0]) + "" + SoloNumero(list[i][1]) + "" + SoloNumero(list[i][2]) + "" + SoloNumero(list[i][3]) + "" + SoloNumero(list[i][4]) + "" + SoloNumero(list[i][5]) + "" + SoloNumero(list[i][6]) + "" + SoloNumero(list[i][7]) + "" + SoloNumero(list[i][8]) + "" + SoloNumero(list[i][9]) + "" + SoloNumero(list[i][10]) + "" + SoloNumero(list[i][11]) + "" + SoloNumero(list[i][12]) + "" + SoloNumero(list[i][13]) + "" + SoloNumero(list[i][14]) + "" + SoloNumero(list[i][15]) + "" + SoloNumero(list[i][16]) + "</tr>";
            //SQL.GuardarDataSql(Convert.ToString(list[i][0]),Convert.ToString(list[i][1]),   Convert.ToString(list[i][2]),  Convert.ToString(list[i][3]),               Convert.ToString(list[i][4]),   Convert.ToString(list[i][5]),  Convert.ToString(list[i][6]), Convert.ToString(list[i][7]),  Convert.ToString(list[i][8]), Convert.ToString(list[i][9]),  Convert.ToString(list[i][10]),    Convert.ToString(list[i][11]),Convert.ToString(list[i][12]),  Convert.ToString(list[i][13]), Convert.ToString(list[i][14]),Convert.ToString(list[i][15]),Convert.ToString(list[i][16]));
        }
        tbody += "</tbody></table>";
        string tablaReporte = tableHead + tbody;
        tablaResultado.InnerHtml = tablaReporte;

    }
    public string ReemplazarVacios(string valor)
    {
        if (valor == "" || valor == null)
        {
            valor = "0";
        }
        return valor;
    }
    public string SoloNumero(string valor)
    {

        int result;

        if (Int32.TryParse(valor, out result))
        {
            return "<td><strong>" + valor + "</strong></td>";
        }
        else
        {
            return "<td class='red' style='background-color:red; color:white'><strong>" + valor + "</strong></td>";
        }

    }
    public void guardarInconsistencia(string datos)
    {
        SQL.GuardarInconsistencia(datos);
    }
    public void EnvioCorreo(string mBody, string usuario)
    {
        string mFrom = "Venta varible <Control.Documentario@adp.com.pe>"; //  Gabriel Villayzan <Gabriel.Villayzan@adp.com.pe>
        string correo = "jose.sotteccani@adp.com.pe"; //Para:
        string mSubject = "Reporte de " + usuario;//Titulo del correo:
        MailMessage message = new MailMessage();
        message.From = new MailAddress(mFrom);
        message.To.Add(new MailAddress(correo));

        //message.To.Add(new MailAddress("gabriel.villayzan@AdP.com.pe"));
        message.Subject = mSubject;
        message.IsBodyHtml = true;
        message.Body = mBody;

        SmtpClient client = new SmtpClient();
        client.Host = "192.168.204.146";
        //client.Host = "correo.adp.com.pe";
        client.Port = 587;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        client.Timeout = 15000;
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential("c_documentario", "Verdadero1", "adpperu.local");

        try
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            client.Send(message);
            //respuesta = true;
        }
        catch (Exception ex1)
        {
            //respuesta = false;
            ex1.Message.ToString();
        }

    }
    public static string Validacion(string dato)
    {
        if (dato == "")
        {
            dato = "0";
            return dato;
        }
        else
        {
            return dato;
        }
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string validarCorrelacion(List<datosAlmacenados> list)
    {
        var j = 0;
        string msj = "<tr><td colspan=4><b></b></td></tr>";
        DateTime fecha = DateTime.Now;
        int dias = DateTime.DaysInMonth(fecha.Year, fecha.Month) - 1;
        for (var i = 0; i < dias; i++)
        {
            if ((list[i].SOLES != "0") && (list[i].FM_DEL != "0" || list[i].FM_AL != "0" || list[i].BM_DEL != "0" || list[i].BM_AL != "0" || list[i].V_DEL != "0" || list[i].V_AL != "0" || list[i].TB_DEL != "0" || list[i].TB_AL != "0" || list[i].TF_DEL != "0" || list[i].TF_AL != "0"))
            {
                if (Convert.ToInt32(list[i].FM_DEL) <= Convert.ToInt32(list[i].FM_AL))
                {
                    bool estado = true;
                    j = i + 1;
                    while (estado & (j <= dias))
                    {
                        int hst = Convert.ToInt32(list[i].FM_AL);
                        int ini = Convert.ToInt32(list[j].FM_DEL);
                        if (ini != 0 && hst != 0)
                        {
                            if ((ini - 1) == hst)
                            {
                                estado = false;
                            }
                            else
                            {
                                msj += "<tr><td>No existe correlatividad entre los valores  </td><td>" + hst + " </td><td> " + ini + "</td><td> C " + (i + 4) + "</td></tr>";
                                j++;
                                estado = false;
                            }
                        }
                        else
                        {
                            j++;
                        }
                    }
                }
                else
                {
                    msj += "<tr><td>'FM DEL' no puede ser mayor 'FM AL' </td><td>" + list[i].FM_DEL + " </td><td> " + list[i].FM_AL + "</td><td> C " + (i + 4) + " B" + (j + 3) + "</td></tr>";
                }
            }
            if ((list[i].SOLES != "0") && (list[i].FM_DEL != "0" || list[i].FM_AL != "0" || list[i].BM_DEL != "0" || list[i].BM_AL != "0" || list[i].V_DEL != "0" || list[i].V_AL != "0" || list[i].TB_DEL != "0" || list[i].TB_AL != "0" || list[i].TF_DEL != "0" || list[i].TF_AL != "0"))
            {
                if (Convert.ToInt32(list[i].BM_DEL) <= Convert.ToInt32(list[i].BM_AL))
                {
                    bool estado = true;
                    j = i + 1;
                    while (estado & (j <= dias))
                    {
                        int hst = Convert.ToInt32(list[i].BM_AL);
                        int ini = Convert.ToInt32(list[j].BM_DEL);
                        if (ini != 0 && hst != 0)
                        {
                            if ((ini - 1) == hst)
                            {
                                estado = false;
                            }
                            else
                            {
                                msj += "<tr><td>No existe correlatividad entre los valores  </td><td>" + hst + " </td><td> " + ini + "</td><td> E " + (i + 4) + "</td></tr>";
                                j++;
                                estado = false;
                            }
                        }
                        else
                        {
                            j++;
                        }

                    }
                }
                else
                {
                    msj += "<tr><td>'BM DEL' NO PUEDE SER MAYOR A 'BM AL' </td><td>" + list[i].BM_DEL + " </td><td> " + list[i].BM_AL + "</td><td> E " + (i + 4) + " D" + (j + 3) + "</td></tr>";
                }
            }
            if ((list[i].SOLES != "0") && (list[i].FM_DEL != "0" || list[i].FM_AL != "0" || list[i].BM_DEL != "0" || list[i].BM_AL != "0" || list[i].V_DEL != "0" || list[i].V_AL != "0" || list[i].TB_DEL != "0" || list[i].TB_AL != "0" || list[i].TF_DEL != "0" || list[i].TF_AL != "0"))
            {
                if (Convert.ToInt32(list[i].V_DEL) <= Convert.ToInt32(list[i].V_AL))
                {
                    bool estado = true;
                    j = i + 1;
                    while (estado & (j <= dias))
                    {
                        int hst = Convert.ToInt32(list[i].V_AL);
                        int ini = Convert.ToInt32(list[j].V_DEL);
                        if (ini != 0 && hst != 0)
                        {
                            if ((ini - 1) == hst)
                            {
                                estado = false;
                            }
                            else
                            {
                                msj += "<tr><td>No existe correlatividad entre los valores  </td><td>" + hst + " </td><td> " + ini + "</td><td> G " + (i + 4) + "</td></tr>";
                                j++;
                                estado = false;
                            }
                        }
                        else
                        {
                            j++;
                        }
                    }
                }
                else
                {
                    msj += "<tr><td>'V DEL' NO PUEDE SER MAYOR A 'V AL' </td><td>" + list[i].V_DEL + " </td><td> " + list[i].V_AL + "</td><td> G " + (i + 4) + " F" + (j + 3) + "</td></tr>";
                }
            }
            if ((list[i].SOLES != "0") && (list[i].FM_DEL != "0" || list[i].FM_AL != "0" || list[i].BM_DEL != "0" || list[i].BM_AL != "0" || list[i].V_DEL != "0" || list[i].V_AL != "0" || list[i].TB_DEL != "0" || list[i].TB_AL != "0" || list[i].TF_DEL != "0" || list[i].TF_AL != "0"))
            {
                if (Convert.ToInt32(list[i].TB_DEL) <= Convert.ToInt32(list[i].TB_AL))
                {
                    bool estado = true;
                    j = i + 1;
                    while (estado & (j <= dias))
                    {
                        int hst = Convert.ToInt32(list[i].TB_AL);
                        int ini = Convert.ToInt32(list[j].TB_DEL);
                        if (ini != 0 && hst != 0)
                        {
                            if ((ini - 1) == hst)
                            {
                                estado = false;
                            }
                            else
                            {
                                msj += "<tr><td>No existe correlatividad entre los valores  </td><td>" + hst + " </td><td> " + ini + "</td><td> I " + (i + 4) + "</td></tr>";
                                j++;
                                estado = false;
                            }
                        }
                        else
                        {
                            j++;
                        }

                    }
                }
                else
                {
                    msj += "<tr><td>'TB DEL' NO PUEDE SER MAYOR A 'TB AL' </td><td>" + list[i].TB_DEL + " </td><td> " + list[i].TB_AL + "</td><td> I " + (i + 4) + " H" + (j + 3) + "</td></tr>";
                }
            }
            if ((list[i].SOLES != "0") && (list[i].FM_DEL != "0" || list[i].FM_AL != "0" || list[i].BM_DEL != "0" || list[i].BM_AL != "0" || list[i].V_DEL != "0" || list[i].V_AL != "0" || list[i].TB_DEL != "0" || list[i].TB_AL != "0" || list[i].TF_DEL != "0" || list[i].TF_AL != "0"))
            {
                if (Convert.ToInt32(list[i].TF_DEL) <= Convert.ToInt32(list[i].TF_AL))
                {
                    bool estado = true;
                    j = i + 1;
                    while (estado & (j <= dias))
                    {
                        int hst = Convert.ToInt32(list[i].TF_AL);
                        int ini = Convert.ToInt32(list[j].TF_DEL);
                        if (ini != 0 && hst != 0)
                        {
                            if ((ini - 1) == hst)
                            {
                                estado = false;
                            }
                            else
                            {
                                msj += "<tr><td>No existe correlatividad entre los valores  </td><td>" + hst + " </td><td> " + ini + "</td><td> K " + (i + 4) + "</td></tr>";
                                j++;
                                estado = false;
                            }
                        }
                        else
                        {
                            j++;
                        }

                    }
                }
                else
                {
                    msj += "<tr><td>'TF DEL' NO PUEDE SER MAYOR A 'TF AL' </td><td>" + list[i].TF_DEL + " </td><td> " + list[i].TF_AL + "</td><td> K " + (i + 4) + " J" + (j + 3) + "</td></tr>";
                }

            }
            else if ((list[i].SOLES == "0") && (list[i].FM_DEL != "0" || list[i].FM_AL != "0" || list[i].BM_DEL != "0" || list[i].BM_AL != "0" || list[i].V_DEL != "0" || list[i].V_AL != "0" || list[i].TB_DEL != "0" || list[i].TB_AL != "0" || list[i].TF_DEL != "0" || list[i].TF_AL != "0"))
            {
                msj += "<tr><td>Soles es igual a 0. Sin embargo existen documentos de ventas registradas en la fila </td><td> </td><td></td><td> FILA : " + (i + 4) + "</td></tr>";
            }
            else if ((list[i].SOLES != "0") && (list[i].FM_DEL == "0" && list[i].FM_AL == "0" && list[i].BM_DEL == "0" && list[i].BM_AL == "0" && list[i].V_DEL == "0" && list[i].V_AL == "0" && list[i].TB_DEL == "0" && list[i].TB_AL == "0" && list[i].TF_DEL == "0" && list[i].TF_AL == "0"))
            {
                msj += "<tr><td>Existen registros de documentos de ventas. Sin embargo el campo SOLES es igual a 0 </td><td> </td><td></td><td> FILA : " + (i + 4) + "</td></tr>";
            }

        }
        return msj;

    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string RegistrarDatos(List<datosAlmacenados> list, string mes, string regAño, string regMes)
    {
        ClaseSql SQL = new ClaseSql();
        string rpta = "";
        DateTime fecha = DateTime.Now;
        int year = Convert.ToInt32(regAño);
        int month = Convert.ToInt32(regMes);
        int dias = DateTime.DaysInMonth(year, month);
        for (int i = 0; i < dias; i++)
        {

            rpta = SQL.GuardarDataSql(mes, list[i].fecha, list[i].FM_DEL, list[i].FM_AL, list[i].BM_DEL, list[i].BM_AL, list[i].V_DEL, list[i].V_AL, list[i].TB_DEL, list[i].TB_AL, list[i].TF_DEL, list[i].TF_AL, list[i].SOLES);
        }

        return rpta;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string reporte(string data)
    {
        ClaseSql sql = new ClaseSql();
        reporteDiario report = new reporteDiario();
        report = sql.reporte(data);
        var json = new JavaScriptSerializer().Serialize(report);
        return json;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string datosUser()
    {
        ClaseSql sql = new ClaseSql();
        var data = (Usuario)HttpContext.Current.Session["Login"];
        var json = new JavaScriptSerializer().Serialize(data);
        return json;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string datosGuardados(string mes,string año)
    {
        ClaseSql sql = new ClaseSql();
        var data = sql.cargaActual(mes,año);
        var json = new JavaScriptSerializer().Serialize(data);
        return json;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string datosDiarios(List<Clase> tabla)
    {
        string resp = "";
        ClaseSql sql = new ClaseSql();
        foreach (Clase item in tabla)
        {
            for (var i = 0; i < tabla.Count; i++)
            {
                resp = sql.GuardarDataDiaria(Validacion(item.A), Validacion(item.B), Validacion(item.C), Validacion(item.D), Validacion(item.E), Validacion(item.F), Validacion(item.G), Validacion(item.H), Validacion(item.I), Validacion(item.J), Validacion(item.K), Validacion(item.L), Validacion(item.M), Validacion(item.N), Validacion(item.O), Validacion(item.P), Validacion(item.A));
            }
        }
        return resp;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string EliminarVD(string fecha)
    {
        ClaseSql sql = new ClaseSql();
        var user = (Usuario)HttpContext.Current.Session["Login"];
        var resp = sql.EliminarReg(user.id, fecha);
        if (resp == "si")
        {
            return "si";
        }
        else
        {
            return "no";
        }

    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string mesesCerrados()
    {
        ClaseSql sql = new ClaseSql();
        var user = (Usuario)HttpContext.Current.Session["Login"];
        var data = sql.mesCerrado(user.id);
        var json = new JavaScriptSerializer().Serialize(data);
        return json;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string cerrarMes(mesCerrado datos)
    {
        Usuario user = (Usuario)HttpContext.Current.Session["Login"];
        datos.usuario = user.id;
        ClaseSql sql = new ClaseSql();
        var data = sql.cerrarMes(datos);
        return data;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string cambiarContraseña(string pw)
    {
        Usuario datos = (Usuario)HttpContext.Current.Session["datosUser"];
        ClaseSql sql = new ClaseSql();
        var data = sql.cambiarContraseña(datos, pw);
        return data;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string capturarDatos(string correo, string telefono, string encargado)
    {
        Usuario datos = (Usuario)HttpContext.Current.Session["datosUser"];
        datos.correo = correo;
        datos.telefono = telefono;
        datos.encargado = encargado;
        ClaseSql sql = new ClaseSql();
        var data = sql.capturarDatos(datos);
        return data;
    }
}
