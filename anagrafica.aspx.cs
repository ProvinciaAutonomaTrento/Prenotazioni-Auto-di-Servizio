/*
 *
 *
 *
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

public partial class anagrafica : System.Web.UI.Page
{
    public ConnessioneFB FBConn = new ConnessioneFB();
    public gmail gm;
    public string msg;
    public string formatodata = "dd-MM-yyyy";
    public user utenti = new user();
    public user cercato = new user();

    private FbConnection cAFbConn = null;
    private ConnessioneFB FBClass = new ConnessioneFB();
    private FbDataAdapter cda = new FbDataAdapter();
    private FbCommand cc = new FbCommand();
    private DataSet cds = new DataSet();
    private DataTable tbl = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
        {
			string s;
			Int32 id = Session["iduser"] != null ? Convert.ToInt32(Session["iduser"].ToString()) : -1;
            if (id <= 0 || !utenti.cercaid(id))
            {
                s = "Sessione scaduta. Prego ricollegarsi.";
                ShowPopUpMsg(s);
                Response.Redirect("default.aspx");
            }
            cbSetPwd.Visible = false;
            tpassword.Visible = false;
            LBenvenuto.Text = " Benvenuto " + utenti.nome + " " + utenti.cognome;
            Session.Add("potere", utenti.potere);
            //string[] p = { "", "-1", "utente", "0", "segreteria", "10" , "operatore", "20", "admin", "50" ,  "superadmin", "100"  };
			ddlPotere.Items.Clear();
			tbl.Clear();
			tbl = FBClass.getfromTbl("select * from poteri where codice <=\'" + (utenti.potere).ToString() + "\' ", out msg);
			int j;
			ddlPotere.Items.Insert(0, new ListItem("", "-1"));
			for (int i = 0; i < tbl.Rows.Count; i++)
			{
                s = tbl.Rows[i]["potere"] != DBNull.Value ? tbl.Rows[i]["potere"].ToString() : "";
                Int32.TryParse(tbl.Rows[i]["codice"].ToString(), out j);
                
                if (utenti.potere <= 100 && j < utenti.potere)
                {                    
                    ddlPotere.Items.Insert(i + 1, new ListItem(s, tbl.Rows[i]["codice"] != DBNull.Value ? tbl.Rows[i]["codice"].ToString() : "-1"));
                }
                else
                {
                    if (utenti.potere > 100)
                    {
                        ddlPotere.Items.Insert(i + 1, new ListItem(s, tbl.Rows[i]["codice"] != DBNull.Value ? tbl.Rows[i]["codice"].ToString() : "-1"));
                    }
                }

            }
            tNikname.Focus();
            //pElenco.Visible = true;
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
        sStato.Text = "";
        // devo salvare o cercare ?
        if (cbCercaa.Text == "Cerca")
        {
            tbl.Clear();
            string s = "", where = ""; bool ok = false;
            s = "SELECT a.*, b.struttura FROM UTENTI a left join strutture b on a.struttura_cod=b.codice ";

            if (tNikname.Text.Trim() != "")
                where += "upper(a.nikname) like \'%" + tNikname.Text.ToUpper() + "%\' and ";
            if (tMatricola.Text.Trim().Length >= 5)
                where += "upper(a.matricola) like \'%" + tMatricola.Text.ToUpper() + "%\' and ";
            if (tNome.Text.Trim() != "" )
                where += "upper(a.nome) like \'%" + tNome.Text.ToUpper() + "%\' and ";
            if (tCognome.Text.Trim() != "")
                where += "upper(a.cognome) like \'%" + tCognome.Text.ToUpper() + "%\' and ";
            if (cbAbilitato.Checked)
                where += "a.abilitato = 1 and ";
            if (ddlPotere.SelectedValue != "-1")
                where += "a.power = " + ddlPotere.SelectedValue  + " and ";
            if ( where != "" ) s += "where " + where.Substring(0, where.LastIndexOf(" and "));
            tbl.Clear();
            tbl = FBConn.getfromTbl(s, out msg);
			if (tbl.Rows.Count > 0)
			{
				RiempiGrid(tbl);
				if (tbl.Rows.Count > 0)
				{
					sStato.Text = "Segli il nominativo voluto. fra i " + tbl.Rows.Count + " trovati! ";
					pElenco.Visible = true;
				}
				else
					pElenco.Visible = false;
			}
			else
			{
				sStato.Text = "Non ci sono utenti con questo nome e cognome!";
				pElenco.Visible = false;
			}
        }
        else // devo salvare i dati
        {
			int key = gElenco.SelectedValue != null ? Convert.ToInt32(gElenco.SelectedValue.ToString()) : -1;
			bool avviso = cercato.abilitato != cbAbilitato.Checked ? true : false;
            cbCercaa.Text = "Cerca";
			cercato.LeggiUtente(key.ToString());
            cercato.abilitato = cbAbilitato.Checked;
            cercato.potere = Convert.ToInt16(ddlPotere.SelectedValue);

            cercato.registradatiutente(out msg);
            if (msg.Length > 0) { sStato.Text = "ERRORE: " + msg; return; }
            piallatext();

            sStato.Text = "Modifica effettuta correttamente.";

            // ora avviso o meno l'utente...
            if (avviso)
            {
                gm = new gmail();
                gm.dachivisualizzato = "uff.gestionigenerali@provincia.tn.it";
                //gm.mail.To.Add(new MailAddress(achi)); // è una lista 
                string[] achi = { cercato.mail };
                gm.achi = achi;
                string s = "";
                if (cercato.abilitato) // utente appena abilitato..non faccio il cambio di password
                {
                    gm.subject = "Gestione flotta provinciale: avviso abilitazione utente e consegna credenziali.";
                    gm.body = string.Format("Buongiorno gentile {0} {1}\n", cercato.nome, cercato.cognome);
                    gm.body += "La informiamo che è stato/a autorizzato/a all\'accesso all\' applicazione \"Prenotazioni Auto di Servizio\". Di seguito Le forniamo le credenziali per il collegamento:\n\n";
                    gm.body += "Username:          \t" + cercato.nikname + "\n";
                    gm.body += "Password:          \t" + cercato.password + "\n\n";
                    gm.body += "\tAl primo accesso, Le sarà chiesto di modificare la password.\n";
                    gm.body += "Grazie.\n\nUff. Gestioni Generali\nGestore autorizzazioni";
                    s = "ABILITAZIONE";
                }
                else
                {
                    gm.subject = "Gestione flotta provinciale: avviso disabilitazione utente.";
                    gm.body = string.Format("Buongiorno gentile {0} {1}\n", cercato.nome, cercato.cognome);
                    gm.body += "La informiamo che la Sua utenza per l\'accesso all\'applicazione \"Prenotazioni Auto di Servizio\" è stata disabilitata.\n\n";
                    gm.body += "Cordilai saluti.\n\nUff. Gestioni Generali\nGestore autorizzazioni";
                    s = "DISABILITAZIONE";
                }
                if (!gm.mandamail("", 0, "", "", out msg))
                {
                    sStato.Text = "Inoltro mail di " + s + " UTENTE non eseguito! CONTATTARE IL NUMERO " + (string)Session["assistenza"] + " Err: " + msg;
                    //ShowPopUpMsg("Richiesta di registrazione effettuata con successo, MAIL DI CONFERMA NON INOLTRATA!. CONTATTARE IL NUMERO " + (string)Session["assistenza"] + " Err: " + msg);
                }
                else
                {
                    sStato.Text = "Inoltro mail di " + s + " UTENTE inviata con successo.";
                }
                gElenco.Visible = false;
            }
        }
    }

    private void piallatext()
    {
        ddlPotere.SelectedIndex = 0;
        cbAbilitato.Checked = false;
        tMatricola.Text = "";
        tNome.Text = "";
        tCognome.Text = "";
        tNikname.Text = "";
    }

    public void RiempiGrid(DataTable gwds)
    {
        if (gElenco.Rows.Count > 0)
        {
            gElenco.DataBind();
        }
        try
        {
            gElenco.DataSource = gwds;
            gwds.DefaultView.Sort = "cognome, nome, struttura";
			gElenco.DataBind();
			gElenco.Visible = true;
		}
        catch (Exception ex)
        {
            sStato.Text = "Riscontrato errore durante la ricerca utenti. Errore: " + ex.ToString() + " Avvertire l'amministratore al n. " + (string)Session["assistenza"].ToString();
        }
    }

    protected void gElenco_SelectedIndexChanged(object sender, EventArgs e)
    {
        int key = gElenco.SelectedValue != null ? Convert.ToInt32(gElenco.SelectedValue.ToString()) : -1;
        if (cercato.cercaid(key))
        {
			cercato.LeggiUtente(key.ToString());
			//GridViewRow row = gElenco.SelectedRow;
			//string id = gElenco.SelectedDataKey.Value.ToString();
			Int32 poteremandante = Session["potere"] != null ? Convert.ToInt32(Session["potere"].ToString()) : -1;
			
			if (poteremandante >= cercato.potere)
            {
                cbAbilitato.Enabled = true;
                ddlPotere.Enabled = true;
                cbAbilitato.Checked = cercato.abilitato;
                tMatricola.Text = cercato.matricola;
                tNikname.Text = cercato.nikname;
                tNome.Text = cercato.nome;
                tCognome.Text = cercato.cognome;
                ddlPotere.SelectedValue = cercato.potere.ToString();
                cbCercaa.Text = "Salva";
                sStato.Text = "";
                cbSetPwd.Visible = true;
                tpassword.Visible = true;
                cbreset.Visible = true;
            }
            else
            {
                sStato.Text = "NON E' POSSIBILE GESTIRE L'UTENTE " + utenti.nikname + "! DIRITTI INSUFFICENTI!";
                piallatext();
                //pElenco.Visible = false;
            }
        }
        else
            sStato.Text = "Errore durante la ricerca dell'utente: " + tCognome.Text + ". Contattare il servizio assistenza al n. " + Session["assistenza"].ToString();
    }

    protected void cbSetPwd_Click(object sender, EventArgs e)
    {
        string msg = "";
        if (cbCercaa.Text == "Salva" && tpassword.Text.Trim().Length > 0)
        {
            // ho il codice utente e la password
            if (cercato.cercanikname(tNikname.Text.Trim(), ""))
            {
                cercato.password = tpassword.Text.Trim();
                if (!cercato.registradatiutente(out msg))
                {
                    sStato.Text = "Problema su impostazione password. Contattare l'amministratore al n. " + Session["assistenza"].ToString();
                }
                else
                    sStato.Text = "Password imposta con successo!";
            };
            tpassword.Text = "";
        }
    }

    protected void cbReset_Click(object sender, EventArgs e)
    {
        cbAbilitato.Enabled = true;
        ddlPotere.Enabled = true;
        cbAbilitato.Checked = true;
        tMatricola.Text = "";
        tNikname.Text = "";
        tNome.Text = "";
        tCognome.Text = "";
        ddlPotere.SelectedIndex = 0;
        cbCercaa.Text = "Cerca";
        sStato.Text = "";
        cbSetPwd.Visible = false;
        tpassword.Visible = false;
        cbreset.Visible = false;
        pElenco.Visible = false;
    }
}