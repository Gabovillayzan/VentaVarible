using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Collections;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

public partial class admin : System.Web.UI.Page
{
    public ClaseSql SQL = new ClaseSql();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["Login"] == null)
        {
            Response.Redirect("Login.aspx");
        }
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string ConsultarCliente()
    {
        ClaseSql sql = new ClaseSql();
        var data = sql.ConsultarCliente();
        var json = new JavaScriptSerializer().Serialize(data);
        return json;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string ConsultarUsuario(string dato)
    {
        ClaseSql sql = new ClaseSql();
        var data = sql.ConsultarUsuario(dato);
        var json = new JavaScriptSerializer().Serialize(data);
        return json;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string RegistrarClientes(Cliente data)
    {
        string resp = "";
        ClaseSql sql = new ClaseSql();
        resp = sql.RegistrarCliente(data);
        return resp;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string ModificarClientes(Cliente data)
    {
        string resp = "";
        ClaseSql sql = new ClaseSql();
        resp = sql.ModificarClientes(data);
        return resp;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string EmpresaZona(string valor)
    {
        ClaseSql sql = new ClaseSql();
        var data = sql.ConsultarEmpresas(valor);
        var json = new JavaScriptSerializer().Serialize(data);
        return json;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string ConsultarEmpresa()
    {
        ClaseSql sql = new ClaseSql();
        var data = sql.ConsultarCliente();
        var json = new JavaScriptSerializer().Serialize(data);
        return json;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string RegistrarUsuarios(Usuario data)
    {
        string resp = "";
        ClaseSql sql = new ClaseSql();
        resp = sql.RegistrarUsuarios(data);
        return resp;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string ModificarUsuario(Usuario data)
    {
        string resp = "";
        ClaseSql sql = new ClaseSql();
        resp = sql.ModificarUsuario(data);
        return resp;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string ReporteEspecifico(string aero, string emp, string mes, string año)
    {
        ClaseSql sql = new ClaseSql();
        var repor = sql.ReporteEspecifico(aero, emp, mes, año);
        var reporte = new JavaScriptSerializer().Serialize(repor);
        return reporte;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string reporte(string data, string user)
    {
        ClaseSql sql = new ClaseSql();
        reporteDiario report = new reporteDiario();
        report = sql.reporteAdministrador(data, user);
        var json = new JavaScriptSerializer().Serialize(report);
        return json;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string reporteGeneral(string aeropuerto, List<List<string>> empresas)
    {
        ClaseSql sql = new ClaseSql();
        int cant = empresas.Count;
        if (cant > 1 && empresas[0] != null)
        {
            DateTime tiempo = DateTime.Now;
            int yy = (tiempo.Year - 1);
            int ms = tiempo.Month;
            List<List<int>> LineaTiempo = new List<List<int>>();
            for (int j = 0; j < 12; j++)
            {
                List<int> mes = new List<int>();
                if (ms <= 11)
                {
                    mes.Add(yy);
                    mes.Add(ms);
                    LineaTiempo.Add(mes);
                    ms = ms + 1;
                }
                else
                {
                    ms = 0;
                    yy = yy + 1;
                    mes.Add(yy);
                    mes.Add(ms);
                    LineaTiempo.Add(mes);
                    ms = ms + 1;
                }

            }
            var algo = LineaTiempo;
            ArrayList datafinal = new ArrayList();
            datafinal.Add(new int[]{yy-1,ms});
            for (int k = 0; k < 12; k++)
            {
                ArrayList dataPorMes = new ArrayList();
                for (int i = 0; i < cant; i++)
                {
                    List<adminRepoGen> item = new List<adminRepoGen>();
                    var a = empresas[i][0];
                    item = sql.repoAdminGeneral(aeropuerto, empresas[i][0], LineaTiempo[k][0], LineaTiempo[k][1] + 1);

                    if (item.Count != 0)
                    {
                        dataPorMes.Add(item[0].ventaMen);
                    }
                    else
                    {
                        dataPorMes.Add(0);
                    }
                    
                }
                datafinal.Add(dataPorMes);
            }
            var json = new JavaScriptSerializer().Serialize(datafinal);
            return json;
        }
        else
        {
            List<adminRepoGen> report = new List<adminRepoGen>();
            var dato = (empresas[0] == null) ? "" : "";
            report = sql.repoGlobalesAdminGeneral(aeropuerto, dato, 0, 0);
            var json = new JavaScriptSerializer().Serialize(report);
            return json;
        }

    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string datosContacto(string usuario)
    {
        ClaseSql sql = new ClaseSql();
        var data = sql.datosUser(usuario);
        var json = new JavaScriptSerializer().Serialize(data);
        return json;
    }
}
