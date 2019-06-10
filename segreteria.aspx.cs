 /*
 * Copyright(C) 2017 Provincia Autonoma di Trento
 *
 * This file is part of<nome applicativo>.
 * Pitre is free software: you can redistribute it and/or modify
 * it under the terms of the LGPL as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Pitre is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the LGPL v. 3
 * along with Pitre.If not, see<https://www.gnu.org/licenses/lgpl.html>.
 * 
 */
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Collections;
using System.ComponentModel;

public partial class segreteria : System.Web.UI.Page
{
    public ConnessioneFB FBConn = new ConnessioneFB();
    public string msg;
    public string formatodata = "dd-MM-yyyy";
    public static user utenti = new user();
	public string idp;
	public Int32 idu = -1;
	private FbConnection cAFbConn = null;
    private ConnessioneFB FBClass = new ConnessioneFB();
    private FbDataAdapter cda = new FbDataAdapter();
    private FbCommand cc = new FbCommand();
    private DataSet ds = new DataSet();
    private DataTable tbl = new DataTable();
    private classeprenota pre = new classeprenota();
	public DataView dv;

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

            LBenvenuto.Text = " Benvenuto " + utenti.nome + " " + utenti.cognome;
            Session.Add("potere", utenti.potere);

            LeggiTarga();
            LeggiNumero();
            LeggiUbi();
			//pElenco.Visible = true;
		}
    }
	private bool checkSession()
	{
		idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;

		if (idu < 0 || !utenti.cercaid(idu))
		{
			string s = "Sessione scaduta. Prego ricollegarsi.";
			Session.Clear();
			Session.Abandon();
			ShowPopUpMsg(s);
			return (false);
		}
		Session.Timeout = 30; // ritacco il conteggio!!!
		Session.Add("strapotere", utenti.potere);
		return (true);
	}
	public void Leggiddl(string select, DropDownList ddl)
	{
		// devo leggere la tabella cambio
		msg = "";
		FBConn.openaFBConn(out msg);
		if (msg.Length >= 1)
		{
			sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
			FBConn.closeaFBConn(out msg);
		}
		else
		{
			msg = "";
			ds.Clear();
			string s;
			s = select;
			ds = FBConn.getfromDSet(s, "etichetta_gruppo", out msg);
			FBConn.closeaFBConn(out msg);
			if (msg.Length >= 1)
				sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
			else
			{
				if (ds.Tables["etichetta_gruppo"].Rows.Count > 0)
				{
					ddl.Items.Clear();
					string ss = "";
					s = "";
					ddl.Items.Insert(0, new ListItem(s, ss));
					for (int i = 0; i < ds.Tables["etichetta_gruppo"].Rows.Count; i++)
					{
						s = ds.Tables["etichetta_gruppo"].Rows[i]["etichetta"] == DBNull.Value ? "" : ds.Tables["etichetta_gruppo"].Rows[i]["etichetta"].ToString();
						ss = ds.Tables["etichetta_gruppo"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["etichetta_gruppo"].Rows[i]["id"].ToString();
						ddl.Items.Insert(i + 1, new ListItem(s, ss));
					}
				}
				else
					sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella etichetta_gruppo. Contattare l'assistenza al numero " + (string)Session["assistenza"];
			}
		}
	}
	public void LeggiUbi() // devo leggere la tabella Ubicazione e Ritiro chiavi
    {
        msg = "";
        FBConn.openaFBConn(out msg);
        if (msg.Length >= 1)
        {
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            FBConn.closeaFBConn(out msg);
        }
        else
        {
            msg = "";
            ds.Clear();
            ds = FBConn.getfromDSet("SELECT a.ID, a.UBICAZIONE, a.VIA, a.CIVICO, a.COMUNE_EK, a.DESCRIZIONE, a.FOTO1, a.FOTO2, a.ABILITATO, b.comune FROM UBICAZIONE a left join comuni b on a.comune_ek = b.COMUNE_K order by comune, ubicazione, via, civico", "Posteggio", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                ddlUbi.Items.Clear();
                string s, ss;
                if (ds.Tables["Posteggio"].Rows.Count > 0)
                {
                    //int k = 0;
                    for (int i = 0; i < ds.Tables["Posteggio"].Rows.Count; i++)
                    {
                        s = ds.Tables["Posteggio"].Rows[i]["comune"] == DBNull.Value ? "" : ds.Tables["Posteggio"].Rows[i]["comune"].ToString();
                        s += " " + (ds.Tables["Posteggio"].Rows[i]["ubicazione"] == DBNull.Value ? "" : ds.Tables["Posteggio"].Rows[i]["ubicazione"].ToString());
                        s += " " + (ds.Tables["Posteggio"].Rows[i]["via"] == DBNull.Value ? "" : ds.Tables["Posteggio"].Rows[i]["via"].ToString());
                        s += " " + (ds.Tables["Posteggio"].Rows[i]["civico"] == DBNull.Value ? "" : ds.Tables["Posteggio"].Rows[i]["civico"].ToString());
                        ss = ds.Tables["Posteggio"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["Posteggio"].Rows[i]["id"].ToString();
                        ddlUbi.Items.Insert(i, new ListItem(s, ss));                        
                    }
                    ddlUbi.Items.Insert(0, new ListItem("", ""));
                }
                else
                    if (ds.Tables["Posteggio"].Rows.Count != 0)
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella UBICAZIONE. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }
    public void LeggiTarga()
    {
        // ddltarga
        ds.Clear();
        string s, ss;
        s = "SELECT a.id, a.targa FROM MEZZI as a order by a.targa";
        ds = FBConn.getfromDSet(s, "targa", out msg);
        if (msg.Length >= 1)
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
        else
        {
            ddlTarga.Items.Clear();
            if (ds.Tables["targa"].Rows.Count > 0)
            {
                s = "";
                for (int i = 0; i < ds.Tables["targa"].Rows.Count; i++)
                {
                    //s = ds.Tables["mezzi"].Rows[i]["targa"] == DBNull.Value ? "" : ds.Tables["mezzi"].Rows[i]["targa"].ToString();
                    s = (ds.Tables["targa"].Rows[i]["targa"] == DBNull.Value ? "" : ds.Tables["targa"].Rows[i]["targa"].ToString());
                    ss = ds.Tables["targa"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["targa"].Rows[i]["id"].ToString();
                    ddlTarga.Items.Insert(i, new ListItem(s, ss));
                }
                ddlTarga.Items.Insert(0, new ListItem("", ""));
            }
            else
                if (ds.Tables["targa"].Rows.Count != 0)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella AUTOMEZZI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
        }
    }
    public void LeggiNumero()
    {
        // ddltarga
        ds.Clear();
        string s, ss;
        s = "SELECT a.id, a.numero FROM MEZZI as a where abilitato=1 order by a.numero";
        ds = FBConn.getfromDSet(s, "numero", out msg);
        if (msg.Length >= 1)
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
        else
        {
            ddlNumero.Items.Clear();
            if (ds.Tables["numero"].Rows.Count > 0)
            {
                s = "";
                for (int i = 0; i < ds.Tables["numero"].Rows.Count; i++)
                {
                    //s = ds.Tables["mezzi"].Rows[i]["targa"] == DBNull.Value ? "" : ds.Tables["mezzi"].Rows[i]["targa"].ToString();
                    s = (ds.Tables["numero"].Rows[i]["numero"] == DBNull.Value ? "" : ds.Tables["numero"].Rows[i]["numero"].ToString());
                    ss = ds.Tables["numero"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["numero"].Rows[i]["id"].ToString();
                    ddlNumero.Items.Insert(i, new ListItem(s, ss));
                }
                ddlNumero.Items.Insert(0, new ListItem("", ""));
            }
            else
                if (ds.Tables["numero"].Rows.Count != 0)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella AUTOMEZZI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
        }
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
	protected void cbCerca_Click(object sender, EventArgs e)
	{
		Cerca(sender, e, "partenza", "DESC");
	}
	protected void Cerca(object sender, EventArgs e, string colonna="partenza", string ordine="DESC")
    {
		if (colonna == null || colonna == "") colonna = "a.partenza";
		if (ordine == null || ordine == "") ordine = "DESC";
		string s;

		if (Session["potere"] == null)
		{
			s = "Sessione scaduta. Prego ricollegarsi.";
			ShowPopUpMsg(s);
			Response.Redirect("default.aspx");
		}
		bnuova.Visible = true;
		sStato.Text = "";

        // controlli
        string where = " where "; s = "";
        if (ddlTarga.SelectedValue.Trim().Length > 0) where += "upper(Targa)=\'" + ddlTarga.SelectedValue.Trim().ToUpper() + "\' and ";
        if (ddlNumero.SelectedValue.Trim().Length > 0) where += "upper(Numero)=\'" + ddlNumero.SelectedValue.Trim().ToUpper() + "\' and ";
        if (ddlUbi.SelectedValue.Trim().Length > 0) where += "ubicazione_ek=\'" + ddlUbi.SelectedValue.Trim() + "\' and ";
        DateTime dti = DateTime.Now.Date.AddDays(-365);
        DateTime dtf = DateTime.Now.Date.AddDays(+365);
		DateTime dt = DateTime.Now;
        if (cldDA.SelectedDate.Date.ToShortDateString() != "01/01/0001") dti = cldDA.SelectedDate; 
        if (cldA.SelectedDate.Date.ToShortDateString() != "01/01/0001" ) dtf = cldA.SelectedDate.AddDays(1);
        if (dti > dtf) { dt = dti; dti = dtf; dtf = dt; }
		//cldDA.SelectedDate = dti; cldA.SelectedDate = dtf;
		string qry = "";
		Int32 id = Session["iduser"] != null ? Convert.ToInt32(Session["iduser"].ToString()) : -1;
		if (id <= 0 || !utenti.cercaid(id))
		{
			if (idu < 0 || !utenti.cercaid(idu))
			{
				s = "Sessione scaduta. Prego ricollegarsi.";
				Session.Clear();
				Session.Abandon();
				ShowPopUpMsg(s);
				Response.Redirect("default.aspx?session=0");
			}
			Session.Timeout = 30; // ritacco il conteggio!!!
			Session.Add("strapotere", utenti.potere);
		}
		tbl = pre.PrenotazioniGerarchiche("", dti, dtf, tNome.Text, tCognome.Text, "", tMatricola.Text, ddlUbi.SelectedValue.Trim(), ddlTarga.SelectedItem.Text.Trim(), ddlNumero.SelectedItem.Text.Trim(), utenti.struttura_cod.Trim(), utenti.dipendeda_ek.Trim(), out qry, out msg); // mi ritorna le prenotazione sino a 365gg fa
        if (msg != null && msg != "")
        {
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            return;
        }
		if (tbl != null && tbl.Rows.Count > 0) // ci sono altre prenotazioni che si sovrappongono con quelle già effettute da quell'utente
		{
			Session.Add("qry", qry);
			pFiltro.Visible = false; // nascono il pannello filtri
			RiempiPrenotazioni(tbl, colonna, ordine);
			piallatext();
			ddlTarga.SelectedValue = "";
			ddlNumero.SelectedValue = "";
			ddlUbi.SelectedValue = "";
			return;
		}
		else
		{
			sStato.Text = "Non sono state trovate prenotazioni che soddisfano i criteri impostati.";
			bnuova.Visible = false;
		}
    }
    private void piallatext()
    {
        tMatricola.Text = "";
        tNome.Text = "";
        tCognome.Text = "";
    }
    public bool RiempiPrenotazioni(DataTable gwpre, string colonna = "numero", string ordine="ASC")
    {
		if (colonna == null || colonna == "")
			colonna = "";
		SortDirection direction;
		if (ordine == "ASC")
			direction = SortDirection.Ascending;
		else
			direction = SortDirection.Descending;
		if (gwpre == null || gwpre.Rows.Count <= 0)
		{
			sStato.Text = "ATTENZIONE: non ci sono records nella tabella....";
		}

		try
        {
            gwPrenotazioni.Columns[10].Visible = true;
            gwPrenotazioni.AllowSorting = true;
			//gwPrenotazioni.DataSource = "";
			//gwPrenotazioni.DataBind();
			gwPrenotazioni.DataSource = gwpre;
			gwpre.DefaultView.Sort = colonna;
			gwPrenotazioni.DataBind();
            //gwPrenotazioni.Sort(colonna, direction);
            gwPrenotazioni.Visible = true;
            gwPrenotazioni.Columns[10].Visible = false;
            int ok = 0;
            int argb = 0, argbold = -1;
            foreach (GridViewRow r in gwPrenotazioni.Rows) // faccio il giro e coloro
            {
                string col = r.Cells[10].Text.ToString();
                if (col != "&nbsp;" && col != "")
                {
                    argb = Int32.Parse(col, System.Globalization.NumberStyles.HexNumber);
                    r.BackColor = System.Drawing.Color.FromArgb(argb);
                    if (argb != argbold) { argbold = argb; ok++; }
                }
                else
                {
                    argb = 0;
                    if (argb != argbold) { argbold = argb; ok++; }
                }
            }						
			sStato.Text = "Prenotazioni trovate: " + gwpre.Rows.Count;
        }
        catch (Exception ex)
        {
            sStato.Text = "Riscontrato errore durante la ricerca utenti. Errore: " + ex.ToString() + " Avvertire l'amministratore al n. " + (string)Session["assistenza"].ToString();
            return (false);
        }
        gwPrenotazioni.Visible = true;
        return (true);
    }
    protected void gPrenotazioni_SelectedIndexChanged(object sender, EventArgs e)
    {
        int key = gwPrenotazioni.SelectedValue != null ? Convert.ToInt32(gwPrenotazioni.SelectedValue.ToString()) : -1;
		//gwPrenotazioni.DataSource = GetData();
		//gwPrenotazioni.DataBind();
		int riga = gwPrenotazioni.SelectedIndex;

		Session.Add("idp", key.ToString());

		GridViewRow row = gwPrenotazioni.SelectedRow;
        DateTime dt;
		
		dt = row.Cells[3] != null ? DateTime.Parse(row.Cells[3].Text) : DateTime.Now.Date.AddDays(-1);
        if (dt < DateTime.Now)
        {
            sStato.Text = "ATTENZIONE: non è possibile modificare una prenotazione con data di fine, già trascorsa!";
            return;
        }
        gwPrenotazioni.Visible = false;
        Response.Redirect("pre.aspx?idp=" + key.ToString());
    }
    protected void cbReset_Click(object sender, EventArgs e)
    {
        piallatext();
        ddlNumero.SelectedIndex = 0;
        ddlTarga.SelectedIndex = 0;
        ddlUbi.SelectedIndex = 0;
        gwPrenotazioni.Visible = false;
		cldDA.TodaysDate = DateTime.Now;
		cldDA.SelectedDates.Clear();
		cldA.TodaysDate = DateTime.Now;
		cldA.SelectedDates.Clear();
	}
	protected void bnuova_Click(object sender, EventArgs e)
	{
		piallatext();
		pFiltro.Visible = true;
		gwPrenotazioni.Visible = false;
		bnuova.Visible = false;
	}
	protected void cbhome_Click(object sender, EventArgs e)
	{
		gwPrenotazioni.Visible = true;
		cbcConferma.Visible = false;
        tRiepilogo.Visible = false;
        Panel1.Visible = true;
    }	
	protected void gwPrenotazioni_Sorting(object sender, GridViewSortEventArgs e)
	{
		string qry = Session["qry"].ToString() != null && Session["qry"].ToString() != "" ? Session["qry"].ToString() : "";
		if (qry != "")
		{
			msg = "";
			ds = FBClass.getfromDSet(qry, "prenotazioni", out msg);
			RiempiPrenotazioni(ds.Tables["prenotazioni"], e.SortExpression, e.SortDirection == SortDirection.Ascending ? "DESC" : "ASC");
		}
		//Cerca(this, e, e.SortExpression, "DESC");
	}
	protected void bSalvacsv_Click(object sender, EventArgs e)
	{

	}
    protected void cbOggi_Click(object sender, EventArgs e)
    {
        DateTime dti = DateTime.Now.Date;
        DateTime dtf = DateTime.Now.Date;
        DateTime dt = DateTime.Now;
        cldDA.SelectedDate = dti;
        cldA.SelectedDate = dtf;
        checkSession();
        Cerca(sender, e, "partenza", "DESC");
    }
    protected void cbRapido_Command(object sender, CommandEventArgs e)
    {
        checkSession();
        DateTime dti = DateTime.Now.Date;
        DateTime dtf = DateTime.Now.Date;
        switch (e.CommandArgument.ToString())
        {
            case "oggi":
                cldDA.SelectedDate = dti;
                cldA.SelectedDate = dtf;
                break;
            case "ieri":
                cldDA.SelectedDate = dti.Date.AddDays(-1);
                cldA.SelectedDate = dtf.Date.AddDays(-1);
                break;
            case "domani":
                cldDA.SelectedDate = dti.Date.AddDays(1);
                cldA.SelectedDate = dtf.Date.AddDays(1);
                break;
            case "corrente":
                cldDA.SelectedDate = dti.AddDays( - (int)dti.DayOfWeek + 1);
                cldA.SelectedDate = dtf.AddDays(7 - (int)dti.DayOfWeek + 0);
                break;
            case "passata":
                cldDA.SelectedDate = dti.AddDays(-7 -(int)dti.DayOfWeek + 1);
                cldA.SelectedDate = dtf.AddDays(- 7 + (int)dti.DayOfWeek + 1);
                break;
            case "prossima":
                cldDA.SelectedDate = dti.AddDays( - (int)dti.DayOfWeek + 7 + 1);
                cldA.SelectedDate = dtf.AddDays(7 - (int)dti.DayOfWeek + 7 + 0);
                break;
        }
        Cerca(sender, e, "partenza", "DESC");
    }
    protected void cbcCancella_Click(object sender, EventArgs e)
    {
        // Cerco la prenotazione, carico i dati in tbl
        // invio mail al prenotante
        // cancello la prenotazione
        string msg = "";
        idp = Session["idp"].ToString();
        pre.id = idp;
        tbl = pre.cercaid(idp, out msg);
        if (tbl != null && tbl.Rows.Count == 1)
            pre.refresh(tbl);
        string idu = Session["iduser"] != null ? Session["iduser"].ToString() : "-1";
        int rr = pre.Cancella(pre.id, idu, out msg);
        if (rr < 0)
        {
            sStato.Text = "Cancellazione non riusicta. Contattare il servizio assistenza al n. " + Session["assistenza"].ToString();
            cbcConferma.Visible = false;
            return;
        };
        if (rr == 0)
        {
            sStato.Text = "Non è possibile cancellare le richieste con data e ora di inizio già passate!";
            cbcConferma.Visible = false;
            return;
        }
        if (tbl != null && tbl.Rows.Count == 1)
        {
            gmail gm = new gmail();

            // OK ora   1) inoltro la password via mail; 2) aggiorno il campo cambio password;
            string[] chi = { pre.mail }; // in questa maniera definisco il numero di elementi dell'array
            gm.achi = chi;
            string[] ccn = { "tiziano.donati@provincia.tn.it" };
            //gm.achiccn = ccn;
            gm.subject = "Avviso di cancellazione della prenotazione automezzi in condivisione.";
            gm.body = "Buongiorno gentile " + pre.nome + " " + pre.cognome + ",\n\n";
            gm.body += "\tLa informiamo che è stata cancellata la Sua prenotazione i cui estremi sono di seguito riportati:\n";
            gm.body += "Destinazione: \t\t" + pre.dove_comune + "\n";
            gm.body += "Partenza:     \t\t" + pre.partenza + "\n";
            gm.body += "Arrivo:       \t\t" + pre.arrivo + "\n";
            gm.body += "Veicolo num.  \t\t" + pre.numero + "\n";
            gm.body += "Targa:        \t\t" + pre.targa + "\n";
            gm.body += "Marca-Modello:\t\t" + pre.marca + " " + pre.modello + "\n";
            gm.body += "Ritiro chiavi:\t\t" + pre.ubicazione_desc + "\n\n";
            gm.body += "Cordiali saluti.\n\n";
            gm.body += "Non rispondere alla mail. La presente è stata inviata da un sistema automatico.";
            gm.numeritel = "0461 - 496415";

            bool spedita = gm.mandamail("", 0, "", "", out msg);
            if (!spedita)
            {
                sStato.Text = "Inoltro mail fallito. Cancellazione non effettuata. Contattare l'assistenza al numero " + gm.numeritel;
                return;
            }
        }

        sStato.Text = "Cancellazione avvenuta! Avvisato l'utente con email...";
        cbcConferma.Visible = false;
        gwPrenotazioni.Visible = true;
        tRiepilogo.Visible = false;
        Panel1.Visible = true;
        bnuova_Click(this, e = new EventArgs());
    }
    protected void gwPrenotazioni_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gwPrenotazioni.Visible = false;
        Panel1.Visible = false;
        string s = "";

        foreach (DictionaryEntry keyEntry in e.Keys)
        {
            s += keyEntry.Value;
        }
        if (s.Length == 0)
            Response.Redirect("menu.aspx?l=anagrafica");

        // mi prendo la prenotazione che avevo selezionato per ultima...
        Session.Add("idp", s);
        pre.id = s;
        Label lcognome = (Label)gwPrenotazioni.Rows[e.RowIndex].FindControl("Cognome");
        Label lnome = (Label)gwPrenotazioni.Rows[e.RowIndex].FindControl("Nome");
        lPrenotante.Text = lcognome.Text + " " + lnome.Text;
        lPartenza.Text = gwPrenotazioni.Rows[e.RowIndex].Cells[1].Text;
        lRientro.Text = gwPrenotazioni.Rows[e.RowIndex].Cells[2].Text;
        lDestinazione.Text = gwPrenotazioni.Rows[e.RowIndex].Cells[3].Text;
        lVeicolo.Text = gwPrenotazioni.Rows[e.RowIndex].Cells[4].Text;
        lRitiro.Text = gwPrenotazioni.Rows[e.RowIndex].Cells[8].Text;
        lNumero.Text = gwPrenotazioni.Rows[e.RowIndex].Cells[6].Text + "    -    " + gwPrenotazioni.Rows[e.RowIndex].Cells[5].Text;
        //lVeicolo.Text = gwPrenotazioni.Rows[e.RowIndex].Cells[6].Text;
        tRiepilogo.Visible = true;
        cbcConferma.Visible = true;
        sStato.Text = "";
    }
    protected void gwPrenotazioni_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells[2] != null)
        {
            DateTime dt;
            DateTime.TryParse(e.Row.Cells[2].Text, out dt);
            if (dt < DateTime.Now)
                e.Row.Cells[0].Enabled = false;
        }
    }
}