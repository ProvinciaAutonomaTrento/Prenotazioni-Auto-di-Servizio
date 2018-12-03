using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class schedausergruppo : System.Web.UI.Page
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
	public string order = "";
	public string verso = "";

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
		{
			checkSession();
			Leggiddl("SELECT a.* FROM etichetta_gruppo as a where abilitato > 0 order by a.etichetta", ddlGruppo);

			//string s = ddlUbi.SelectedValue.ToString();
			//s = ddlUbi.SelectedItem.ToString();
			// devo caricare i dati della prima ubicazione.... forse
			if (Request.QueryString["gruppo"] != null)
			{
				int gruppo;
				int.TryParse(Request.QueryString["gruppo"].ToString(), out gruppo);
				ddlGruppo.SelectedValue = Request.QueryString["gruppo"].ToString();
				string worder, wverso;
				worder = Request.QueryString["wo"] != null ? Request.QueryString["wo"].ToString() : "Cognome, nome";
				wverso = Request.QueryString["wv"] != null ? Request.QueryString["wv"].ToString() : "asc";
				gw_refresh(gwUser, ("ug.gruppo_ek = " + ddlGruppo.SelectedValue.ToString().Trim()), worder, wverso);
				gw_refresh(gwEsclusi, (" not u.id in (select utente_ek from utente_gruppo where gruppo_ek = " + ddlGruppo.SelectedValue.ToString().Trim() + ")"), worder, wverso);
				lGruppo.Text = "Utenti abilitati alla flotta " + ddlGruppo.SelectedItem.ToString().Trim() + ": " + gwUser.Rows.Count;
				lEscluse.Text = "Utenti non abilitati alla flotta " + ddlGruppo.SelectedItem.ToString().Trim() + ": " + gwEsclusi.Rows.Count;
				sStato.Text = "Porre molta attenzione nell'aggiungere o togliere veicoliveicoli al gruppo...";
			}
			else
			{
				//gw_refresh(gwUser, ("gruppo_ek = " + ddl.SelectedValue.ToString().Trim()), order, verso);
				//gw_refresh(gwEsclusi, (" not auto_ek in (select utente_ek from gruppi where abilitato > 0 and gruppo_ek = " + ddl.SelectedValue.ToString().Trim() + ")"), order, verso);
				sStato.Text = "Selezionare un gruppo..";
			}
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

	protected void ddlGruppo_SelectedIndexChanged(object sender, EventArgs e)
	{
		string where = ddlGruppo.SelectedValue.ToString();
		if (where.Trim() == "")
			return;
		sStato.Text = "Porre molta attenzione nel abilitare o disabilitare utenti alla flotta...";
		Response.Redirect("schedausergruppo.aspx?gruppo=" + ddlGruppo.SelectedValue.ToString());
		//gw_refresh(gwUser, ("gruppo_ek = " + where));
		//gw_refresh(gwEsclusi, (" not auto_ek in (select auto_ek from gruppi where gruppo_ek = " + where + ")"));
	}
	protected void ddlFlotte_SelectedIndexChanged(object sender, EventArgs e)
	{
		string where = ddlGruppo.SelectedValue.ToString();
		if (where.Trim() == "")
			return;

		gwEsclusi.Visible = true;
		gwUser.Visible = true;
		pUtentiFlotta.Visible = true;
		// carico i dati delle auto dell'utente
		pDettaglio.Visible = false;
		pnominativo.Visible = false;

		sStato.Text = "Porre molta attenzione nel abilitare o disabilitare utenti alla flotta...";
		Response.Redirect("schedausergruppo.aspx?gruppo=" + ddlGruppo.SelectedValue.ToString());
		//gw_refresh(gwUser, ("gruppo_ek = " + where));
		//gw_refresh(gwEsclusi, (" not auto_ek in (select auto_ek from gruppi where gruppo_ek = " + where + ")"));
	}
	protected void gwEsclusi_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (ddlGruppo.SelectedValue.ToString().Trim() == "")
			return;
		int idgruppo;
		Int32.TryParse(ddlGruppo.SelectedValue.ToString(), out idgruppo);
		checkSession();

		int key = gwEsclusi.SelectedValue != null ? Convert.ToInt32(gwEsclusi.SelectedValue.ToString()) : -1;
		//int riga = gwEsclusi.SelectedIndex;
		// ok devo aggiungere a gruppi una riga con id gruppo in gruppo_ek e id mezzi in auto_ek
		// refrescare....
		gruppo_utenti gru = new gruppo_utenti();
		if (!gru.gru_add(idgruppo, key, idu))
		{
			sStato.Text = "ATTENZIONE: l'abilitazione dell'utente alla flotta non è riuscita. Contattare assistenza al numero: " + Session["assistenza"].ToString();
			return;
		}
		//gw_refresh(gwUser, ("gruppo_ek = " + ddl.SelectedValue.ToString().Trim()));
		//gw_refresh(gwEsclusi, (" not auto_ek in (select auto_ek from gruppi where gruppo_ek = " + ddl.SelectedValue.ToString().Trim() + ")"));
		Response.Redirect("schedausergruppo.aspx?gruppo=" + ddlGruppo.SelectedValue.ToString());
	}

	protected void gwUser_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (ddlGruppo.SelectedValue.ToString().Trim() == "")
			return;
		int idgruppo;
		Int32.TryParse(ddlGruppo.SelectedValue.ToString(), out idgruppo);
		checkSession();

		int key = gwUser.SelectedValue != null ? Convert.ToInt32(gwUser.SelectedValue.ToString()) : -1;
		//int riga = gwEsclusi.SelectedIndex;
		// ok devo aggiungere a gruppi una riga con id gruppo in gruppo_ek e id mezzi in auto_ek
		// refrescare....
		gruppo_utenti gru = new gruppo_utenti();
		if (!gru.gru_delete(idgruppo, key, idu))
		{
			sStato.Text = "ATTENZIONE: cancellazione dell'utente alla flotta non è riuscita. Contattare assistenza al numero: " + Session["assistenza"].ToString();
			return;
		}
		//gw_refresh(gwUser, ("gruppo_ek = " + ddl.SelectedValue.ToString().Trim()));
		//gw_refresh(gwEsclusi, (" not auto_ek in (select auto_ek from gruppi where gruppo_ek = " + ddl.SelectedValue.ToString().Trim() + ")"));
		Response.Redirect("schedausergruppo.aspx?gruppo=" + ddlGruppo.SelectedValue.ToString());
	}

	protected void gw_refresh(GridView gw, string where, string order, string verso)
	{
		where = "where u.abilitato > 0 and " + where + " ";
		order = order == "" ? "Cognome, Nome" : order;
		verso = verso == "" ? "asc" : verso;
		string s;
		s = "SELECT distinct u.id as ida, u.cognome, u.nome, u.ente, u.matricola, u.telefono ";
		s += "FROM UTENTI u ";
		//s += "left join ETICHETTA_GRUPPO as e on e.id = a.GRUPPO_EK ";
		//s += "left join mezzi as m on m.id = a.auto_ek ";
		//s += "left join marca as ma on ma.id = m.marca_ek ";
		//s += "left join modello as mo on mo.id = m.modello_ek ";
		//s += "left join UTENTE_GRUPPO as ug on ug.gruppo_ek = a.GRUPPO_EK ";
		s += "left join utente_gruppo as ug on ug.utente_ek = u.id ";
		//s += "order by e.etichetta, ma.marca, m.targa ";
		s += where;
		s += "ORDER BY " + order + " " + verso;
		// ora devo caricare i dati della qry
		// devo leggere la tabella gruppi
		msg = "";
		try
		{
			int gvHasRows = gw.Rows.Count;
			if (gvHasRows > 0)
			{
				gw.Columns.Clear();
				gw.DataBind();
			}
			ds.Clear();
			ds = FBConn.getfromDSet(s, gw.ID.ToString(), out msg);
			gw.DataSource = ds.Tables[gw.ID.ToString()];
			gw.DataBind();
			gw.Visible = true;
		}
		catch (Exception ex)
		{
			sStato.Text = "Riscontrato errore durante la ricerca degli utenti: " + ex.ToString() + " Avvertire l'amministratore al n. " + (string)Session["assistenza"].ToString();
		}
	}

	protected void gwUser_Sorting(object sender, GridViewSortEventArgs e)
	{
		//string qry = Session["qry"].ToString() != null && Session["qry"].ToString() != "" ? Session["qry"].ToString() : "";
		//if (qry != "")
		//{
		msg = "";
		order = e.SortExpression;
		verso = verso == "asc" ? "desc" : "asc";
		Response.Redirect("schedausergruppo.aspx?gruppo=" + ddlGruppo.SelectedValue.ToString() + "&wo=" + order + "&wv=" + verso);
		//gw_refresh(gwUser, ("gruppo_ek = " + ddl.SelectedValue.ToString().Trim()), order, verso);
		//gw_refresh(gwEsclusi, (" not auto_ek in (select auto_ek from gruppi where gruppo_ek = " + ddl.SelectedValue.ToString().Trim() + ")"), order, verso);
		//}
	}

	protected void gwUser_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (e.CommandName == "auto_utente")
		{
			int ida = Convert.ToInt32(e.CommandArgument);
			Visualizza_auto(ida);
		}
	}

	protected void gwEsclusi_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (e.CommandName == "auto_utente")
		{
			int ida = Convert.ToInt32(e.CommandArgument);
			Visualizza_auto(ida);
		}
	}

	protected void Visualizza_auto(int ida)
	{
		gwEsclusi.Visible = false;
		gwUser.Visible = false;
		pUtentiFlotta.Visible = false;
		// carico i dati delle auto dell'utente
		pnominativo.Visible =false;
		pDettaglio.Visible = true;
		if (utenti.cercaid(ida))
		{
			lNome.Text = utenti.nome + " " + utenti.cognome;
			lStruttura.Text = utenti.ente;
			lMatricola.Text = utenti.matricola;
			lNome.Visible = true;
			lStruttura.Visible = true;
			lMatricola.Visible = true;
			pnominativo.Visible = true;
		}
		gw_refresh_auto(gwAuto, "ug.utente_ek=\'"+ida.ToString() +"\' ", "e.ordine desc, marca, modello", "desc");
		//gwAuto.SelectedRow.Cells[12].ToString();
		sStato.Text = "Seleziona flotta... per proseguire";
	}
	
	protected void gw_refresh_auto(GridView gw, string where, string order, string verso)
	{
		where = "where " + where + " ";
		order = order == "" ? "Numero" : order;
		verso = verso == "" ? "asc" : verso;
		string s;
		s = "SELECT distinct m.id as ida, M.NUMERO, M.TARGA, MA.MARCA, MO.MODELLO, co.comune, ub.ubicazione, ub.via, ub.civico, e.etichetta, e.ordine ";
		s += "FROM MEZZI AS M ";
		s += "LEFT JOIN MARCA AS MA ON MA.ID = M.MARCA_EK ";
		s += "LEFT JOIN MODELLO AS MO ON MO.ID = M.MODELLO_EK ";
		s += "left join gruppi as gr on gr.auto_ek=m.id ";
		s += "left join ubicazione as ub on ub.id=m.ubicazione_ek ";
		s += "left join comuni as co on co.comune_k=ub.comune_ek ";
		s += "left join ETICHETTA_GRUPPO as e on e.id= gr.gruppo_ek ";
		s += "left join UTENTE_GRUPPO as ug on ug.gruppo_ek = gr.gruppo_ek ";
		/*s += "LEFT JOIN classificazione AS cl ON cl.ID = M.classificazione_EK ";
		s += "LEFT JOIN cambio AS ca ON ca.id = m.cambio_ek ";
		s += "LEFT JOIN alimentazione AS al ON al.id = m.alimentazione_ek ";
		s += "LEFT JOIN gomme AS go ON go.id = m.gomme_ek ";
		s += "LEFT JOIN trazione AS tr ON tr.id = m.trazione_ek "; */
		s += where;
		s += "ORDER BY " + order ;
		// ora devo caricare i dati della qry
		// devo leggere la tabella gruppi
		msg = "";
		try
		{
			int gvHasRows = gw.Rows.Count;
			if (gvHasRows > 0)
			{
				gw.Columns.Clear();
				gw.DataBind();
			}
			ds.Clear();
			ds = FBConn.getfromDSet(s, gw.ID.ToString(), out msg);
			gw.DataSource = ds.Tables[gw.ID.ToString()];
			gw.DataBind();
			gw.Visible = true;
		}
		catch (Exception ex)
		{
			sStato.Text = "Riscontrato errore durante la ricerca dei veicoli della flotta. Errore: " + ex.ToString() + " Avvertire l'amministratore al n. " + (string)Session["assistenza"].ToString();
		}
	}

	protected void gwAuto_Sorting(object sender, GridViewSortEventArgs e)
	{
		//string qry = Session["qry"].ToString() != null && Session["qry"].ToString() != "" ? Session["qry"].ToString() : "";
		//if (qry != "")
		//{
		msg = "";
		order = e.SortExpression;
		
		Response.Redirect("schedausergruppo.aspx?gruppo=" + ddlGruppo.SelectedValue.ToString() + "&wo=" + order + "&wv=" + verso);
		//gw_refresh(gwAuto, ("gruppo_ek = " + ddl.SelectedValue.ToString().Trim()), order, verso);
		//gw_refresh(gwEscluse, (" not auto_ek in (select auto_ek from gruppi where gruppo_ek = " + ddl.SelectedValue.ToString().Trim() + ")"), order, verso);
		//}
	}
}
