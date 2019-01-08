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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Drawing;

public partial class prealtro : System.Web.UI.Page
{
    public ConnessioneFB FBConn = new ConnessioneFB();
    public string msg;
    public string formatodata = "dd-MM-yyyy";
    public static user utenti = new user();

    private static FbConnection cAFbConn = null;
    private ConnessioneFB FBClass = new ConnessioneFB();
    private FbDataAdapter cda = new FbDataAdapter();
    private FbCommand cc = new FbCommand();
    private DataSet cds = new DataSet();
    private DataTable tbl = new DataTable();
    public Color rosso = Color.Red;
    public Color nero = Color.Black;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
        {
            Int32 id = Session["iduser"] != null ? Convert.ToInt32(Session["iduser"].ToString()) : -1;
            if (id <= 0 || !utenti.cercaid(id))
            {
                string s = "Sessione scaduta. Prego ricollegarsi.";
                ShowPopUpMsg(s);
                Response.Redirect("default.aspx");
            }
            if (utenti.potere == 20)
                Response.Redirect("Menu.aspx?msg='Utente non autorizzato a prenotare veicoli per conto terzi! Contattare l'assistenza al n. " + Session["assistenza"].ToString());

            LBenvenuto.Text = " Benvenuto " + utenti.nome + " " + utenti.cognome;
            Session.Add("potere", utenti.potere);
            LeggiTabella(ddlPotere, "select * from poteri order by codice", "poteri");
            /*string[] p = { "utente", "0" ,  "segreteria", "10" , "operatore", "20", "admin", "50" ,  "superadmin", "100"  };
            ddlPotere.Items.Clear();
            for ( int i = 0, j = 0; i < 9; i += 2, j++ ) {
                if (utenti.potere >= Convert.ToInt16(p[i + 1]))
                    ddlPotere.Items.Insert(j, new ListItem(p[i], p[i + 1]));                
            }*/
            //pElenco.Visible = true;
            cbCerca.Focus();
        }
    }
    protected void Stato(string msg, Color c)
    {
        if (c == null) c = Color.Black;
        sStato.ForeColor = c;
        sStato.Text = msg;
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
        if (cbCerca.Text == "Cerca")
        {
            tbl.Clear();
            string s = "", where = ""; bool ok = false;
            s = "SELECT a.*, b.struttura FROM UTENTI a left join strutture b on a.struttura_cod=b.codice ";

            if (tNikname.Text.Trim() != "")
                where += "upper(a.nikname) like \'%" + tNikname.Text.ToUpper() + "%\' and ";
            if (tMatricola.Text.Trim().Length >= 5)
                where += "upper(a.matricola) like \'%" + tMatricola.Text.ToUpper() + "%\' and ";
            if (tNome.Text.Trim() != "")
                where += "upper(a.nome) like \'%" + tNome.Text.ToUpper() + "%\' and ";
            if (tCognome.Text.Trim() != "")
                where += "upper(a.cognome) like \'%" + tCognome.Text.ToUpper() + "%\' and ";
            if (where != "") s += "where " + where.Substring(0, where.LastIndexOf(" and "));
            tbl.Clear();
            tbl = FBConn.getfromTbl(s, out msg);
            if (tbl.Rows.Count > 0)
            {
                RiempiGrid(tbl);
                if (tbl.Rows.Count > 0)
                {
                    sStato.Text = "Segli il nominativo voluto. fra i " + tbl.Rows.Count + " trovati! ";
                }
                else
                    gElenco.Visible = false;
            }
            else
            {
                sStato.Text = "Non ci sono utenti ceh soddisfano i criteri di ricerca indicati!";
                return;
            }
        }
        else
        { 
            if (string.IsNullOrEmpty(Session["potere"].ToString()))
            {
                string s = "Sessione scaduta. Prego ricollegarsi.";
                ShowPopUpMsg(s);
                Response.Redirect("default.aspx");
            }
            if (utenti.potere <= Convert.ToInt16(Session["potere"].ToString()))
            {
                cbAbilitato.Enabled = false;
                ddlPotere.Enabled = false;
                cbAbilitato.Checked = utenti.abilitato;
                tMatricola.Text = utenti.matricola;
                tNikname.Text = utenti.nikname;
                tNome.Text = utenti.nome;
                tCognome.Text = utenti.cognome;

                ddlPotere.SelectedValue = utenti.potere.ToString();
                cbCerca.Text = "Prenota";
            }
            else
            {
                sStato.Text = "NON E' POSSIBILE GESTIRE L'UTENTE " + utenti.nikname + "! DIRITTI INSUFFICENTI!";
                piallatext();
                return;
            }
			Session.Add("aid", utenti.iduser);
            Response.Redirect("pre.aspx?aid=" + utenti.iduser + "&nome="+utenti.nome+"&cognome="+utenti.cognome+"&p="+utenti.potere);
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
        cbAbilitato.Enabled = false;
        ddlPotere.Enabled = false;
        gElenco.Visible = false;
    }

    public void RiempiGrid(DataTable gwds)
    {
        try
        {
            //int gvHasRows = gElenco.Rows.Count;
            gElenco.DataSource = gwds;
            gElenco.DataBind();            
            gElenco.Visible = true;
            string ss = gwds.TableName + " id0=" + gwds.Rows[0]["id"].ToString();
            sStato.Text = ss;
        }
        catch (Exception ex)
        {
            sStato.Text = "Riscontrato errore durante la ricerca utenti. Errore: " + ex.ToString() + " Avvertire l'amministratore al n. " + (string)Session["assistenza"].ToString();
            return; 
        }
        gElenco.Visible = true;
    }

    protected void gElenco_SelectedIndexChanged(object sender, EventArgs e)
    {
        int key = gElenco.SelectedValue != null ? Convert.ToInt32(gElenco.SelectedValue.ToString()) : -1;
        //gElenco.Visible = false;
        if (utenti.cercaid(key))
        {
            //GridViewRow row = gElenco.SelectedRow;
            //string id = gElenco.SelectedDataKey.Value.ToString();
            if (utenti.potere <= Convert.ToInt16(Session["potere"].ToString()))
            {
                cbAbilitato.Checked = utenti.abilitato;
                tMatricola.Text = utenti.matricola;
                tNikname.Text = utenti.nikname;
                tNome.Text = utenti.nome;
                tCognome.Text = utenti.cognome;

                ddlPotere.SelectedValue = utenti.potere.ToString();
                cbCerca.Text = "Prenota";
            }
            else
            {
                sStato.Text = "NON E' POSSIBILE GESTIRE L'UTENTE " + utenti.nikname + "! DIRITTI INSUFFICENTI!";
                piallatext();
            }
        }
        else
            sStato.Text = "Errore durante la ricerca dell'utente: " + tCognome.Text + ". Contattare il servizio assistenza al n. " + Session["assistenza"].ToString();
    }

    protected void LeggiTabella(DropDownList ddl, string sql, string tabella)
    {
        // devo leggere la tabella Province
        msg = "";
        FBConn.openaFBConn(out msg);
        if (msg.Length >= 1)
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
        else
        {
            msg = "";
            cds.Clear();
            cds = FBConn.getfromDSet(sql, tabella, out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (cds.Tables[tabella].Rows.Count > 0)
                {
                    ddl.Items.Clear();
                    string s = "", ss = ""; int k = 0;
                    for (int i = 0; i < cds.Tables[tabella].Rows.Count; i++)
                    {
                        s = cds.Tables[tabella].Rows[i][1] == DBNull.Value ? "" : cds.Tables[tabella].Rows[i][1].ToString();
                        ss = cds.Tables[tabella].Rows[i][0] == DBNull.Value ? "" : cds.Tables[tabella].Rows[i][0].ToString().Trim();
                        ddl.Items.Insert(i, new ListItem(s, ss));
                        //if (s == "Trento") k = i;
                    }
                }
                else
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella UBICAZIONI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }
    protected void cbVisualizza_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void cbVisualizza_CheckedChanged1(object sender, EventArgs e)
    {
        gElenco.Visible = !gElenco.Visible;
        sStato.Text = "gElenco.visible = " + (gElenco.Visible ? "Visibile" : "INVISIBLE!");
    }
}




