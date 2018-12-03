using System;
using System.Text;
using System.Web.UI;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Drawing;
using System.Web.UI.WebControls;

public partial class MyDatas : System.Web.UI.Page
{
	public bool loggato;
	public user utente = new user(); // ex static	

	public ConnessioneFB FBClass = new ConnessioneFB();
	public DataSet ds = new DataSet();
	public DataTable tbl = new DataTable();
	public gmail gm;
	public Color rosso = Color.Red;
	public Color nero = Color.Black;
	public Int32 idu = -1;

	protected void Page_Load(object sender, EventArgs e)
	{

		// posso arrivare da Registrati... senza nessun parametro nella chiamata
		// posso arrivare da defaul... con idu=iduser per patente scaduta
		// posso arrivare dal mondo con l=SI
		if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
		{
			string par = Request.QueryString["l"];
			idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;

			Leggiddl("select s.struttura, s.codice from strutture as s order by s.struttura", ddlStruttura);

			if (idu >= 0) // meglio utlizzare le variabili di sessione....
			{
				hlHome.NavigateUrl = "pre.aspx"; hltophome.NavigateUrl = "pre.aspx";
            }

			loggato = par == "si" || idu >= 0 ? true : false;

			if (loggato) // riempio la maschera con i dati
			{
				pPat.Visible = false;
				Int32 id = 0;
				Int32.TryParse(Session["iduser"].ToString(), out id);

				if (utente.cercaid(id))
				{
					cbRegistrati.Text = "Salva";
					tNikname.Text = utente.nikname;
					tNome.Text = utente.nome;
					tCognome.Text = utente.cognome;
					tMail1.Text = utente.mail;
					tMatricola.Text = utente.matricola;
					tEnte.Text = utente.ente;
					ddlStruttura.SelectedValue = utente.struttura_cod;
					if (utente.struttura == "")
					{
						cbnoninelenco.Checked = true;
						ddlStruttura.Visible = false;
						tEnte.Visible = true;
					}
					else
					{
						cbnoninelenco.Checked = false;
						ddlStruttura.Visible = true;
						tEnte.Visible = false;
					}
					tIndirizzo.Text = utente.indirizzo;
					tCivico.Text = utente.civico;
					tCap.Text = utente.cap;
					tCitta.Text = utente.città;
					tTelefono.Text = utente.telefono;
					tMail2.Text = utente.mail;
					cbConsenso.Checked = true;
					tScadenza.Text = utente.scadenza_patente.ToString("dd/MM/yyyy");
					if (Request.QueryString["msg"] != null)
						Stato(Request.QueryString["msg"].ToString(), rosso);
				}
				else
					Stato("Utente non trovato. Effettuare il login.", rosso);
			}
			else
				pPat.Visible = true;
		}
	}
	private bool checkSession()
	{
		idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;

		if (idu < 0 || !utente.cercaid(idu))
		{
			string s = "Sessione scaduta. Prego ricollegarsi.";
			Session.Clear();
			Session.Abandon();
			ShowPopUpMsg(s);
			return (false);
		}
		Session.Timeout = 30; // ritacco il conteggio!!!
		return (true);
	}
	public void ShowPopUpMsg(string msg)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("alert('");
		sb.Append(msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'"));
		sb.Append("');");
		ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
	}
	protected void Stato(string msg, Color c)
	{
		if (c == null) c = Color.Black;
		tStato.ForeColor = c;
		tStato.Text = msg;
	}
	protected void cbRegistrati_Click(object sender, EventArgs e)
	{
		// uno arriva qui per tre motivi:
		// 1) non è connesso e chiede la regisrtazione
		// 2) è già connesso e modifica i suoi dati

		string par = Request.QueryString["l"];
		if (par == "SI") checkSession();
		// avvia i controlli
		string er = "";
		if (tNikname.Text.Length < 5) er = "Username almeno 5 caratteri; ";
		if (tNome.Text.Length < 2) er += "Nome almeno 2 caratteri; ";
		if (tCognome.Text.Length < 2) er += "Cognome almeno 2 caratteri; ";
		if (tMail1.Text.Length < 6 || (tMail1.Text.IndexOf("@") < 1) || (tMail1.Text.IndexOf(".") < 1)) er += "E-Mail; ";
		if (tMail1.Text != tMail2.Text) er += "E-Mail e Conferma E-Mail; ";
		if (tMatricola.Text.Length < 4) er += "Matricola almeno 4 caratteri; ";
		if (tScadenza.Text.Length < 10) er += "Data scadenza licenza di guida; ";
		DateTime dt; dt = DateTime.Parse(tScadenza.Text.Trim());
		if (dt.AddDays(-10) <= DateTime.Now.Date) er += "Licenza di  guida in scadenza o già scaduta; ";
		if (cbnoninelenco.Checked == false)
		{
			if (ddlStruttura.SelectedItem.Value.ToString() == "")
				er += "Selezionare Ente di appartenenza; ";
			else
				tEnte.Text = ddlStruttura.SelectedItem.ToString();
		}
		else
		{ 
			if (tEnte.Text.Trim().Length < 3) er += "Ente almeno 3 caratteri; ";
		}
		if (tIndirizzo.Text.Length < 5) er += "Indirizzo almeno 5 caratteri; ";
		//if (tCivico.Text.Trim().Length < 1) er += "Civico; ";
		if (tCap.Text.Trim().Length < 5) er += "Cap ";
		if (tCitta.Text.Trim().Length < 2) er += "Città almeno 2 caratteri; ";
		if (tTelefono.Text.Trim().Length < 8) er += "Telefono almeno 8 caratteri; ";
		//if (tTelefono.Text.IndexOf(" ") > 0) er += "Telefono: solo numeri senza spazi; ";
		if (!cbConsenso.Checked) er += "Consenso al trattamento; ";

		if (er != "")
		{
			er = "Verificare la consistenza dei campi indicati. Dati errati o mancanti: " + er;
			Stato(er, rosso);
			return;
		}
		else
			Stato("", nero);

		Int32 id = 0;
		id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
		if (id > 0)
		{
			utente.iduser = id;
			utente.LeggiUtente(utente.iduser.ToString()); // carico i dati utente.. prima di aggiornarli
		}
		utente.nikname = tNikname.Text.Trim().ToUpper();
		utente.nome = tNome.Text.Trim();
		utente.cognome = tCognome.Text.Trim();
		utente.mail = tMail1.Text.Trim();
		utente.matricola = tMatricola.Text.Trim();
		utente.ente = tEnte.Text.Trim();
		utente.indirizzo = tIndirizzo.Text.Trim();
		utente.civico = tCivico.Text.Trim();
		utente.cap = tCap.Text.Trim();
		utente.città = tCitta.Text.Trim();
		utente.telefono = tTelefono.Text.Trim();
		if (cbnoninelenco.Checked)
		{
			utente.ente = tEnte.Text;
			utente.struttura_cod = "";
			utente.struttura_ek = utente.struttura_cod;
		}
		else
		{
			utente.struttura_cod = ddlStruttura.SelectedValue.ToString();
			utente.struttura_ek = utente.struttura_cod;
		}
		//utente.struttura_ek = utente.struttura_cod;
		utente.struttura = tEnte.Text; // ?? non so perchè 
		utente.scadenza_patente = dt;

		// non ci sono errori di inserimento e posso registrare la richiesta o la modifica
		string msg = "";
		if (cbRegistrati.Text == "Home") // ho appena chiesto la registrazione....
			Response.Redirect("default.aspx");

		if (cbRegistrati.Text == "Salva") // è una modifica
		{
			bool ok = utente.registradatiutente(out msg);
			if (!ok)
			{
				Stato("Modifiche non apportate. Problemi ricerca utente o sessione scaduta.", rosso);
				return;
			}
			ShowPopUpMsg("Modifiche apportate con successo.");
			//Response.Redirect("pre.aspx?l=si");
		}
		else
		{   // devo controllare se l'utente c'è già, se no lo registro ed invio mail di richiesta registrazione ed abilitazione
			// poi ritorno alla pagina di default con loggato = no
			DataSet ds = new DataSet();
			FBClass = new ConnessioneFB();
			if (FBClass.getFBdata("select * from utenti where nikname=\'" + utente.nikname + "\'", "utenti", out msg) > 0)
			//if (utente.niknamecè(utente.nikname)) // se utente con stesso nikname cerca di registrarsi.....
			{
				Stato("Utente con username " + utente.nikname + " già presente. Accertarsi di non essere già stati registrati o cambiare Username!", rosso);
				ShowPopUpMsg("Utente con username " + utente.nikname + " già presente. Accertarsi di non essere già stati registrati o cambiare Username!");
				return;
			}
			else
			{
				utente.password = utente.CalcolaPasswordCasuale(8, 3, 3, 3);
				utente.abilitato = false;
				utente.forzocambiopassword = true;
				utente.giornalizza = false;
				if (utente.aggiungiutente())
				{
					// ok registrazione effettuata. ora manda mail agli amministratori
					// devo rileggere il record aggiunto per avere l'id
					if (!utente.cercanikname(virgolette(utente.nikname), utente.password))
					{
						Stato("Richiesta di registrazione non andata a buon fine. Prego contattare l'assistenza al n. " + (string)Session["assistenza"], rosso);
						ShowPopUpMsg("Richiesta di registrazione non andata a buon fine. Prego contattare l'assistenza al n. " + (string)Session["assistenza"]);
					}

					Session.Add("Utente", utente.nikname);
					Session.Add("IDU", utente.iduser);

					// inoltro mail a tutti gli amministratori

					msg = "";
					ds.Clear();
					ds = FBClass.getfromDSet("select id, nikname, nome, cognome, mail, power from utenti where (power >= 50)", "Admin", out msg);
					int k = 0, n = ds.Tables["Admin"].HasErrors ? 0 : ds.Tables["Admin"].Rows.Count;
					string[] achiscc = new string[n];
					string[] achi = new string[1];
					achi[0] = utente.mail;

					for (int i = 0; i < n; i++)
					{
						if (ds.Tables["Admin"].Rows[i]["Mail"] != DBNull.Value)
							achiscc[k++] = ds.Tables["Admin"].Rows[i]["Mail"].ToString();
					}
					if (k == 0)
					{
						Stato("Attenzione: non ci sono amministratori che posso confermare la tua richiesta. Prego contattare il n. " + (string)Session["assistenza"], rosso);
						ShowPopUpMsg("Attenzione: non ci sono amministratori che posso confermare la tua richiesta. Prego contattare il n. " + (string)Session["assistenza"]);
					}
					else
					{
						gm = new gmail();
						gm.dachivisualizzato = "uff.gestionigenerali@provincia.tn.it";
						gm.achi = achi;
						gm.achicc = achiscc;
						gm.numeritel = "0461-496415";
						gm.subject = "Gestione flotta provinciale: richiesta registrazione nuovo utente";
						gm.body = "Buongiorno,\n";
						gm.body += "la informiamo che la richiesta di accesso all\'applicazione \"Prenotazioni autoveicoli di servizio\" è stata inoltrata correttamente.\n";
						gm.body += "Richiedente:\n";
						gm.body += "Username:          \t" + utente.nikname + "\n";
						gm.body += "Nome:              \t" + utente.nome + "\n";
						gm.body += "Cognome:           \t" + utente.cognome + "\n";
						gm.body += "Mail:              \t" + utente.mail + "\n";
						gm.body += "Tel.:              \t" + utente.telefono + "\n\n\n";
						gm.body += "Dopo l'approvazione, riceverà automaticamente la mail con le credenziali per l'accesso.\n\n\tGrazie.\n\nUff. Gestioni Generali\nGestore autorizzazioni";
						if (!gm.mandamail("", 0, "", "", out msg))
						{
							Stato("Richiesta di registrazione non eseguita! MAIL DI CONFERMA NON INOLTRATA!. CONTATTARE IL NUMERO " + (string)Session["assistenza"] + " Err: " + msg, rosso);
							//ShowPopUpMsg("Richiesta di registrazione effettuata con successo, MAIL DI CONFERMA NON INOLTRATA!. CONTATTARE IL NUMERO " + (string)Session["assistenza"] + " Err: " + msg);
						}
						else
						{
							Stato("Richiesta di registrazione effettuata con successo. Riceverà via email le credenziali per l'accesso.", nero);
							ShowPopUpMsg("Richiesta di registrazione effettuata con successo. Riceverà, entro 24 ore via email, le credenziali per l'accesso.");
							enablefields(false);
							cbRegistrati.Text = "Home";
						}
					}
				}
				else
					Stato("Richiesta di registrazione non andata a buon fine. Prego contattare l'assistenza al n. " + (string)Session["assistenza"], rosso);
			}
		}
	}
	void enablefields(bool cosa)
	{
		tNikname.Enabled = cosa;
		tNome.Enabled = cosa;
		tCognome.Enabled = cosa;
		tMail1.Enabled = cosa;
		tMail2.Enabled = cosa;
		tMatricola.Enabled = cosa;
		tEnte.Enabled = cosa;
		tScadenza.Enabled = cosa;
		tIndirizzo.Enabled = cosa;
		tCivico.Enabled = cosa;
		tCitta.Enabled = cosa;
		tCap.Enabled = cosa;
		tTelefono.Enabled = cosa;
		cbConsenso.Enabled = cosa;
		ddlStruttura.Enabled = cosa;
		cbnoninelenco.Enabled = cosa;
	}
	protected string virgolette(string s)
	{
		string ss = "";
		string virgoletta = "'";

		string doppie = string.Format("{0}", "\"");
		for (int j = 0; j < s.Length; j++)
		{
			if (s[j] == virgoletta[0])
				ss += virgoletta + virgoletta;
			else ss += s[j];
		}
		return (ss);
	}

	public void Leggiddl(string select, DropDownList ddl)
	{
		// devo leggere la tabella cambio
		string msg = "";
		msg = "";
		ds.Clear();
		string s;			
		ds = FBClass.getfromDSet(select, "tbl_ddl", out msg);
		FBClass.closeaFBConn(out msg);
		if (msg.Length >= 1)
			tStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
		else
		{
			if (ds.Tables["tbl_ddl"].Rows.Count > 0)
			{
				ddl.Items.Clear();
				string ss = "";
				s = "";
				ddl.Items.Insert(0, new ListItem(s, ss));
				for (int i = 0; i < ds.Tables["tbl_ddl"].Rows.Count; i++)
				{
					s = ds.Tables["tbl_ddl"].Rows[i][0] == DBNull.Value ? "" : ds.Tables["tbl_ddl"].Rows[i][0].ToString();
					ss = ds.Tables["tbl_ddl"].Rows[i][1] == DBNull.Value ? "" : ds.Tables["tbl_ddl"].Rows[i][1].ToString();
					ddl.Items.Insert(i + 1, new ListItem(s, ss));
				}
			}
			else
				tStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella etichetta_gruppo. Contattare l'assistenza al numero " + (string)Session["assistenza"];
		}
	}

	protected void cbnoninelenco_CheckedChanged(object sender, EventArgs e)
	{
		if (cbnoninelenco.Checked) { tEnte.Visible = true; ddlStruttura.Visible = false; } else
		{ tEnte.Visible = false; ddlStruttura.Visible = true; }
	}
}
