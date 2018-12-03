using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.ComponentModel;

public partial class prenota : System.Web.UI.Page
{
	public ConnessioneFB FBConn = new ConnessioneFB();
	public DataSet ds = new DataSet();
	public DataTable tbl = new DataTable();
	public string s;
	public string msg;
	public string formatodata = "dd-MM-yyyy";
	public user utenti = new user();
	public user altro = new user();
	public classeprenota pre = new classeprenota(); // ex public static
	public string anome = "";
	public string acognome = "";
	public string aid = ""; // id altro utente... 
	public string apotere = "";
	private int strapotere;
	public string idp = ""; // ex static; è una modifica e contiene l'id della prenotazione che voglio modificare
	public bool giàinserito; // ex static
	public Int32 idu = -1;
	public DateTime dtinizio, dtfine;
	public string gw_select;
	public string gw_sort;
	public int prenotazionimax = 3;

	protected void Page_Load(object sender, EventArgs e)
	{
        bConferma.Enabled = true;
        if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
		{
			if (Request.QueryString["new"] != null)
			{
				sStato.Text = "una nuova prenotazione.....";
				Session.Add("aid", ""); Session.Add("idp", ""); // azzero sia altroutente che modifica prenotazione
			}
			if (Request.QueryString["idp"] != null)
			{
				Session.Add("idp", Request.QueryString["idp"]);
			}

			if (!checkSession()) Response.Redirect("default.aspx");
			if (utenti.potere >= 10) bhome.Visible = true; else bhome.Visible = false;
			
			// per sicurezza
			anome = ""; acognome = ""; apotere = "";
			if (Request.QueryString["new"] != null) // ????
				sStato.Text = "una nuova prenotazione.....";
			s = "";
			if (aid != "" && altro.cercaid(Int32.Parse(aid)))
				s = ". Prenotazione per " + altro.nome.ToUpper() + " " + altro.cognome.ToUpper();
			LBenvenuto.Text = " Benvenuto/a " + utenti.nome + " " + utenti.cognome + s;

			ddlOrainizio.Items.Clear();
			for (int i = 0; i < 24; i++)
				ddlOrainizio.Items.Insert(i, i.ToString().PadLeft(2, '0'));
			ddlOrainizio.SelectedValue = DateTime.Now.Hour.ToString().PadLeft(2, '0'); // metto ora di oggi anche se modifica ??

			ddlMininizio.Items.Clear();
			for (int i = 0; (i * 5) <= 55; i++)
				ddlMininizio.Items.Insert(i, (i * 5).ToString().PadLeft(2, '0'));
			ddlMininizio.SelectedValue = string.Format("{0}", (((int)DateTime.Now.Minute) / 5) * 5);

			ddlOrafine.Items.Clear();
			for (int i = 0; i < 24; i++)
				ddlOrafine.Items.Insert(i, i.ToString().PadLeft(2, '0'));
			//ddlOrafine.SelectedValue = string.Format("{0}", (int)DateTime.Now.Hour + 1);

			ddlMinfine.Items.Clear();
			for (int i = 0; (i * 5) <= 55; i++)
				ddlMinfine.Items.Insert(i, (i * 5).ToString().PadLeft(2, '0'));
			//ddlMinfine.SelectedValue = string.Format("{0}", (((int)DateTime.Now.Minute) / 5) * 5);

			ddlPasseggeri.Items.Clear();
			for (int i = 0; i <= 4; i++)
				ddlPasseggeri.Items.Insert(i, i.ToString());
			LeggiUbi(utenti.iduser.ToString());

			// aggiungo valori alla ddl Stato
			ddlStato.Items.Clear();
			ddlStato.Items.Insert(0, new ListItem("Italia", "0"));
			ddlStato.Items.Insert(1, new ListItem("Estero", "1"));
			LeggiProvince();

			// vedo se è una modifica
			if (idp != null && idp != "")
			{
				tbl = pre.cercaid(idp, out msg);
				if (tbl == null || tbl.Rows.Count != 1)
				{
					sStato.Text = "PROBLEMA SU TENTATIVO DI MODIFICA!";
				}
				else
				{
					if (pre.refresh(tbl))
					{
						//sStato.Text = "SI TRATTA DI UNA MODIFICA!";
						ddlProvincia.SelectedValue = pre.dove_prov_ek.ToString().Trim();
						ddlProvincia_SelectedIndexChanged(this, e = new EventArgs()); // leggo i comuni come da ddlProvincia
						ddlStato.SelectedValue = pre.dove_stato_ek.ToString().Trim();
						if (ddlStato.SelectedValue == "1") { tComuneEstero.Enabled = true; tComuneEstero.Text = pre.dove_comune; tComuneEstero.Enabled = false; }
						ddlComune.SelectedValue = pre.dove_comune_ek.ToString().Trim();
						CldInizio.SelectedDates.Clear();
						CldInizio.SelectedDate = pre.partenza.Date;
						CldInizio.VisibleDate = CldInizio.SelectedDate;
						CldFine.SelectedDates.Clear();
						CldFine.SelectedDate = pre.arrivo.Date;
						CldFine.VisibleDate = CldFine.SelectedDate;
						ddlOrainizio.SelectedValue = Right((pre.partenza.Hour.ToString()).PadLeft(2, '0'),2);
						ddlOrafine.SelectedValue = Right(("0" + pre.arrivo.Hour.ToString()).PadLeft(2, '0'), 2);
						ddlMininizio.SelectedValue = Right(pre.partenza.Minute.ToString().PadLeft(2, '0'), 2);
						ddlMinfine.SelectedValue = Right(pre.arrivo.Minute.ToString().PadLeft(2, '0'), 2);
						ddlRitiro.SelectedValue = pre.ubicazione_ek.ToString().Trim();
						ddlPasseggeri.SelectedValue = pre.aggregati.ToString();
						tMotivo.Text = pre.motivo;
						ddlProvincia.Enabled = false;
						ddlComune.Enabled = false;
						ddlStato.Enabled = false;
						ddlRitiro.Enabled = false;
						tComuneEstero.Enabled = false;

						if (pre.user_ek != null && pre.user_ek != "")
						{
							Session.Add("aid", pre.user_ek.ToString());
							aid = pre.user_ek;
							if (altro.cercaid(Int32.Parse(aid)))
							{
								s = ". Prenotazione per conto di " + altro.nome.ToUpper() + " " + altro.cognome.ToUpper();
								LBenvenuto.Text = " Benvenuto/a " + utenti.nome + " " + utenti.cognome + s;
							}
						}
					}
				}
			}
			else
			{
				ddlProvincia.SelectedValue = "22"; // se non è una modifica... preseleziono quella di Trento
				ddlProvincia_SelectedIndexChanged(this, e = new EventArgs());
			}
			initChartLibere(utenti.iduser.ToString());
			ddlProvincia.Focus();
		}
		else
		{
			idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
			initChartLibere(idu.ToString());
		}
	}
	private bool checkSession()
	{
		idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
		aid = Session["aid"] != null ? Session["aid"].ToString() : "";
		idp = Session["idp"] != null ? Session["idp"].ToString() : "";
		Session.Add("idprenotazione", idp);

		if (idu < 0 || !utenti.cercaid(idu))
		{
			s = "Sessione scaduta. Prego ricollegarsi.";
			Session.Clear();
			Session.Abandon();
			ShowPopUpMsg(s);
			return (false);
		}
		Session.Timeout = 30; // ritacco il conteggio!!!
		Session.Add("strapotere", utenti.potere);
		return (true);
	}
	protected void LeggiProvince()
	{
		// devo leggere la tabella Province
		msg = "";
		FBConn.openaFBConn(out msg);
		if (msg.Length >= 1)
			sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
		else
		{
			msg = "";
			ds.Clear();
			ds = FBConn.getfromDSet("select * from province order by provincia", "province", out msg);
			FBConn.closeaFBConn(out msg);
			if (msg.Length >= 1)
				sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
			else
			{
				if (ds.Tables["province"].Rows.Count > 0)
				{
					ddlProvincia.Items.Clear();
					string s = "", ss = ""; int k = 0;
					for (int i = 0; i < ds.Tables["province"].Rows.Count; i++)
					{
						s = ds.Tables["province"].Rows[i]["provincia"] == DBNull.Value ? "" : ds.Tables["province"].Rows[i]["provincia"].ToString();
						s += ds.Tables["province"].Rows[i]["sigla"] == DBNull.Value ? "" : ", " + ds.Tables["province"].Rows[i]["sigla"].ToString();
						ss = ds.Tables["province"].Rows[i]["provincia_k"] == DBNull.Value ? "" : ds.Tables["province"].Rows[i]["provincia_k"].ToString().Trim();
						ddlProvincia.Items.Insert(i, new ListItem(s, ss));
						//if (s == "Trento") k = i;
					}
					//ddlProvincia.SelectedIndex = k;
				}
				else
					sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella UBICAZIONI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
			}
		}
	}
	protected void LeggiUbi(string iduser)    // devo leggere la tabella ubicazini
	{
		msg = "";
		//FBConn.openaFBConn(out msg);
		if (msg.Length >= 1)
			sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
		else
		{
			msg = "";
			ds.Clear();
			string s;
			s = "select a.*, b.comune from ubicazione as a left join comuni as b on a.comune_ek=b.comune_k where a.abilitato > 0 order by b.comune, a.ubicazione";
			s = "SELECT distinct  ub.id, co.comune, ub.via, ub.civico, ub.ubicazione ";
			s += "FROM UTENTE_GRUPPO ug ";
			s += "left join utenti as u on u.id=ug.utente_ek ";
			s += "left join gruppi as gr on gr.gruppo_ek=ug.gruppo_EK ";
			s += "left join mezzi as m on m.id=gr.auto_ek ";
			s += "left join UBICAZIONE as ub on ub.id=m.ubicazione_ek ";
			s += "left join COMUNI as co on co.comune_k=ub.comune_ek ";
			s += "left join ETICHETTA_GRUPPO as e on e.id=gr.gruppo_ek ";
			s += "where m.abilitato=1 and utente_ek=" + iduser + " ";
			s += "order by e.ordine desc, co.comune, ub.via, ub.civico ";		
			ds = FBConn.getfromDSet(s, "ubicazioni", out msg);
			FBConn.closeaFBConn(out s);
			if (msg.Length >= 1)
				sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
			else
			{
				if (ds.Tables["ubicazioni"].Rows.Count > 0)
				{
					ddlRitiro.Items.Clear();
					string ss = "";
					s = "";
					ddlRitiro.Items.Insert(0, new ListItem("", ""));
					for (int i = 0; i < ds.Tables["ubicazioni"].Rows.Count; i++)
					{
						s = ds.Tables["ubicazioni"].Rows[i]["comune"] == DBNull.Value ? "" : ds.Tables["ubicazioni"].Rows[i]["comune"].ToString();
						s += ds.Tables["ubicazioni"].Rows[i]["via"] == DBNull.Value ? "" : ", " + ds.Tables["ubicazioni"].Rows[i]["via"].ToString();
						s += ds.Tables["ubicazioni"].Rows[i]["civico"] == DBNull.Value ? "" : " " + ds.Tables["ubicazioni"].Rows[i]["civico"].ToString();
						s += ds.Tables["ubicazioni"].Rows[i]["ubicazione"] == DBNull.Value ? "" : ", " + ds.Tables["ubicazioni"].Rows[i]["ubicazione"].ToString();
						ss = ds.Tables["ubicazioni"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["ubicazioni"].Rows[i]["id"].ToString().Trim();
						ddlRitiro.Items.Insert(i + 1, new ListItem(s, ss));
					}
				}
				else
					sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella UBICAZIONI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
			}
		}
	}

	protected void ddlOraInizio_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (CldInizio.SelectedDate == DateTime.Now.Date && ddlOrainizio.SelectedIndex < DateTime.Now.Hour && Session["idp"] == null)
			ddlOrainizio.SelectedIndex = DateTime.Now.Hour;

		if (CldInizio.SelectedDate == CldFine.SelectedDate && ddlOrafine.SelectedIndex < (ddlOrainizio.SelectedIndex + 1) && ddlOrainizio.SelectedIndex + 1 < 23)
			ddlOrafine.SelectedIndex = ddlOrainizio.SelectedIndex + 1;
		CldInizio.VisibleDate = CldInizio.SelectedDate;
		CldFine.VisibleDate = CldFine.SelectedDate;
	}

	protected void ddlMinInizio_SelectedIndexChanged(object sender, EventArgs e)
	{
		// fare controllo se data e ora uguale a ora..... minuti > di ora
	}

	protected void ddlOraFine_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (CldInizio.SelectedDate == CldFine.SelectedDate && ddlOrafine.SelectedIndex < ddlOrainizio.SelectedIndex)
			sStato.Text = "Controllare ora inizio e e ora fine missione!";
	}
	protected void ddlMinFine_SelectedIndexChanged(object sender, EventArgs e)
	{
		// se data fine e ora uguale a ora + 30min... minuti > ora.minuti insizio + 30
	}

	protected void ddlRitiro_SelectedIndexChanged(object sender, EventArgs e)
	{
		idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
		initChartLibere(idu.ToString());
	}

	protected void ddlStato_SelectedIndexChanged(object sender, EventArgs e)
	{
		bool abilitato;
		abilitato = ddlStato.SelectedItem.Text == "Estero" ? false : true;

		ddlComune.Enabled = abilitato;
		ddlProvincia.Enabled = abilitato;

		tComuneEstero.Enabled = !abilitato;
		tComuneEstero.Visible = !abilitato;
		lComuneEstero.Enabled = !abilitato;
		lComuneEstero.Visible = !abilitato;
	}

	protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
	{
		// devo leggere la tabella Comuni
		msg = "";
		FBConn.openaFBConn(out msg);
		if (msg.Length >= 1)
			sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
		else
		{
			msg = "";
			ds.Clear();
			msg = ddlProvincia.SelectedValue.ToString();
			s = "SELECT a.* from comuni a where abilitato = 1 and provincia_ek = " + msg + " order by comune";
			ds = FBConn.getfromDSet(s, "comuni", out msg);
			if (msg.Length >= 1)
				sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
			else
			{
				if (ds.Tables["comuni"].Rows.Count > 0)
				{
					ddlComune.Items.Clear();
					string s = "", ss = "";
					ddlComune.Items.Insert(0, new ListItem("", ""));
					for (int i = 0; i < ds.Tables["comuni"].Rows.Count; i++)
					{
						s = ds.Tables["comuni"].Rows[i]["comune"] == DBNull.Value ? "" : ds.Tables["comuni"].Rows[i]["comune"].ToString().Trim();
						ss = ds.Tables["comuni"].Rows[i]["comune_k"] == DBNull.Value ? "" : ds.Tables["comuni"].Rows[i]["comune_k"].ToString().Trim();
						ddlComune.Items.Insert(i + 1, new ListItem(s, ss));
					}
					ddlComune.SelectedIndex = 0;
				}
				else
					sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella UBICAZIONI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
			}
		}
		ddlComune.Focus();
	}

	protected void bVerifica_Click(object sender, EventArgs e)
	{
		bElencoDisponibili.Visible = false;
		
		// se arrivo dalla conferma... o dalla cancellazione.... resetto
		if (bVerifica.Text != "Verifica richiesta")
		{
			bVerifica.Text = "Verifica richiesta";
			GWDD.Visible = false;
			GWPRE.Visible = false;
			PImput.Visible = true; //initChartLibere();
			sStato.Text = "";
            pConferma.Visible = false;
			return;
		}

		checkSession();  // ripristino idu, aid e carico dati user

		// devo effetuare le verifiche sui dati di input
		int ko = 0;
		s = "";
		if (ddlStato.SelectedItem.Text == "Italia" && ddlComune.SelectedValue == "") { s = "Comune di destinazione; "; ko++; }
		if (ddlStato.SelectedItem.Text == "Estero" && tComuneEstero.Text == "") { s = "Comune estero di destinazione; "; ko++; }
		if (CldInizio.SelectedDate.ToString() == "01/01/0001 00:00:00") { s += "Data inizio; "; ko++; }
		if (CldFine.SelectedDate.ToString() == "01/01/0001 00:00:00") { s += "Data fine; "; ko++; }
		if (ddlRitiro.SelectedValue == "") { s += "Punto di ritiro; "; ko++; }
		if (ko > 0) { sStato.Text = "ATTENZIONE: completare la richiesta inserendo : " + s; return; }
		string destinazione, prelievo;
		destinazione = ddlComune.SelectedItem.Text;
		prelievo = ddlRitiro.SelectedItem.Text;
		int.TryParse(Session["strapotere"].ToString(), out strapotere);
		if (strapotere < 50 && destinazione.Trim() == "Trento" && prelievo.IndexOf("Trento") == 0)
		{ sStato.Text = "ATTENZIONE: si prega di utilizzare i mezzi pubblici o di contattare il settore car sharing al numero " + Session["assistenza"].ToString(); return; }
		PImput.Visible = false;
		dtinizio = new DateTime(CldInizio.SelectedDate.Year, CldInizio.SelectedDate.Month, CldInizio.SelectedDate.Day, ddlOrainizio.SelectedIndex, ddlMininizio.SelectedIndex * 5, 0);
		dtfine = new DateTime(CldFine.SelectedDate.Year, CldFine.SelectedDate.Month, CldFine.SelectedDate.Day, ddlOrafine.SelectedIndex, ddlMinfine.SelectedIndex * 5, 0);
		lRientro.ForeColor = System.Drawing.Color.Black;
		s = ddlMininizio.SelectedValue.Length > 1 ? ddlMininizio.SelectedValue.ToString() : "0" + ddlMininizio.SelectedValue.ToString();
		int k, j;
		string alarr = "", alpar = "";
		if (tDIS.Value.Length > 0)
		{
			k = tDIS.Value.IndexOf("p");
			pre.dislivello = tDIS.Value.Substring(0, k - 1).Trim();
			j = tDIS.Value.IndexOf("a");
			alarr = tDIS.Value.Substring(j + 1).Trim();
			alpar = tDIS.Value.Substring(k + 1, j - k - 1).Trim();
		}
		else pre.dislivello = null;

		lDestinazione.Text = (ddlStato.SelectedValue == "1" ? tComuneEstero.Text + " - estero" : ddlComune.SelectedItem.Text) + " - " + ddlProvincia.SelectedItem.Text + (tDIS.Value != null ? "  (dislivello minimo: " + pre.dislivello + "m.)" + " altitudini: " + alpar + "-" + alarr : "");
		lPartenza.Text = string.Format("{0} alle ore {1:00}:{2:00}", CldInizio.SelectedDate.ToString(formatodata), ddlOrainizio.SelectedValue.ToString(), s);
		s = ddlMinfine.SelectedValue.Length > 1 ? ddlMinfine.SelectedValue.ToString() : "0" + ddlMinfine.SelectedValue.ToString();
		lRientro.Text = string.Format("{0} alle ore {1}:{2}", CldFine.SelectedDate.ToString(formatodata), ddlOrafine.SelectedValue.ToString(), s);
		lRitiro.Text = string.Format("{0}", ddlRitiro.SelectedItem.Text);
		lPasseggeri.Text = string.Format("{0}", ddlPasseggeri.SelectedIndex);
		lVeicolo.Text = "";// non ancora determinato... string.Format("{0} - {1}  targa {2}{3}", pre.marca, pre.modello, pre.targa, pre.blackbox == "1" ? ", black box a bordo" : "");
		tRiepilogo.Visible = true;
		if (dtinizio.AddMinutes(59) > dtfine)
		{
			sStato.Text = "ATTENZIONE: data e ora partenza o data e ora di arrivo errate! Durata della prenotazione minima è pari a 60 minuti.";
			lPartenza.ForeColor = System.Drawing.Color.Red;
			lRientro.ForeColor = System.Drawing.Color.Red;
			bModifica.Visible = true;
			bVerifica.Visible = false;
			//tRiepilogo.Visible = false;
			return;
		}
		// contrllo dei tre giorni (non serve interrogazione)
		if (utenti.potere < 50 && dtinizio.AddDays(3) < dtfine) // vedere se conviene per ore o giornate
		{
			sStato.Text = "ATTENZIONE: data e ora partenza o data e ora di arrivo errate! Durata massima della prenotazione 3 giorni.";
			lPartenza.ForeColor = System.Drawing.Color.Red;
			lRientro.ForeColor = System.Drawing.Color.Red;
			bModifica.Visible = true;
			bVerifica.Visible = false;
			//tRiepilogo.Visible = false;
			return;
		}

		sStato.Text = "";
		bVerifica.Focus();
		pAcconsento.Visible = true;
		msg = "";

		/*  0) carico tutti i dati utili nella classe prenota
         *  1) se non è un'amministratore vedo se ci sono macchine prenotate dal tipo nello stesso periodo
         *  2) vedo se ci sono altre missioni in quel comune
         *  3) trovo l'elenco delle vetture disponibili con criterio (non già prenotate nelle date scelte)
         *  4) propongo elenco e faccio scegliere il veicolo
         *  5) conferma prenotazione
         *  6) stampa tagliandino
         */

		// 0) passo tutti i paramentri alla classe e poi richiamo il metodo inserisci
		if (aid == "") // prenotazione per conto di altro utente: NO
			aid = idu.ToString();
		pre.user_ek = aid;
		if (pre.user_ek == "") // controllo se c'è la prenotazione.... Teoricamente devo ancora farla la prenotazione.....
		{
			s = "Sessione scaduta. Prego ricollegarsi.";
			ShowPopUpMsg(s);
			Response.Redirect("default.aspx?session=0");
		}
		// 1) se non è un'amministratore vedo se ci sono macchine prenotate dal tipo nello stesso periodo
		bVerifica.Visible = false;
		bModifica.Visible = true;
		bool filtri = true;
		cbFiltri.Visible = false;
		msg = "";

		// devo vedere se sto prenotando per un'altro... nel caso controllo le sovrapposizioni per l'altro
		ds.Clear();
		ds = pre.Sovrapposte(aid, dtinizio, dtfine, out msg);
		if (msg != "")
		{
			sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
			return;
		}
		int contiene = 0;
		DataRow[] fr = null;
		if (ds != null && idp != "")
		{
			fr = ds.Tables["sovrapposte"].Select("id='" + idp + "'");
			DataRowCollection drc = ds.Tables["sovrapposte"].Rows;
			contiene = fr.Length > 0 ? 1 : 0;
		}
        
        // ci sono altre prenotazioni che si sovrappongono con quelle già effettute da quell'utente
        if (ds != null && ds.Tables["sovrapposte"].Rows.Count > contiene) 
		{
			// nella tabella visualizzo l'elenco delle prenotazioni sovrapposte
			if (utenti.potere < 50)
			{
				sStato.Text = "Impossibile assegnare ad un utente più macchine nello stesso periodo. Controllare le prenotazioni in \'Mie prenotazioni\' o contattare l'assistenza al numero: " + (string)Session["assistenza"].ToString();
				pAcconsento.Visible = false;
				GWDD.Visible = false;
				RiempiMissioni(ds.Tables["sovrapposte"], -1);
				return;
			}
			else
				sStato.Text = "ATTENZIONE: PIù MACCHINE PRENOTATE NELLO STESSO PERIODO!";
		}

		if (utenti.potere >= 50)
		{
			cbFiltri.Visible = true;
			filtri = (utenti.potere >= 50 && cbFiltri.Text == "Togli filtri ?") ? true : false;
			//filtri = false; // solo temporaneamente.... sino che abbiamo 17 macchine!
		}
		// carico le proprietà della classe pre
		idp = Session["idp"] != null ? Session["idp"].ToString() : "";
		if (idp != null && idp != "") // carico i dati della prenotazione per poi modificarli con quelli inseriti dall'utente
		{
			tbl = pre.cercaid(idp, out msg); pre.id = idp;
			if (msg.Trim().Length == 0 && tbl != null && tbl.Rows.Count > 0)
			{
				pre.refresh(tbl);
				sStato.Text = "ATTENZIONE: CANCELLARE IL VECCHIO FOGLIO DI PRENOTAZIONE! LA PRENOTAZIONE E' STATA CAMBIATA!";
			}
		}
        // se è una modifica ho già caricato tutti i dati della prenotazione e ora modifico solo i dati che provengono dalla maschera
		pre.partenza = dtinizio;
		pre.arrivo = dtfine;
		pre.dove_stato_ek = ddlStato.SelectedIndex.ToString();
		pre.dove_prov_ek = ddlProvincia.SelectedValue;
		pre.dove_comune_ek = ddlComune.SelectedValue.ToString() == "" ? "-1" : ddlComune.SelectedValue.ToString();
		pre.dove_comune = (ddlStato.SelectedItem.Text == "Estero" && tComuneEstero.Text != "") ? tComuneEstero.Text : ddlComune.SelectedItem.ToString() != "" ? ddlComune.SelectedItem.ToString() : "";
		pre.passeggeri = ddlPasseggeri.SelectedValue;
		pre.ubicazione_ek = ddlRitiro.SelectedValue.ToString();
		pre.aggregati = "0"; // alla prenotazione non ci sono aggregati in car pooling
		pre.motivo = tMotivo.Text.Trim();
		pre.maker_ek = idu.ToString(); // nel caso di modifica maker_ek va aggiornato

		// vedo se ci sono altre missioni in quel comune
		ds.Clear();
		GW.DataBind();
		GW.Visible = false;

		// 3) trovo l'elenco delle vetture disponibili con criterio (non già prenotate nelle date scelte)
		if (utenti.potere < 50)
		{
            // qui posso controllare il totale prenotazioni attive, dato che non ho ancora scelto la macchina
			tbl = pre.PrenotazioniInDateDifferenti(utenti.iduser.ToString(), out msg);
			if (msg.Trim() != "")
			{
				sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
				return;
			}
			if (tbl.Rows.Count > 0 && idp.Trim() == "") // non è una modifica)
			{
                int mp = 0;
                int.TryParse(tbl.Rows[0]["max_prenotazioni"].ToString(), out mp);
                if (tbl.Rows.Count >= mp)
                {
                    pConferma.Visible = false;
                    pAcconsento.Visible = false;
                    tRiepilogo.Visible = false;
                    sStato.Text = "ATTENZIONE: a causa del limitato numero di automezzi disponibili, ogni utente non può avere più di " + mp.ToString() + " prenotazioni attive! Contattare l'assistenza al numero " + (string)Session["assistenza"];
                    return;
                }
            }
		}
		// visualizzo pulsante verifica disponibilità in altre sedi
		bElencoDisponibili.Visible = true;
        ds.Clear();
		GWDD.DataBind();
		GWDD.Visible = false;
		pAcconsento.Visible = false;
		cbAcconsento.Visible = false;
		bConferma.Visible = false;
		msg = "";
		//bElencoDisponibili.Visible = false;

		if (idp.Trim() == "") // non è una modifica
		{
			// controllo se ci sono mezzi disponibili per quegli orari e per quella sede
            
			ds = pre.mezzidisponibili(utenti.iduser.ToString(), pre.ubicazione_ek.Trim(), pre.dove_comune, pre.dove_prov_ek, pre.partenza, pre.arrivo, pre.dislivello, filtri, out msg);
			if (msg != "")
			{
				sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
				return;
			}
			if (ds != null && ds.Tables["disponibili"].Rows.Count > 0)   // elenco dei mezzi disponibili
			{
				//cbFiltri.Checked = false;
				//cbFiltri.Text = (cbFiltri.Text == "Applica filtri ?") ? "Togli filtri ?" : "Applica filtri ?";
				// ordinato per ordine e per comune
				RiempiGrid(ds.Tables["disponibili"]); // vetture disponibili
			}
			else
			{
				sStato.Text = "Non ci sono mezzi disponibili con le caratteristiche richieste! Modificare le caratteristiche o contattare il call center al n. 0461.496415.";
				PImput.Visible = true; //initChartLibere();
				return;
			}
		}
		else // è una modifica... quindi devo vedere se la mia prenotazione originale ha o no sovrapposizione
		{
			msg = "";
			tbl = pre.VerificaModificaPrenotazione(pre.mezzo_ek, pre.id, pre.partenza, pre.arrivo, out msg);
			if (tbl != null && tbl.Rows.Count <= 0)
			{
				lVeicolo.Text = string.Format("{0} - {1} numero {2}", pre.marca, pre.modello, pre.numero);
				trVeicolo.Visible = true;
				CarPooling(contiene);
				sStato.Text = "ATTENZIONE: CANCELLARE IL FOGLIO DI PRENOTAZIONE PRECEDENTE. STA PER ESSERE CAMBIATA LA PRENOTAZIONE! CONTROLLARE IN 'LE MIE PRENOTAZIONI' DOPO LA MODIFICA.";
			}
			else
			{
				sStato.Text = "Non ci sono mezzi disponibili con le caratteristiche richieste! Modificare le caratteristiche o contattare il call center al n. 0461.496415.";
				PImput.Visible = true; // initChartLibere();
				return;
			}
		}
	}

	public void RiempiGrid(DataTable gwds) // veicoli disponibili
	{
		try
		{
            GW.Columns[11].Visible = true;
            GW.Columns[12].Visible = true;
            int gvHasRows = GW.Rows.Count;
			if (gvHasRows > 0)
			{
				GW.Columns.Clear();
				GW.DataBind();
			}
			GW.DataSource = gwds;
			GW.DataBind();
            GW.Columns[11].ShowHeader = false; // nascondo la colonna colore... solo dopo aver associato la tabella ai dati 
            GW.Columns[11].ItemStyle.Width = 0;
            GW.Columns[11].ItemStyle.Wrap = false;
            GW.Columns[11].Visible = false;
            GW.Columns[12].ShowHeader = false; // nascondo la colonna flotta_ek... dopo aver associato la tabella ai dati 
            GW.Columns[12].ItemStyle.Width = 0;
            GW.Columns[12].ItemStyle.Wrap = false;
            GW.Columns[12].Visible = false;
            int ok = 0;
            int argb = 0, argbold = -1;
            foreach (GridViewRow r in GW.Rows)
            {
                string col = r.Cells[11].Text.ToString();
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
            GW.Visible = true;
            if ( ok > 1 ) sStato.Text = "ATTENZIONE: i mezzi appartenenti alle stesse flotte, hanno colori uguali. Si prega di dare la preferenza ai veicoli appartenenti alla propria flotta, presenti nella parte alta dell'elenco!";
        }
		catch (Exception ex)
		{
			sStato.Text = "Riscontrato errore durante la ricerca dei mezzi disponibili. Errore: " + ex.ToString() + " Avvertire l'amministratore al n. " + (string)Session["assistenza"].ToString();
		}
	}

	public void RiempiDisponibili(DataTable gwds) // veicoli disponibili
	{
		try
		{
			int gvHasRows = GWMezziDisponibili.Rows.Count;
			if (gvHasRows > 0)
			{
				GWMezziDisponibili.Columns.Clear();
				GWMezziDisponibili.DataBind();
			}
			GWMezziDisponibili.DataSource = gwds;
			GWMezziDisponibili.DataBind();
            GWMezziDisponibili.Columns[7].ShowHeader = false; // nascondo la colonna flotta_ek... dopo aver associato la tabella ai dati 
            GWMezziDisponibili.Columns[7].ItemStyle.Width = 0;
            GWMezziDisponibili.Columns[7].ItemStyle.Wrap = false;
            GWMezziDisponibili.Columns[7].Visible = false;
            int ok = 0;
            int argb = 0, argbold = -1;
            foreach (GridViewRow r in GWMezziDisponibili.Rows)
            {
                string col = r.Cells[7].Text.ToString();
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
            GWMezziDisponibili.Visible = true;
		}
		catch (Exception ex)
		{
			sStato.Text = "Riscontrato errore durante la ricerca dei mezzi disponibili in ogni sede. Errore: " + ex.ToString() + " Avvertire l'amministratore al n. " + (string)Session["assistenza"].ToString();
		}
	}

	public void RiempiMissioni(DataTable gwds, int comando) // missioni
	{
		try
		{
			if (GWDD.Rows.Count > 0)
			{
				GWDD.Columns.Clear();
				GWDD.DataBind();
			}
			GWDD.DataSource = gwds;
			gwds.DefaultView.Sort = "Arrivo DESC";
			GWDD.DataBind();
			if (GWDD.Columns.Count > 0) GWDD.Columns[0].Visible = comando == 0 ? true : false; // mon serve +
			if (GWDD.Columns.Count > 1) GWDD.Columns[1].Visible = comando == 1 ? true : false;
			//GWDD.Sort(GWDD.Columns[2].HeaderText, SortDirection.Descending);
			GWDD.Visible = true;
		}
		catch (Exception ex)
		{
			sStato.Text = "Riscontrato errore durante la ricerca dei mezzi disponibili/missioni. Errore: " + ex.ToString() + " Avvertire l'amministratore al n. " + (string)Session["assistenza"].ToString();
		}
	}
	public void GestioneMissioni(DataTable gwpre, int comando) // missioni
	{
		try
		{
			if (GWPRE.Rows.Count > 0)
			{
				GWPRE.DataBind();
			}
            GWPRE.Columns[9].Visible = true;
            GWPRE.DataSource = gwpre;
			gwpre.DefaultView.Sort = "partenza DESC";
			GWPRE.DataBind();
			if (GWPRE.Columns.Count > 0) GWPRE.Columns[0].Visible = comando >= 0 ? true : false; // mon serve +
			if (GWPRE.Columns.Count > 1) GWPRE.Columns[1].Visible = comando >= 1 ? true : false;
            GWPRE.Columns[8].ShowHeader = false; // nascondo la colonna id
            GWPRE.Columns[8].ItemStyle.Width = 0;
            GWPRE.Columns[8].ItemStyle.Wrap = false;
            GWPRE.Columns[8].Visible = false;

            GWPRE.Columns[9].Visible = false;

            int ok = 0;
            int argb = 0, argbold = -1;
            foreach (GridViewRow r in GWPRE.Rows) // faccio il giro e coloro
            {
                string col = r.Cells[9].Text.ToString();
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
            GWPRE.Visible = true;
		}
		catch (Exception ex)
		{
			sStato.Text = "Riscontrato errore durante la ricerca dei mezzi disponibili/missioni. Errore: " + ex.ToString() + " Avvertire l'amministratore al n. " + (string)Session["assistenza"].ToString();
		}
	}
	protected void GW_SelectedIndexChanged(object sender, EventArgs e) // selezione veicoli disponibili
	{
		int riga = GW.SelectedIndex;
		GridViewRow row = GW.SelectedRow;
		pre.mezzo_ek = GW.SelectedDataKey.Value.ToString();
        // ora devo controllare che se il mezzo selezionato appartiene ad una flotta che ha troppe prenotazioni per l'utente....
        s = "";
        // qui posso controllare il totale prenotazioni attive, dato che non ho ancora scelto la macchina
        if (tbl != null && tbl.Rows.Count > 0) tbl.Clear();
        idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
        // devo vedere se sto prenotando per altri...
        aid = Session["aid"] != null ? Session["aid"].ToString().Trim() : "";
        string codiceutente = aid;
        if (aid == "") codiceutente = idu.ToString();
        string codflotta = row.Cells[12].Text;
        Session.Add("flotta_ek", codflotta);
        tbl = pre.PrenotazioniPerMezzoDiUnGruppo(codiceutente, pre.mezzo_ek, codflotta, out msg);
        if (msg.Trim() != "")
        {
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            return;
        }
        if (tbl != null && tbl.Rows.Count > 0 && idp.Trim() == "") // non è una modifica)
        {
            int mp = 0;
            int.TryParse(tbl.Rows[0]["max_prenotazioni"].ToString(), out mp); // una riga qualsiasi... in quanto per mi serve il numero max di prenotazioni della flotta
            if (tbl.Rows.Count >= mp && (aid == ""))
            {
                pConferma.Visible = false;
                pAcconsento.Visible = false;
                tRiepilogo.Visible = false;
                sStato.Text = "ATTENZIONE: non si possono avere più di " + mp + " prenotazioni attive per la flotta " + tbl.Rows[0]["etichetta"].ToString() + "! Contattare l'assistenza al numero " + (string)Session["assistenza"];
                return;
            }
            //Session.Add("flotta_ek", tbl.Rows[0]["id"].ToString()); // aggiungo id flotta
            pre.flotta_ek = codflotta;
        }
        Session.Add("idmezzo", pre.mezzo_ek.ToString());
		Session.Add("mezzo_numero", row.Cells[1].Text);
		pre.numero = row.Cells[1].Text;
		pre.marca = row.Cells[2].Text;
		pre.modello = row.Cells[3].Text;
		pre.posteggio = Server.HtmlDecode(row.Cells[10].Text.Trim().Replace("&nbsp;", ""));
		lPosteggio.Text = pre.posteggio; // la riga sopra non mantiene i dati in pre.... allora metto in label e poi ricopio
		lVeicolo.Text = string.Format("{0} - {1} numero {2}", pre.marca, pre.modello, pre.numero);
		trVeicolo.Visible = true;
		CarPooling(0);
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

	public void CarPooling(int contiene)
	{
		// ora devo vedere se ci sono altri che vanno nella stessa destinazione
		ds.Clear();
		GWDD.DataBind();
		GWDD.Visible = false;
		pre.dove_comune = ddlComune.SelectedItem.ToString();
		pre.partenza = new DateTime(CldInizio.SelectedDate.Year, CldInizio.SelectedDate.Month, CldInizio.SelectedDate.Day, ddlOrainizio.SelectedIndex, ddlMininizio.SelectedIndex * 5, 0);

		ds = pre.CercaDD((pre.dove_comune), pre.partenza, out msg);
		if (msg != "")
		{
			sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
			return;
		}
		cbFiltri.Visible = false;
		GW.Visible = false;
		pAcconsento.Visible = true;
		cbAcconsento.Visible = true;
		bConferma.Visible = true;
		if (ds != null && ds.Tables["prenotazioniDD"].Rows.Count > contiene)  // ci sono già prenotazioni per quel comune con quella data 
		{
			RiempiMissioni(ds.Tables["prenotazioniDD"], 0);
			sStato.Text = "ATTENZIONE: ci sono altri colleghi che andranno ad " + pre.dove_comune + " il giorno " + pre.partenza.ToString("dd-MM-yyyy") + ". Aggregati selezionando la missione oppure procedi con una tua, cliccando su conferma!";
			return;
		}
		else
		{
			sStato.Text = "Confermare la richiesta di prenotazione per " + ((ddlStato.SelectedItem.Text == "Estero" && tComuneEstero.Text != "") ? tComuneEstero.Text : ddlComune.SelectedItem.ToString() != "" ? ddlComune.SelectedItem.ToString() : "") + "!";
			GWDD.Visible = false;
		}
	}

	protected void GWDD_SelectedIndexChanged(object sender, EventArgs e) // Missioni.... aggregazione in car pooling
	{
		int riga = GWDD.SelectedIndex;
		GridViewRow row = GWDD.SelectedRow;
		trVeicolo.Visible = true;
		// devo aggiungere i passeggeri alla prenotazione scelta
		idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
		//Int32.TryParse(Session["iduser"].ToString(), out idu);
		if (pre.Aggrega(row.Cells[9].Text, idu) != 1)
			sStato.Text = "Non è stato possibile esegure l'aggragazione! Errore durante l\'update! Contattare il servizio assistenza al n. " + (string)Session["assistenza"].ToString();
		else
		{
			sStato.Text = "Per attivare il car pooling contatta il driver " + row.Cells[8].Text + ", " + row.Cells[7].Text + " al numero " + row.Cells[5].Text;
			// ora devo esplicitare i dati della prenotazione, eventualmente notificando entrambe gli interessati
			GWDD.Visible = false;
			//s = "select * from prenotazioni where user_ek=\'" + pre.user_ek + "\' and cast(partenza as timestamp)=cast(\'" + pre.partenza.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp)";
			s = "Contatta il driver " + row.Cells[8].Text + " " + row.Cells[7].Text + " al numero " + row.Cells[5].Text + " per verificare la possibilità di car pooliing.";
			ShowPopUpMsg(s);
			bConferma.Enabled = false;
			bConferma.Visible = false;
			bModifica.Visible = false;
			pAcconsento.Visible = false;
			cbAcconsento.Visible = false;
			idp = "";
			//Response.Redirect("Confermaprenotazionedn.aspx?c=" + row.Cells[9].Text); // passo i dati della prenotazione alla pagina Conferma.aspx
		}
	}

	protected void GWPRE_SelectedIndexChanged(object sender, EventArgs e) // Missioni.... modifica
	{
		int riga = GWPRE.SelectedIndex;
		GridViewRow row = GWPRE.SelectedRow;
		DateTime dt;
		dt = row.Cells[3] != null ? DateTime.Parse(row.Cells[3].Text) : DateTime.Now.Date.AddDays(-1);
		if (dt < DateTime.Now)
		{
			Session.Add("idp", ""); // meglio azzerare
			sStato.Text = "ATTENZIONE: non è possibile modificare una prenotazione con data di fine, già trascorsa!";
			return;
		}
        if (GWPRE.SelectedDataKey.Value != null)
            Session.Add("idp", GWPRE.SelectedDataKey.Value); // memorizzo come variabile di sessione
		GWPRE.Visible = false;
		pConferma.Visible = false;
		PImput.Visible = true; //initChartLibere();
		Response.Redirect("pre.aspx?idp=" + Session["idp"].ToString());
	}
	protected void GWPRE_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
        lccDestinazione.Text = GWPRE.Rows[e.RowIndex].Cells[5].Text;
		lccPartenza.Text = GWPRE.Rows[e.RowIndex].Cells[2].Text;
		//pre.id = GWPRE.SelectedDataKey[0].ToString(); // preservo id prenotazione da cancellare
		string s = "";
		if (GWPRE.Rows[e.RowIndex].Cells[2] != null)
			s = GWPRE.Rows[e.RowIndex].Cells[2].Text;
        pre.id = GWPRE.DataKeys[e.RowIndex].Value.ToString();

        lccArrivo.Text = GWPRE.Rows[e.RowIndex].Cells[3].Text;
        lccUbi.Text = GWPRE.Rows[e.RowIndex].Cells[6].Text;
        //if (GWPRE.SelectedDataKey.Value != null)

        Session.Add("idprenotazione", pre.id);
		//pre.id = GW.SelectedDataKey.Value.ToString();

		GWPRE.Visible = false; // elenco prenotazioni e visualizza pannello di conferma
		pConferma.Visible = true;
		sStato.Text = "ATTENZIONE: dopo la conferma della cancellazione, assicurarsi di eliminare il foglio di prenotazione in qualunque formato posseduto! (mail, cartaceo, file)";
	}

	protected void GWPRE_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.Cells[2] != null)
		{
			DateTime dt;
			DateTime.TryParse(e.Row.Cells[2].Text, out dt);
			if (dt < DateTime.Now)
				e.Row.Cells[1].Enabled = false;
		}
		if (e.Row.Cells[3] != null)
		{
			DateTime dt;
			DateTime.TryParse(e.Row.Cells[3].Text, out dt);
			if (dt < DateTime.Now)
				e.Row.Cells[0].Enabled = false;
		}
	}
	protected void cbcCancella_Click(object sender, EventArgs e)
	{
		string msg = "";
		//if (pre.mezzo_ek == null && GW.SelectedDataKey.Value != null) pre.mezzo_ek = GW.SelectedDataKey.Value.ToString();
		if (pre.id == null && Session["idprenotazione"] != null)
            pre.id = Session["idprenotazione"] != null ? Session["idprenotazione"].ToString() : "";

		Int32 idu = Session["iduser"] != null ? Convert.ToInt32(Session["iduser"].ToString()) : -1;
		int rr = pre.Cancella(pre.id, idu.ToString(), out msg);
		if (rr < 0)
		{
			sStato.Text = "Cancellazione non riusicta. Contattare il servizio assistenza al n. " + Session["assistenza"].ToString();
			pConferma.Visible = false;
			return;
		};
		if (rr == 0)
		{
			sStato.Text = "Non è possibile cancellare le richieste con data e ora di inizio già passate!";
			pConferma.Visible = false;
			return;
		}
		GWPRE.Visible = false;
		sStato.Text = "Cancellazione avvenuta! ATTENZIONE: provvedere a eliminare il foglio di prenotazione in qualunque formato posseduto! (mail, cartaceo, file)";
		//sStato.Text = "Cancellazione avvenuta!";
		pConferma.Visible = false;
		bmieprenotazioni_Click(this, e = new EventArgs());
	}

	protected void bUscita_Click(object sender, EventArgs e)
	{
		Session.Clear();
		Session.Abandon();
		Response.Redirect("Default.aspx");
	}

	protected void bhome_Click(object sender, EventArgs e)
	{
		Session.Add("idp", "");
		Response.Redirect("Menu.aspx");
	}

	protected void ShowPopUpMsg(string msg)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("alert('");
		sb.Append(msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'"));
		sb.Append("');");
		ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
	}

	protected void bConferma_Click(object sender, EventArgs e)
	{
        if (!checkSession()) Response.Redirect("default.aspx"); // ripristino idu e aid e utenti e verifico sessione ancora attiva
        if (!cbAcconsento.Checked)
		{
			sStato.Text = "E' necessario leggere il disciplinare d'uso dei veicoli....Prima di procedere.";
			ShowPopUpMsg(sStato.Text);
			return;
		}

		bConferma.Enabled = false; // non si sa mai...

		string cosastofacendo = "";
		// Può essere un inserimento nuova prenotazione; una prenotazione per conto di un'altro; una modifica
		// se aid <> "" -> prenotazione x conto di altro
		// se idp <> "" -> prenotazione da modificare

		if (aid == "")
			aid = idu.ToString(); // se non prenoto x conto terzi, prenoto per idu.... 
		else
			cosastofacendo = "Prenotazione x altri"; // credo che se sto facendo una prenotazione per altri non è una modifica

		dtinizio = new DateTime(CldInizio.SelectedDate.Year, CldInizio.SelectedDate.Month, CldInizio.SelectedDate.Day, ddlOrainizio.SelectedIndex, ddlMininizio.SelectedIndex * 5, 0);
		dtfine = new DateTime(CldFine.SelectedDate.Year, CldFine.SelectedDate.Month, CldFine.SelectedDate.Day, ddlOrafine.SelectedIndex, ddlMinfine.SelectedIndex * 5, 0);
		if (idp != null && idp.Trim() != "") // modifica prenotazione quindi carico le pre con i dati della vecchia prenotazione
		{
			cosastofacendo = "Modifica";
			tbl = pre.cercaid(idp, out msg);
			if (msg.Trim() == "")
			{
				pre.refresh(tbl); // carico i dati della vecchia...
			}
		}
		// e ora riempio le proprietà con  i nuovi dati o i dati modificati....
		pre.partenza = dtinizio; // vediamo se tiene..... altrimenti session....
		pre.arrivo = dtfine;
		pre.dove_stato_ek = ddlStato.SelectedIndex.ToString();
		pre.dove_prov_ek = ddlProvincia.SelectedValue;
		pre.dove_comune_ek = ddlComune.SelectedValue.ToString() == "" ? "-1" : ddlComune.SelectedValue.ToString();
		pre.dove_comune = (ddlStato.SelectedItem.Text == "Estero" && tComuneEstero.Text != "") ? tComuneEstero.Text : ddlComune.SelectedItem.ToString() != "" ? ddlComune.SelectedItem.ToString() : "";
		pre.passeggeri = ddlPasseggeri.SelectedValue;
		pre.ubicazione_ek = ddlRitiro.SelectedValue.ToString();
		pre.aggregati = "0"; // alla prenotazione non ci sono aggregati in car pooling
		pre.motivo = tMotivo.Text.Trim();
		pre.maker_ek = idu.ToString();
		pre.posteggio = lPosteggio.Text.Trim();
		if (pre.posteggio == null) pre.posteggio = "";
        if (cosastofacendo != "Modifica") // non è una modifica, quindi non conosco il numero del mezzo
        {
            if (pre.mezzo_ek == null && GW.SelectedDataKey.Value != null)
            {
                pre.mezzo_ek = Session["idmezzo"] != null ? Session["idmezzo"].ToString() : "";
            }
        }
        pre.flotta_ek = Session["flotta_ek"] != null ? Session["flotta_ek"].ToString() : "";
       
        if (cosastofacendo == "Modifica") // modifica prenotazione
		{
            bModifica.Enabled = true;
			msg = "";
			FBConn.openaFBConn(out msg);
			if (msg.Length >= 1)
			{
				sStato.Text = "ATTENZIONE: connessione interrotta durante la modifica della prenotazione. Contattare il servizio assistenza al n. " + Session["assistenza"].ToString();
				return;
			}
			//idp = (Request.QueryString["idp"] != null && Request.QueryString["idp"].ToString().Trim() != "") ? Request.QueryString["idp"].ToString().Trim() : idp;
			// ATTENZIONE: ho tolto gli aggregati..
			//s = "update prenotazioni set user_ek = \'" + aid + "\', maker=\'" + idu.ToString() + "\', tempo=cast(\'now\' as timestamp), partenza=cast(\'" + + "\' as timestamp), arrivo=cast(\'" +  + "\' as timestamp), passeggeri=\'" + + "\', descrizione=\'" +  + "\' ";
			//public int Modifica(string userid, string prenotazioneid)
			if (pre.Modifica(aid, idp) != 1)
			{
				sStato.Text = "ATTENZIONE: si è verificato un\'errore durante l\'inserimento della prenotazione. Contattare l'assistenza al numero " + (string)Session["assistenza"];
				return;
			}
		}
		else
		{
			if (cosastofacendo == "Prenotazione x altri" || cosastofacendo == "")
			{
				if (pre.Inserisci(aid.ToString()) != 1)
				{
					sStato.Text = "ATTENZIONE: si è verificato un\'errore durante l\'inserimento della prenotazione. Contattare l'assistenza al numero " + (string)Session["assistenza"];
					return;
				}
				// ora devo controllare se qualcuno ha già prenotato nel frattempo che questo dormiva sulla pagina web
				// cerco prenotazioni di quella macchina negli orari che toccano quelli della mia prenotazione
				s = "SELECT * FROM PRENOTAZIONI as a where mezzo_ek=" + pre.mezzo_ek.ToString() + " and (";
				s += "(cast(a.partenza as timestamp) <= cast(\'" + pre.partenza.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp) and cast(a.arrivo as timestamp) >= cast(\'" + pre.arrivo.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp)) or ";
				s += "(cast(a.partenza as timestamp) <= cast(\'" + pre.partenza.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp) and cast(a.arrivo as timestamp) >= cast(\'" + pre.partenza.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp)) or ";
				s += "(cast(a.partenza as timestamp) <= cast(\'" + pre.arrivo.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp) and cast(a.arrivo as timestamp) >= cast(\'" + pre.arrivo.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp)) or ";
				s += "(cast(a.partenza as timestamp) >= cast(\'" + pre.partenza.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp) and cast(a.arrivo as timestamp) <= cast(\'" + pre.arrivo.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp))) ";
				s += "order by tempo desc ";
				ds = FBConn.getfromDSet(s, "andata", out msg); // controllo se c'è una prenotazione oltre alla mia....

				Int32 idu = Session["iduser"] != null ? Convert.ToInt32(Session["iduser"].ToString()) : -1;
				int rr = -1;
				msg = "";
				if (ds.Tables["andata"] != null && ds.Tables["andata"].Rows.Count > 1) // c'è già un'altra prenotazione
				{
					// cancello tutte le prenotazioni di quel mezzo con il mio codice utente per quelle ore
					for (int i = 0; i <= ds.Tables["andata"].Rows.Count; i++)
					{
						if (ds.Tables["andata"].Rows[i]["user_ek"].ToString() == idu.ToString())
						{
							if (pre.id == null && Session["idprenotazione"] != null) pre.id = Session["idprenotazione"] != null ? Session["idprenotazione"].ToString() : "";
							pre.id = ds.Tables["andata"].Rows[i]["id"].ToString();
							rr = pre.Cancella(pre.id, idu.ToString(), out msg);
							if (rr < 0)
							{
								sStato.Text = "Cancellazione non riusicta. Contattare il servizio assistenza al n. " + Session["assistenza"].ToString();
								pConferma.Visible = false;
								return;
							};
							sStato.Text = "Spiacente.... l'automezzo che hai cercato di prenotare è appena stato prenotato da un'altro utente! Rifai la ricerca.... e prenota un'altro automezzo!";
							ShowPopUpMsg(sStato.Text);
							return;
						}
					}
				}
			}
		}
		// solo in caso di nuovo inserimento.... devo ricercare l'id
		s = "select * from prenotazioni where user_ek=\'" + aid + "\' and maker=\'" + pre.maker_ek + "\' and cast(partenza as timestamp)=cast(\'" + dtinizio.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp)";
		msg = "";
		bConferma.Enabled = false; // per prevenire pressioni ripetute
		bConferma.Visible = false;
		string codice = idp;
		idp = "";
		if (cosastofacendo != "Modifica")
		{
			ds = FBConn.getfromDSet(s, "prenotazioni", out msg);
			// Solo dopo aver inserito e ricercato la prenotzione posso aggiungere la IDP ( a meno che non si tratti di una modifica )
			Session.Add("idp", ds.Tables["prenotazioni"].Rows[0]["id"].ToString());
			codice = ds.Tables["prenotazioni"].Rows[0]["id"].ToString();
		}
		Response.Redirect("ConfermaPrenotazionedn.aspx?c=" + codice); // passo i dati della prenotazione alla pagina Conferma.aspx
	}

	protected void bModifica_Click(object sender, EventArgs e)
	{
		bElencoDisponibili.Visible = false;
		GWMezziDisponibili.Visible = false;
		PImput.Visible = true;	//initChartLibere();
		bModifica.Visible = false;
		bVerifica.Visible = true;
		tRiepilogo.Visible = false;
		lPartenza.ForeColor = System.Drawing.Color.Black;
		lRientro.ForeColor = System.Drawing.Color.Black;
		GW.Visible = false;
		GWDD.Visible = false;
		bConferma.Visible = false;
		cbAcconsento.Visible = false;
		pAcconsento.Visible = false;
		sStato.Text = "";
		cbFiltri.Visible = false;
	}

	protected void cbFiltriOnOff(object sender, EventArgs e)
	{
		cbFiltri.Text = (cbFiltri.Text == "Togli filtri ?") ? "Applica filtri" : "Togli filtri ?";
		cbFiltri.Checked = false;
		bVerifica_Click(this, new EventArgs());
	}

	protected void bmieprenotazioni_Click(object sender, EventArgs e)
	{
		PImput.Visible = false;
		tRiepilogo.Visible = false;
		GW.Visible = false;
		GWMezziDisponibili.Visible = false;
		bElencoDisponibili.Visible = false;
		pre.user_ek = Session["iduser"] != null ? Session["iduser"].ToString() : "";
		if (pre.user_ek == "")
		{
			s = "Sessione scaduta. Prego ricollegarsi.";
			ShowPopUpMsg(s);
			Response.Redirect("default.aspx");
		}
		// 1) se non è un'amministratore vedo se ci sono macchine prenotate dal tipo nello stesso periodo
		bVerifica.Visible = true;
		bModifica.Visible = false;
		bVerifica.Text = "Ritorna a PRENOTAZIONE";
		msg = "";
		ds.Clear();
		DateTime dti = DateTime.Parse("01/01/1900");
		ds = pre.Sovrapposte(pre.user_ek, dti, DateTime.Now.AddDays(365), out msg); // mi ritorna le prenotazione sino a 365gg fa
		if (msg != "")
		{
			sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
			return;
		}

		if (ds != null && ds.Tables["sovrapposte"].Rows.Count > 0) // ci sono altre prenotazioni che si sovrappongono con quelle già effettute da quell'utente
		{
			// nella tabella visualizzo l'elenco delle prenotazioni sovrapposte
			pAcconsento.Visible = false;
			GestioneMissioni(ds.Tables["sovrapposte"], 1);
            sStato.Text = "Trovate " + ds.Tables["sovrapposte"].Rows.Count.ToString() + " prenotazioni. Differenti colori indicano prenotazioni di veicoli appartenenti a diverse flotte.";
            return;
		}
		else
		{
			sStato.Text = "Non ci sono prenotazioni. Eventualmente contattare l'assistenza al numero: " + (string)Session["assistenza"].ToString();
			pAcconsento.Visible = false;
			GWPRE.Visible = false;
			return;
		}
	}

	protected void CldInizio_SelectionChanged(object sender, EventArgs e)
	{
		//CldInizio.TodayDayStyle.BackColor = System.Drawing.Color.White;
		//CldInizio.TodayDayStyle.BorderColor = System.Drawing.Color.Orange;
		// se giorno scelto < di oggi: messagio e posiione su oggi
		if (DateTime.Now.Date > CldInizio.SelectedDate)
		{
			CldInizio.SelectedDate = DateTime.Now.Date;
			//CldInizio.SelectedDayStyle.BackColor = System.Drawing.Color.Blue;
			sStato.Text = "Data inizio viaggio deve essere maggiore o uguale a data di oggi!";
		}
		if (CldInizio.SelectedDate > CldFine.SelectedDate)
		{
			CldFine.SelectedDate = CldInizio.SelectedDate;
			//CldFine.SelectedDayStyle.BackColor = System.Drawing.Color.FromName("Blue");
		}
		//initChartLibere();
	}

	protected void CldFine_SelectionChanged(object sender, EventArgs e)
	{
		//CldFine.TodayDayStyle.BackColor = System.Drawing.Color.White;
		//CldFine.TodayDayStyle.BorderColor = System.Drawing.Color.Orange;
		// se giorno scelto < giorno inizio: messaggio e setto giorno fine = giorno inizio
		if (DateTime.Now.Date > CldFine.SelectedDate)
		{
			CldFine.SelectedDate = CldInizio.SelectedDate;
			//CldInizio.SelectedDayStyle.BackColor = System.Drawing.Color.FromName("Blue");
			sStato.Text = "Data fine viaggio deve essere maggiore o uguale a data di oggi!";
		}
		if (CldFine.SelectedDate < CldInizio.SelectedDate)
		{
			CldFine.SelectedDate = CldInizio.SelectedDate;
			//CldFine.SelectedDayStyle.BackColor = System.Drawing.Color.Blue;
			sStato.Text = "Data fine viaggio deve essere maggiore o uguale a data di inizio viaggio!";
		}
		CldInizio.VisibleDate = CldInizio.SelectedDate;
		CldFine.VisibleDate = CldFine.SelectedDate;
		//initChartLibere();
	}

	protected void ConfirmMsg(string msg)
	{
		/*StringBuilder sb = new StringBuilder();
        sb.Append("var sei_sicuro = confirm('" + msg + "')";

        sb.Append(msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'"));
        sb.Append("');"); */
		/*
        string sc = "var sei_sicuro = ";
        sc += "confirm('" + msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'") + "');\n";
        sc += "if (sei_sicuro === true) \n ";
        sc += "    { document.getElementById('MainContent_hfConferma').Value = 'Vero'; }\n else \n";
        sc += "    { document.getElementById('MainContent_hfConferma').Value = 'NO'; }\n"; 
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showaconfirm", sc, true);
        */
	}

	public void Leggiddl(string tab, DropDownList ddl, bool primoblank) // devo leggere la tabella Proposta
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
			s = "SELECT * from " + tab;
			tbl = FBConn.getfromTbl(s, out msg);
			FBConn.closeaFBConn(out msg);
			if (msg.Length >= 1)
				sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
			else
			{
				if (tbl.Rows.Count > 0)
				{
					ddl.Items.Clear();
					string ss = ""; s = "";
					for (int i = 0; i < tbl.Rows.Count; i++)
					{
						s = tbl.Rows[i][1] == DBNull.Value ? "" : tbl.Rows[i][1].ToString();
						ss = tbl.Rows[i][0] == DBNull.Value ? "" : tbl.Rows[i][0].ToString().Trim();
						ddl.Items.Insert(i, new ListItem(s, ss));
					}
					if (primoblank)
						ddl.Items.Insert(0, new ListItem("", ""));
				}
				else
					sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella " + tab.ToUpper() + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
			}
		}
	}

	protected void bMyData_Click(object sender, EventArgs e)
	{
		Response.Redirect("mydatas.aspx?idu=" + utenti.iduser);
	}

	protected void Nuova_Click(object sender, EventArgs e)
	{
		GWMezziDisponibili.Visible = false;
		GW.Visible = false;
		idp = "";
		Session.Add("idp", "");
		Response.Redirect("pre.aspx");
	}

	protected void GW_Sorting(object sender, GridViewSortEventArgs e)
	{
		//ds = pre.mezzidisponibili(pre.user_ek.Trim(), pre.ubicazione_ek.Trim(), pre.dove_comune, pre.dove_prov_ek, pre.partenza, pre.arrivo, pre.dislivello, filtri, out msg);
	}

	protected void CldInizio_DayRender(object sender, DayRenderEventArgs e)
	{
		int strapotere = 0;
        strapotere = Session["strapotere"] != null ? Int32.Parse(Session["strapotere"].ToString()) : -1;
        //int.TryParse(Session["strapotere"].ToString(), out strapotere);		
		if (e.Day.Date > DateTime.Now.AddDays(31) && strapotere < 50)
			e.Day.IsSelectable = false;
	}

	protected void CldFine_DayRender(object sender, DayRenderEventArgs e)
	{
		int strapotere = 0;
        strapotere = Session["strapotere"] != null ? Int32.Parse(Session["strapotere"].ToString()) : -1;
        if (e.Day.Date > DateTime.Now.AddDays(31 + 3) && strapotere < 50)
			e.Day.IsSelectable = false;
	}

	protected void bElencoDisponibili_Click(object sender, EventArgs e)
	{
		checkSession();
		GW.Visible = false; // tolgo la visualizzazione di quelle già ricercate
		ds.Clear();
		GWMezziDisponibili.DataBind();
		pAcconsento.Visible = false;
		cbAcconsento.Visible = false;
		bConferma.Visible = false;
		msg = "";
		dtinizio = new DateTime(CldInizio.SelectedDate.Year, CldInizio.SelectedDate.Month, CldInizio.SelectedDate.Day, ddlOrainizio.SelectedIndex, ddlMininizio.SelectedIndex * 5, 0);
		dtfine = new DateTime(CldFine.SelectedDate.Year, CldFine.SelectedDate.Month, CldFine.SelectedDate.Day, ddlOrafine.SelectedIndex, ddlMinfine.SelectedIndex * 5, 0);
		s = ddlMininizio.SelectedValue.Length > 1 ? ddlMininizio.SelectedValue.ToString() : "0" + ddlMininizio.SelectedValue.ToString();
		bElencoDisponibili.Visible = false;
		pre.partenza = dtinizio;
		pre.arrivo = dtfine;
		//idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
		pre.user_ek = idu.ToString();
		ds = pre.mezzidisponibili(pre.user_ek, null, "", "", pre.partenza, pre.arrivo, "", false, out msg);
		if (msg != "")
		{
			sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
			return;
		}
		if (ds != null && ds.Tables["disponibili"].Rows.Count > 0)   // elenco dei mezzi disponibili
		{
			//cbFiltri.Checked = false;
			//cbFiltri.Text = (cbFiltri.Text == "Applica filtri ?") ? "Togli filtri ?" : "Applica filtri ?";
			RiempiDisponibili(ds.Tables["disponibili"]); // vetture disponibili
			sStato.Text = "Trovati " + ds.Tables["disponibili"].Rows.Count.ToString() + " mezzi. Cliccare sul pulsante Modifica e scegliere l'eventuale altro punto di ritiro.";
		}
		else
		{
			sStato.Text = "Non ci sono mezzi disponibili con le caratteristiche richieste! Modificare le caratteristiche o contattare il call center al n. 0461.496415.";
			PImput.Visible = true; //initChartLibere();
		}
	}

	protected void initChartLibere(string utente)
	{
		cLibere.Visible = true; lnota.Visible = true;
		DateTime dada = DateTime.Now, ada = DateTime.Now;
		string sede = ddlRitiro.SelectedIndex <= 0 ? "nei punti di ritiro di Trento" : "in "+(ddlRitiro.SelectedItem.ToString());
		string sqlconta, sqlstr, filtro, titolo;
		string formatodatasql = "yyyy/MM/dd";
		sqlconta = "SELECT count(distinct g.auto_ek) as \"num.\" ";
		sqlconta += "from GRUPPi as g ";
		sqlconta += "left join UTENTE_GRUPPO as ug on ug.gruppo_ek=g.gruppo_ek ";
		sqlconta += "left join UTENTI as u on u.id = ug.utente_ek ";
		sqlconta += "left join mezzi as m on m.id=g.auto_ek ";
		sqlconta += "left join ubicazione as ub on ub.id=m.ubicazione_ek ";
		sqlconta += "left join comuni as c on c.comune_k=ub.comune_ek ";
		sqlconta += "where " + (ddlRitiro.SelectedIndex <= 0 ? " c.comune = \'Trento\' and m.ubicazione_ek != 0 " : "m.ubicazione_ek=" + ddlRitiro.SelectedValue.ToString()) + " ";
		sqlconta += "and m.abilitato = 1  and m.scadenza > cast(\'" + DateTime.Now.Date.ToString("yyyy/MM/dd") + "\' as date) and u.id=" + utente;

		if ( tbl != null && tbl.Rows.Count > 0) tbl.Clear();
		tbl.Columns.Clear();
		tbl = FBConn.getfromTbl(sqlconta, out msg);
		int sostanza = 0;
		s = (tbl != null && tbl.Rows.Count > 0) ? tbl.Rows[0]["num."].ToString() : "0";
		int.TryParse(s, out sostanza);

		ada = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek); ada = ada.AddDays(21); 

		string dadata = dada.ToString(formatodatasql), adata = ada.ToString(formatodatasql);
		titolo = "n. veicoli prenotabili dal " + dada.ToString("dd-MM-yyyy") + " al " + ada.ToString("dd-MM-yyyy") + " " + sede + " (" + sostanza.ToString() + " veicoli)";

		filtro = " (cast(a.partenza as date) between cast(\'" + dadata + "\' as date) and cast(\'" + adata + "\' as date) or ";
		filtro += "cast(a.arrivo as date) between cast(\'" + dadata + "\' as date) and cast(\'" + adata + "\' as date) or ";
		filtro += "cast(a.partenza as date) <= cast(\'" + dadata + "\' as date) and cast(a.arrivo as date) >= cast(\'" + adata + "\' as date) ) ";
		filtro += (ddlRitiro.SelectedIndex <= 0 ? " and c.comune='Trento' " : " and a.ubicazione_ek = \'" + ddlRitiro.SelectedValue.ToString() + "\' ");

		cLibere.Titles.Clear();

		sqlstr = "select a.id, a.partenza, a.arrivo ";
		sqlstr += "from prenotazioni as a ";
		sqlstr += "left join mezzi as b on b.id = a.mezzo_ek ";
		sqlstr += "left join ubicazione as u on u.id = a.ubicazione_ek ";
		sqlstr += "left join comuni as c on c.comune_k = u.comune_ek ";
		sqlstr += "where u.abilitato = 1 and ";		
		sqlstr += filtro;
		sqlstr += "order by a.partenza ";

		//tLiberi.Text = "Prenotazioni dal " + dada.ToString("dd-MM-yyyy") + " sino al " + ada.ToString("dd-MM-yyyy");

		tbl.Clear();
		tbl.Columns.Clear();
		tbl = FBConn.getfromTbl(sqlstr, out msg);

		if (tbl.Rows.Count > 0)
		{
			//tbl.Rows[0].Delete(); // cancello la prima riga
			//titolo += " " + tbl.Rows.Count.ToString();
			int giorni = (int)(ada - dada).TotalDays + 1; // compresi estremi 
			giorni = giorni <= 2 ? 1 : giorni; // (a meno che non sia un giorno)
			int[] numero = new int[giorni];
			String[] dates = new String[giorni];
			for (int i = 0; i < giorni; i++) { numero[i] = 0; }// inizializzo
			string s; //int y, m, d, H, min, sec;
			DateTime dd, ad, oggi = dada;
			string[] gio = { "dom", "lun", "mar", "mer", "gio", "ven", "sab" };
			for (int gg = 0; gg < giorni; gg++)
			{
				dates[gg] = gio[(int)oggi.DayOfWeek]; // domenica = 0
				dates[gg] += " " + oggi.ToString("dd/MM");
				for (int n = 0; n < tbl.Rows.Count; n++)
				{
					s = tbl.Rows[n][1].ToString(); dd = DateTime.ParseExact(s, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
					s = tbl.Rows[n][2].ToString(); ad = DateTime.ParseExact(s, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
					if (dd.Date <= oggi.Date && ad.Date >= oggi.Date) numero[gg]++;
				}
				numero[gg] = sostanza - numero[gg]; numero[gg] = numero[gg] < 0 ? numero[gg] = 0 : numero[gg];
				oggi = oggi.AddDays(1);
			}
			//fr = ds.Tables["sovrapposte"].Select("id='" + idp + "'");
			//DataRowCollection drc = ds.Tables["sovrapposte"].Rows;
			//contiene = fr.Length > 0 ? 1 : 0;

			cLibere.Series.Clear();
			//cLibere.DataSource = tbl;
			cLibere.Titles.Add(titolo).Font = new System.Drawing.Font("Thaoma", 14);
			cLibere.BackColor = System.Drawing.Color.White; // colore dello sfondo lo sfondo del grafico
			cLibere.ChartAreas["ChartArea1"].AxisY.Title = "veicoli senza preno_\ntazione per data";
			cLibere.ChartAreas["ChartArea1"].AxisY.TitleFont = new System.Drawing.Font("Thaoma", 14);
			cLibere.ChartAreas["ChartArea1"].AxisY.TextOrientation = System.Web.UI.DataVisualization.Charting.TextOrientation.Rotated270;
			cLibere.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Thaoma", 7);
			cLibere.ChartAreas["ChartArea1"].AxisX.Interval = 1;
			cLibere.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = false;
			cLibere.ChartAreas["ChartArea1"].AxisX.LabelStyle.TruncatedLabels = false;
			cLibere.ChartAreas["ChartArea1"].BackColor = System.Drawing.Color.FromName("AliceBlue");
			cLibere.ChartAreas["ChartArea1"].BackSecondaryColor = System.Drawing.Color.FromName("Red");
			cLibere.ChartAreas["ChartArea1"].BackGradientStyle = System.Web.UI.DataVisualization.Charting.GradientStyle.TopBottom;
			cLibere.ChartAreas["ChartArea1"].AxisY.LabelStyle.TruncatedLabels = true;
			cLibere.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
			cLibere.ChartAreas["ChartArea1"].AxisY2.Enabled = new System.Web.UI.DataVisualization.Charting.AxisEnabled();
			cLibere.Series.Add("Prenotazionixdata");
			cLibere.Series["Prenotazionixdata"].BorderWidth = 3;
			cLibere.Series["Prenotazionixdata"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.SplineArea;
			//XData.Series["Prenotazionixdata"].XValueMember = "Data1";
			cLibere.Series["Prenotazionixdata"].LabelAngle = 33;
			cLibere.Series["Prenotazionixdata"].Font = new System.Drawing.Font("Thaoma", 14);
			cLibere.Series["Prenotazionixdata"].Color = System.Drawing.Color.Orange;
			//XData.Series["Prenotazionixdata"].YValueMembers = "numerop";
			//XData.ChartAreas["ChartArea1"].AxisY.Interval = 30;
			cLibere.Series["Prenotazionixdata"].IsValueShownAsLabel = true;
			cLibere.Series["Prenotazionixdata"].BorderWidth = 5;
			cLibere.Series["Prenotazionixdata"].BackSecondaryColor = System.Drawing.Color.Orange; // ?
			cLibere.Series["Prenotazionixdata"].LabelForeColor = System.Drawing.Color.Black; // colore dei Marker
			cLibere.Series["Prenotazionixdata"].LabelBorderWidth = 2;
			cLibere.Series["Prenotazionixdata"].MarkerSize = 17; // distanza dalla serie delle etichette (Marker)
			cLibere.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Thaoma", giorni > 28 ? 7 : 8);
			cLibere.ChartAreas[0].AxisX.TextOrientation = System.Web.UI.DataVisualization.Charting.TextOrientation.Rotated270;
			cLibere.Series["Prenotazionixdata"].Points.DataBindXY(dates, numero);
		}
		else
		{
			cLibere.Visible = false; lnota.Visible = false;
			sStato.Text = "Non risultano automezzi presenti in questa sede!";
		}
	}

	public string Right(string str, int length)
	{
		str = (str ?? string.Empty);
		return (str.Length >= length) ? str.Substring(str.Length - length, length) : str;
	}
}