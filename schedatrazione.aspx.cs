using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class schedatrazione : System.Web.UI.Page
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
            s = "update trazione set trazione=\'" + tTesto.Text + "\', abilitato=" + (cbAbilitata.Checked ? "1" : "0") + ", rischio=\'" + ddlSicurezza.SelectedValue + "\'";
            where = " where id=" + ddl.SelectedValue;
            Int32 rr = FBConn.EsegueCmd(s + where, out msg);
            if (rr == 1)
			{
				sStato.Text = "Operazione salvataggio dati andata a buon fine.";
				id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
				CLogga.logga("trazione", s + where, 2, "Modifica trazione", id, out msgl);
			}
            else
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg +". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            Leggiddl();
            ddl.SelectedIndex = i;
        }
    }

    protected void bInsert_Click(object sender, EventArgs e)
    {
		// devo fare tutti i controlli per cedere la validità dei campi obbligatori        
		// devo controllare se ci ubicazioni già caricate (stessa via, stesso civico, stesso palazzo, stessa città
		checkSession();
		msg = "";
        FBConn.openaFBConn(out msg);
        if (msg.Length >= 1)
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
        else
        {
            msg = "";
            string s, where;
            s = "SELECT a.* FROM trazione as a ";
            where = "where trazione =\'" + tTesto.Text + "\'";
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
                    s = "insert into trazione (id, trazione, abilitato, rischio) values (null, \'" + tTesto.Text + "\', " + (cbAbilitata.Checked ? "1" : "0") + ", " + ddlSicurezza.SelectedValue + ")";
                    msg = "";
                    Int32 rr = FBConn.EsegueCmd(s, out msg);
                    if (rr == 1)
                    {
                        sStato.Text = "Operazione inserimento dati andata a buon fine.";
						id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
						CLogga.logga("trazione", s, 1, "Inserimento trazione", id, out msgl);
						Leggiddl(); Caricadatiddl(ddl.SelectedValue);
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
            s = "delete FROM trazione ";
            where = "where id=" + ddl.SelectedValue + " and trazione =\'" + tTesto.Text + "\'";
            msg = "";
            Int32 rr = FBConn.EsegueCmd(s + where, out msg);

            if (rr != 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
                sStato.Text = "Cancellazione di " + tTesto.Text + " avvenuta con successo!";
			id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
			CLogga.logga("trazione", s + where, 3, "Delete trazione", id, out msgl);
			Leggiddl(); Caricadatiddl(ddl.SelectedValue);
        }
    }

    public void Leggiddl()
    {
        // devo leggere la tabella trazione
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
            s = "SELECT a.* FROM trazione as a order by a.trazione";
            ds = FBConn.getfromDSet(s, "trazione", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                ddl.Items.Clear();
                if (ds.Tables["trazione"].Rows.Count > 0)
                {
                    string ss = "";
                    s = "";
                    for (int i = 0; i < ds.Tables["trazione"].Rows.Count; i++)
                    {
                        s = ds.Tables["trazione"].Rows[i]["trazione"] == DBNull.Value ? "" : ds.Tables["trazione"].Rows[i]["trazione"].ToString();
                        ss = ds.Tables["trazione"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["trazione"].Rows[i]["id"].ToString();
                        ddl.Items.Insert(i, new ListItem(s, ss));
                    }
                }
                else
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella TRAZIONE. Contattare l'assistenza al numero " + (string)Session["assistenza"];
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
        for (int i = 0; i < 10; i++)
        {
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
            s = "SELECT a.* FROM trazione as a " + (id == "" ? "" : " where a.id=" + id);
            ds.Clear();
            ds = FBConn.getfromDSet(s, "trazione", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["trazione"].Rows.Count >= 1)
                {
                    tTesto.Text = ds.Tables["trazione"].Rows[0]["trazione"] == DBNull.Value ? "" : ds.Tables["trazione"].Rows[0]["trazione"].ToString();
                    cbAbilitata.Checked = ds.Tables["trazione"].Rows[0]["abilitato"] == DBNull.Value ? false : ds.Tables["trazione"].Rows[0]["abilitato"].ToString() == "1" ? true : false;
                    ddlSicurezza.SelectedValue = (ds.Tables["trazione"].Rows[0]["rischio"] == DBNull.Value || ds.Tables["trazione"].Rows[0]["rischio"].ToString() == "0") ? "5" : ds.Tables["trazione"].Rows[0]["rischio"].ToString();
                }
                else
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella TRAZIONE. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }
}




