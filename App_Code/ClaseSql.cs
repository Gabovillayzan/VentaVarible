using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public class ClaseSql
{
    //string cadenaConexion = "Persist Security Info=False;User ID=VentaVariable;Password=VentaV4r;Initial Catalog=VentaVariableDB;Server=192.168.20.38";
    string cadenaConexion = ConfigurationManager.ConnectionStrings["conVenta"].ConnectionString;

    public string GuardarDataSql(string mes, string celda1, string celda2, string celda3, string celda4, string celda5, string celda6, string celda7, string celda8, string celda9, string celda10, string celda11, string celda12)
    {
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        string estado;
        Usuario data = (Usuario)HttpContext.Current.Session["datosUser"];
        string user = data.usuario;
        double total = 0;
        total = Convert.ToDouble(celda12);
        var hoy = mes + "-" + celda1;
        conex.Open();
        string textoCmd = "INSERT INTO VENTA (USUARIO,FECHA_REG,FM_DEL,FM_AL,BM_DEL,BM_AL,V_DEL,V_AL,TB_DEL,TB_AL,TF_DEL,TF_AL,MONTO,CIERRE,TOTAL) VALUES('" + user + "','" + hoy + "'," + celda2 + "," + celda3 + "," + celda4 + "," + celda5 + "," + celda6 + "," + celda7 + "," + celda8 + "," + celda9 + "," + celda10 + "," + celda11 + "," + celda12 + ",'NO'," + total + ")";
        SqlCommand cmd = new SqlCommand(textoCmd, conex);
        try
        {
            int valor = cmd.ExecuteNonQuery();
            if (valor > 0)
            {
                estado = "bien";
            }
            else
            {
                estado = "error";
            }
        }
        catch (Exception ex)
        {
            estado = "error: " + ex.Message;
        }
        conex.Close();
        return estado;
    }
    public string GuardarDataDiaria(string celda1, string celda2, string celda3, string celda4, string celda5, string celda6, string celda7, string celda8, string celda9, string celda10, string celda11, string celda12, string celda13, string celda14, string celda15, string celda16, string celda17)
    {
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        string estado;
        Usuario data = (Usuario)HttpContext.Current.Session["datosUser"];
        string user = data.usuario;
        var total = Convert.ToDouble(celda12);
        var hoy = celda1;
        conex.Open();
        string textoCmd = "INSERT INTO VENTA (USUARIO,FECHA_REG,FM_DEL,FM_AL,BM_DEL,BM_AL,V_DEL,V_AL,TB_DEL,TB_AL,TF_DEL,TF_AL,MONTO,CIERRE,TOTAL) VALUES('" + user + "','" + hoy + "'," + celda2 + "," + celda3 + "," + celda4 + "," + celda5 + "," + celda6 + "," + celda7 + "," + celda8 + "," + celda9 + "," + celda10 + "," + celda11 + "," + celda12 + ",'NO'," + total + ")";
        SqlCommand cmd = new SqlCommand(textoCmd, conex);
        int valor = cmd.ExecuteNonQuery();
        if (valor > 0)
        {
            estado = "bien";
        }
        else
        {
            estado = "error";
        }
        conex.Close();
        return estado;
    }
    public string ConsultarSesion(string usuario, string contraseña)
    {
        try
        {
            SqlConnection conex = new SqlConnection();
            conex.ConnectionString = cadenaConexion;

            string querySQL = "sp_login";
            SqlCommand cmd = new SqlCommand(querySQL, conex);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@user", usuario);
            cmd.Parameters.AddWithValue("@pass", contraseña);
            cmd.Parameters.Add("@valor", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            conex.Open();
            cmd.ExecuteNonQuery();
            string valor = cmd.Parameters["@valor"].Value.ToString();
            return Convert.ToString(valor);
        }
        catch (Exception exep)
        {
            string msj = exep.Message;
            throw;
        }
        


    }
    public reporteDiario reporte(string mesConsulta)
    {
        reporteDiario datosReporte = new reporteDiario();
        //string usuario = HttpContext.Current.Session["login"].ToString();        
        Usuario dataUser = new Usuario();
        Usuario data = (Usuario)HttpContext.Current.Session["datosUser"];
        string usuario = data.usuario;
        string rs = data.razonSoc;
        //DateTime mes = DateTime.Now;
        string mes = mesConsulta;
        string[] valuesList = new string[4];
        SqlConnection conexion = new SqlConnection();
        conexion.ConnectionString = cadenaConexion;
        conexion.Open();
        //Read from the database
        SqlCommand command = new SqlCommand("SELECT FECHA_REG,MONTO,DOLAR FROM VENTA WHERE USUARIO LIKE '" + usuario + "' and FECHA_REG like '" + mes + "-%';", conexion);
        SqlDataReader dataReader = command.ExecuteReader();
        datosReporte.usuario = usuario;
        datosReporte.empresa = rs;
        datosReporte.aeropuerto = data.aeropuerto;
        List<diario> testMes = new List<diario>();
        while (dataReader.Read())
        {
            string fecha1 = dataReader[0].ToString();
            string soles1 = dataReader[1].ToString();
            string dolares1 = dataReader[2].ToString();
            testMes.Add(new diario() { fecha = fecha1, soles = soles1, dolares = dolares1 });
        }
        datosReporte.reporte = testMes;
        conexion.Close();
        return datosReporte;
    }
    public Usuario datosUser(string user)
    {
        Usuario usuario = new Usuario();
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        conex.Open();
        //SELECT U.USUARIO,U.NIVEL, C.RAZON_SOC,U.AEROPUERTO,U.DESCRIPCION, U.ID_USER FROM T_CLIENTE AS C,T_USUARIO AS U WHERE U.USUARIO = '" + user + "' and U.CLIENTE = c.ID_CLIENTE

        string querySQL = "SELECT U.USUARIO,U.NIVEL, C.RAZON_SOC,U.AEROPUERTO,U.DESCRIPCION, U.ID_USER, U.CORREO,U.TELEFONO,U.ENCARGADO, U.EXONERADO FROM T_CLIENTE AS C,T_USUARIO AS U WHERE U.USUARIO = '" + user + "' and U.CLIENTE = c.ID_CLIENTE";
        SqlCommand cmd = new SqlCommand(querySQL, conex);
        SqlDataReader dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            usuario.usuario = dr[0].ToString();
            usuario.nivel = dr[1].ToString();
            usuario.razonSoc = dr[2].ToString();
            usuario.aeropuerto = dr[3].ToString();
            usuario.descripcion = dr[4].ToString();
            usuario.id = dr[5].ToString();
            usuario.correo = dr[6].ToString();
            usuario.telefono = dr[7].ToString();
            usuario.encargado = dr[8].ToString();
            usuario.exonerado = dr[9].ToString();
        }
        conex.Close();
        return usuario;
    }
    //public void CerrarConexion(SqlConnection conex)
    //{
    //    conex.Close();
    //}
    public List<datosAlmacenados> cargaActual(string mesConsulta, string añoConsulta)
    {
        var usuario = (Usuario)HttpContext.Current.Session["login"];
        SqlConnection conexion = new SqlConnection();
        conexion.ConnectionString = cadenaConexion;
        conexion.Open();
        //Read from the database
        //#region armar fecha
        //DateTime fecha = DateTime.Now;
        //int mes = fecha.Month;
        //int año = fecha.Year;
        //int dias = System.DateTime.DaysInMonth(año, mes);
        //#endregion
        SqlCommand command = new SqlCommand("sp_cargaActual", conexion);
        command.Parameters.AddWithValue("@mes", mesConsulta);
        command.Parameters.AddWithValue("@year", añoConsulta);
        command.Parameters.AddWithValue("@usuario", usuario.usuario);
        command.CommandType = System.Data.CommandType.StoredProcedure;

        List<datosAlmacenados> cargaAct = new List<datosAlmacenados>();

        SqlDataReader dataReader = command.ExecuteReader();
        while (dataReader.Read())
        {
            //no funciona!
            datosAlmacenados data = new datosAlmacenados();
            data.fecha = dataReader[0].ToString();
            data.FM_DEL = dataReader[1].ToString();
            data.FM_AL = dataReader[2].ToString();
            data.BM_DEL = dataReader[3].ToString();
            data.BM_AL = dataReader[4].ToString();
            data.V_DEL = dataReader[5].ToString();
            data.V_AL = dataReader[6].ToString();
            data.TB_DEL = dataReader[7].ToString();
            data.TB_AL = dataReader[8].ToString();
            data.TF_DEL = dataReader[9].ToString();
            data.TF_AL = dataReader[10].ToString();
            data.SOLES = dataReader[11].ToString();
            data.CIERRE = dataReader[12].ToString();
            cargaAct.Add(data);
        }
        conexion.Close();


        return cargaAct;
    }
    public string EliminarReg(string user, string fecha)
    {
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        conex.Open();
        string querySQL = "delete VENTA where usuario like '" + user + "' and FECHA_REG  = '" + fecha + "'";
        SqlCommand cmd = new SqlCommand(querySQL, conex);
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            conex.Close();
            return "si";
        }
        else
        {
            conex.Close();
            return "no";
        }



    }
    public void GuardarInconsistencia(string data)
    {
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        Usuario user = (Usuario)HttpContext.Current.Session["datosUser"];
        conex.Open();
        string textoCmd = "INSERT INTO T_INCONSISTENCIA(CADENA,USUARIO,FECHA_REG,ESTADO) VALUES ('" + data + "','" + user.id + "',GETDATE(),'SI');";
        SqlCommand cmd = new SqlCommand(textoCmd, conex);
        int valor = cmd.ExecuteNonQuery();
        conex.Close();

    }
    //METODOS PARA LA VISTA ADMINISTRADOR
    public List<Cliente> ConsultarEmpresas(string ruc)
    {

        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        conex.Open();
        //SELECT U.USUARIO,U.NIVEL, C.RAZON_SOC,U.AEROPUERTO,U.DESCRIPCION FROM T_CLIENTE AS C,T_USUARIO AS U WHERE U.USUARIO = '" + user + "' and U.CLIENTE = c.ID_CLIENTE
        string querySQL = "select DISTINCT c.ID_CLIENTE, c.RAZON_SOC from T_USUARIO as u inner join T_CLIENTE as c on u.CLIENTE = c.ID_CLIENTE where AEROPUERTO = '" + ruc + "'";
        SqlCommand cmd = new SqlCommand(querySQL, conex);
        SqlDataReader dr = cmd.ExecuteReader();
        var clientes = new List<Cliente>();
        while (dr.Read())
        {
            Cliente cli = new Cliente();
            cli.id = dr[0].ToString();
            cli.razonSoc = dr[1].ToString();
            clientes.Add(cli);
        }
        conex.Close();
        return clientes;
    }
    public List<Cliente> ConsultarCliente()
    {

        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        conex.Open();
        //SELECT U.USUARIO,U.NIVEL, C.RAZON_SOC,U.AEROPUERTO,U.DESCRIPCION FROM T_CLIENTE AS C,T_USUARIO AS U WHERE U.USUARIO = '" + user + "' and U.CLIENTE = c.ID_CLIENTE
        string querySQL = "select ID_CLIENTE, RAZON_SOC, ESTADO from T_CLIENTE  order by(razon_soc)";
        SqlCommand cmd = new SqlCommand(querySQL, conex);
        SqlDataReader dr = cmd.ExecuteReader();
        var clientes = new List<Cliente>();
        while (dr.Read())
        {
            Cliente cli = new Cliente();
            cli.id = dr[0].ToString();
            cli.razonSoc = dr[1].ToString();
            cli.estado = dr[2].ToString();
            clientes.Add(cli);
        }
        conex.Close();
        return clientes;
    }
    public List<Usuario> ConsultarUsuario(string dato)
    {
        var unUsuario = (dato == "") ? null : "and u.USUARIO='" + dato + "'";
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        conex.Open();
        //SELECT U.USUARIO,U.NIVEL, C.RAZON_SOC,U.AEROPUERTO,U.DESCRIPCION FROM T_CLIENTE AS C,T_USUARIO AS U WHERE U.USUARIO = '" + user + "' and U.CLIENTE = c.ID_CLIENTE
        string querySQL = "select AEROPUERTO, USUARIO ,RAZON_SOC,NIVEL, DESCRIPCION,ID_USER,EXONERADO,CORREO,TELEFONO,ENCARGADO from T_USUARIO U,T_CLIENTE C WHERE U.CLIENTE = C.ID_CLIENTE  " + unUsuario + " order by(AEROPUERTO)";
        SqlCommand cmd = new SqlCommand(querySQL, conex);
        SqlDataReader dr = cmd.ExecuteReader();
        var usuarios = new List<Usuario>();
        while (dr.Read())
        {
            Usuario usuario = new Usuario();
            usuario.aeropuerto = dr[0].ToString();
            usuario.usuario = dr[1].ToString();
            usuario.razonSoc = dr[2].ToString();
            usuario.nivel = dr[3].ToString();
            usuario.descripcion = dr[4].ToString();
            usuario.id = dr[5].ToString();
            usuario.exonerado = dr[6].ToString();
            usuario.correo = dr[7].ToString();
            usuario.telefono = dr[8].ToString();
            usuario.encargado = dr[9].ToString();
            usuarios.Add(usuario);
        }
        conex.Close();
        return usuarios;
    }
    public string RegistrarCliente(Cliente data)
    {
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        string estado;
        conex.Open();
        string textoCmd = " insert into T_CLIENTE (ID_CLIENTE,RAZON_SOC,ESTADO) values (" + data.id + ",'" + data.razonSoc + "','" + data.estado + "')";
        SqlCommand cmd = new SqlCommand(textoCmd, conex);
        int valor = cmd.ExecuteNonQuery();
        if (valor > 0)
        {
            estado = "bien";
        }
        else
        {
            estado = "error";
        }
        conex.Close();
        return estado;
    }
    public string ModificarClientes(Cliente datos)
    {
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        conex.Open();
        string querySQL = "update T_CLIENTE set ID_CLIENTE = " + datos.id + ", RAZON_SOC ='" + datos.razonSoc.ToUpper() + "',ESTADO ='" + datos.estado.ToUpper() + "',FECHA_MOD = GETDATE() WHERE ID_CLIENTE=" + datos.id;
        SqlCommand cmd = new SqlCommand(querySQL, conex);
        int valor = cmd.ExecuteNonQuery();
        conex.Close();
        return "si";
    }
    public string RegistrarUsuarios(Usuario data)
    {
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        string estado;
        conex.Open();
        string correo = (data.correo == "") ? null : data.correo;
        string telefono = (data.telefono == "") ? null : data.telefono;
        string encargado = (data.encargado == "") ? null : data.encargado;
        string textoCmd = "insert into T_USUARIO(ID_USER,CLIENTE,AEROPUERTO,USUARIO,CONTRASEÑA,NIVEL,DESCRIPCION,EXONERADO,CORREO,TELEFONO,ENCARGADO) values('" + data.id + "'," + data.razonSoc + ",'" + data.aeropuerto + "','" + data.usuario + "',PWDENCRYPT('" + data.pass + "'),'" + data.nivel + "','" + data.descripcion + "','" + data.exonerado + "','" + correo + "','" + telefono + "','" + encargado + "')";
        SqlCommand cmd = new SqlCommand(textoCmd, conex);
        int valor = cmd.ExecuteNonQuery();
        if (valor > 0)
        {
            estado = "bien";
        }
        else
        {
            estado = "error";
        }
        conex.Close();
        return estado;
    }
    public string ModificarUsuario(Usuario data)
    {
        string querySQL;
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        conex.Open();
        string correo = (data.correo == "") ? null : data.correo;
        string telefono = (data.telefono == "") ? null : data.telefono;
        string encargado = (data.encargado == "") ? null : data.encargado;
        if (data.pass == "")
        {
            querySQL = " update T_USUARIO set CLIENTE=" + data.razonSoc + ",AEROPUERTO='" + data.aeropuerto + "',USUARIO='" + data.usuario + "',NIVEL='" + data.nivel + "',DESCRIPCION='" + data.descripcion + "',EXONERADO='" + data.exonerado + "',CORREO='" + correo + "',TELEFONO='" + telefono + "',ENCARGADO='" + encargado + "' WHERE ID_USER='" + data.id + "'";
        }
        else
        {
            querySQL = " update T_USUARIO set CLIENTE=" + data.razonSoc + ",AEROPUERTO='" + data.aeropuerto + "',USUARIO='" + data.usuario + "', CONTRASEÑA=PWDENCRYPT('" + data.pass + "'),NIVEL='" + data.nivel + "',DESCRIPCION='" + data.descripcion + "',EXONERADO='" + data.exonerado + "',CORREO='" + correo + "',TELEFONO='" + telefono + "',ENCARGADO='" + encargado + "' WHERE ID_USER='" + data.id + "'";
        }

        SqlCommand cmd = new SqlCommand(querySQL, conex);
        int valor = cmd.ExecuteNonQuery();
        conex.Close();
        return "si";
    }
    public reporteDiario reporteAdministrador(string mesConsulta, string user)
    {
        reporteDiario datosReporte = new reporteDiario();
        //string usuario = HttpContext.Current.Session["login"].ToString();        
        string usuario = user;
        //DateTime mes = DateTime.Now;
        string mes = mesConsulta;
        string[] valuesList = new string[4];
        SqlConnection conexion = new SqlConnection();
        conexion.ConnectionString = cadenaConexion;
        conexion.Open();
        //Read from the database
        SqlCommand command = new SqlCommand("SELECT FECHA_REG,MONTO,DOLAR FROM VENTA WHERE USUARIO LIKE '" + usuario + "' and FECHA_REG like '" + mes + "-%';", conexion);

        SqlDataReader dataReader = command.ExecuteReader();
        datosReporte.usuario = usuario;
        datosReporte.empresa = "";
        datosReporte.aeropuerto = "";
        List<diario> testMes = new List<diario>();
        while (dataReader.Read())
        {
            //no funciona!

            string fecha1 = dataReader[0].ToString();
            string soles1 = dataReader[1].ToString();
            string dolares1 = dataReader[2].ToString();
            testMes.Add(new diario() { fecha = fecha1, soles = soles1, dolares = dolares1 });

        }
        datosReporte.reporte = testMes;
        conexion.Close();
        return datosReporte;
    }
    public List<adminRepoGen> repoAdminGeneral(string data1, string data2, int año, int mes)
    {
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        conex.Open();
        //select   c.ID_CLIENTE, a.ID_AEROPUERTO,SUM(V.MONTO)VENTA,MONTH(V.FECHA_REG)MES,YEAR(V.FECHA_REG)AÑO from VENTA AS V INNER JOIN T_USUARIO AS U ON V.USUARIO = U.USUARIO INNER JOIN T_AEROPUERTO AS A ON U.AEROPUERTO = A.ID_AEROPUERTO INNER JOIN T_CLIENTE AS C ON C.ID_CLIENTE = U.CLIENTE where c.ID_CLIENTE= 20514513172  GROUP BY   MONTH(V.FECHA_REG),YEAR(V.FECHA_REG),c.ID_CLIENTE, a.ID_AEROPUERTO order by YEAR(V.FECHA_REG);
        //SELECT U.USUARIO,U.NIVEL, C.RAZON_SOC,U.AEROPUERTO,U.DESCRIPCION FROM T_CLIENTE AS C,T_USUARIO AS U WHERE U.USUARIO = '" + user + "' and U.CLIENTE = c.ID_CLIENTE
        string querySQL = "";
        if (data1 == "TODOS")
        {
            if (data2 == "")
            {
                querySQL = "select SUM(V.MONTO)VENTA,MONTH(V.FECHA_REG)MES,YEAR(V.FECHA_REG)AÑO from VENTA AS V INNER JOIN T_USUARIO AS U ON V.USUARIO = U.USUARIO INNER JOIN T_AEROPUERTO AS A ON U.AEROPUERTO = A.ID_AEROPUERTO INNER JOIN T_CLIENTE AS C ON C.ID_CLIENTE = U.CLIENTE GROUP BY  MONTH(V.FECHA_REG),YEAR(V.FECHA_REG) order by YEAR(V.FECHA_REG)";
            }
            else
            {
                querySQL = "select   c.ID_CLIENTE, a.ID_AEROPUERTO,SUM(V.MONTO)VENTA,MONTH(V.FECHA_REG)MES,YEAR(V.FECHA_REG)AÑO from VENTA AS V INNER JOIN T_USUARIO AS U ON V.USUARIO = U.USUARIO INNER JOIN T_AEROPUERTO AS A ON U.AEROPUERTO = A.ID_AEROPUERTO INNER JOIN T_CLIENTE AS C ON C.ID_CLIENTE = U.CLIENTE where c.ID_CLIENTE= " + data2 + "  GROUP BY   MONTH(V.FECHA_REG),YEAR(V.FECHA_REG),c.ID_CLIENTE, a.ID_AEROPUERTO order by YEAR(V.FECHA_REG);";
            }

        }
        else
        {
            querySQL = "select   c.ID_CLIENTE, a.ID_AEROPUERTO,SUM(V.MONTO)VENTA,MONTH(V.FECHA_REG)MES,YEAR(V.FECHA_REG)AÑO from VENTA AS V INNER JOIN T_USUARIO AS U ON V.USUARIO = U.USUARIO INNER JOIN T_AEROPUERTO AS A ON U.AEROPUERTO = A.ID_AEROPUERTO INNER JOIN T_CLIENTE AS C ON C.ID_CLIENTE = U.CLIENTE where c.ID_CLIENTE= " + data2 + "  and a.ID_AEROPUERTO ='" + data1 + "' and YEAR(v.FECHA_REG)=" + año + " and MONTH(v.FECHA_REG)=" + mes + " group BY   MONTH(V.FECHA_REG),YEAR(V.FECHA_REG),c.ID_CLIENTE, a.ID_AEROPUERTO order by YEAR(V.FECHA_REG);";

        }
        SqlCommand cmd = new SqlCommand(querySQL, conex);
        SqlDataReader dr = cmd.ExecuteReader();
        List<adminRepoGen> testMes = new List<adminRepoGen>();
        var a = dr.FieldCount;
        while (dr.Read())
        {
            if (a == 3)
            {
                adminRepoGen repor = new adminRepoGen();
                repor.ventaMen = dr[0].ToString();
                repor.mes = dr[1].ToString();
                repor.año = dr[2].ToString();
                testMes.Add(repor);
            }
            else if (a == 4)
            {
                adminRepoGen repor = new adminRepoGen();
                repor.empresa = dr[0].ToString();
                repor.ventaMen = dr[1].ToString();
                repor.mes = dr[2].ToString();
                repor.año = dr[3].ToString();
                testMes.Add(repor);
            }
            else
            {
                adminRepoGen repor = new adminRepoGen();
                repor.empresa = dr[0].ToString();
                repor.aeropuerto = dr[1].ToString();
                repor.ventaMen = dr[2].ToString();
                repor.mes = dr[3].ToString();
                repor.año = dr[4].ToString();
                testMes.Add(repor);
            }

        }
        conex.Close();
        return testMes;
    }
    //-----------------------------------------
    public List<adminRepoGen> repoGlobalesAdminGeneral(string data1, string data2, int año, int mes)
    {
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        conex.Open();
        string querySQL = "";
        if (data1 == "TODOS")
        {
            if (data2 == "")
            {
                querySQL = "select SUM(V.MONTO)VENTA,MONTH(V.FECHA_REG)MES,YEAR(V.FECHA_REG)AÑO from VENTA AS V INNER JOIN T_USUARIO AS U ON V.USUARIO = U.USUARIO INNER JOIN T_AEROPUERTO AS A ON U.AEROPUERTO = A.ID_AEROPUERTO INNER JOIN T_CLIENTE AS C ON C.ID_CLIENTE = U.CLIENTE GROUP BY  MONTH(V.FECHA_REG),YEAR(V.FECHA_REG) order by YEAR(V.FECHA_REG),month(v.FECHA_REG)";
            }
            else
            {
                querySQL = "select   c.ID_CLIENTE, a.ID_AEROPUERTO,SUM(V.MONTO)VENTA,MONTH(V.FECHA_REG)MES,YEAR(V.FECHA_REG)AÑO from VENTA AS V INNER JOIN T_USUARIO AS U ON V.USUARIO = U.USUARIO INNER JOIN T_AEROPUERTO AS A ON U.AEROPUERTO = A.ID_AEROPUERTO INNER JOIN T_CLIENTE AS C ON C.ID_CLIENTE = U.CLIENTE where c.ID_CLIENTE= " + data2 + "  GROUP BY   MONTH(V.FECHA_REG),YEAR(V.FECHA_REG),c.ID_CLIENTE, a.ID_AEROPUERTO order by YEAR(V.FECHA_REG);";
            }

        }
        else
        {
            querySQL = " select SUM(V.MONTO)VENTA,MONTH(V.FECHA_REG)MES,YEAR(V.FECHA_REG)AÑO from VENTA AS V INNER JOIN T_USUARIO AS U ON V.USUARIO = U.USUARIO INNER JOIN T_AEROPUERTO AS A ON U.AEROPUERTO = A.ID_AEROPUERTO INNER JOIN T_CLIENTE AS C ON C.ID_CLIENTE = U.CLIENTE where a.ID_AEROPUERTO ='" + data1 + "' GROUP BY  MONTH(V.FECHA_REG),YEAR(V.FECHA_REG) order by YEAR(V.FECHA_REG)";

        }
        SqlCommand cmd = new SqlCommand(querySQL, conex);
        SqlDataReader dr = cmd.ExecuteReader();
        List<adminRepoGen> testMes = new List<adminRepoGen>();
        var a = dr.FieldCount;
        while (dr.Read())
        {
            if (a == 3)
            {
                adminRepoGen repor = new adminRepoGen();
                repor.ventaMen = dr[0].ToString();
                repor.mes = dr[1].ToString();
                repor.año = dr[2].ToString();
                testMes.Add(repor);
            }
            else if (a == 4)
            {
                adminRepoGen repor = new adminRepoGen();
                repor.empresa = dr[0].ToString();
                repor.ventaMen = dr[1].ToString();
                repor.mes = dr[2].ToString();
                repor.año = dr[3].ToString();
                testMes.Add(repor);
            }
            else
            {
                adminRepoGen repor = new adminRepoGen();
                repor.empresa = dr[0].ToString();
                repor.aeropuerto = dr[1].ToString();
                repor.ventaMen = dr[2].ToString();
                repor.mes = dr[3].ToString();
                repor.año = dr[4].ToString();
                testMes.Add(repor);
            }

        }
        conex.Close();
        return testMes;
    }
    public List<mesCerrado> mesCerrado(string user)
    {

        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        conex.Open();
        //SELECT U.USUARIO,U.NIVEL, C.RAZON_SOC,U.AEROPUERTO,U.DESCRIPCION FROM T_CLIENTE AS C,T_USUARIO AS U WHERE U.USUARIO = '" + user + "' and U.CLIENTE = c.ID_CLIENTE
        string querySQL = "SELECT USUARIO,MONTH(FECHA_REG)mes,YEAR(FECHA_REG),sum(MONTO) ,CIERRE FROM VENTA WHERE USUARIO = '" + user + "' group by MONTH(FECHA_REG),YEAR(FECHA_REG),CIERRE,USUARIO";
        SqlCommand cmd = new SqlCommand(querySQL, conex);
        SqlDataReader dr = cmd.ExecuteReader();
        var meses = new List<mesCerrado>();
        while (dr.Read())
        {
            mesCerrado mes = new mesCerrado();
            mes.usuario = dr[0].ToString();
            mes.mes = dr[1].ToString();
            mes.año = dr[2].ToString();
            mes.monto = dr[3].ToString();
            mes.cerrado = dr[4].ToString();
            meses.Add(mes);
        }
        conex.Close();
        return meses;
    }
    public string cerrarMes(mesCerrado datos)
    {
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        conex.Open();
        string querySQL = "UPDATE VENTA SET CIERRE = 'SI' WHERE MONTH(FECHA_REG)=" + datos.mes + " AND YEAR(FECHA_REG)=" + datos.año + " AND USUARIO = '" + datos.usuario + "'";
        SqlCommand cmd = new SqlCommand(querySQL, conex);
        int valor = cmd.ExecuteNonQuery();
        conex.Close();
        return "si";
    }
    public string cambiarContraseña(Usuario data, string pw)
    {
        string querySQL;
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        conex.Open();
        querySQL = " update T_USUARIO set contraseña=PWDENCRYPT('" + pw + "') WHERE ID_USER='" + data.id + "'";
        SqlCommand cmd = new SqlCommand(querySQL, conex);
        int valor = cmd.ExecuteNonQuery();
        conex.Close();
        return "si";
    }
    public List<ReporteEspecifico> ReporteEspecifico(string aero, string emp, string mes, string año)
    {
        var sqlAero = (aero != "TODO") ? " AND A.ID_AEROPUERTO ='" + aero + "' " : "";
        var sqlEmp = (emp != "TODO") ? " AND U.CLIENTE = " + emp : "";
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        conex.Open();
        string querySQL = "select A.NOMBRE_AERO,C.RAZON_SOC,U.USUARIO,SUM(V.MONTO)VENTA,V.CIERRE,MONTH(V.FECHA_REG)MES,YEAR(V.FECHA_REG)AÑO,U.DESCRIPCION, U.EXONERADO,COUNT(v.FECHA_REG)cant from VENTA AS V INNER JOIN T_USUARIO AS U ON V.USUARIO = U.USUARIO INNER JOIN T_AEROPUERTO AS A ON U.AEROPUERTO = A.ID_AEROPUERTO INNER JOIN T_CLIENTE AS C ON C.ID_CLIENTE = U.CLIENTE WHERE YEAR(v.FECHA_REG)=" + año + " and MONTH(v.FECHA_REG)=" + mes + " " + sqlAero + sqlEmp + " GROUP BY A.NOMBRE_AERO, C.RAZON_SOC,U.USUARIO,V.CIERRE, MONTH(V.FECHA_REG),YEAR(V.FECHA_REG),U.DESCRIPCION,U.EXONERADO";
        SqlCommand cmd = new SqlCommand(querySQL, conex);
        int cantRegistros = 0;
        SqlDataReader dr = cmd.ExecuteReader();
        var reportes = new List<ReporteEspecifico>();
        int dias = System.DateTime.DaysInMonth(Convert.ToInt32(año), Convert.ToInt32(mes));
        while (dr.Read())
        {
            ReporteEspecifico repo = new ReporteEspecifico();
            repo.aeropuerto = dr[0].ToString();
            repo.empresa = dr[1].ToString();
            repo.usuario = dr[2].ToString();
            repo.ventaMen = dr[3].ToString();
            repo.cierre = dr[4].ToString();
            repo.mes = dr[5].ToString();
            repo.año = dr[6].ToString();
            repo.descripcion = dr[7].ToString();
            repo.exonerado = dr[8].ToString();
            repo.porcentaje = Convert.ToString((Convert.ToInt32(dr[9]) * 100) / dias);
            reportes.Add(repo);
            cantRegistros++;
        }
        conex.Close();
        return reportes;

    }
    public string capturarDatos(Usuario data)
    {
        string querySQL;
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        conex.Open();
        querySQL = "update T_USUARIO set CORREO='" + data.correo + "',TELEFONO='" + data.telefono + "',ENCARGADO='" + data.encargado + "' where  ID_USER='" + data.id + "'";
        SqlCommand cmd = new SqlCommand(querySQL, conex);
        int valor = cmd.ExecuteNonQuery();
        conex.Close();
        return "si";
    }
    public List<Usuario> UsuariosZona(string aeropuerto, string empresa)
    {
        string querySQL;
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        conex.Open();
        querySQL = "select ID_USER, USUARIO,DESCRIPCION from T_USUARIO where CLIENTE = '" + empresa + "' and AEROPUERTO = '" + aeropuerto + "';";
        SqlCommand cmd = new SqlCommand(querySQL, conex);
        SqlDataReader valoresTabla = cmd.ExecuteReader();
        List<Usuario> listUsuarios = new List<Usuario>();
        while (valoresTabla.Read())
        {
            Usuario usuario = new Usuario();
            usuario.id = valoresTabla[0].ToString();
            usuario.usuario = valoresTabla[1].ToString();
            usuario.descripcion = valoresTabla[2].ToString();
            listUsuarios.Add(usuario);
        }
        conex.Close();
        return listUsuarios;
    }
    public List<datosAlmacenados> auditoriaMensual(string empresa, string año, string mes)
    {
        string querySQL;
        SqlConnection conex = new SqlConnection();
        conex.ConnectionString = cadenaConexion;
        conex.Open();
        querySQL = "select FECHA_REG,FM_DEL,FM_AL,BM_DEL,BM_AL,V_DEL,V_AL,TB_DEL,TB_AL,TF_DEL,TF_AL,MONTO from VENTA where venta.USUARIO = '" + empresa + "' and MONTH(venta.FECHA_REG)=" + mes + " and YEAR(venta.FECHA_REG)=" + año;
        SqlCommand cmd = new SqlCommand(querySQL, conex);
        SqlDataReader valoresTabla = cmd.ExecuteReader();
        List<datosAlmacenados> listaDatos = new List<datosAlmacenados>();
        while (valoresTabla.Read())
        {
            datosAlmacenados usuario = new datosAlmacenados();
            usuario.fecha = valoresTabla[0].ToString();
            usuario.FM_DEL = valoresTabla[1].ToString();
            usuario.FM_AL = valoresTabla[2].ToString();
            usuario.BM_DEL = valoresTabla[3].ToString();
            usuario.BM_AL = valoresTabla[4].ToString();
            usuario.V_DEL = valoresTabla[5].ToString();
            usuario.V_AL = valoresTabla[6].ToString();
            usuario.TB_DEL = valoresTabla[7].ToString();
            usuario.TB_AL = valoresTabla[8].ToString();
            usuario.TF_DEL = valoresTabla[9].ToString();
            usuario.TF_AL = valoresTabla[10].ToString();
            usuario.SOLES = valoresTabla[11].ToString();
            listaDatos.Add(usuario);
        }
        conex.Close();
        return listaDatos;
    }
}