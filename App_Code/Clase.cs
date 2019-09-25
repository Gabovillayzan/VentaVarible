using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 

/// <summary>
/// Descripción breve de Clase
/// </summary>
public class Clase
{
    public string A
    {
        get;
        set;
    }


    public string B
    {
        get;
        set;
    }
    public string C
    {
        get;
        set;
    }


    public string D
    {
        get;
        set;
    }
    public string E
    {
        get;
        set;
    }


    public string F
    {
        get;
        set;
    }
    public string G
    {
        get;
        set;
    }


    public string H
    {
        get;
        set;
    }
    public string I
    {
        get;
        set;
    }


    public string J
    {
        get;
        set;
    }
    public string K
    {
        get;
        set;
    }


    public string L
    {
        get;
        set;
    }
    public string M
    {
        get;
        set;
    }


    public string N
    {
        get;
        set;
    }
    public string O
    {
        get;
        set;
    }


    public string P
    {
        get;
        set;
    }
    public string Q
    {
        get;
        set;
    }
}
public class diario
{
    public string fecha { get; set; }
    public string soles { get; set; }
    public string dolares { get; set; }
}
public class reporteDiario
{
    public string aeropuerto { get; set; }
    public string usuario { get; set; }
    public string empresa { get; set; }
    public List<diario> reporte { get; set; }
}
public class datosAlmacenados
{
    public string fecha
    {
        get;
        set;
    }
    public string FM_DEL
    {
        get;
        set;
    }
    public string FM_AL
    {
        get;
        set;
    }
    public string BM_DEL
    {
        get;
        set;
    }
    public string BM_AL
    {
        get;
        set;
    }
    public string V_DEL
    {
        get;
        set;
    }
    public string V_AL
    {
        get;
        set;
    }
    public string TB_DEL
    {
        get;
        set;
    }
    public string TB_AL
    {
        get;
        set;
    }
    public string TF_DEL
    {
        get;
        set;
    }
    public string TF_AL
    {
        get;
        set;
    }
    public string SOLES
    {
        get;
        set;
    }
    public string CIERRE { get; set; }
}
public class Cliente
{
    public string id { get; set; }
    public string razonSoc { get; set; }
    public string estado { get; set; }
}
public class mesCerrado
{
    public string usuario { get; set; }
    public string mes { get; set; }
    public string año { get; set; }
    public string monto { get; set; }
    public string cerrado { get; set; }
}
public class Usuario{
    public string id { get; set; }
    public string pass { get; set; }
    public string usuario { get; set; }
    public string razonSoc { get; set; }
    public string nivel { get; set; }
    public string aeropuerto { get; set; }
    public string descripcion { get; set; }
    public string exonerado { get; set; }
    public string correo { get; set; }
    public string telefono { get; set; }
    public string encargado { get; set; }
}
public class ReporteEspecifico
{
    public string aeropuerto { set; get; }
    public string empresa { set; get; }
    public string usuario { set; get; }
    public string ventaMen { set; get; }
    public string cierre { set; get; }
    public string mes { set; get; }
    public string año { set; get; }
    public string descripcion { set; get; }
    public string exonerado { set; get; }
    public string porcentaje { set; get; }
}
public class adminRepoGen:ReporteEspecifico
{
    //public string ventaMen { set; get; }
    //public string mes { set; get; }
    //public string año { set; get; }
    //public string empresa { set; get; }
}


