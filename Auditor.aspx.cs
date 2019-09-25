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

public partial class Auditor : System.Web.UI.Page
{
    public ClaseSql SQL = new ClaseSql();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["Login"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            Usuario user = (Usuario)HttpContext.Current.Session["Login"];
            var datosU = SQL.datosUser(user.id);
            HttpContext.Current.Session["datosUser"] = datosU;
            if (datosU.nivel != "AUDITOR")
            {
                Server.Transfer("Login.aspx", true);
            }
        }
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string ConsultarUsuarios(string aero, string empresa)
    {
        ClaseSql sql = new ClaseSql();
        var data = sql.UsuariosZona(aero, empresa);
        var json = new JavaScriptSerializer().Serialize(data);
        return json;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string ConsultarUsuarios2(string empresa, string yy, string mm)
    {
        ClaseSql sql = new ClaseSql();
        var data = sql.auditoriaMensual(empresa, yy, mm);
        var json = new JavaScriptSerializer().Serialize(data);
        return json;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    //public static string ventasMensuales(string empresa, string yy, string month)
    public static string ventasMensuales()
    {
        ClaseSql sql = new ClaseSql();
        //var data = sql.auditoriaMensual(empresa, yy, month);
        //var json = new JavaScriptSerializer().Serialize(data);
        return "";
    }
}