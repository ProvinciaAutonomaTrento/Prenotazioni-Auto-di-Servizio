using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class schedamodello : System.Web.UI.Page
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
            s = "update modello set marca=\'" + tTesto.Text + "\', abilitato=" + (cbAbilitata.Checked ? "1" : "0");
            where = " where id=" + ddl.SelectedValue;
            Int32 rr = FBConn.EsegueCmd(s + where, out msg);
			if (rr == 1)
			{
				sStato.Text = "Operazione salvataggio dati andata a buon fine.";
				id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
				CLogga.logga("modello", s + where, 2, "Modifica modello", id, out msgl);
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
            s = "SELECT a.* FROM modello as a ";
            where = "where modello =\'" + tTesto.Text + "\'";
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
                    s = "insert into modello (id, modello, abilitato) values (null, \'" + tTesto.Text + "\', " + (cbAbilitata.Checked ? "1" : "0") + ")";
                    msg = "";
                    Int32 rr = FBConn.EsegueCmd(s, out msg);
					if (rr == 1)
					{
						sStato.Text = "Operazione salvataggio dati andata a buon fine.";
						id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
						CLogga.logga("modello", s, 1, "Insert modello", id, out msgl);
					}
					else
						sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
                    Leggiddl();
                    Caricadatiddl(ddl.SelectedValue.ToString());
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
            s = "delete FROM modello ";
            where = "where id=" + ddl.SelectedValue + " and modello =\'" + tTesto.Text + "\'";
            msg = "";
            Int32 rr = FBConn.EsegueCmd(s + where, out msg);

            if (rr != 1)
			{
				sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
				id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
				CLogga.logga("modello", s + where, 3, "Delete modello", id, out msgl);
			}
			else
                sStato.Text = "Cancellazione di " + tTesto.Text + " avvenuta con successo!";
            Leggiddl();
            Caricadatiddl(ddl.SelectedValue.ToString());
        }
    }

    public void Leggiddl()
    {
        // devo leggere la tabella Marche
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
            s = "SELECT a.* FROM modello as a order by a.modello";
            ds = FBConn.getfromDSet(s, "mdl", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                ddl.Items.Clear();
                if (ds.Tables["mdl"].Rows.Count > 0)
                {
                    string ss = "";
                    s = "";
                    for (int i = 0; i < ds.Tables["mdl"].Rows.Count; i++)
                    {
                        s = ds.Tables["mdl"].Rows[i]["modello"] == DBNull.Value ? "" : ds.Tables["mdl"].Rows[i]["modello"].ToString();
                        ss = ds.Tables["mdl"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["mdl"].Rows[i]["id"].ToString();
                        ddl.Items.Insert(i, new ListItem(s, ss));
                    }
                }
                else
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella MARCHE. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
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
            s = "SELECT a.* FROM modello as a " + (id == "" ? "" : " where a.id=" + id);
            ds.Clear();
            ds = FBConn.getfromDSet(s, "mdl", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["mdl"].Rows.Count >= 1)
                {
                    tTesto.Text = ds.Tables["mdl"].Rows[0]["modello"] == DBNull.Value ? "" : ds.Tables["mdl"].Rows[0]["modello"].ToString();
                    cbAbilitata.Checked = ds.Tables["mdl"].Rows[0]["abilitato"] == DBNull.Value ? false : ds.Tables["mdl"].Rows[0]["abilitato"].ToString() == "1" ? true : false;
                }
                else
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella MARCHE. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }
}




