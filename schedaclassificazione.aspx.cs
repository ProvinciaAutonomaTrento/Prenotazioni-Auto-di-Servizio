using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class schedaclassificazione : System.Web.UI.Page
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
            s = "update classificazione set classificazione=\'" + tTesto.Text + "\', abilitato=" + (cbAbilitata.Checked ? "1" : "0") + ", rischio=\'" + ddlSicurezza.SelectedValue + "\'"; ;
            where = " where id=" + ddl.SelectedValue;
            Int32 rr = FBConn.EsegueCmd(s + where, out msg);
			if (rr == 1)
			{
				sStato.Text = "Operazione salvataggio dati andata a buon fine.";
				id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
				CLogga.logga("cambio", s + where, 2, "Modifica classificazione", id, out msgl);
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
            s = "SELECT a.* FROM classificazione as a ";
            where = "where classificazione =\'" + tTesto.Text + "\'";
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
                    s = "insert into classificazione (id, classificazione, abilitato, rischio) values (null, \'" + tTesto.Text + "\', " + (cbAbilitata.Checked ? "1" : "0") + ", " + ddlSicurezza.SelectedValue + ")";
                    msg = "";
                    Int32 rr = FBConn.EsegueCmd(s, out msg);
					if (rr == 1)
					{
						sStato.Text = "Operazione salvataggio dati andata a buon fine.";
						id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
						CLogga.logga("cambio", s, 1, "Insert cambio", id, out msgl);
					}
					else
						sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
                    Leggiddl();
                    Caricadatiddl(ddl.SelectedValue);
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
            s = "delete FROM classificazione ";
            where = "where id=" + ddl.SelectedValue + " and classificazione =\'" + tTesto.Text + "\'";
            msg = "";
            Int32 rr = FBConn.EsegueCmd(s + where, out msg);

			if (rr != 1)
			{
				sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
				id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
				CLogga.logga("cambio", s + where, 3, "Delete classificazione cambio", id, out msgl);
			}
			else
				sStato.Text = "Cancellazione di " + tTesto.Text + " avvenuta con successo!";
            Leggiddl();
            Caricadatiddl(ddl.SelectedValue);
        }
    }

    public void Leggiddl()
    {
        // devo leggere la tabella classificazione
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
            s = "SELECT a.* FROM classificazione as a order by a.classificazione";
            ds = FBConn.getfromDSet(s, "clf", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                ddl.Items.Clear();
                if (ds.Tables["clf"].Rows.Count > 0)
                {
                    string ss = "";
                    s = "";
                    for (int i = 0; i < ds.Tables["clf"].Rows.Count; i++)
                    {
                        s = ds.Tables["clf"].Rows[i]["classificazione"] == DBNull.Value ? "" : ds.Tables["clf"].Rows[i]["classificazione"].ToString();
                        ss = ds.Tables["clf"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["clf"].Rows[i]["id"].ToString();
                        ddl.Items.Insert(i, new ListItem(s, ss));
                    }
                }
                else
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella CLASSIFICAZIONE. Contattare l'assistenza al numero " + (string)Session["assistenza"];
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
            s = "SELECT a.* FROM classificazione as a " + (id == "" ? "" : " where a.id=" + id);
            ds.Clear();
            ds = FBConn.getfromDSet(s, "clf", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["clf"].Rows.Count >= 1)
                {
                    tTesto.Text = ds.Tables["clf"].Rows[0]["classificazione"] == DBNull.Value ? "" : ds.Tables["clf"].Rows[0]["classificazione"].ToString();
                    cbAbilitata.Checked = ds.Tables["clf"].Rows[0]["abilitato"] == DBNull.Value ? false : ds.Tables["clf"].Rows[0]["abilitato"].ToString() == "1" ? true : false;
                    ddlSicurezza.SelectedValue = (ds.Tables["clf"].Rows[0]["rischio"] == DBNull.Value || ds.Tables["clf"].Rows[0]["rischio"].ToString() == "0") ? "5" : ds.Tables["clf"].Rows[0]["rischio"].ToString();
                }
                else
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella CLASSIFICAZIONE. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }
}