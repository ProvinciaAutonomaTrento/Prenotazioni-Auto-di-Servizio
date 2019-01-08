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

public partial class schedagruppo : System.Web.UI.Page
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
			Leggiddl();

			//string s = ddlUbi.SelectedValue.ToString();
			//s = ddlUbi.SelectedItem.ToString();
			// devo caricare i dati della prima ubicazione.... forse
			if (Request.QueryString["gruppo"] != null)
			{
				int gruppo;
				int.TryParse(Request.QueryString["gruppo"].ToString(), out gruppo);
				ddl.SelectedValue = Request.QueryString["gruppo"].ToString();
				string worder, wverso;
				worder = Request.QueryString["wo"] != null ? Request.QueryString["wo"].ToString() : "Numero";
				wverso = Request.QueryString["wv"] != null ? Request.QueryString["wv"].ToString() : "asc";
				gw_refresh(gwAuto, ("M.ABILITATO > 0 AND gruppo_ek = " + ddl.SelectedValue.ToString().Trim()), worder, wverso);
				gw_refresh(gwEscluse, ("M.ABILITATO > 0 AND not m.id in (select auto_ek from gruppi where gruppo_ek = " + ddl.SelectedValue.ToString().Trim() + ")"), worder, wverso);
				lGruppo.Text = "Mezzi associati alla flotta " + ddl.SelectedItem.ToString().Trim() + ": " + gwAuto.Rows.Count;
				LEscluse.Text = "Mezzi non associati alla flotta " + ddl.SelectedItem.ToString().Trim() + ": " + gwEscluse.Rows.Count;
				sStato.Text = "Porre molta attenzione nell'aggiungere o togliere veicoliveicoli alla flotta...";
			}
			else
			{
				gw_refresh(gwAuto, ("M.ABILITATO > 0 AND gruppo_ek = " + ddl.SelectedValue.ToString().Trim()), order, verso);
				gw_refresh(gwEscluse, ("M.ABILITATO > 0 AND not m.id in (select auto_ek from gruppi where gruppo_ek = " + ddl.SelectedValue.ToString().Trim() + ")"), order, verso);
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

    public void Leggiddl()
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
            s = "SELECT a.* FROM etichetta_gruppo as a where abilitato > 0 order by a.etichetta";
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
		string where = ddl.SelectedValue.ToString();
		if (where.Trim() == "")
			return;
		Response.Redirect("schedagruppo.aspx?gruppo=" + ddl.SelectedValue.ToString());
		sStato.Text = "Porre molta attenzione nell'aggiungere o togliere veicoliveicoli alla flotta...";
		//gw_refresh(gwAuto, ("gruppo_ek = " + where));
		//gw_refresh(gwEscluse, (" not auto_ek in (select auto_ek from gruppi where gruppo_ek = " + where + ")"));
	}
	protected void gwEscluse_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (ddl.SelectedValue.ToString().Trim() == "")
			return;
		int idgruppo;
		Int32.TryParse(ddl.SelectedValue.ToString(), out idgruppo);
		checkSession();

		int key = gwEscluse.SelectedValue != null ? Convert.ToInt32(gwEscluse.SelectedValue.ToString()) : -1;
		//int riga = gwEscluse.SelectedIndex;
		// ok devo aggiungere a gruppi una riga con id gruppo in gruppo_ek e id mezzi in auto_ek
		// refrescare....
		gruppo gr = new gruppo();
		if (!gr.gr_add(idgruppo, key, idu))
		{
			sStato.Text = "ATTENZIONE: aggiunta auto a gruppo non riuscita. Contattare assistenza al numero: " + Session["assistenza"].ToString();
			return;
		}
				
		//gw_refresh(gwAuto, ("gruppo_ek = " + ddl.SelectedValue.ToString().Trim()));
		//gw_refresh(gwEscluse, (" not auto_ek in (select auto_ek from gruppi where gruppo_ek = " + ddl.SelectedValue.ToString().Trim() + ")"));
		Response.Redirect("schedagruppo.aspx?gruppo=" + ddl.SelectedValue.ToString());
	}

	protected void gwAuto_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (ddl.SelectedValue.ToString().Trim() == "")
			return;
		int idgruppo;
		Int32.TryParse(ddl.SelectedValue.ToString(), out idgruppo);
		checkSession();

		int key = gwAuto.SelectedValue != null ? Convert.ToInt32(gwAuto.SelectedValue.ToString()) : -1;
		//int riga = gwEscluse.SelectedIndex;
		// ok devo aggiungere a gruppi una riga con id gruppo in gruppo_ek e id mezzi in auto_ek
		// refrescare....
		gruppo gr = new gruppo();
		if (!gr.gr_delete(idgruppo, key, idu))
		{
			sStato.Text = "ATTENZIONE: cancellazione automezzo dalla flotta non riuscita. Contattare assistenza al numero: " + Session["assistenza"].ToString();
			return;
		}
		//gw_refresh(gwAuto, ("gruppo_ek = " + ddl.SelectedValue.ToString().Trim()));
		//gw_refresh(gwEscluse, (" not auto_ek in (select auto_ek from gruppi where gruppo_ek = " + ddl.SelectedValue.ToString().Trim() + ")"));
		Response.Redirect("schedagruppo.aspx?gruppo=" + ddl.SelectedValue.ToString());
	}

	protected void gw_refresh(GridView gw, string where, string order, string verso)
	{
		where = "where " + where + " ";
		order = order == "" ? "Numero" : order;
		verso = verso == "" ? "asc" : verso;
		string s;
		s = "SELECT distinct m.id as ida, M.NUMERO, M.TARGA, MA.MARCA, MO.MODELLO, co.comune, ub.ubicazione, ub.via, ub.civico ";
		s += "FROM MEZZI AS M ";
		s += "LEFT JOIN MARCA AS MA ON MA.ID = M.MARCA_EK ";
		s += "LEFT JOIN MODELLO AS MO ON MO.ID = M.MODELLO_EK ";
		s += "left join gruppi as gr on gr.auto_ek=m.id ";
		s += "left join ubicazione as ub on ub.id=m.ubicazione_ek ";
		s += "left join comuni as co on co.comune_k=ub.comune_ek ";
		/*s += "LEFT JOIN classificazione AS cl ON cl.ID = M.classificazione_EK ";
		s += "LEFT JOIN cambio AS ca ON ca.id = m.cambio_ek ";
		s += "LEFT JOIN alimentazione AS al ON al.id = m.alimentazione_ek ";
		s += "LEFT JOIN gomme AS go ON go.id = m.gomme_ek ";
		s += "LEFT JOIN trazione AS tr ON tr.id = m.trazione_ek "; */
		s += where;
		s += "ORDER BY "+ order + " " + verso;
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
			verso = verso == "asc" ? "desc" : "asc";
			Response.Redirect("schedagruppo.aspx?gruppo=" + ddl.SelectedValue.ToString() + "&wo=" + order + "&wv="+verso);
			//gw_refresh(gwAuto, ("gruppo_ek = " + ddl.SelectedValue.ToString().Trim()), order, verso);
			//gw_refresh(gwEscluse, (" not auto_ek in (select auto_ek from gruppi where gruppo_ek = " + ddl.SelectedValue.ToString().Trim() + ")"), order, verso);
		//}
	}
}