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
using System.IO;

public partial class importastrutture : System.Web.UI.Page
{
	public ConnessioneFB FBConn = new ConnessioneFB();
    public string Path = @"u:\get\strutture.csv";
    List<string[]> result = new List<string[]>();
    /*public static OleDbConnection Olecnn = null;
	public string strOleConn = "Microsoft Jet 4.0 OLE DB;"; // Data Source=S:/DatiGare/dati/schemas.mdb";
	public string strOleDataSource = "u:/get/strutture.mdb";
	*/
    /*public OleDbCommand c = new OleDbCommand();
	public OleDbDataAdapter da = new OleDbDataAdapter(); */
    public DataTable tbl = new DataTable();
    public DataSet da = new DataSet();
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
			tPercorso.Text = Path;
			//tData.Text = strOleDataSource;
			//tTabella.Text = "Strutture_i";
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
        string ss = "", cod = "";
        msg = ""; s = "";
        if (tPercorso.Text.Trim().Length <= 3) s = "Problema con percorso/nome file da importare; ";
        if (!File.Exists(tPercorso.Text))
            //if (tData.Text.Trim().Length <= 3) s += "Problema con Data source; ";
            //if (tTabella.Text.Trim().Length <= 3) s += "Problema con nome tabella;";
            //if ( s.Length >= 1 )
            tStato.Text = "File inesistene! Contattare l'assistenza al numero " + (string)Session["assistenza"];
        else
        {
            tStato.Text = "Inizio aggiornamento.....";
            /*string strconn = "Provider=" + tProvider.Text.Trim() + "Data source=" + tData.Text.Trim();
            Olecnn = openaOleConn(strconn);*/
            using (System.IO.StreamReader sr = new System.IO.StreamReader(tPercorso.Text))
            {
                while (!sr.EndOfStream)
                {
                    result.Add(sr.ReadLine().Split(new char[] { ';', '|' }));
                }
            }
            //ds = getdata("select * from " + tTabella.Text.Trim(), ds, "origine", Olecnn);
            idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
            if (result.Count > 0)
            {
                DataTable tbl = da.Tables["origine"]; // ho la tabella strutture esterna....
                                                      // ora devo importarla in una tabella temporanea locale... per poi poterla joinare o fare altro...
                FBConn.EsegueCmd("delete from strutturetmp", out msg);
                //s = "insert into strutturetmp (codice, struttura, mail, dtla, dtc, chi) ";
                //s += "select cod_s, XSTRUTTURA, e-mail, cast(now as timestamp), cast(now as timestamp), " + idu.ToString();
                //FBConn.EsegueCmd(s, out msg);
                // BUSINES LOGIC
                // 1) disattivo tutte le strutture presenti (E' meglio di no!)
                // 2) vedo se la struttura c'è già: se c'è ed è uguale devo attivarla
                //    se c'è ed è diversa ne aggiungo una nuova con la dicitura nuova e lo stesso codice (non va bene... perchè avrò il moltiplicarsi delle strutture e 
                //    non potrò più risalire allo storico, a meno che non scriva la data sulla vecchia di decadenza e sulla nuova di entrata in funzione
                //    devo anche essere sicuro di avere a portata di mano la tabella utenti e predisporre subito dopo l'aggiornamento! Altrimenti corro il
                //    rischio che un utente rimanga senza struttura attiva!
                //FBConn.EsegueCmd("update strutture set attiva = -1 where tipo_ek = 1", out msg);  // Solo strutture pat; attenzione agli enti non pat -> tagr
                ss = "";
                DateTime dt;
                for (int i = 0; i < result.Count; i++) // for each no ???
                {
                    //ss = (tbl.Rows[i]["cod_sup"] == DBNull.Value || tbl.Rows[i]["cod_sup"].ToString().Length <= 2) ? "\'\'" : "\'" + tbl.Rows[i]["cod_sup"].ToString() + "\'";
                    cod = (result[i][0]);
                    if (cod.Trim().Length > 3)
                    {
                        // ricavo la data
                        ss = virgolette(result[i][22]);
                        DateTime.TryParse(ss, out dt);
                        s = "insert into strutturetmp (codice, struttura, mail, dtla, dtc, chi, dipendeda_ek, data_nascita_giuridica, attiva) values (";
                        s += "\'" + cod + "\', \'" + virgolette((result[i][4])) + "\', ";
                        s += "\'" + (result[i][12]) + "\', cast(\'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp), cast(\'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp), ";
                        s += "\'" + idu.ToString() + "\', \'" + (result[i][8]) + "\', cast(\'" + dt.ToString("yyyy/MM/dd") + "\' as date), 1)";
                        FBConn.EsegueCmd(s, out msg);
                        if (msg != "")
                        {
                            tStato.Text = "Attenzione... problema su " + msg;
                            return;
                        }
                        // ok aggiunta nuova struttura.... ora vedo se c'è già nel db.. se non c'è devo aggiungerla.... e se cè la aggiorno
                        s = "select * from strutture ";
                        s += "where codice=\'" + cod + "\' and ";
                        s += "struttura = \'" + virgolette((result[i][4])) + "\' ";
                        s += "order by codice, dtla desc ";

                        if (esiste != null && esiste.Rows.Count > 0) esiste.Clear();
                        esiste = FBConn.getfromTbl(s, out msg);
                        if (esiste == null || esiste.Rows.Count <= 0) // nuova struttura ?????
                        {
                            s = "insert into strutture (codice, struttura, mail, dtla, dtc, chi, dipendeda_ek, data_nascita_giuridica, attiva) values (";
                            s += "\'" + cod + "\', \'" + virgolette((result[i][4])) + "\', ";
                            s += "\'" + (result[i][12]) + "\', cast(\'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp), cast(\'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp), ";
                            s += "\'" + idu.ToString() + "\', \'" + (result[i][8]) + "\', cast(\'" + dt.ToString("yyyy/MM/dd") + "\' as date), 1)";
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
                                s = "update strutture set ";
                                s += "mail = \'" + virgolette((result[i][12])).Trim() + "\', ";
                                s += "dtla = cast(\'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\' as timestamp), ";
                                s += "chi =\'" + idu.ToString() + "\', ";
                                s += "dipendeda_ek = \'" + (result[i][8]) + "\', ";
                                s += "data_nascita_giuridica = cast(\'" + dt.ToString("yyyy/MM/dd") + "\' as date), ";
                                s += "attiva = 1 ";
                                s += "where id=\'" + esiste.Rows[0]["id"].ToString() + "\'";
                                FBConn.EsegueCmd(s, out msg);
                                if (msg != "") { tStato.Text = msg; return; }
                            }
                        }
                    }
                }
                tStato.Text = "OK. Analizzate ed inserite " + result.Count.ToString() + " strutture!";
                FBConn.EsegueCmd("delete from strutturetmp", out msg);  // piallo la tabella strutturetmp
            }
        }
    }
	protected void bImportuser_Click(object sender, EventArgs e)
	{
        checkSession();
        msg = ""; string s = tPercorso.Text;
        /*if (tProvider.Text.Trim().Length <= 3) s = "Problema con Provider; ";
        if (tData.Text.Trim().Length <= 3) s += "Problema con Data source; ";
        if (tTabella.Text.Trim().Length <= 3) s += "Problema con nome tabella;";*/
        if (s.Length <= 5)
            tStato.Text = s + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
        else
        {
            // leggo dati tabella .csv
            //ds = getdata("select a.*, s.descrizione_s from anagrafica_i as a left join strutture_i as s on s.cod_s=a.codice_s_app ", ds, "usercallcenter", Olecnn);
            using (System.IO.StreamReader sr = new System.IO.StreamReader(tPercorso.Text))
            {
                result.Clear();
                while (!sr.EndOfStream)
                {
                    result.Add(sr.ReadLine().Split(new char[] { ';', '|' }));
                }
            }
            FBConn.closeaFBConn(out msg);
            idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
            if (result.Count > 0)
            {
                // cancello eventuale tabella presente nel db di destinazione
                FBConn.EsegueCmd("drop table utenti_tmp", out msg);
                FBConn.closeaFBConn(out msg);
                // la ricreo ex-novo
                s = "CREATE TABLE UTENTI_TMP (PWD Varchar(20), POWER Integer DEFAULT 0, MAIL Varchar(50), CELL Varchar(40), NOME Varchar(30), ";
                s += "COGNOME Varchar(40), DATACREAZIONE Timestamp DEFAULT current_timestamp, NIKNAME Varchar(33), IP Varchar(15), ";
                s += "DTLA Timestamp DEFAULT current_timestamp, FORZOCAMBIOPASSWORD Smallint, MATRICOLA Varchar(16), INDIRIZZO Varchar(55), ABILITATO Smallint, ";
                s += "CIVICO Varchar(22), CAP Varchar(15), CITTA Varchar(50), TELEFONO Varchar(40), TIPODOC Varchar(50), NUMDOC Varchar(50), SCADENZA Date, ";
                s += "ENTE Varchar(150), COD_STRU_SUP Varchar(5), STRUTTURA_EK Varchar(10), SCADENZA_PATENTE Date, ATTIVO Smallint, ";
                s += "sede varchar(150), sede_indirizzo varchar(150), sede_cap varchar(8) ) ";

                FBConn.EsegueCmd(s, out msg);

                //DataTable tbl = ds.Tables["usercallcenter"]; // ho la tabella strutture esterna....
                // ora devo importarla in una tabella temporanea locale... per poi poterla joinare o fare altro...
                // ora prendo i dati in memoria e gli aggiungo alla nuova tabella temporanea nel db
                string cod = "";
                for (int i = 0; i < result.Count; i++) // for each no ???
                {
                    s = "insert into utenti_tmp (cognome, nome, matricola, telefono, cell, struttura_ek, ente, cod_stru_sup, mail, sede, sede_indirizzo, sede_cap ) ";
                    s += "select ";
                    s += "\'" + virgolette(result[i][1].Trim()) + "\', ";
                    s += "\'" + virgolette(result[i][2].Trim()) + "\', ";
                    s += "\'" + virgolette(result[i][0].Trim()) + "\', ";
                    s += "\'" + virgolette(result[i][4].Trim()) + "\', ";
                    s += "\'" + virgolette(result[i][14].Trim()) + "\', ";
                    s += "\'" + virgolette(result[i][9].Trim()) + "\', ";
                    s += "struttura, dipendeda_ek, ";
                    s += "\'" + virgolette(result[i][8].Trim()) + "\', ";
                    s += "\'" + virgolette(result[i][16].Trim()) + "\', ";
                    s += "\'" + virgolette(result[i][17].Trim()) + "\',";
                    s += "\'" + virgolette(result[i][18].Trim()) + "\' ";
                    s += "from strutture where codice=\'" + virgolette(result[i][9].Trim()) + "\' and attiva=1 ";
                    FBConn.EsegueCmd(s, out msg);
                    if (msg != "")
                    {
                        tStato.Text = "Attenzione... problema su " + msg;
                        return;
                    }
                    // ok aggiunto utente provinciale
                }
                FBConn.closeaFBConn(out msg);
                tStato.Text = "OK. Caricati " + result.Count.ToString() + " dipendenti!";
                // ora ho sia le strutture aggiornate che gli utenti aggiornati.... vediamo cosa possiamo fare...
                // 1) partire da utenti... cercare la matricola (trasformata), se trovo allora confronto i dati e completo...
                // 2) se non trovo.... faccio file txt e dumpo matricole... non trovate
                // in alternativa.... potrei anche fare una query che mi lega le due tabelle....
                s = "SELECT u.ID, u.ente as uente, ut.*, s.* ";
                s += "from utenti as u ";
                s += "left join utenti_tmp as ut on ut.matricola=u.matr ";
                s += "left join strutture as s on s.codice=ut.struttura_ek ";
                s += "where u.matr is not null and ut.matricola is not null ";
                s += "order by upper(u.matr) ";
                //tbl.Clear();
                string ss = s; // verifica qry
                DataTable tbl = FBConn.getfromTbl(s, out msg);
                user utenti = new user();
                if (tbl != null && tbl.Rows.Count > 0) // vaicon i controlli
                {
                    for (Int32 i = 0; i < tbl.Rows.Count; i++)
                    {
                        // attenzione.... qui viene piallato tutto! Tell, Cell. Varrebeb la pena fare aggiornamento dei dati giuridici ed eventualmente aggiornare dove vuoti
                        // quindi controllare se in utenti maca dato o è inconsistente per i campi di contorno.... e proporre update differenti
                        s = "update utenti set ";
                        s += "matricola=\'PR" + tbl.Rows[i]["matricola"].ToString() + "\', ";
                        s += "indirizzo=\'" + utenti.virgolette(tbl.Rows[i]["sede_indirizzo"].ToString()) + "\', ";
                        s += "cap=\'" + utenti.virgolette(tbl.Rows[i]["sede_cap"].ToString()) + "\', ";
                        s += "citta=\'" + utenti.virgolette(tbl.Rows[i]["sede"].ToString()) + "\', ";
                        if (tbl.Rows[i]["struttura_ek"].ToString() != "" && tbl.Rows[i]["struttura"].ToString() != "" &&
                            tbl.Rows[i]["uente"].ToString() != tbl.Rows[i]["struttura"].ToString())
                            s += "ente=\'" + utenti.virgolette(tbl.Rows[i]["struttura"].ToString()) + "\', ";
                        s += "struttura_ek=\'" + tbl.Rows[i]["struttura_ek"].ToString() + "\', ";
                        s += "ente=\'" + tbl.Rows[i]["ente"].ToString() + "\', ";
                        s += "cod_stru=\'" + tbl.Rows[i]["dipendeda_ek"].ToString() + "\', ";
                        s += "telefono=\'" + tbl.Rows[i]["telefono"].ToString() + "\', ";
                        s += "cell=\'" + tbl.Rows[i]["cell"].ToString() + "\' ";
                        s += "where matr=\'" + utenti.virgolette(tbl.Rows[i]["matricola"].ToString()) + "\' ";
                        FBConn.EsegueCmd(s, out msg);
                        if (tbl.Rows[i]["matricola"].ToString() == "30866")
                            s = "";
                    }
                }
                FBConn.EsegueCmd("delete from utenti_tmp", out msg);
            }
        }
    }
    protected string togli_virgolette(string s) // tolgo la virgolette finali e iniziali
    {
        string ss = "";
        string virgoletta = "'";        
        string doppie = string.Format("{0}", "\"");
        if ( s.Length > 2)
        {
            if (s[s.Length - 1] == doppie[0])
                ss = s.Substring(0, s.Length - 1);
            if (s[0] == doppie[0])
                ss = ss.Substring(1);
        }
        return (ss);
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
			//da = new OleDbDataAdapter(select, conn);
			
			//da.Fill(dset, strTableName);
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
        DataTable tbl = da.Tables["utenti"]; // ho la tabella strutture esterna....
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