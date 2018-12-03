using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class schedaetichettegruppo : System.Web.UI.Page
{
    public ConnessioneFB FBConn = new ConnessioneFB();
    public DataSet ds = new DataSet();
	private www CLogga = new www();
	public user utenti = new user();
    public string s = "";
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
            Caricadatiddl(ddl.SelectedValue.ToString());
            //string s = ddlUbi.SelectedValue.ToString();
            //s = ddlUbi.SelectedItem.ToString();
            // devo caricare i dati della prima ubicazione.... forse
            ddlColori.Items.Clear();
            ddlColori.Items.Insert(0, new ListItem("", ""));
            ddlColori.Items.Insert(1, new ListItem("Verde chiaro", "AFCFAA"));
            ddlColori.Items.Insert(2, new ListItem("Rosa", "DE9AA4"));
            ddlColori.Items.Insert(3, new ListItem("Rosso corallo", "A93629"));
            ddlColori.Items.Insert(4, new ListItem("Turchese", "72AAA8"));
            ddlColori.Items.Insert(5, new ListItem("Giallo chiaro", "F0CFA1"));
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
            s = "update etichetta_gruppo set ";
            s += "etichetta =\'" + tTesto.Text + "\', ";
            s += "nome_prenotazione =\'" + tPrenotazione.Text + "\', ";
            s += "nome_delega =\'" + tDelega.Text + "\', ";
            s += "ordine=\'" + Ordine.Text.Trim() + "\', ";
            s += "abilitato=" + (cbAbilitata.Checked ? "1" : "0") + ", ";
            s += "durata_max =\'" + tDurataMax.Text + "\', ";
            s += "max_prenotazioni =\'" + tNumeroMaxPrenotazioni.Text + "\', ";
            s += "BLDislivello =" + (cBLPercorso.Checked ? "1" : "0") + ", BLElettrico =" + (cBLElettrici.Checked ? "1" : "0") + ", ";
            s += "colore = \'" + ddlColori.SelectedValue.ToString() + "\' ";
            where = " where id=" + ddl.SelectedValue;
            Int32 rr = FBConn.EsegueCmd(s + where, out msg);
			if (rr == 1)
			{
				sStato.Text = "Operazione salvataggio dati andata a buon fine.";
				id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
				CLogga.logga("etichetta_gruppo", s + where, 2, "Modifica etichetta_gruppo", id, out msgl);
			}
			else
				sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            Leggiddl();
            ddl.SelectedIndex = i;
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
            s = "SELECT a.* FROM etichetta_gruppo as a ";
            where = "where etichetta =\'" + tTesto.Text + "\'";
            ds.Clear();
            ds = FBConn.getfromDSet(s + where, "cègià", out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["cègià"].Rows.Count > 0)
                {
                    sStato.Text = "ATTENZIONE: una flotta con questo nome è già presente nel database!";
                }
                else //posso inserire nuova sede
                {
                    s = "insert into etichetta_gruppo (id, etichetta, nome_prenotazione, nome_delega, abilitato, ordine, durata_max, max_prenotazioni, bldislivello, blelettrico, colore) values (";
                    s += "null, \'" + tTesto.Text + "\', \'" + tPrenotazione.Text + "\', \'" + tDelega.Text + "\', ";
                    s += (cbAbilitata.Checked ? "1" : "0") + ", \'" + Ordine.Text.Trim() + "\', ";
                    s += "\'" + tDurataMax.Text + "\', ";
                    s += "\'" + tNumeroMaxPrenotazioni.Text + "\', ";
                    s += (cBLPercorso.Checked ? "1" : "0") + ", ";
                    s += (cBLElettrici.Checked ? "1" : "0") + ", ";
                    s += "\'" + ddlColori.SelectedValue.ToString() + "\')";
                    msg = "";
                    Int32 rr = FBConn.EsegueCmd(s, out msg);
                    if (rr == 1)
                    {
						id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
						CLogga.logga("etichetta_gruppo", s, 2, "Insert etichetta_gruppo voce", id, out msgl);
						sStato.Text = "Aggiunta nuova flotta andata a buon fine.";
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
            s = "delete FROM etichetta_gruppo ";
            where = "where id=" + ddl.SelectedValue + " and etichetta =\'" + tTesto.Text + "\'";
            msg = "";
            Int32 rr = FBConn.EsegueCmd(s + where, out msg);

			if (rr != 1)
				sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
			else
			{
				sStato.Text = "Cancellazione di " + tTesto.Text + " avvenuta con successo!";
				id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
				CLogga.logga("etichetta_gruppo", s + where, 3, "Cancellazione etichetta_gruppo", id, out msgl);
			}
            Leggiddl(); Caricadatiddl(ddl.SelectedValue.ToString());
        }
    }

    public void Leggiddl()
    {
        // devo leggere la tabella cambio
        msg = "";
		ds.Tables.Clear();
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
            s = "SELECT a.* FROM etichetta_gruppo as a order by a.etichetta";
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
                    ddl.Items.Insert(0, new ListItem("", ""));
                    for (int i = 0; i < ds.Tables["etichetta_gruppo"].Rows.Count; i++)
                    {
                        s = ds.Tables["etichetta_gruppo"].Rows[i]["etichetta"] == DBNull.Value ? "" : ds.Tables["etichetta_gruppo"].Rows[i]["etichetta"].ToString();
                        ss = ds.Tables["etichetta_gruppo"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["etichetta_gruppo"].Rows[i]["id"].ToString();
                        ddl.Items.Insert(i+1, new ListItem(s, ss));
                    }
                    ddl.SelectedIndex = 0;
                }
                else
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella etichetta_gruppo. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }

    public void Caricadatiddl(string id)
    {
        // devo leggere la tabella ubicazini
        msg = "";
		ds.Tables.Clear();
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
            s = "SELECT a.* FROM etichetta_gruppo as a " + (id == "" ? "-1" : " where a.id=" + id);
            ds.Clear();
            ds = FBConn.getfromDSet(s, "etichetta_gruppo", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["etichetta_gruppo"] != null && ds.Tables["etichetta_gruppo"].Rows.Count >= 1)
                {
                    tTesto.Text = ds.Tables["etichetta_gruppo"].Rows[0]["etichetta"] == DBNull.Value ? "" : ds.Tables["etichetta_gruppo"].Rows[0]["etichetta"].ToString();
                    tPrenotazione.Text = ds.Tables["etichetta_gruppo"].Rows[0]["nome_prenotazione"] == DBNull.Value ? "" : ds.Tables["etichetta_gruppo"].Rows[0]["nome_prenotazione"].ToString();
                    tDelega.Text = ds.Tables["etichetta_gruppo"].Rows[0]["nome_delega"] == DBNull.Value ? "" : ds.Tables["etichetta_gruppo"].Rows[0]["nome_delega"].ToString();
                    cbAbilitata.Checked = ds.Tables["etichetta_gruppo"].Rows[0]["abilitato"] == DBNull.Value ? false : ds.Tables["etichetta_gruppo"].Rows[0]["abilitato"].ToString() == "1" ? true : false;
                    Ordine.Text = ds.Tables["etichetta_gruppo"].Rows[0]["ordine"] == DBNull.Value ? "" : ds.Tables["etichetta_gruppo"].Rows[0]["ordine"].ToString();
                    cBLElettrici.Checked = ds.Tables["etichetta_gruppo"].Rows[0]["blelettrico"] == DBNull.Value ? false : ds.Tables["etichetta_gruppo"].Rows[0]["blelettrico"].ToString() == "1" ? true : false;
                    cBLPercorso.Checked = ds.Tables["etichetta_gruppo"].Rows[0]["bldislivello"] == DBNull.Value ? false : ds.Tables["etichetta_gruppo"].Rows[0]["bldislivello"].ToString() == "1" ? true : false;
                    tDurataMax.Text = ds.Tables["etichetta_gruppo"].Rows[0]["durata_max"] == DBNull.Value ? "" : ds.Tables["etichetta_gruppo"].Rows[0]["durata_max"].ToString();
                    tNumeroMaxPrenotazioni.Text = ds.Tables["etichetta_gruppo"].Rows[0]["max_prenotazioni"] == DBNull.Value ? "" : ds.Tables["etichetta_gruppo"].Rows[0]["max_prenotazioni"].ToString();
                    ddlColori.SelectedValue = ds.Tables["etichetta_gruppo"].Rows[0]["colore"] == DBNull.Value ? "" : ds.Tables["etichetta_gruppo"].Rows[0]["colore"].ToString();
                }
                else
                {
                    tTesto.Text = ""; tPrenotazione.Text = ""; tDelega.Text = "";
                    cbAbilitata.Checked = false;
                    Ordine.Text = "";
                    cBLElettrici.Checked = false;
                    cBLPercorso.Checked = false;
                    tDurataMax.Text = "";
                    tNumeroMaxPrenotazioni.Text = "";
                    ddlColori.SelectedIndex = 0;
                }                
            }
        }
    }
}