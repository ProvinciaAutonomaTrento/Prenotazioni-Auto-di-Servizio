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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class schedagomme : System.Web.UI.Page
{
    public ConnessioneFB FBConn = new ConnessioneFB();
    public DataSet ds = new DataSet();
	private www CLogga = new www();
	public user utenti = new user();
	Int32 idu;
	string msgl = "";
	Int32 id;
	public string msg;
    public string formatodata = "dd-MM-yyyy";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
        {
			checkSession();
			Leggiddl();
            LeggiSicurezza();
            Caricadatiddl(ddl.SelectedValue.ToString());
            //string s = ddlUbi.SelectedValue.ToString();
            //s = ddlUbi.SelectedItem.ToString();
            // devo caricare i dati della prima ubicazione.... forse
        }
    }
	private bool checkSession()
	{
		string s;
		idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;

		if (idu < 0 || !utenti.cercaid(idu))
		{
			s = "Sessione scaduta. Prego ricollegarsi.";
			Session.Clear();
			Session.Abandon();
			ShowPopUpMsg(s);
			Response.Redirect("default.aspx?session=0");
		}
		Session.Timeout = 30; // ritacco il conteggio!!!
		return (true);
	}
	protected void ShowPopUpMsg(string msg)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("alert('");
		sb.Append(msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'"));
		sb.Append("');");
		ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
	}
	protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        // si deve fare una procedura che legga i dati dell'ubicazione selezionate.... quindi si deve 
        // in leggi ubicazioni tirar su anche il codice ubicazione
        Caricadatiddl(ddl.SelectedValue.ToString());
    }

    protected void bSalva_Click(object sender, EventArgs e)
    {
		checkSession();
		// devo fare tutti i controlli per cedere la validità dei campi obbligatori        
		// devo controllare se ci ubicazioni già caricate (stessa via, stesso civico, stesso palazzo, stessa città
		msg = "";
        FBConn.openaFBConn(out msg);
        if (msg.Length >= 1)
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
        else
        {
            msg = "";
            ds.Clear();
            string s, where; int i = ddl.SelectedIndex;
            s = "update gomme set gomme=\'" + tTesto.Text + "\', abilitato=" + (cbAbilitata.Checked ? "1" : "0") + ", punti=\'" + ddlSicurezza.SelectedValue + "\'";
            where = " where id=" + ddl.SelectedValue;
            Int32 rr = FBConn.EsegueCmd(s + where, out msg);
            if (rr == 1)
            {
                sStato.Text = "Operazione salvataggio dati andata a buon fine.";
				id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
				CLogga.logga("gomme", s + where, 2, "Modifica gomme", id.ToString(), ddl.SelectedValue.ToString(), out msgl);
			}
            else
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            Leggiddl(); ddl.SelectedIndex = i;
        }
    }

    protected void bInsert_Click(object sender, EventArgs e)
    {
		checkSession();
		// devo fare tutti i controlli per cedere la validità dei campi obbligatori        
		// devo controllare se ci ubicazioni già caricate (stessa via, stesso civico, stesso palazzo, stessa città
		msg = "";
        FBConn.openaFBConn(out msg);
        if (msg.Length >= 1)
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
        else
        {
            msg = "";
            string s, where;
            s = "SELECT a.* FROM gomme as a ";
            where = "where gomme =\'" + tTesto.Text + "\'";
            ds.Clear();
            ds = FBConn.getfromDSet(s + where, "cègià", out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["cègià"].Rows.Count > 0)
                {
                    sStato.Text = "ATTENZIONE: un produttore con questo nome è già presente nel database!";
                }
                else //posso inserire nuova sede
                {
                    s = "insert into gomme (id, gomme, abilitato, punti) values (null, \'" + tTesto.Text + "\', " + (cbAbilitata.Checked ? "1" : "0") + ", " + ddlSicurezza.SelectedValue + ")  returning id ";
                    msg = "";
                    Int32 rr = FBConn.AddCmd(s, out msg);
                    if (rr > 0)
                    {
                        sStato.Text = "Operazione salvataggio dati andata a buon fine.";
						id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
						CLogga.logga("cambio", s, 1, "Insert gomme", id.ToString(), rr.ToString(), out msgl);
						Leggiddl(); Caricadatiddl(ddl.SelectedValue.ToString());
                    }
                    else
                        sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
                }
            }
        }
    }

    protected void bDel_Click(object sender, EventArgs e)
    {
        // devo fare tutti i controlli per cedere la validità dei campi obbligatori        
        // devo controllare se ci ubicazioni già caricate (stessa via, stesso civico, stesso palazzo, stessa città
        msg = "";
        FBConn.openaFBConn(out msg);
        if (msg.Length >= 1)
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
        else
        {
            msg = "";
            string s, where;
            s = "delete FROM gomme ";
            where = "where id=" + ddl.SelectedValue + " and gomme =\'" + tTesto.Text + "\'";
            msg = "";
            Int32 rr = FBConn.EsegueCmd(s + where, out msg);

			if (rr == 1)
			{
                id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
                CLogga.logga("cambio", s + where, 3, "Delete gomme", id.ToString(), ddl.SelectedValue.ToString(), out msgl);
                sStato.Text = "Cancellazione di " + tTesto.Text + " avvenuta con successo!";
            }
			else
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            Leggiddl(); Caricadatiddl(ddl.SelectedValue.ToString());
        }
    }

    public void Leggiddl()
    {
        // devo leggere la tabella gomme
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
            s = "SELECT a.* FROM gomme as a order by a.gomme";
            ds = FBConn.getfromDSet(s, "gmm", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                ddl.Items.Clear();
                if (ds.Tables["gmm"].Rows.Count > 0)
                {
                    string ss = "";
                    s = "";
                    for (int i = 0; i < ds.Tables["gmm"].Rows.Count; i++)
                    {
                        s = ds.Tables["gmm"].Rows[i]["gomme"] == DBNull.Value ? "" : ds.Tables["gmm"].Rows[i]["gomme"].ToString();
                        ss = ds.Tables["gmm"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["gmm"].Rows[i]["id"].ToString();
                        ddl.Items.Insert(i, new ListItem(s, ss));
                    }
                }
                else
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella gomme. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }

    }

    public void LeggiSicurezza()
    {
        // devo leggere la tabella gomme
        msg = "";
        ddlSicurezza.Items.Clear();
        string ss = "";
        string s = "";
        for (int i = 0; i < 10; i++) {
            s = i.ToString();
            ss = (i + 1).ToString();
            ddlSicurezza.Items.Insert(i, new ListItem(s, ss));
        }
    }

    public void Caricadatiddl(string id)
    {
        // devo leggere la tabella ubicazini
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
            string s;
            s = "SELECT a.* FROM gomme as a " + (id == "" ? "" : " where a.id=" + id);
            ds.Clear();
            ds = FBConn.getfromDSet(s, "gmm", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["gmm"].Rows.Count >= 1)
                {
                    tTesto.Text = ds.Tables["gmm"].Rows[0]["gomme"] == DBNull.Value ? "" : ds.Tables["gmm"].Rows[0]["gomme"].ToString();
                    cbAbilitata.Checked = ds.Tables["gmm"].Rows[0]["abilitato"] == DBNull.Value ? false : ds.Tables["gmm"].Rows[0]["abilitato"].ToString() == "1" ? true : false;
                    ddlSicurezza.SelectedValue = ds.Tables["gmm"].Rows[0]["punti"] == DBNull.Value ? "5" : ds.Tables["gmm"].Rows[0]["punti"].ToString();
                }
                else
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella PNEUMATICI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }
}




