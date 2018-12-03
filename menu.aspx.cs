﻿using System;
using System.Web.UI;
using System.Data;
using System.Text;
using System.Drawing;

public partial class menu : System.Web.UI.Page
{
    public ConnessioneFB FBConn = new ConnessioneFB();
    public DataSet ds = new DataSet();
    public string msg;
    public string formatodata = "dd-MM-yyyy";
    public static user utenti = new user();
    public Color rosso = Color.Red;
    public Color nero = Color.Black;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
        {
            Int32 id = Session["iduser"] != null ? Convert.ToInt32(Session["iduser"].ToString()) : -1;
            if (id <= 0 || !utenti.cercaid(id))
            {
                string s = "Sessione scaduta. Prego ricollegarsi.";
                ShowPopUpMsg(s);
                Response.Redirect("default.aspx");
            }
            LBenvenuto.Text = " Benvenuto/a " + utenti.nome + " " + utenti.cognome;

			if (utenti.potere >= 10) pPrenota.Visible = true; else pPrenota.Visible = false;
			if (utenti.potere == 15) pServizio.Visible = true; else pServizio.Visible = false;
			if (utenti.potere >= 20) PRegistro.Visible = true; else PRegistro.Visible = false;
			if (utenti.potere >= 50)  pGestione.Visible = true; else pGestione.Visible = false; 
			if (utenti.potere >= 100) { pTabelle.Visible = true; pFlotta.Visible = true; }
			if (utenti.potere >= 120) pAggiorna.Visible = true;
				if (Request.QueryString["msg"] != null)
                Stato(Request.QueryString["msg"].ToString(), rosso);
        }
    }
    protected void Stato(string msg, Color c)
    {
        if (c == null) c = Color.Black;
        sStato.ForeColor = c;
        sStato.Text = msg;
    }

    protected void ShowPopUpMsg(string msg)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("alert('");
        sb.Append(msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'"));
        sb.Append("');");
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
    }

    protected void bUscita_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Response.Redirect("Default.aspx");
    }
}




