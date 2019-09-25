using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.SessionState;


public partial class Login : System.Web.UI.Page
{
   protected void Page_Load(object sender, EventArgs e)
    {
        HttpContext.Current.Session["login"] = null;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

    public static string Logueo(string user, string pass)
    {
        try
        {
            ClaseSql cnxSql = new ClaseSql();
            string resp = cnxSql.ConsultarSesion(user, pass);
            if (resp == "0")
            {
                var datosU = cnxSql.datosUser(user);
                HttpContext.Current.Session["login"] = datosU;
                if (datosU.nivel == "ADMIN")
                {
                    return "admin.aspx";
                }
                else if (datosU.nivel == "ESTANDAR")
                {
                    return "registroVentas.aspx";
                }
                else
                {
                    return "Auditor.aspx";
                }
            }
            else
            {
                return resp;
            }        
        }
        catch (Exception exep)
        {
            string msj = exep.Message;
            throw;
        }
        
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

    public static string cerrarSesion()
    {
            HttpContext.Current.Session["login"] = null;
            return "si";  
    }
}