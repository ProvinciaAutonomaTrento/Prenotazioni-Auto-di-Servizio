/**
 * 
 * Copyright (C) 2017 Provincia Autonoma di Trento
 *
 * This file is part of <nome applicativo>.
 * Pitre is free software: you can redistribute it and/or modify
 * it under the terms of the LGPL as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Pitre is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the LGPL v. 3
 * along with Pitre. If not, see <https://www.gnu.org/licenses/lgpl.html>.
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;

public partial class _Default : Page
{
    public string TEST = "NO";
    public DataSet DSet = new DataSet();
    public string strSelect = "";
    public DataSet ds = new DataSet();
    public DataTable tbl = new DataTable();
    public string formatoeuro = "###.###.###";
    public string formatodata = "dd-MM-yyyy";
    public bool loggato;
    public string ms = "";
    public user utenti = new user();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA....
        {
            // devo vedere se ci manutenzioni/comunicazini in corso
            Manu manu = new Manu();
			if (TEST != "SI")
			{
				string ris = manu.Verifica(2); // manutenzione in corso
				if (ris.Trim().Length > 0)
				{
					Response.Redirect(ris.Trim());
					return;
				}
				ris = manu.Verifica(1);
				if (ris.Trim().Length > 0)
					sStato.Text = ris;
			}
            lAsterisconn.Text = ""; lAsteriscopwd.Text = "";
            string par = Request.QueryString["l"];
            loggato = par == "si" ? true : false;
			if (Request.QueryString["session"] != null && Request.QueryString["session"].ToString() == "0")
			{
				sStato.Text = "Sessione scaduta. Prego ricollegarsi!";
				sStato.ForeColor = Color.Red;
			}
			Session.Add("arrivo_da", (string)"default");
            Session.Add("assistenza", (string)"0461-496415 - 0461-496466");
        }
    }

    protected void cbpwddimenticata_Click1(object sender, EventArgs e)
    {
        string nn = Server.HtmlDecode(nikname.Text).ToString().Trim().ToUpper();
        utenti.nikname = nn;
        string msg = "";
        bool ok = utenti.dimenticatolapassword(nn, out msg);
        utenti.clearuser();
		sStato.Text = "Nuove credenziali di accesso inviate con successo.";
        ShowPopUpMsg(msg);
    }

    protected void cbAccedi_Click(object sender, EventArgs e)
    {
        string msg = "";
        utenti.nikname = Server.HtmlDecode(nikname.Text);
        utenti.password = Server.HtmlDecode(password.Text);
		msg = utenti.nikname.Trim() == "" ? "User name non inserito o inconsistente!" : "";
		msg += utenti.password.Trim() == "" ? "   Password mancante o inconsistente!" : "";
		if (msg != "")
		{
			sStato.Text = msg;
			lAsterisconn.Visible = true;
			lAsterisconn.Enabled = true;
			lAsterisconn.Text = "*";
			lAsterisconn.Enabled = false;
			lAsteriscopwd.Visible = true;
			lAsteriscopwd.Enabled = true;
			lAsteriscopwd.Text = "*";
			lAsteriscopwd.Enabled = false;
			Console.Beep();
			utenti.clearuser();
			return;
		}

        if (!utenti.cercanikname(utenti.nikname, utenti.password))
        {            
            sStato.Text = "Username o password non trovati!";
            lAsterisconn.Visible = true;
            lAsterisconn.Enabled = true; 
            lAsterisconn.Text = "*";            
            lAsterisconn.Enabled = false;
            lAsteriscopwd.Visible = true;
            lAsteriscopwd.Enabled = true;
            lAsteriscopwd.Text = "*";
            lAsteriscopwd.Enabled = false;
            Console.Beep();
            utenti.clearuser();
        }
        else
        {
            if (utenti.abilitato)
            {
                if (utenti.forzocambiopassword)
                {                    
                    sStato.Text = string.Format("Benvenuto {0} {1}. La tua password deve essere cambiata!", utenti.nome, utenti.cognome);
                    Session["arrivo_da"] = "login";
                    Session["iduser"] = utenti.iduser;  // devo registrarlo anche se non è un accesso definitivo
                    // devo registrare last access
                    msg = utenti.registraDTLA();
                    if (msg != "")
                    {
                        sStato.Text = msg;
                        return;
                    }
                    Response.Redirect("cambiopassword.aspx?l=si");
                }
                else
                {
                    Session.Add("iduser", utenti.iduser);  // questo è il primo punto dove determino se l'utente si è loggato o meno. L'altro sulla riuscita del cambio pasowr forzato.
                    Session.Add("potere", utenti.potere);  // mi serve per portarmelo appresso nelle maschere
                    sStato.Text = string.Format("Benvenuto {0} {1}", utenti.nome, utenti.cognome);
                    // devo registrare last access
                    msg = utenti.registraDTLA();
                    if (msg != "")
                    {
                        sStato.Text = msg;
                        return;
                    }                    
                    lAsterisconn.Text = "";
                    lAsterisconn.Visible = false;
                    lAsteriscopwd.Text = "";
                    lAsteriscopwd.Visible = false;
                    if (utenti.scadenza_patente == null || utenti.scadenza_patente < DateTime.Now.Date)
                    {
                        Response.Redirect("mydatas.aspx?idu=" + utenti.iduser + "&msg=ATTENZIONE: patente scaduta o data scadenza documento errato o mancante!");
                    }
                    // solo ora può entrare in prenotazioni
                    if (utenti.potere >= 10)
                        Response.Redirect("menu.aspx?l=si");
                    else
                        Response.Redirect("pre.aspx?l=si");
                }
            }
            else // utente non abilitato
            {
                sStato.Text = string.Format("Benvenuto {0} {1}. Il suo account deve ancora essere abilitato. Se sono già trascorse almeno 24 ore dalla richiesta di registrazione, contatti l'assistenza al n. {2}", utenti.nome, utenti.cognome, (string)Session["assistenza"]);
                ShowPopUpMsg(sStato.Text);
            }
        }
    }

    protected void cbRegistrati_Click(object sender, EventArgs e)
    {
        Response.Redirect("mydatas.aspx");
    }

    protected void ShowPopUpMsg(string msg)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("alert('");
        sb.Append(msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'"));
        sb.Append("');");
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
    }

    protected string costruiscistringa(string sp, int n)
    {
        if (n <= 0) return ("");
        string s = "";
        for (int i = 1; i <= n; i++) s += sp;
        return (s);
    }

    protected string virgolette(string s)
    {
        string ss = "";
        string virgoletta = "'";
        string doppie = string.Format("{0}", "\"");
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == virgoletta[0])
                ss += virgoletta+virgoletta;
            else ss += s[i];
            /*else
                if (s[i] == doppie[0])
                    ss += virgoletta;
            else
                ss += s[i]; */
        }
        return (ss);
    } 
    protected void cbMyAccount_Click(object sender, EventArgs e)
    {
        Session["arrivo_da"] = "home";
        Response.Redirect("mydata.aspx?idu="+utenti.iduser);
    }
}