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
using System.Data.OleDb;

public partial class importastrutture : System.Web.UI.Page
{
	public ConnessioneFB FBConn = new ConnessioneFB();

	public static OleDbConnection Olecnn = null;
	public string strOleConn = "Microsoft.Jet.OLEDB.4.0;"; // Data Source=S:/DatiGare/dati/schemas.mdb";
	public string strOleDataSource = "u:/get/strutture.mdb";
	
	public OleDbCommand c = new OleDbCommand();
	public OleDbDataAdapter da = new OleDbDataAdapter();
	public DataSet ds = new DataSet();
	private www CLogga = new www();
	public user utenti = new user();
	string msgl = "";
	Int32 idu;
	public string msg;
    public string formatodata = "dd-MM-yyyy";
    public string s, ss;
    public string[] filesfoto = new string[4];

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
        {
			tProvider.Text = strOleConn;
			tData.Text = strOleDataSource;
			tTabella.Text = "Strutture_i";
			tStato.Text = "Completa i valori dei campi nella maschera e clicca su 'Importa strutture provinciali'";
			bImport.Visible = true;
			checkSession();
        }
    }

	private bool checkSession()
	{
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

    protected bool controllavalidita(string ext)
    {
        string[] validFileTypes = { "bmp", "gif", "png", "jpg", "jpeg" };
        bool isValidFile = false;
        for (int j = 0; j < validFileTypes.Length; j++)
        {
            if (ext == "." + validFileTypes[j])
            {
                isValidFile = true;
                break;
            }
        }
        return (isValidFile);
    }

	protected void bImport_Click(object sender, EventArgs e)
	{
        DataTable esiste = null;
        checkSession();
        string ss = "";
		msg = ""; s = "";
		if (tProvider.Text.Trim().Length <= 3) s = "Problema con Provider; ";
		if (tData.Text.Trim().Length <= 3) s += "Problema con Data source; ";
		if (tTabella.Text.Trim().Length <= 3) s += "Problema con nome tabella;";
		if ( s.Length >= 1 )
			tStato.Text = s + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
		else
		{
            tStato.Text = "Inizio aggionramento.....";
			string strconn = "Provider=" + tProvider.Text.Trim() + "Data source=" + tData.Text.Trim();
			Olecnn = openaOleConn(strconn);
			ds = getdata("select * from " + tTabella.Text.Trim(), ds, "origine", Olecnn);
			idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
			if (ds != null && ds.Tables["origine"].Rows.Count > 0)
			{
				DataTable tbl = ds.Tables["origine"]; // ho la tabella strutture esterna....
													  // ora devo importarla in una tabella temporanea locale... per poi poterla joinare o fare altro...
				FBConn.EsegueCmd("delete from strutturetmp", out msg);				
				//s = "insert into strutturetmp (codice, struttura, mail, dtla, dtc, chi) ";
				//s += "select cod_s, XSTRUTTURA, e-mail, cast(now as timestamp), cast(now as timestamp), " + idu.ToString();
				//FBConn.EsegueCmd(s, out msg);
				for (int i = 0; i < tbl.Rows.Count; i++) // for each no ???
				{
                    ss = (tbl.Rows[i]["cod_sup"] == DBNull.Value || tbl.Rows[i]["cod_sup"].ToString().Length <= 2) ? "\'\'" : "\'" + tbl.Rows[i]["cod_sup"].ToString() + "\'";
                    s = "insert into strutturetmp (codice, struttura, mail, dtla, dtc, chi, dipendeda_ek) values (";
					s += "\'" + utenti.virgolette(tbl.Rows[i]["cod_s"].ToString()) + "\', \'" + utenti.virgolette(tbl.Rows[i]["descrizione_s"].ToString()) + "\', ";
					s += "\'" + utenti.virgolette(tbl.Rows[i]["e-mail"].ToString()) + "\', cast(\'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp), cast(\'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp), ";
					s += idu.ToString() + ", " + ss + ")";
					FBConn.EsegueCmd(s, out msg);
					if (msg != "")
					{
						tStato.Text = "Attenzione... problema su " + msg;
						return;
					}
					// ok aggiunta nuova struttura.... ora vedo se c'è già nel db.. se non c'è devo aggiungerla.... e se cè la aggiorno
					s = "select * from strutture ";
					s += "where codice=\'" + utenti.virgolette(tbl.Rows[i]["cod_s"].ToString()) + "\' and ";
					s += "struttura = \'" + utenti.virgolette(tbl.Rows[i]["descrizione_s"].ToString()) + "\' ";
					s += "order by codice, dtla desc ";
					
                    if (esiste != null && esiste.Rows.Count > 0 ) esiste.Clear();
					esiste = FBConn.getfromTbl(s, out msg);
					if (esiste != null && esiste.Rows.Count <= 0) // nuova struttura
					{
						s = "insert into strutture (codice, struttura, mail, dtla, dtc, chi, dipendeda_ek) values (";
						s += "\'" + utenti.virgolette(tbl.Rows[i]["cod_s"].ToString()) + "\', \'" + utenti.virgolette(tbl.Rows[i]["descrizione_s"].ToString()) + "\', ";
						s += "\'" + utenti.virgolette(tbl.Rows[i]["e-mail"].ToString()) + "\', cast(\'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp), cast(\'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp), ";
                        s += idu.ToString() +  ", " + ss + ")";
						FBConn.EsegueCmd(s, out msg);
						if (msg != "")
						{
							tStato.Text = "Attenzione... problema su " + msg;
							return;
						}
					}
					else
					{
						if (esiste != null && esiste.Rows.Count >= 1) // aggiorno eventuale mail
						{
							//if (esiste.Rows[0]["mail"].ToString().Trim() != tbl.Rows[i]["e-mail"].ToString().Trim())
							//{
                                s = "update strutture set ";
                                s += "mail = \'" + tbl.Rows[i]["e-mail"].ToString().Trim() + "\', ";
                                s += "dtla = cast(\'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\' as timestamp), ";
                                s += "chi =\'" + idu.ToString() + "\', ";
                                s += "dipendeda_ek = " + ss + " ";
                                s += "where id=\'" + esiste.Rows[0]["id"].ToString() + "\'";
								FBConn.EsegueCmd(s, out msg);
								if (msg != "")
                                    { tStato.Text = msg; return; }
							//}
						}
					}
				}
				tStato.Text = "OK. Analizzate ed inserite " + tbl.Rows.Count.ToString() + " strutture!";
				FBConn.EsegueCmd("delete from strutturetmp", out msg);  // piallo la tabella strutturetmp
			}
		}
	}

	protected void bImportuser_Click(object sender, EventArgs e)
	{
		checkSession();
		msg = ""; string s = "";
		if (tProvider.Text.Trim().Length <= 3) s = "Problema con Provider; ";
		if (tData.Text.Trim().Length <= 3) s += "Problema con Data source; ";
		if (tTabella.Text.Trim().Length <= 3) s += "Problema con nome tabella;";
		if (s.Length >= 1)
			tStato.Text = s + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
		else
		{
			// leggo dati tabella access e dati tabella strutture
			string strconn = "Provider=" + tProvider.Text.Trim() + "Data source=" + tData.Text.Trim();
			Olecnn = openaOleConn(strconn);
			ds = getdata("select a.*, s.descrizione_s from anagrafica_i as a left join strutture_i as s on s.cod_s=a.codice_s_app ", ds, "usercallcenter", Olecnn);
			FBConn.closeaFBConn(out msg);
			idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
			if (ds != null && ds.Tables["usercallcenter"].Rows.Count > 0)
			{				
				// cancello eventuale tabella presente nel db di destinazione
				FBConn.EsegueCmd("drop table utenti_tmp", out msg);
				FBConn.closeaFBConn(out msg);
				// la ricreo ex-novo
				s = "CREATE TABLE UTENTI_TMP (PWD Varchar(20), POWER Integer DEFAULT 0, MAIL Varchar(50), CELL Varchar(40), NOME Varchar(30), ";
				s += "COGNOME Varchar(40), DATACREAZIONE Timestamp DEFAULT current_timestamp, NIKNAME Varchar(33), IP Varchar(15), ";
				s += "DTLA Timestamp DEFAULT current_timestamp, FORZOCAMBIOPASSWORD Smallint, MATRICOLA Varchar(16), INDIRIZZO Varchar(55), ABILITATO Smallint, ";
				s += "CIVICO Varchar(22), CAP Varchar(15), CITTA Varchar(50), TELEFONO Varchar(40), TIPODOC Varchar(50), NUMDOC Varchar(50), SCADENZA Date, ";
				s += "ENTE Varchar(150), STRUTTURA_COD Varchar(5), STRUTTURA_EK Varchar(10), SCADENZA_PATENTE Date, ATTIVO Smallint, COD_STRU Varchar(5), ";
				s += "sede varchar(150), sede_indirizzo varchar(150), sede_cap varchar(8) ) ";
				
				FBConn.EsegueCmd(s, out msg);

				DataTable tbl = ds.Tables["usercallcenter"]; // ho la tabella strutture esterna....
															 // ora devo importarla in una tabella temporanea locale... per poi poterla joinare o fare altro...
				// ora prendo i dati in memoria e gli aggiungo alla nuova tabella temporanea nel db
				for (int i = 0; i < tbl.Rows.Count; i++) // for each no ???
				{
					s = "insert into utenti_tmp (cognome, nome, matricola, ente, telefono, cell, struttura_ek, struttura_cod, cod_stru, mail, sede, sede_indirizzo, sede_cap ) values (";
					s += "\'" + utenti.virgolette(tbl.Rows[i]["cognome"].ToString()) + "\', \'" + utenti.virgolette(tbl.Rows[i]["nome"].ToString()) + "\', ";
					s += "\'" + utenti.virgolette(tbl.Rows[i]["matricola"].ToString()) + "\', ";
					s += "\'" + utenti.virgolette(tbl.Rows[i]["descrizione_s"].ToString()) + "\', ";
					s += "\'" + utenti.virgolette(tbl.Rows[i]["telefono_1"].ToString()) + "\', \'" + utenti.virgolette(tbl.Rows[i]["cellulare"].ToString()) + " " + utenti.virgolette(tbl.Rows[i]["nr_breve"].ToString()) + "\', ";
					s += "\'" + utenti.virgolette(tbl.Rows[i]["codice_s_app"].ToString()) + "\', ";
					s += "\'" + utenti.virgolette(tbl.Rows[i]["codice_s_app"].ToString()) + "\', ";
					s += "\'" + utenti.virgolette(tbl.Rows[i]["Sede_lavoro"].ToString()) + "\', ";
					s += "\'" + utenti.virgolette(tbl.Rows[i]["E-mail"].ToString()) + "\', ";
					s += "\'" + utenti.virgolette(tbl.Rows[i]["minuscolo"].ToString()) + "\', \'" + utenti.virgolette(tbl.Rows[i]["xindirizzo"].ToString()) + "\', ";
					s += "\'" + utenti.virgolette(tbl.Rows[i]["xcap"].ToString()) + "\'";
					s += ")";
					FBConn.EsegueCmd(s, out msg);
					if (msg != "")
					{
						tStato.Text = "Attenzione... problema su " + msg;
						return;
					}
					// ok aggiunto utente provinciale
				}
				FBConn.closeaFBConn(out msg);
				tStato.Text = "OK. Caricati " + tbl.Rows.Count.ToString() + " dipendenti!";
				// ora ho sia le strutture aggiornate che gli utenti aggiornati.... vediamo cosa possiamo fare...
				// 1) partire da utenti... cercare la matricola (trasformata), se trovo allora confronto i dati e completo...
				// 2) se non trovo.... faccio file txt e dumpo matricole... non trovate
				// potrei anche fare una query che mi lega le due tabelle....
				s = "SELECT u.ID, u.ente as uente, ut.*, s.* ";
				s += "from utenti as u ";
				s += "left join utenti_tmp as ut on ut.matricola=u.matr ";
				s += "left join strutture as s on s.codice=ut.struttura_ek ";
				s += "where u.matr is not null and ut.matricola is not null ";
				s += "order by upper(u.matr) ";
				tbl.Clear();
				string ss = s; // verifica qry
				tbl = FBConn.getfromTbl(s, out msg);
				if (tbl != null && tbl.Rows.Count > 0) // vaicon i controlli
				{
					for (Int32 i = 0; i < tbl.Rows.Count; i++)
					{
						s = "update utenti set ";
						s += "matricola=\'PR" + tbl.Rows[i]["matricola"].ToString() + "\', ";
						s += "indirizzo=\'" + utenti.virgolette(tbl.Rows[i]["sede_indirizzo"].ToString()) + "\', ";						
						s += "cap=\'" + utenti.virgolette(tbl.Rows[i]["sede_cap"].ToString()) + "\', ";
						s += "citta=\'" + utenti.virgolette(tbl.Rows[i]["sede"].ToString()) + "\', ";
						if (tbl.Rows[i]["struttura_cod"].ToString() != "" && tbl.Rows[i]["struttura"].ToString() != "" &&
							tbl.Rows[i]["uente"].ToString() != tbl.Rows[i]["struttura"].ToString())
								s += "ente=\'" + utenti.virgolette(tbl.Rows[i]["struttura"].ToString()) + "\', ";
					    s += "struttura_ek=\'" + tbl.Rows[i]["struttura_ek"].ToString() + "\', ";
						s += "struttura_cod=\'" + tbl.Rows[i]["struttura_cod"].ToString() + "\', ";
						s += "cod_stru=\'" + tbl.Rows[i]["cod_stru"].ToString() + "\', ";
						s += "telefono=\'" + tbl.Rows[i]["telefono"].ToString() + "\', ";
						s += "cell=\'" + tbl.Rows[i]["cell"].ToString() + "\' ";
						s += "where matr=\'" + utenti.virgolette(tbl.Rows[i]["matricola"].ToString()) + "\' ";
						FBConn.EsegueCmd(s, out msg);
					}
				}
			}
		}
	}

	protected DataSet getdata(string select, DataSet dset, string strTableName, OleDbConnection conn)
	{
		long rr = -1;
		if (conn.State != ConnectionState.Open)
		{
			tStato.Text = string.Format("Error: Aprire la connessione al DB prima di chiamare getdata!");
			return (null);
		}
		try
		{
			da = new OleDbDataAdapter(select, conn);
			//dset.Clear();
			da.Fill(dset, strTableName);
			rr = dset.Tables[strTableName].Rows.Count;
		}
		catch (Exception ex)
		{
			tStato.Text = string.Format("Error: Failed to retrieve the required data from the DataBase.\n{0}", ex.Message);
			return (null);
		}
		finally
		{
			conn.Close();
		}
		return (dset);
	}

	protected OleDbConnection openaOleConn(string strconn)
	{
		// Set Access connection and select strings.
		// The path to BugTypes.MDB must be changed if you build 
		// the sample from the command line:
		OleDbConnection conn = null;
		try
		{
			conn = new OleDbConnection(strconn);
			conn.Open();
		}
		catch (Exception ex)
		{
			string tStato = ex.Message;
			return (conn);
		}
		return (conn);
	}

    protected void bhasha_Click(object sender, EventArgs e)
    {
        DataTable tbl = ds.Tables["utenti"]; // ho la tabella strutture esterna....
                                             // ora devo importarla in una tabella temporanea locale... per poi poterla joinare o fare altro...
        tbl = FBConn.getfromTbl("select * from utenti where not pwd is null", out msg);
        string s;
        if (tbl != null)
        {
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                s = tbl.Rows[i]["pwdhashed"] == null ? "" : tbl.Rows[i]["pwdhashed"].ToString().Trim();
                if (s == "")
                {
                    utenti.passwordhashed = utenti.haspwd(tbl.Rows[i]["pwd"].ToString());
                    s = "update utenti set pwdhashed=\'" + utenti.passwordhashed + "\' where id=\'" + tbl.Rows[i]["id"].ToString() + "\'";
                    int r = FBConn.EsegueCmd(s, out msg);
                }
            }
        }
    }
}