/**

 * 
 * Copyright (C) 2017 Provincia Autonoma di Trento
 *
 * This file is part of <Prenotazioni auto di servizio>.
 * Pitre is free software: you can redistribute it and/or modify
 * it under the terms of the LGPL as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Pitre is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the LGPL v. 3
 * along with Pitre. If not, see <https://www.gnu.org/licenses/lgpl.html>.
 * 
 */

using System;
using System.Net.Mail;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using System.Net.Mime;
using System.Net;
using System.Security.Cryptography;
using System.Text;

public class Manu  // Manutenzione - messagi
{
    // proprietà
    private int _stato; 
    
    // area usata solo per motivi interni alla classe
    public static FbConnection cAFbConn = null;
    public ConnessioneFB FBClass = new ConnessioneFB();
    private FbDataAdapter cda = new FbDataAdapter();
    public FbCommand cc = new FbCommand();
    private DataSet cds = new DataSet();
    private DataTable tbl = new DataTable();

    // metodi standard;
    public int stato { get { return _stato; } set { _stato = value; } }

    // metodi
    public DataTable Messaggi(int tipo)
    {
        string msg = "";
        string s = "SELECT * FROM manutenzione where abilitato=1 and tipo= " + tipo.ToString();

        // da questa qry possono arrivare + records
        msg = "";
        //cAFbConn = FBClass.openaFBConn(out msg);
        //if (cAFbConn.State == ConnectionState.Closed) { cAFbConn.Open(); }
        cds.Clear();
        tbl = FBClass.getfromTbl(s, out msg);  // carico dati utente
        //cAFbConn.Close();         
        return (tbl);
    }
    public string Verifica(int tipo)
    {
        string s = "";
        tbl = this.Messaggi(tipo);
        if (tbl.Rows.Count == 1)
        {
            switch (tbl.Rows[0]["tipo"].ToString())
            {
                case "1": // ritorna msg
                    s = tbl.Rows[0]["msg"].ToString();
                    break;
                case "2": // indirizza su pagina
                    s = tbl.Rows[0]["pagina"].ToString();
                    break;
            }
        }
        return (s);
    }
}

public class user : password // eredito la classe password
{
    // proprietà
    private Int32 _iduser;
    private int _potere;
    private string _nome;
    private string _cognome;
    private string _nikname;
    private string _password;
    private string _passwordhashed;
    private string _ip;
    private DateTime _lastaccess;
    private DateTime _scadenza;
    private bool _giornalizza;
    private string _mail;
    private bool _forzocambiopassword;
    private bool _abilitato;
    private string _ente;
    private string _struttura_ek;
    private string _ente_ek;
    private string _struttura;
	private string _struttura_cod;
    private string _dipendeda_ek;
    private string _indirizzo;
    private string _civico;
    private string _cap;
    private string _città;
    private string _telefono;
    private string _cell;
    private string _tipodoc;
    private string _numdoc;
    private string _matricola;
    private DateTime _scadenza_patente;
    private string _prepas = "24!";
    private string _postpas = "tin@232";
    public string msg; // mi serve per i messaggi di ritorno;
    public SHA256 cripto = SHA256.Create();

    // area usata solo per motivi interni alla classe
    public  FbConnection cAFbConn = null;
    public ConnessioneFB FBClass = new ConnessioneFB();
    private FbDataAdapter cda = new FbDataAdapter();
    public FbCommand cc = new FbCommand();
    private DataSet cds = new DataSet();
    private DataTable tbl = new DataTable();
	private www CLogga = new www();

    // metodi;
    public Int32 iduser { get { return _iduser; } set { _iduser = value; } }
    public int potere { get { return _potere; } set { _potere = value; } }
    public string nome { get { return _nome; } set { _nome = value; } }
    public string cognome { get { return _cognome; } set { _cognome = value; } }
    public string nikname { get { return _nikname; } set { _nikname = value; } }
    public string password { get { return _password; } set { _password = value; } }
    public string passwordhashed { get { return _passwordhashed; } set { _passwordhashed = value; } }
    public string ip { get { return _ip; } set { _ip = value; } }
    public DateTime lastaccess { get { return _lastaccess; } set { _lastaccess = value; } }
    public DateTime scadenza { get { return _scadenza; } set { _scadenza = value; } }
    public bool giornalizza { get { return _giornalizza; } set { _giornalizza = value; } }
    public string mail { get { return _mail; } set { _mail = value; } }
    public bool forzocambiopassword { get { return _forzocambiopassword; } set { _forzocambiopassword = value; } }
    public bool abilitato { get { return _abilitato; } set { _abilitato = value; } }
    public string indirizzo { get { return _indirizzo; } set { _indirizzo = value; } }
    public string civico { get { return _civico; } set { _civico = value; } }
    public string cap { get { return _cap; } set { _cap = value; } }
    public string città { get { return _città; } set { _città = value; } }
    public string tipodoc { get { return _tipodoc; } set { _tipodoc = value; } }
    public string numdoc { get { return _numdoc; } set { _numdoc = value; } }    
    public string matricola { get { return _matricola; } set { _matricola = value; } }
    public string telefono { get { return _telefono; } set { _telefono = value; } }
    public string cell { get { return _cell; } set { _cell = value; } }
    public string struttura_ek { get { return _struttura_ek; } set { _struttura_ek = value; } }
	public string struttura { get { return _struttura; } set { _struttura = value; } }
	public string struttura_cod { get { return _struttura_cod; } set { _struttura_cod = value; } }
    public string dipendeda_ek { get { return _dipendeda_ek; } set { _dipendeda_ek = value; } }
    public string ente { get { return _ente; } set { _ente = value; } }
    public string ente_ek { get { return _ente_ek; } set { _ente_ek = value; } }
    public DateTime scadenzapassword { get { return _scadenza; } set { _scadenza = value; } }
    public DateTime scadenza_patente { get { return _scadenza_patente; } set { _scadenza_patente = value; } }
    
    // costruttori
    public user() // costruttore semplice
    {
        msg = "";   
    }
    public user(int id) // precarica tutti i dati dell'id, se c'è
    {
        bool esiste = cercaid(id);
    }
    public user(string nikname, string password) // precarica tutti i dati dell'nikname, se c'è
    {
        msg = "";
        bool esiste = cercanikname(nikname, password);
    }

    // metodi
    public bool cercaid(long numerouser)
    {
        return (cerca(numerouser, null, null, null, null, null, true));
    }
    public bool cercanikname(string nikname, string password)
    {
        return (cerca(-1, null, null, nikname, password, null, true));
    }
    public bool cercamatricola(string matricola)
    {
        return (cerca(-1, null, null, null, null, matricola, true));
    }
    public bool niknamecè(string nikname)
    {
        return (verifica(-1, null, null, nikname, null));
    }
    public DataTable cercanomecognome(string nome, string cognome, bool tutti)
    {
        bool ok = cerca(-1, nome, cognome, null, null, null, tutti);
        return (cds.Tables["utente"]); // ritorno l'intera tabella con i risultati (omonimie)
    }
    public bool dimenticatolapassword(string nn, out string er) // nn = nikname
    {
        er = "";
        string sSelect;
        string where;
        if (nn.Trim().Length < 5)
        {
            er = "Inserire Username valido!";
            return (false);
        }
        where = " b.ATTIVA = 1 and nikname = \'" + nn.Trim() + "\'";

        sSelect = "SELECT a.*, b.struttura, b.dipendeda_ek FROM UTENTI a left join strutture b on a.struttura_ek=b.codice ";
		sSelect += "where " + where;
        msg = "";
        cAFbConn = FBClass.openaFBConn(out msg);
        if (cAFbConn.State == ConnectionState.Closed) { cAFbConn.Open(); }
        this.nikname = nn;
        cds.Clear();
        bool risultato = getFBdata(sSelect, cds, "utente", cAFbConn) == 1 ? true : false ;  // carico dati utente
        cAFbConn.Close();
        if (!risultato)
        {
            er = "Username non ancora abilitato, non valido o utente non incardinato nella struttura corretta! Inserire Username valido e riprovare.";
            return (false);
        }
        gmail gm = new gmail();
		//string pwd this.CalcolaPasswordCasuale(8, 3, 3, 3);
		string pwd = CalcolaPassword(this.nome, this.cognome, this.lastaccess);
        // OK ora   1) inoltro la password via mail; 2) aggiorno il campo cambio password;
        string[] chi = { this.mail }; // in questa maniera definisco il numero di elementi dell'array
        gm.achi = chi;
        string[] ccn = { "tiziano.donati@provincia.tn.it" };
        //gm.achiccn = ccn;
		gm.subject = "Assegnazione nuova password per accesso Prenotazione Auto di Servizio";
        gm.body = "Buongiorno gentile " + this.nome + " " + this.cognome + ",\n\n";
        gm.body += "Le consegnamo le nuove credenziali di accesso, pregandoLa di inserirle esattamente come sono, rispettando le maiuscole/minuscole.\n";
        gm.body += "La password dovra\' essere necessariamente cambiata al primo accesso.\n\n";
        gm.body += string.Format("USERNAME :\t{0}\n", nikname);
        gm.body += string.Format("Password :\t{0}\n\n", pwd);
        gm.body += "Le ricordiamo che la password per l'accesso all\'applicazioni \"Prenotazione autoveicoli di servizio\", deve essere formata da almeno 8 caratteri, deve contenere almeno una lettera maiscola e una minuscola, deve contenere almeno un numero o un segno di punteggiatura.\n\n";
        gm.body += "Cordiali saluti.\n\n\n";
        gm.body += "La presente mail e\' stata inviata da un sistema automatizzato. Non saranno prese in considerazione mail di sisposta.\n\n\n";
        gm.numeritel = "0461 - 496415";
        bool spedita = gm.mandamail("", 0, "", "", out msg);       
        if (!spedita)
            er = "Inoltro password fallito. Contattare l'assistenza al numero " + gm.numeritel;
        else
        {
            // ora devo sostituire la password dell'utente e chiedere il cambio password 
            if (cAFbConn.State == ConnectionState.Closed)
            {
                cAFbConn.Open();
            }
            // arrichiesco la password e la hasho
            string pwdhashed = GetHash(cripto, (_prepas + pwd + _postpas)); //HashAlgorithm hashAlgorithm, string input
            sSelect = "UPDATE Utenti SET pwd = \'" + pwd + "\', pwdhashed = \'" + pwdhashed + "\',  forzocambiopassword = 1, dtla = '" + string.Format("{0:MM/dd/yyyy}", DateTime.Now) + "' WHERE id = " + this.iduser.ToString();
            cAFbConn = FBClass.openaFBConn(out msg);
            if (cAFbConn.State == ConnectionState.Closed) { cAFbConn.Open(); }
            //cc = new SqlCommand(sSelect, cAConn);
            cc.CommandText = sSelect;
            cc.Connection = cAFbConn;
            cda.UpdateCommand = cc;
            long rr = cda.UpdateCommand.ExecuteNonQuery();  // aggiorno nuova password           
            cAFbConn.Close();
            if ( rr > 0 )
                er = "A breve riceverà le nuove credenziali via e-mail.";
            else
                er = "A breve riceverà le nuove credenziali via e-mail. Ma non sono stati aggiornati i dati dell'utente! Avvertire l'amministratore.";
        }
        return (true);
    }
    public string haspwd(string pwd)
    {
        // arrichiesco la password e la hasho
        string pwdhashed = GetHash(cripto, (_prepas + pwd + _postpas));
        return (pwdhashed);
    }
    public bool cambiopassword(long id) // cerco l'utente e cambio password, registrando la nuova e azzerando il flag forzocambiopassword
    {
        return (true); // ok pwd cambiata
    }
    public bool cerca(long id, string nome, string cognome, string nikname, string pwd, string matricola, bool tutti)
    {
        string sSelect;
        bool risultato = false;
        string where = " b.attiva=1 ";

        if (id > 0)
        {
            where += " and a.id = " + id;
        }
        else
        {
            if ((cognome != null && cognome.Trim().Length > 0) && (nome != null && nome.Trim().Length > 0))
            {
                where += " and upper(a.nome) = \'" + nome.ToUpper() + "\' and upper(a.cognome) = \'" + cognome.ToUpper() + "\'";
            }
            else
            {
                if (nikname != null && nikname.Trim().Length > 0)
                {
                    where += " and upper(a.nikname) = \'" + nikname.ToUpper() + "\'";
                    if (pwd != null && pwd.Trim().Length > 0)
                        where += " and a.pwdhashed = \'" + GetHash(cripto, (_prepas + pwd + _postpas)) + "\'";
                }
                else
                    if (matricola != null && matricola.Trim().Length > 0)
                        where += "and upper(a.matricola) = \'" + matricola.ToUpper() + "\'";
                    else
                        return (false);
            }
        }
        if (!tutti)
            where += (where.Trim().Length > 0 ? " and " : "") + " a.abilitato=1 ";

        sSelect = "SELECT a.*, b.struttura, b.dipendeda_ek FROM UTENTI a ";
        sSelect += "left join strutture b on b.codice=a.struttura_ek ";
        sSelect += "left join enti as e on e.id=a.ente_ek ";
        sSelect += "where " + where;

        // da questa qry possono arrivare + records
        msg = "";
        cAFbConn = FBClass.openaFBConn(out msg);
        if (cAFbConn.State == ConnectionState.Closed) { cAFbConn.Open(); }
        cds.Clear();
        risultato = getFBdata(sSelect, cds, "utente", cAFbConn) == 1 ? true : false;  // carico dati utente
        cAFbConn.Close();
        return (risultato);
    }
    public bool verifica(long id, string nome, string cognome, string nikname, string pwd)
    {   
        string sSelect;
        string where, msg;
        Int32 rr = -1;
       
        if (id > 0)
        {
            where = "id = " + id;
        }
        else
        {
            if ((cognome != null && cognome.Trim().Length > 0) && (nome != null && nome.Trim().Length > 0))
            {
                where = "nome = \'" + nome + "\' and cognome = \'" + cognome + "\'";
            }
            else
            {
                if (nikname != null && nikname.Trim().Length > 0) 
                {
                    where = "nikname = \'" + nikname + "\'";
                    if ( pwd != null && pwd.Trim().Length > 0)
                        where += " and pwd = \'" +  pwd + "\'";
                    //DAda.SelectCommand.Parameters.AddWithValue("@pnikname", nikname);
                    //DAda.SelectCommand.Parameters.AddWithValue("@ppwd", password);
                }
                else
                    return (false);
            }
        }

        sSelect = "SELECT * ";
        sSelect += "from utenti ";
        sSelect += "where " + where;

        // da questa qry possono arrivare + records
        msg = "";
        FBClass.openaFBConn(out msg);
        if (FBClass.State() != ConnectionState.Open) { cAFbConn.Open(); }
        cds.Clear();

        // (string comandotxt, FbConnection cnn, out string message)
        msg = "";
        rr = FBClass.EsegueCmd(sSelect, out msg);  // verifico se c'è l'utente senza caricare i dati
        
        return (rr > 0 ? true : false);
    }
    private OleDbConnection openaConn(string strconn)
    {
        OleDbConnection conn = null;
        try
        {
            conn = new OleDbConnection(strconn);
            conn.Open();
        }
        catch (Exception ex)
        {
            Console.Write("Exception: {0}", ex);
        }
        return (conn); // conn può essere valida o meno
    }
    public long getFBdata(string select, DataSet dset, string strTableName, FbConnection fbconn)
    {
        long rr = -1;
        try
        {   FbDataAdapter cda = new FbDataAdapter(select, fbconn);
            cda.Fill(cds, strTableName);
            rr = cds.Tables[strTableName].Rows.Count;
            cAFbConn.Close();
        }
        catch (Exception ex)
        {
            Console.Write("Exception: {0}", ex);
            cAFbConn.Close();
            return (rr);
        }

        // leggo il record
        if (rr == 1)
        {
            Int32 ii;
            Int32.TryParse(cds.Tables[strTableName].Rows[0]["power"].ToString(), out ii); potere = ii;
            Int32.TryParse(cds.Tables[strTableName].Rows[0]["id"].ToString(), out ii); iduser = ii;
            nome = cds.Tables[strTableName].Rows[0]["nome"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["nome"].ToString() : "";
            cognome = cds.Tables[strTableName].Rows[0]["cognome"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["cognome"].ToString() : "";
            nikname = cds.Tables[strTableName].Rows[0]["nikname"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["nikname"].ToString() : "";
            password = cds.Tables[strTableName].Rows[0]["pwd"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["pwd"].ToString() : "";
            passwordhashed = cds.Tables[strTableName].Rows[0]["pwdhashed"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["pwdhashed"].ToString() : "";
            ip = cds.Tables[strTableName].Rows[0]["ip"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["ip"].ToString() : "";
            lastaccess = cds.Tables[strTableName].Rows[0]["DTLA"] != DBNull.Value ? Convert.ToDateTime(cds.Tables[strTableName].Rows[0]["DTLA"]) : DateTime.Now; // vedi website3 accessi
            giornalizza = cds.Tables[strTableName].Rows[0]["giornalizza"] != DBNull.Value ? Convert.ToBoolean(cds.Tables[strTableName].Rows[0]["giornalizza"]) : false;
            mail = cds.Tables[strTableName].Rows[0]["mail"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["mail"].ToString() : "";
            forzocambiopassword = cds.Tables[strTableName].Rows[0]["forzocambiopassword"] != DBNull.Value ? Convert.ToBoolean(cds.Tables[strTableName].Rows[0]["forzocambiopassword"]) : true;
            abilitato = cds.Tables[strTableName].Rows[0]["abilitato"] != DBNull.Value ? Convert.ToBoolean(cds.Tables[strTableName].Rows[0]["abilitato"]) : false;
            matricola = cds.Tables[strTableName].Rows[0]["matricola"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["matricola"].ToString() : "";
            indirizzo = cds.Tables[strTableName].Rows[0]["indirizzo"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["indirizzo"].ToString() : "";
            civico = cds.Tables[strTableName].Rows[0]["civico"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["civico"].ToString() : "";
            cap = cds.Tables[strTableName].Rows[0]["cap"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["cap"].ToString() : "";
            città = cds.Tables[strTableName].Rows[0]["citta"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["citta"].ToString() : "";
            struttura_ek = cds.Tables[strTableName].Rows[0]["struttura_ek"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["struttura_ek"].ToString() : "";
			struttura_cod = cds.Tables[strTableName].Rows[0]["struttura_cod"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["struttura_cod"].ToString() : "";
			struttura = cds.Tables[strTableName].Rows[0]["struttura"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["struttura"].ToString() : "";
			ente = cds.Tables[strTableName].Rows[0]["ente"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["ente"].ToString() : "";
            telefono = cds.Tables[strTableName].Rows[0]["telefono"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["telefono"].ToString() : "";
            cell = cds.Tables[strTableName].Rows[0]["cell"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["cell"].ToString() : "";
            tipodoc = cds.Tables[strTableName].Rows[0]["tipodoc"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["tipodoc"].ToString() : "";
            numdoc = cds.Tables[strTableName].Rows[0]["numdoc"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["numdoc"].ToString() : "";
            scadenza_patente = cds.Tables[strTableName].Rows[0]["scadenza_patente"] != DBNull.Value ? Convert.ToDateTime(cds.Tables[strTableName].Rows[0]["scadenza_patente"]) : DateTime.Now.AddDays(-1);            //DateTime dt;
            //DateTime.TryParse(cds.Tables[strTableName].Rows[0]["scadenza"].ToString(), out dt);
            scadenza = cds.Tables[strTableName].Rows[0]["scadenza"] != DBNull.Value ? Convert.ToDateTime(cds.Tables[strTableName].Rows[0]["scadenza"].ToString()) : Convert.ToDateTime("01/01/2900");
            //struttura_ek = cds.Tables[strTableName].Rows[0]["ente_ek"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["ente_ek"].ToString() : "";
            dipendeda_ek = cds.Tables[strTableName].Rows[0]["dipendeda_ek"] != DBNull.Value ? cds.Tables[strTableName].Rows[0]["dipendeda_ek"].ToString() : "";
        }
        //cAFbConn.Close();
        return (rr);
    }
    public long LeggiUtente(string idu)
    {
        long rr = -1;
        string s = "SELECT a.*, b.struttura, b.dipendeda_ek, b.attiva FROM UTENTI a ";
        s += "left join strutture b on a.struttura_ek=b.codice ";
        s += "where b.attiva=1 and a.id=\'" + idu.Trim() + "\'";
        try
        {   
            msg = "";
            cAFbConn = FBClass.openaFBConn(out msg);
            if (cAFbConn.State == ConnectionState.Closed) { cAFbConn.Open(); }
            FbDataAdapter cda = new FbDataAdapter(s, cAFbConn);
			cds.Tables.Clear();
			cda.Fill(cds, "utente");
            rr = cds.Tables["utente"].Rows.Count;
            cAFbConn.Close();
        }
        catch (Exception ex)
        {
            Console.Write("Exception: {0}", ex);
            cAFbConn.Close();
            msg = ex.ToString();
            return (rr);
        }

        // leggo il record
        if (rr == 1)
        {
            Int32 ii;
            Int32.TryParse(cds.Tables["utente"].Rows[0]["power"].ToString(), out ii); potere = ii;
            Int32.TryParse(cds.Tables["utente"].Rows[0]["id"].ToString(), out ii); iduser = ii;
            nome = cds.Tables["utente"].Rows[0]["nome"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["nome"].ToString() : "";
            cognome = cds.Tables["utente"].Rows[0]["cognome"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["cognome"].ToString() : "";
            nikname = cds.Tables["utente"].Rows[0]["nikname"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["nikname"].ToString() : "";
            password = cds.Tables["utente"].Rows[0]["pwd"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["pwd"].ToString() : "";
            passwordhashed = cds.Tables["utente"].Rows[0]["pwdhashed"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["pwdhashed"].ToString() : "";
            ip = cds.Tables["utente"].Rows[0]["ip"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["ip"].ToString() : "";
            lastaccess = cds.Tables["utente"].Rows[0]["DTLA"] != DBNull.Value ? Convert.ToDateTime(cds.Tables["utente"].Rows[0]["DTLA"]) : DateTime.Now; // vedi website3 accessi
            giornalizza = cds.Tables["utente"].Rows[0]["giornalizza"] != DBNull.Value ? Convert.ToBoolean(cds.Tables["utente"].Rows[0]["giornalizza"]) : false;
            mail = cds.Tables["utente"].Rows[0]["mail"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["mail"].ToString() : "";
            forzocambiopassword = cds.Tables["utente"].Rows[0]["forzocambiopassword"] != DBNull.Value ? Convert.ToBoolean(cds.Tables["utente"].Rows[0]["forzocambiopassword"]) : true;
            abilitato = cds.Tables["utente"].Rows[0]["abilitato"] != DBNull.Value ? Convert.ToBoolean(cds.Tables["utente"].Rows[0]["abilitato"]) : false;
            matricola = cds.Tables["utente"].Rows[0]["matricola"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["matricola"].ToString() : "";
            indirizzo = cds.Tables["utente"].Rows[0]["indirizzo"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["indirizzo"].ToString() : "";
            civico = cds.Tables["utente"].Rows[0]["civico"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["civico"].ToString() : "";
            cap = cds.Tables["utente"].Rows[0]["cap"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["cap"].ToString() : "";
            città = cds.Tables["utente"].Rows[0]["citta"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["citta"].ToString() : "";
			struttura_ek = cds.Tables["utente"].Rows[0]["struttura_ek"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["struttura_ek"].ToString() : "";
			struttura_cod = cds.Tables["utente"].Rows[0]["struttura_cod"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["struttura_cod"].ToString() : "";
			struttura = cds.Tables["utente"].Rows[0]["struttura"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["struttura"].ToString() : "";
            ente = cds.Tables["utente"].Rows[0]["ente"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["ente"].ToString() : "";
            telefono = cds.Tables["utente"].Rows[0]["telefono"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["telefono"].ToString() : "";
            cell = cds.Tables["utente"].Rows[0]["cell"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["cell"].ToString() : "";
            tipodoc = cds.Tables["utente"].Rows[0]["tipodoc"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["tipodoc"].ToString() : "";
            numdoc = cds.Tables["utente"].Rows[0]["numdoc"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["numdoc"].ToString() : "";
            scadenza = cds.Tables["utente"].Rows[0]["scadenza"] != DBNull.Value ? Convert.ToDateTime(cds.Tables["utente"].Rows[0]["scadenza"].ToString()) : Convert.ToDateTime("01/01/1900");
			scadenza_patente = cds.Tables["utente"].Rows[0]["scadenza_patente"] != DBNull.Value ? Convert.ToDateTime(cds.Tables["utente"].Rows[0]["scadenza_patente"]) : DateTime.Now.AddDays(-1);
            //struttura_ek = cds.Tables["utente"].Rows[0]["ente_ek"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["ente_ek"].ToString() : "";
            dipendeda_ek = cds.Tables["utente"].Rows[0]["dipendeda_ek"] != DBNull.Value ? cds.Tables["utente"].Rows[0]["dipendeda_ek"].ToString() : "";
        }
        //cAFbConn.Close();
        return (rr);
    }
    public void clearuser() // inizializza dati della classe utente
    {
        nome = ""; cognome = ""; potere = -1;nikname = "";password = "";ip = "";
        lastaccess = DateTime.Now; giornalizza = false; mail = ""; forzocambiopassword = false;
        abilitato = false;matricola = "";  indirizzo = ""; civico = ""; cap = ""; città = "";
    }
    public bool aggiungiutente()
    {
        bool ok = false;
        if (iduser < 0)
            return (ok);
        // ,  forzocambiopassword=\"" + forzocambiopassword + "\", abilitato=\"" + abilitato + "\""
        // nikname, pwd, id, power, dtla, mail, nome, cognome, forzocambiopassword, abilitato, giornalizza, matricola, ip, indirizzo, civico, cap, citta        
        passwordhashed = GetHash(cripto, (_prepas + password + _postpas)); // cripro la pwd a 256 bit
        string sAdd = "insert into utenti (nikname, pwd, pwdhashed, power, nome, cognome, forzocambiopassword, abilitato, giornalizza, matricola, mail, ente, struttura_ek, struttura_cod, indirizzo, civico, cap, citta, telefono, cell, tipodoc, numdoc, scadenza, scadenza_patente, ente_ek) ";
        sAdd += "Values (\'" + nikname + "\', \'" + password + "\',  \'" + passwordhashed + "\', \'" + potere.ToString() + "\', \'" + nome + "\', \'" + cognome + "\', " + (forzocambiopassword ? "\'1\'" : "\'0\'") + ", " + (abilitato ? "\'1\'" : "\'0\'") + ", " + (giornalizza ? "\'1\'" : "\'0\'");
        sAdd +=", \'" + matricola + "\', \'" + mail + "\', \'"+ virgolette(ente) + "\', \'"+struttura_ek + "\', \'"+struttura_cod + "\', \'" + indirizzo + "\', \'" + civico + "\', \'" + cap + "\', \'" + città + "\'";
		sAdd += ", \'"+ virgolette(telefono) + "\', \'" + virgolette(cell) + "\', \'" + tipodoc + "\', \'" + numdoc + "\', " + string.Format("cast(\'{0:yyyy-MM-dd}\' as date)", scadenza) + ", " + string.Format("cast(\'{0:yyyy-MM-dd}\' as date)", scadenza_patente) + ", 1) returning id ";
        
        //cAFbConn = FBClass.openaFBConn(out msg);
        //if (cAFbConn.State == ConnectionState.Closed) { cAFbConn.Open(); }
        Int32 r = FBClass.AddCmd(sAdd, out msg);  // Inserisco dati utente
        //cAFbConn.Close();
		string msgl = "";
		if (r > 0) CLogga.logga("utenti", sAdd, 1, "Nuovo utente", "-1", r.ToString(), out msgl);
		return (r > 1 ? true : false);
    }
	public string virgolette(string s)
	{
		string ss = "";
		string virgoletta = "'";
		if (s == null || s.Length <= 0) return ("");

		string doppie = string.Format("{0}", "\"");
		for (int j = 0; j < s.Length; j++)
		{
			if (s[j] == virgoletta[0])
				ss += virgoletta + virgoletta;
			else ss += s[j];
		}
		return (ss);
	}
	public bool registradatiutente(string maker, out string msg) // registro tutti i dati dell'utente
    {
        msg = "Errore registrazione dati utente: iduser inesistente!";
        bool ok = false;
        if (iduser < 0)
            return (ok);
        // ,  forzocambiopassword=\"" + forzocambiopassword + "\", abilitato=\"" + abilitato + "\""
        // nikname, pwd, id, power, dtla, mail, nome, cognome, forzocambiopassword, abilitato, giornalizza, matricola, ip, indirizzo, civico, cap, citta
        passwordhashed = GetHash(cripto, (_prepas + password + _postpas)); // cripro la pwd a 256 bit
        string sUpdate = "update utenti set nikname=\'" + virgolette(nikname) + "\', pwd=\'" + password + "\', pwdhashed=\'" + passwordhashed + "\', power=" + potere.ToString() + ", nome=\'" + nome + "\', cognome=\'" + virgolette(cognome) + "\'";
        sUpdate += ", forzocambiopassword =" + (forzocambiopassword ? "\'1\'" : "\'0\'") + ", abilitato=" + (abilitato ? "\'1\'" : "\'0\'") + ", giornalizza=" + (giornalizza ? "\'1\'" : "\'0\' ");
        sUpdate += ", matricola =\'" + matricola + "\', struttura_ek = \'" + struttura_ek + "\', struttura_cod = \'" + struttura_cod + "\', ente = \'" + virgolette(struttura) + "\', indirizzo = \'" + virgolette(indirizzo) + "\', civico = \'" + civico + "\', cap = \'" + cap + "\', citta=\'" + virgolette(città) + "\'";
        sUpdate += ", telefono =\'" + virgolette(telefono) + "\', cell=\'" + virgolette(cell) + "\', mail=\'" + virgolette(mail) + "\'";
        sUpdate += ", tipodoc =\'" + tipodoc + "\', numdoc = \'" + numdoc + "\', scadenza = cast(" + string.Format("\'{0:yyyy-MM-dd}\'", scadenza) + " as date), scadenza_patente=cast(\'"+scadenza_patente.ToString("yyyy/MM/dd") +"\' as date), ente_ek=1 ";
        sUpdate += "where id=" + iduser;
        cAFbConn = FBClass.openaFBConn(out msg);
        if (cAFbConn.State == ConnectionState.Closed) { cAFbConn.Open(); }
        ok = FBClass.EsegueCmd(sUpdate,  out msg) == 1 ? true : false;  // carico dati utente
        cAFbConn.Close();
		string msgl = "";
		if (ok) CLogga.logga("utenti", sUpdate, 1, "Modifica dati utente", iduser.ToString(), maker, out msgl);
		return (ok);
    }
    public string registraDTLA() // registro tutti i dati dell'utente
    {
        bool ok = false;
        string msg = "";
        if (iduser < 0)
            return ("Problema registrazione dati! (DTLA): contattare l'assistenza.");

        string sUpdate = "update utenti set DTLA = cast(" + string.Format("\'{0:yyyy-MM-dd HH:mm:ss}\'", DateTime.Now) + " as timestamp) ";
        sUpdate += "where id=" + iduser;
        //cAFbConn = FBClass.openaFBConn(out msg);
        //if (cAFbConn.State == ConnectionState.Closed) { cAFbConn.Open(); }
        ok = FBClass.EsegueCmd(sUpdate, out msg) > 0 ? true : false;
        //cAFbConn.Close();
		string msgl = "";
		if (ok)
		{
			CLogga.logga("utenti", sUpdate, 2, "Login utente", iduser.ToString(), iduser.ToString(), out msgl);
			return ("");
		}
		else
			return ("Problema registrazione dati! (DTLA): contattare l'assistenza.");
    }
}
public class www
{
    public FbConnection cAFbConn = null;
    public ConnessioneFB FBClass = new ConnessioneFB();

    // proprietà
    private DateTime _dtla;
    private string _tbl;
    private string _sql;
    private string _specifica;
    private int _chi;
    private string _ip;
    private string _userloggato;

    // metodi
    public DateTime dtla { get { return _dtla; } set { _dtla = value; } }
    public string tbl { get { return _tbl; } set { _tbl = value; } }
    public string sql { get { return _sql; } set { _sql = value; } }
    public string specifica { get { return _specifica; } set { _specifica = value; } }
    public int chi { get { return _chi; } set { _chi = value; } }
    public string ip { get { return _ip; } set { _ip = value; } }
    public string userloggato { get { return _userloggato; } set { _userloggato = value; } }
    /*public bool logga(string tabella, string sql, int tipo_ek, string specifica, Int32 chi, out string msg)
	{
		bool ok = false;
		string s;
		s = "insert into log ( tabella, tipo_ek, sql, specifica, chi ) values (@tabella, @tipo_ek, @sql, @specifica, @chi)";
		FBClass.FBc.Parameters.Clear();
		FBClass.FBc.Parameters.Add("@tabella", FbDbType.VarChar); FBClass.FBc.Parameters["@tabella"].Value = tabella;
		FBClass.FBc.Parameters.Add("@sql", FbDbType.VarChar); FBClass.FBc.Parameters["@sql"].Value = sql;
		FBClass.FBc.Parameters.Add("@tipo_ek", FbDbType.Integer); FBClass.FBc.Parameters["@tipo_ek"].Value = System.Convert.ToInt16(tipo_ek);
		FBClass.FBc.Parameters.Add("@specifica", FbDbType.VarChar); FBClass.FBc.Parameters["@specifica"].Value = specifica;
		FBClass.FBc.Parameters.Add("@chi", FbDbType.Integer); FBClass.FBc.Parameters["@chi"].Value = System.Convert.ToInt16(chi);
		cAFbConn = FBClass.openaFBConn(out msg);
		//if (cAFbConn.State == ConnectionState.Closed) { cAFbConn.Open(); }
		ok = FBClass.EsegueCmd(s, out msg) == 1 ? true : false;  // carico dati utente
		cAFbConn.Close();
		return (ok);
	}*/
    public bool logga(string tabella, string sql, int tipo_ek, string specifica, string chi, string idaffected, out string msg)
    {
        bool ok = false;
        string s;
        s = "insert into log ( tabella, tipo_ek, sql, specifica, chi, idaffected, ip ) values (@tabella, @tipo_ek, @sql, @specifica, @chi, @idaffected,  @ip)";
        FBClass.FBc.Parameters.Clear();
        FBClass.FBc.Parameters.Add("@tabella", FbDbType.VarChar); FBClass.FBc.Parameters["@tabella"].Value = tabella.Trim();
        FBClass.FBc.Parameters.Add("@sql", FbDbType.VarChar); FBClass.FBc.Parameters["@sql"].Value = sql.Trim();
        FBClass.FBc.Parameters.Add("@tipo_ek", FbDbType.Integer); FBClass.FBc.Parameters["@tipo_ek"].Value = System.Convert.ToInt32(tipo_ek);
        FBClass.FBc.Parameters.Add("@specifica", FbDbType.VarChar); FBClass.FBc.Parameters["@specifica"].Value = specifica;
        FBClass.FBc.Parameters.Add("@chi", FbDbType.Integer); FBClass.FBc.Parameters["@chi"].Value = System.Convert.ToInt32(chi);
        FBClass.FBc.Parameters.Add("@idaffected", FbDbType.VarChar); FBClass.FBc.Parameters["@idaffected"].Value = idaffected;
        FBClass.FBc.Parameters.Add("@ip", FbDbType.VarChar); FBClass.FBc.Parameters["@ip"].Value = GetIPAddress();
        cAFbConn = FBClass.openaFBConn(out msg);
        //if (cAFbConn.State == ConnectionState.Closed) { cAFbConn.Open(); }
        ok = FBClass.EsegueCmd(s, out msg) == 1 ? true : false;  // carico dati utente
        cAFbConn.Close();
        return (ok);
    }
    public string GetIPAddress()
    {
        {
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            string ip = addr[1].ToString();

            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            //return Host.AddressList[1].ToString();
            string IPAddress = "";
            int c = 0;
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    c++;
                    if (c == 2) IPAddress += "-";
                    IPAddress += Convert.ToString(IP);
                    if (c >= 2)
                        break;
                }
            }
            return IPAddress;
        }
    }
}
public class gruppo
{
	public FbConnection cAFbConn = null;
	public ConnessioneFB FBClass = new ConnessioneFB();
	private www CLogga = new www();

	// proprietà
	private DateTime _dtla;
	private string _tbl;
	private string _sql;
	private string _specifica;
	private DataTable tbl = new DataTable();

	// metodi
	public string sql { get { return _sql; } set { _sql = value; } }
	public string specifica { get { return _specifica; } set { _specifica = value; } }

	public bool gr_add(int gruppo_ek, int auto_ek, int chi)
	{
		bool ok = false;
		if (gruppo_ek < 0 || auto_ek < 0)
			return (ok);
		string msg, sAdd;
		if (gr_cerca(gruppo_ek, auto_ek, out msg)) return (true); // non serve aggiungere.... c'è già

		sAdd = "insert into gruppi (GRUPPO_EK, AUTO_EK) ";
		sAdd += "VALUES (\'" + gruppo_ek.ToString() + "\', \'" + auto_ek.ToString() + "\') returning id ";
		Int32 r = FBClass.AddCmd(sAdd, out msg);  // aggiungo auto a gruppo
		string msgl = "";		
		if (r > 0) CLogga.logga("gruppi", sAdd, 1, "Aggiunta auto a gruppo", chi.ToString(), r.ToString(), out msgl);
		return (r == 0 ? true : false);
	}
	public bool gr_delete(int gruppo_ek, int auto_ek, int chi)
	{
		bool ok = false;
		if (gruppo_ek < 0 || auto_ek < 0)
			return (ok);
		string msg, sDelete;		

		sDelete = "delete from gruppi where GRUPPO_EK=" + gruppo_ek.ToString() + " and auto_ek=" + auto_ek.ToString() + " ";
		ok = FBClass.EsegueCmd(sDelete, out msg) == 1 ? true : false;  // aggiungo auto a gruppo
		string msgl = "";
		if (ok) CLogga.logga("gruppi", sDelete, 3, "Cancellata auto da gruppo", chi.ToString(), auto_ek.ToString(), out msgl);
		return (ok);
	}
	public bool gr_cerca(int gruppo_ek, int auto_ek, out string id)
	{
		id = ""; bool ok = false;
		if (gruppo_ek < 0 || auto_ek < 0)
			return (ok);
		string msg, sSelect;

		sSelect = "select * from gruppi where GRUPPO_EK=\'" + gruppo_ek.ToString() + "\' and auto_ek=\'" + auto_ek.ToString() + "\'";

		tbl = FBClass.getfromTbl(sSelect, out msg);
		if (tbl.Rows.Count > 0)
		{
			id = tbl.Rows[0]["id"].ToString(); // anche nel caso siano state trovate + occorrenze.... restituisco primo id che trovo
			if (tbl.Rows.Count == 1)
				ok = true;
		}		
		return (ok);
	}
}
public class gruppo_utenti
{
	public FbConnection cAFbConn = null;
	public ConnessioneFB FBClass = new ConnessioneFB();
	private www CLogga = new www();

	// proprietà
	private DateTime _dtla;
	private string _tbl;
	private string _sql;
	private string _specifica;
	private DataTable tbl = new DataTable();

	// metodi
	public string sql { get { return _sql; } set { _sql = value; } }
	public string specifica { get { return _specifica; } set { _specifica = value; } }

	public bool gru_add(int gruppo_ek, int utente_ek, int chi)
	{
		bool ok = false;
		if (gruppo_ek < 0 || utente_ek < 0)
			return (ok);
		string msg, sAdd;
		if (gru_cerca(gruppo_ek, utente_ek, out msg)) return (true); // non serve aggiungere.... c'è già

		sAdd = "insert into utente_gruppo (GRUPPO_EK, utente_EK) ";
		sAdd += "VALUES (\'" + gruppo_ek.ToString() + "\', \'" + utente_ek.ToString() + "\') returning id ";
		Int32 r = FBClass.AddCmd(sAdd, out msg);  // aggiungo auto a gruppo
		string msgl = "";
		if (r > 0) CLogga.logga("utente_gruppi", sAdd, 1, "Aggiunta utente a gruppo", chi.ToString(), r.ToString(), out msgl);
		return (r > 0 ? true : false);
	}
	public bool gru_delete(int gruppo_ek, int auto_ek, int chi)
	{
		bool ok = false;
		if (gruppo_ek < 0 || auto_ek < 0)
			return (ok);
		string msg, sDelete;

		sDelete = "delete from utente_gruppo where GRUPPO_EK=" + gruppo_ek.ToString() + " and utente_ek=" + auto_ek.ToString() + " ";
		ok = FBClass.EsegueCmd(sDelete, out msg) == 1 ? true : false;  // aggiungo auto a gruppo
		string msgl = "";
		if (ok) CLogga.logga("utente_gruppi", sDelete, 3, "Cancellato utente dal gruppo", chi.ToString(), auto_ek.ToString(), out msgl);
		return (ok);
	}
	public bool gru_cerca(int gruppo_ek, int utente_ek, out string id)
	{
		id = ""; bool ok = false;
		if (gruppo_ek < 0 || utente_ek < 0)
			return (ok);
		string msg, sSelect;

		sSelect = "select * from utente_gruppo where GRUPPO_EK=\'" + gruppo_ek.ToString() + "\' and utente_ek=\'" + utente_ek.ToString() + "\'";

		tbl = FBClass.getfromTbl(sSelect, out msg);
		if (tbl.Rows.Count > 0)
		{
			id = tbl.Rows[0]["id"].ToString(); // anche nel caso siano state trovate + occorrenze.... restituisco primo id che trovo
			if (tbl.Rows.Count == 1)
				ok = true;
		}
		return (ok);
	}
}
public class gmail
{
    // proprietà
    private string _smtp;
    private int _port;
    string _username;
    string _password;
    string _dachi;
    string _dachivis;
    string[] _achi;
    string[] _achicc;
    string[] _achiccn;
    string _subject;
    string _body;
    string _file;
    string _numeritel;
    MailMessage mail = new MailMessage();

    // costruttori
    public gmail()
    {   
        _dachi = "xxxxxxxxxxxxxxxxxxxxx@provincia.tn.it"; // da cambiare....
        _dachivis = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
        mail.From = new MailAddress(_dachi, "Gestione auto di Servizio"); // oppure from e fromName es: (name@bb.it, myname)        
        _smtp = "smtp.xxxxxxxxxxx"; // "smtp.xxxxxxxxxxxxxxxxxxxxxx";
        _password = "xxxxxxxxxxxxxxxxxxxxxxxx";
        _port = 587; // 25;
        _username = "xxxxxxxxxxxxxxxxxxxxxxxxx";
        _numeritel = "+39 0461 49xxxxxxxx o +39 0461 423424234234";
        _file = "";
    }

    // metodi
    public string serversmtp { get { return _smtp; }  set { _smtp = value; } }
    public int port          { get { return _port; }  set { _port = value; } }
    public string Username   { get { return _username; } set { _username = value; } }
    public string password   { get { return _password; } set { _password = value; } }
    public string dachi      { get { return _dachi; } set { _dachi = value; } }
    public string dachivisualizzato { get { return _dachivis; } set { _dachivis = value; } }
    public string[] achi     { get { return _achi; }  set { _achi = value; } }
    public string[] achicc   { get { return _achicc; } set { _achicc = value; } }
    public string[] achiccn  { get { return _achiccn; } set { _achiccn = value; } }
    public string subject    { get { return _subject; } set { _subject = value; } }
    public string body       { get { return _body; } set { _body = value; } }
    public string numeritel  { get { return _numeritel; } set { _numeritel = value; } }
    public string file       { get { return _file; } set { _file = value; } }

    public bool mandamail(string server, int porta, string userName, string pwd, out string msg)
    {
        mail.From = new MailAddress(dachi, dachivisualizzato); // oppure from e fromName es: (name@bb.it, myname)
        mail.To.Clear();
        if (achi != null)
        {
            foreach (string ac in achi)
                mail.To.Add(new MailAddress(ac)); // è una lista 
        }
        mail.CC.Clear();
        if (achicc != null)
        {
            foreach (string ac in achicc)
                mail.CC.Add(new MailAddress(ac)); // è una lista
        } 
        mail.Bcc.Clear();
        if (achiccn != null)
        {
            foreach (string ac in achiccn)
                mail.Bcc.Add(new MailAddress(ac)); // è una lista
        }
        mail.Subject = subject;
        mail.Body = body;
        
        msg = "";

        if (server.Trim() == "") server = serversmtp;
        if (porta <= 0) porta = port;
        if (userName.Trim() == "") userName = Username;
        if (pwd.Trim() == "") pwd = password;
        if (file.Trim() != "")
        {
            Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
			//Attachment data = new Attachment(ms, "Credenziali.pdf", "application/pdf");
			// Add time stamp information for the file.
			ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(file);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(file);

            // Add the file attachment to this e-mail message.
            mail.Attachments.Add(data);
        }
        SmtpClient client = new SmtpClient(server, porta);
        client.UseDefaultCredentials = false;
        client.EnableSsl = true;
        client.Credentials = new System.Net.NetworkCredential(userName, pwd);
        try
        {
            client.Send(this.mail);
        }
        catch (Exception ex)
        {
            msg = "inoltro mail fallito: " + ex.ToString();
            //Console.WriteLine("Exception caught in CreateTestMessage1(): {0}", ex.ToString());
            return (false);
        }
        return (true);
    }
}
public class classeprenota
{   // proprietà
    string _id;
    string _user_ek;
    string _maker_ek;
    string _nome;
    string _cognome;
    string _mail;
    string _tel;
    DateTime _partenza;
    DateTime _arrivo;
    string _dove_stato_ek;
    string _dove_prov_ek;
    string _dove_comune_ek;
    string _dove_comune;
    string _dove_sigla;
    string _dove_provincia;
    string _passeggeri;
    string _ubicazione_ek;
    string _ubicazione;
    string _ubicazione_Via;
    string _ubicazione_Civico;
    string _ubicazione_Cap;
    string _ubicazione_Città;
    DateTime _ubicazione_dalle;
    DateTime _ubicazione_alle;
    string _ubicazione_desc;
    string _ubicazione0_ek; // ritiro chiavi
    string _ubicazione0;
    string _ubicazione0_Via;
    string _ubicazione0_Civico;
    string _ubicazione0_Cap;
    string _ubicazione0_Città;
    DateTime _ubicazione0_dalle;
    DateTime _ubicazione0_alle;
    string _ubicazione0_desc;
    string _mezzo_ek;
    string _aggregati;
    string _aggregato_ek;
    string _aggregato_nome;
    string _aggregato_cognome;
    string _aggregato_tel;
    string _aggregato_mail;
    string _descrizione;
    string _motivo;
    string _numero;
    string _marca;
    string _modello;
    string _targa;
    string _trazione_ek;
    string _trazione;
    string _gomme_ek;
    string _gomme;
    string _classificazione_ek;
    string _classificazione;
    string _dislivello;
    string _distanzamin;
    string _abilitato;
    string _blackbox;
	string _posteggio;
    string _flotta_ek;
    string _foglio;
    string _delega;

    // oggetti privati interni alla classe
    private FbConnection cAFbConn = null;
    private ConnessioneFB FBClass = new ConnessioneFB();
    private FbDataAdapter cda = new FbDataAdapter();
    private FbCommand cc = new FbCommand();
    private DataSet cds = new DataSet();
    private DataTable tbl = new DataTable();
	private www CLogga = new www();
	private string msg;
    private string s;

    // metodi set-get
    public string id { get { return _id; } set { _id = value; } }
    public string user_ek { get { return _user_ek; } set { _user_ek = value; } }
    public string maker_ek { get { return _maker_ek; } set { _maker_ek = value; } }
    public string nome { get { return _nome; } set { _nome = value; } }
    public string cognome { get { return _cognome; } set { _cognome = value; } }
    public string mail { get { return _mail; } set { _mail = value; } }
    public string tel { get { return _tel; } set { _tel = value; } }
    public DateTime partenza { get { return _partenza; } set { _partenza = value; } }
    public DateTime arrivo { get { return _arrivo; } set { _arrivo = value; } }
    public string dove_stato_ek { get { return _dove_stato_ek; } set { _dove_stato_ek = value; } }
    public string dove_prov_ek { get { return _dove_prov_ek; } set { _dove_prov_ek = value; } }
    public string dove_comune_ek { get { return _dove_comune_ek; } set { _dove_comune_ek = value; } }
    public string dove_comune { get { return _dove_comune; } set { _dove_comune = value; } }
    public string dove_sigla { get { return _dove_sigla; } set { _dove_sigla = value; } }
    public string dove_provincia { get { return _dove_provincia; } set { _dove_provincia = value; } }
    public string ubicazione_ek { get { return _ubicazione_ek; } set { _ubicazione_ek = value; } }
    public string ubicazione { get { return _ubicazione; } set { _ubicazione = value; } }
    public string ubicazione_Via { get { return _ubicazione_Via; } set { _ubicazione_Via = value; } }
    public string ubicazione_Civico { get { return _ubicazione_Civico; } set { _ubicazione_Civico = value; } }
    public string ubicazione_Cap { get { return _ubicazione_Cap; } set { _ubicazione_Cap = value; } }
    public string ubicazione_Città { get { return _ubicazione_Città; } set { _ubicazione_Città = value; } }
    public DateTime ubicazione_dalle { get { return _ubicazione_dalle; } set { _ubicazione_dalle = value; } }
    public DateTime ubicazione_alle { get { return _ubicazione_alle; } set { _ubicazione_alle = value; } }
    public string ubicazione_desc { get { return _ubicazione_desc; } set { _ubicazione_desc = value; } }

    public string ubicazione0_ek { get { return _ubicazione0_ek; } set { _ubicazione0_ek = value; } }
    public string ubicazione0 { get { return _ubicazione0; } set { _ubicazione0 = value; } }
    public string ubicazione0_Via { get { return _ubicazione0_Via; } set { _ubicazione0_Via = value; } }
    public string ubicazione0_Civico { get { return _ubicazione0_Civico; } set { _ubicazione0_Civico = value; } }
    public string ubicazione0_Cap { get { return _ubicazione0_Cap; } set { _ubicazione0_Cap = value; } }
    public string ubicazione0_Città { get { return _ubicazione0_Città; } set { _ubicazione0_Città = value; } }
    public DateTime ubicazione0_dalle { get { return _ubicazione0_dalle; } set { _ubicazione0_dalle = value; } }
    public DateTime ubicazione0_alle { get { return _ubicazione0_alle; } set { _ubicazione0_alle = value; } }
    public string ubicazione0_desc { get { return _ubicazione0_desc; } set { _ubicazione0_desc = value; } }

    public string mezzo_ek { get { return _mezzo_ek; } set { _mezzo_ek = value; } }
    public string passeggeri { get { return _passeggeri; } set { _passeggeri = value; } }
    public string aggregati { get { return _aggregati; } set { _aggregati = value; } }
    public string aggregato_ek { get { return _aggregato_ek; } set { _aggregato_ek = value; } }
    public string aggregato_nome { get { return _aggregato_nome; } set { _aggregato_nome = value; } }
    public string aggregato_cognome { get { return _aggregato_cognome; } set { _aggregato_cognome = value; } }
    public string aggregato_tel { get { return _aggregato_tel; } set { _aggregato_tel = value; } }
    public string aggregato_mail { get { return _aggregato_mail; } set { _aggregato_mail = value; } }
    public string descrizioni { get { return _descrizione; } set { _descrizione = value; } }
    public string motivo { get { return _motivo; } set { _motivo = value; } }
    public string numero { get { return _numero; } set { _numero = value; } }
    public string marca { get { return _marca; } set { _marca = value; } }
    public string modello { get { return _modello; } set { _modello = value; } }
    public string targa { get { return _targa; } set { _targa = value; } }
    public string trazione_ek { get { return _trazione_ek; } set { _trazione_ek = value; } }
    public string trazione { get { return _trazione; } set { _trazione = value; } }
    public string gomme_ek { get { return _gomme_ek; } set { _gomme_ek = value; } }
    public string gomme { get { return _gomme; } set { _gomme = value; } }
    public string classificazione_ek { get { return _classificazione_ek; } set { _classificazione_ek = value; } }
    public string classificazione { get { return _classificazione; } set { _classificazione = value; } }
    public string dislivello { get { return _dislivello; } set { _dislivello = value; } }
    public string abilitato { get { return _abilitato; } set { _abilitato = value; } }
    public string blackbox { get { return _blackbox; } set { _blackbox = value; } }
	public string posteggio { get { return _posteggio; } set { _posteggio = value; } }
    public string flotta_ek { get { return _flotta_ek; } set { _flotta_ek = value; } }
    public string foglio { get { return _foglio; } set { _foglio = value; } }
    public string delega { get { return _delega; } set { _delega = value; } }

    // costruttori
    public classeprenota() // costruttore semplice
    {
        msg = "";
    }
    public classeprenota(string prenotazione_id) // precarica tutti i dati dell'id, se c'è
    {
        msg = "";
        tbl = cercaid(prenotazione_id, out msg);
    }
    // metodi pubblici
    public bool cerca_partenza_user(DateTime da_partenza, string id)
    {
        s = "SELECT a.*, b.*, c.* FROM PRENOTAZIONI a left join comuni b on a.DOVE_COMUNE_EK=b.ID left join ubicazione c on a.ubicazione_EK=c.ID";
        string where = " where a.id = \'" + id + "\' and partenza >= cast(\'" + da_partenza.ToString() + "\' as timestamp) order by partenza)";

        cds.Clear();
        cds = FBClass.getfromDSet((s + where), "prenotazioniID", out msg);
        if (msg != "")
            return (false);
        if (cds.Tables["prenotazioniid"].Rows.Count > 0)
            return (true);
        return (false);
    }
    public DataTable cercaid(string id, out string msg)
    {
        msg = "";
        s = "SELECT a.*, b.*, p.sigla as dove_sigla, p.provincia as dove_provincia, c.*, ";
        s += "c0.id as ubicazione0_ek, c0.ubicazione as ubicazione0, c0.via as via0, c0.civico as civico0, c0.descrizione as descrizione0, c0.apertodalle as apertodalle0, c0.apertofino as apertofino0, ";
        s += "g.comune as ucomune, g.cap as ucap, ";
        s += "g0.comune as ucomune0, g0.cap as ucap0, ";
        s += "ag.nome as anome, ag.cognome as acognome, ag.mail as amail, ag.telefono as atel, u.*, d.*, e.*, f.*, eg.* ";
        s += "from prenotazioni as a ";
        s += "left join comuni b on a.DOVE_COMUNE_EK = b.comune_k ";
        s += "left join PROVINCE p on b.PROVINCIA_EK = p.PROVINCIA_K ";
        s += "left join ubicazione c on a.ubicazione_EK = c.id ";
        s += "left join comuni g on c.COMUNE_EK = g.comune_k ";
        s += "left join mezzi d on a.mezzo_EK = d.ID ";
        s += "left join marca e on d.marca_EK = e.ID ";
        s += "left join modello f on d.modello_EK = f.ID ";
        s += "left join ubicazione c0 on c0.id=d.ritirochiavi_ek ";
        s += "left join comuni g0 on c0.COMUNE_EK = g0.comune_k ";
        s += "left join utenti u on a.user_ek = u.id ";
        s += "left join utenti ag on a.AGGREGATO_EK = ag.id ";
        s += "left join etichetta_gruppo as eg on eg.id=a.flotta_ek ";
        string where = " where a.id = \'" + id + "\'";

        cds.Clear();
        cds = FBClass.getfromDSet((s + where), "prenotazioniID", out msg);
        if (msg != "")
            return (null);
        if (cds.Tables["PrenotazioniID"].Rows.Count > 0)
            return (cds.Tables["PrenotazioniID"]);
        return (null);
    }
    public bool refresh(DataTable tb)
    {
        DateTime dt;
        id = tb.Rows[0]["id"].ToString();
        user_ek = tb.Rows[0]["user_ek"].ToString();
        maker_ek = tb.Rows[0]["maker"].ToString();
        nome = tb.Rows[0]["nome"].ToString();
        cognome = tb.Rows[0]["cognome"].ToString();
        mail = tb.Rows[0]["mail"].ToString();
        tel = tb.Rows[0]["telefono"].ToString();
        if (DateTime.TryParse(tb.Rows[0]["partenza"].ToString(), out dt)) partenza = dt;
        if (DateTime.TryParse(tb.Rows[0]["arrivo"].ToString(), out dt)) arrivo = dt;
        dove_stato_ek = tb.Rows[0]["dove_stato_ek"].ToString();
        dove_prov_ek = tb.Rows[0]["dove_prov_ek"].ToString();
        dove_comune_ek = tb.Rows[0]["dove_comune_ek"].ToString();
        dove_comune = tb.Rows[0]["dove_comune"].ToString();
        dove_sigla = tb.Rows[0]["dove_sigla"].ToString();
        dove_provincia = tb.Rows[0]["dove_provincia"].ToString();
        passeggeri = tb.Rows[0]["passeggeri"].ToString();
        aggregati = tb.Rows[0]["aggregati"].ToString();
        aggregato_ek = tb.Rows[0]["aggregato_ek"].ToString();
        aggregato_nome = tb.Rows[0]["anome"].ToString();
        aggregato_cognome = tb.Rows[0]["acognome"].ToString();
        aggregato_tel = tb.Rows[0]["atel"].ToString();
        aggregato_mail = tb.Rows[0]["amail"].ToString();
        ubicazione_ek = tb.Rows[0]["ubicazione_ek"].ToString();
        ubicazione = tb.Rows[0]["ubicazione"].ToString();
        ubicazione_Via = tb.Rows[0]["Via"].ToString();
        ubicazione_Civico = tb.Rows[0]["Civico"].ToString();
        ubicazione_Cap = tb.Rows[0]["ucap"].ToString();
        ubicazione_Città = tb.Rows[0]["uComune"].ToString();
        if (DateTime.TryParse(tb.Rows[0]["apertodalle"].ToString(), out dt)) ubicazione_dalle = dt;
        if (DateTime.TryParse(tb.Rows[0]["apertofino"].ToString(), out dt)) ubicazione_alle = dt;
        ubicazione_desc = tb.Rows[0]["descrizione"].ToString();

        ubicazione0_ek = tb.Rows[0]["ubicazione0_ek"].ToString();
        ubicazione0 = tb.Rows[0]["ubicazione0"].ToString();
        ubicazione0_Via = tb.Rows[0]["Via0"].ToString();
        ubicazione0_Civico = tb.Rows[0]["Civico0"].ToString();
        ubicazione0_Cap = tb.Rows[0]["ucap0"].ToString();
        ubicazione0_Città = tb.Rows[0]["uComune0"].ToString();
        if (DateTime.TryParse(tb.Rows[0]["apertodalle0"].ToString(), out dt)) ubicazione0_dalle = dt;
        if (DateTime.TryParse(tb.Rows[0]["apertofino0"].ToString(), out dt)) ubicazione0_alle = dt;
        ubicazione0_desc = tb.Rows[0]["descrizione0"].ToString();

        posteggio = tb.Rows[0]["posteggio"].ToString();
		mezzo_ek = tb.Rows[0]["mezzo_ek"].ToString();
        motivo = tb.Rows[0]["descrizione"].ToString();
        numero = tb.Rows[0]["numero"].ToString();
        marca = tb.Rows[0]["marca"].ToString();
        modello = tb.Rows[0]["modello"].ToString();
        blackbox = tb.Rows[0]["blackbox"].ToString().Trim();
        abilitato =tb.Rows[0]["abilitato"].ToString();
        targa = tb.Rows[0]["targa"].ToString();
        trazione_ek = tb.Rows[0]["trazione_ek"].ToString();
        trazione = tb.Rows[0]["trazione"].ToString();
        gomme_ek = tb.Rows[0]["gomme_ek"].ToString();
        gomme = tb.Rows[0]["gomme"].ToString();
        classificazione_ek = tb.Rows[0]["classificazione_ek"].ToString();
        classificazione = tb.Rows[0]["classificazione"].ToString();
        dislivello = tb.Rows[0]["dislivello"].ToString();
        flotta_ek = tb.Rows[0]["flotta_ek"].ToString();
        foglio = tb.Rows[0]["nome_prenotazione"].ToString();
        delega = tb.Rows[0]["nome_delega"].ToString();
        return (true);
    }
    public DataSet CercaDD(string comune, DateTime inizio, out string msg) // destinazione e date
    {
        s = "SELECT a.partenza, a.arrivo, b.cap, b.comune, d.telefono, d.mail, d.cognome, d.nome, a.id FROM PRENOTAZIONI a left join comuni b on a.DOVE_COMUNE_EK=b.comune_k left join ubicazione c on a.ubicazione_EK=c.ID left join utenti as d on user_ek=d.id ";
        string where = " where a.dove_comune = \'" + virgolette(comune) + "\' and partenza >= cast(\'NOW\' as timestamp) and partenza >= cast(\'" + inizio.ToString("yyyy/MM/dd") + "\' as date) and partenza < cast(\'" + inizio.ToString("yyyy/MM/dd") + "\' as date) +1 order by partenza";

        cds.Clear();
        cds = FBClass.getfromDSet((s + where), "prenotazioniDD", out msg);
        if (msg != "") // msg viene già restituito dalla connessione
            return (null);
        if (cds != null && cds.Tables["prenotazioniDD"].Rows.Count > 0)
            return (cds);

        return (null);
    }
    public Int32 Aggrega(string prenotazione, Int32 chi)
    {
        s = "update prenotazioni set aggregati = (aggregati + 1), aggregato_ek=\'"+ chi.ToString() + "\' ";
        string where = " where id=\'" + prenotazione.ToString() + "\'";
        msg = "";
        Int32 rr = FBClass.EsegueCmd(s + where, out msg);
		string msgl = "";
		//	public bool logga(string tabella, string sql, int tipo_ek, string specifica, Int32 chi, out string msg)
		CLogga.logga("prenotazioni", s + where, 2, "modifica numero aggregati", chi.ToString(), prenotazione.ToString(), out msgl);
		if (msg != "")
            return (-1);
        return (rr);
    }
    public string virgolette(string s)
    {
		if (s == null) return ("");
        string ss = "";
        string virgoletta = "'";
   
        string doppie = string.Format("{0}", "\"");
        for (int j = 0; j < s.Length; j++)
        {
            if (s[j] == virgoletta[0])
                ss += virgoletta + virgoletta; // raddoppio le virgolette singole
            else ss += s[j];
        }
        return (ss);
    }
    public Int32 Inserisci(string userid) // aggiungo record con dati 
    {
        if (userid == null || userid == "")
            userid = _user_ek;
        if( _dislivello == null || _dislivello.Trim().Length == 0 ) _dislivello = "0";
        /*if (_trazione == null || _trazione.Trim().Length == 0) _trazione = "-1";
        if (_gomme == null || _gomme.Trim().Length == 0) _gomme = "-1";
        if (_classificazione == null || _classificazione.Trim().Length == 0) _classificazione = "-1";
        */
		s = "insert into prenotazioni (id, user_ek, maker, tempo, partenza, arrivo, dove_stato_ek, dove_prov_ek, dove_comune_ek, dove_comune, passeggeri, ubicazione_ek, posteggio, aggregati, descrizione, mezzo_ek, trazione, gomme, classificazione, dislivello, flotta_ek) values (";
        s += "null, \'" + userid + "\', \'" + _maker_ek + "\', cast(\'now\' as timestamp), cast(\'" + _partenza.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp), cast(\'" + _arrivo.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp), ";
        s += "\'" + _dove_stato_ek + "\', \'" + _dove_prov_ek + "\', \'" + _dove_comune_ek + "\', \'" + virgolette(_dove_comune) + "\', \'" + _passeggeri + "\', \'" + _ubicazione_ek + "\', \'" + _posteggio + "\', ";
        s += "\'" + _aggregati + "\', \'" + _descrizione + "\', \'" + _mezzo_ek + "\', \'" + _trazione + "\', \'" + _gomme + "\', \'" + _classificazione + "\', \'" + _dislivello + "\', \'" + _flotta_ek + "\') returning id ";
        msg = "";
        Int32 rr = FBClass.AddCmd(s, out msg);
        if (msg != "")
            return (-1);
		string msgl = "";
		//int id = Convert.ToInt32(userid);
		if ( rr > 0 ) CLogga.logga("prenotazioni", s, 1, "Aggiungi nuova prenotazione", userid, rr.ToString(), out msgl);
		return (rr);
    }
    public Int32 Modifica(string userid, string prenotazioneid) // modifco prenotazione.. non serve modificare la flotta xche la macchina rimane uguale
    {
        if (userid == null || userid == "")
            userid = _user_ek;
        s = "update prenotazioni set user_ek = \'"+userid+"\', maker=\'"+_user_ek+ "\', tempo=cast(\'now\' as timestamp), partenza=cast(\'" + _partenza.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp), arrivo=cast(\'" + _arrivo.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp), passeggeri=\'" + _passeggeri + "\', aggregati=\'" + _aggregati + "\', descrizione=\'" + _descrizione + "\' ";
        s += "where id=\'" + prenotazioneid.Trim() + "\'";
        msg = "";
        Int32 rr = FBClass.EsegueCmd(s, out msg);
        if (msg != "")
            return (-1);
		string msgl = "";
		//int id = Convert.ToInt32(userid);
		CLogga.logga("prenotazioni", s, 2, "modifica prenotazione", id, rr.ToString(), out msgl);
		return (rr);
    }
	// non so perchè chiedo utente, comune ???
    public DataSet mezzidisponibili(string utente_ek, string ubi,  string comune_ek, string provincia_ek, DateTime partenza, DateTime arrivo, string dislivello, bool filtri, out string msg)
    {
        // implemento la busines logic delle proposte
        // 1) dislivello > di 500m: 4x4, gomme, invernali, in ordine di classificazione
        // 2) viaggio fuori provincia: classificazione viaggi lunghi
        // 3) se entrambe (dislivello > 500 e fuori provincia: 4x4 e gomme, in ordine di classificazioe
        string f = "";
        if (filtri)
        {
            int dis = 0;
            if (dislivello == null) dislivello = "0";
            dislivello = dislivello.IndexOf(".") > 0 ? dislivello.Substring(0, dislivello.IndexOf(".")) : dislivello;
            Int32.TryParse(dislivello, out dis);
            bool alto = dis > 499 ? true : false;
            bool lungo = false;
            if (provincia_ek != "22") lungo = true;
            f = alto ? "( trazione like \'%INTEGRALE%' or gomme like \'%INVERNALI%\' OR punti > 8 ) and " : "trazione not like \'%INTEGRALE%\' and ";
            f += lungo ? "classificazione like \'%lunghi%\' and " : "classificazione not like \'%lunghi%\' and ";

			f = ""; // nel mentre che ci sono solo pochi veicoli

			//f += (lungo && alto) ? "" : "trazione not like \'%INTEGRALE%\' and ";
        }

		s = "SELECT distinct m.id, m.numero as \"num.\", ug.UTENTE_EK, u.cognome, u.nome, ga.auto_ek, ma.marca, mo.modello, m.posti, ";
		s += "al.alimentazione, ca.cambio, t.trazione, ub.ubicazione, ub.via, ub.civico, cl.classificazione, go.gomme, co.comune, m.posti, m.posteggio, e.colore, e.id as flotta_ek ";
		s += "from utente_gruppo as ug ";
		s += "left join Utenti as u on u.id=ug.UTENTE_EK ";
		s += "left join gruppi as ga on ga.gruppo_ek=ug.GRUPPO_EK ";
		s += "left join mezzi as m on m.id=ga.auto_ek ";
		s += "left join marca as ma on ma.id=m.marca_ek ";
		s += "left join modello as mo on mo.id = m.modello_ek ";
		s += "left join classificazione as cl on cl.id = m.classificazione_ek ";
		s += "left join alimentazione as al on al.id = m.alimentazione_ek ";
		s += "left join cambio as ca on ca.id = m.cambio_ek ";
		s += "left join gomme as go on go.id = m.gomme_ek ";
		s += "left join trazione as t on t.id = m.trazione_ek ";
		s += "left join ubicazione as ub on ub.id = m.ubicazione_ek ";
		s += "left join comuni as co on co.comune_k = ub.comune_ek ";
		s += "left join ETICHETTA_GRUPPO as e on e.id = ug.GRUPPO_EK ";

		string where = "where m.abilitato = 1 and ug.utente_ek = \'" + utente_ek.Trim() + "\' ";
		where += ubi != null && ubi != "" ? " and m.ubicazione_ek=\'" + ubi + "\' " : "";
		where += " and m.scadenza > cast(\'" + arrivo.ToString("yyyy/MM/dd")  + "\' as date) and " + f + "  m.id not in ";
        where += "(SELECT p.MEZZO_EK FROM PRENOTAZIONI as p where p.partenza between cast(\'"+ partenza.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) and cast(\'"+arrivo.ToString("yyyy/MM/dd HH:mm:ss") + "' as timestamp) or ";
		where += "p.ARRIVO between cast(\'" + partenza.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) and cast(\'" + arrivo.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) or ";
		where += "p.partenza <= cast(\'" + partenza.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) and p.arrivo >= cast(\'" + arrivo.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) ) ";
		// considero tutte quelle che hanno la partenza dentro l'intervallo; tutte quelle che hanno l'arrivo dentro l'intervallo e quelle che hanno la partenza prima e l'arrivo dopo!
		string order = " order by e.ordine desc, co.comune, ub.ubicazione, ub.via, ub.civico, cl.classificazione, ma.marca, mo.modello";            
        cds.Clear();
        cds = FBClass.getfromDSet((s + where + order), "disponibili", out msg);
        if (msg != "") // msg viene già restituito dalla connessione
            return (null);
        return (cds);
    }
	public DataTable PrenotazioniInDateDifferenti(string id, out string msg)
	{
		s = "select b.id,  a.arrivo, count(cast(a.arrivo as date)) as da, e.max_prenotazioni ";
		s += "from prenotazioni as a ";
		s += "left join utenti as b on b.id = a.user_ek ";
        s += "left join enti as e on e.id=b.ente_ek ";
        //s += "left join utente_gruppo as ug on ug.utente_ek=b.id ";
        //s += "left join etichetta_gruppo as eg on eg.id=ug.gruppo_ek ";
        s += "group by b.id, a.arrivo, e.max_prenotazioni ";
        s += "having b.id = " + id.Trim() + " and cast(a.arrivo as date) >= cast('" + DateTime.Now.ToString("yyyy-MM-dd") + "\' as date) ";
		s += "order by a.arrivo ";
		msg = "";
		//if ( cds != null && cds.Tables["prenotazioni"].Rows.Count > 0 )
        //    cds.Tables["prenotazioni"].Clear();
		cds = FBClass.getfromDSet((s), "prenotazioni", out msg);
		if (msg != "") // msg viene già restituito dalla connessione
			return (null);
		return (cds.Tables["prenotazioni"]);
	}
    public DataTable PrenotazioniPerMezzoDiUnGruppo(string id, string idcar, string idflotta_ek, out string msg)
    {
        // 1) mi trovo la flotta di appartenenza della macchina selzionata
        // 2) cerco tutte le prenotazioni di quell'utente delle macchine appartenenti a quella flotta
        /*s = "select g.gruppo_ek ";
        s += "from UTENTE_GRUPPO as ug ";
        s += "left join gruppi as g on g.gruppo_ek = ug.gruppo_ek ";
        s += "left join ETICHETTA_GRUPPO as e on e.id = ug.gruppo_ek ";
        s += "where ug.utente_ek = " + id.Trim() + " and g.auto_ek = " + idcar.Trim() + " and g.gruppo_ek = " + idflotta_ek.Trim() + " ";
        
        msg = "";
        cds = FBClass.getfromDSet((s), "gruppousercar", out msg);
        if (msg != "") // msg viene già restituito dalla connessione
            return (null);
        string gruppo;
        gruppo = (cds != null && cds.Tables["gruppousercar"].Rows.Count == 1) ? cds.Tables["gruppousercar"].Rows[0]["gruppo_ek"].ToString().Trim() : "";
        if (gruppo == "") return (null);
        */
        s = "select p.dove_comune, p.partenza, p.arrivo, p.mezzo_ek, e.max_prenotazioni, e.etichetta, p.flotta_ek, u.power ";
        s += "from prenotazioni p ";
        s += "left join ETICHETTA_GRUPPO as e on e.id=p.flotta_ek ";
        s += "left join utenti as u on u.id=p.user_ek ";
        s += "where p.arrivo >= cast('now' as timestamp) and p.user_ek = " + id.Trim() + " and p.flotta_ek = " + idflotta_ek.Trim() + " ";

        cds = FBClass.getfromDSet((s), "gruppousercar", out msg);
        if (msg != "") // msg viene già restituito dalla connessione
            return (null);
        return (cds.Tables["gruppousercar"]);
    }
    public DataSet Sovrapposte(string id, DateTime inizio, DateTime fine, out string msg)
    {
        msg = "";
        s = "SELECT a.partenza, a.arrivo, b.cap, a.dove_comune as comune, d.cell, d.mail, d.cognome, d.nome, a.id, ";
        s += "(ma.marca || ' ' || mo.modello) as mezzo, (c.ubicazione || ' ' || c.via || ' ' || c.CIVICO) as posteggio, a.id, e.colore ";
        s += "FROM PRENOTAZIONI a ";
        s += "left join comuni b on a.DOVE_COMUNE_EK=b.comune_k ";
        s += "left join ubicazione c on a.ubicazione_EK=c.ID ";
        s += "left join utenti as d on a.user_ek=d.id ";
        s += "left join mezzi as m on m.id=a.mezzo_ek ";
        s += "left join marca as ma on ma.id=m.marca_ek ";
        s += "left join modello as mo on mo.id=m.modello_ek ";
        s += "left join etichetta_gruppo as e on e.id=a.flotta_ek ";
        string where = " where a.user_EK = \'" + id + "\' and ";
        where += "((partenza between cast(\'" + inizio.ToString("yyyy/MM/dd HH:mm:dd") + "\' as timestamp) and cast(\'" + fine.ToString("yyyy/MM/dd HH:mm:dd") + "\' as timestamp)) or ";
        where += "(arrivo between cast(\'" + inizio.ToString("yyyy/MM/dd HH:mm:dd") + "\' as timestamp) and cast(\'" + fine.ToString("yyyy/MM/dd HH:mm:dd") + "\' as timestamp)) or ";
		where += "(partenza <= cast(\'" + inizio.ToString("yyyy/MM/dd HH:mm:dd") + "\' as timestamp) and arrivo >= cast(\'" + fine.ToString("yyyy/MM/dd HH:mm:dd") + "\' as timestamp)) or ";
		where += "(cast(a.partenza as timestamp) >= cast(\'" + inizio.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp) and cast(a.arrivo as timestamp) <= cast(\'" + fine.ToString("yyyy-MM-dd HH:mm:ss") + "\' as timestamp))) ";
		where += " order by a.partenza";

        cds.Clear();
        cds = FBClass.getfromDSet((s + where), "sovrapposte", out msg);
        if (msg != "") // msg viene già restituito dalla connessione
            return (null);
        return (cds);
    }
	public DataTable VerificaModificaPrenotazione(string mezzo_id, string pre_id, DateTime inizio, DateTime fine, out string msg)
	{
		msg = "";
		s = "SELECT p.MEZZO_EK , p.id, p.partenza, p.arrivo ";
		s += "FROM PRENOTAZIONI as p ";
		s += "where p.mezzo_ek = " + mezzo_id + " and p.id != " + pre_id + " and ( ";
		s += "p.partenza between cast(\'" + inizio.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) and cast(\'" + fine.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) or ";
		s += "p.ARRIVO between cast(\'" + inizio.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) and cast(\'" + fine.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) or ";
		s += "p.partenza <= cast(\'" + inizio.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) and p.arrivo >= cast(\'" + fine.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) )";
  		cds.Clear();
		cds = FBClass.getfromDSet((s), "prenotazionisovrapposte", out msg);
		if (msg != "") // msg viene già restituito dalla connessione
			return (null);
		return (cds.Tables["prenotazionisovrapposte"]);
	}
	public DataTable Prenotazioni(string id, DateTime inizio, DateTime fine, string nome, string cognome, string nikname, string matricola, string ubi_ek, string targa, string numero, string struttura_cod, string dipendeda_ek, out string qry, out string msg)
    {
        msg = "";
        s = "SELECT a.partenza, a.arrivo, a.dove_comune, b.cap, b.comune, c.ubicazione, d.telefono, d.cell, d.mail, d.cognome, d.nome, e.targa, ma.marca, mo.modello, e.numero, ";
        s += "a.id, a.user_ek, a.flotta_ek, eg.colore ";
        s += "FROM PRENOTAZIONI a ";
        s += "left join comuni b on a.DOVE_COMUNE_EK=b.comune_k ";
        s += "left join ubicazione c on a.ubicazione_EK=c.ID ";
        s += "left join utenti as d on a.user_ek=d.id ";
        s += "left join strutture as s on s.codice=d.STRUTTURA_EK ";
        s += "left join mezzi as e on a.mezzo_ek=e.id ";
        s += "left join marca as ma on e.marca_ek=ma.id ";
        s += "left join modello as mo on e.modello_ek=mo.id ";
        s += "left join etichetta_gruppo as eg on eg.id=a.flotta_ek ";
        string where = " where s.attiva = 1 and ";
        if ( id.Trim().Length > 0 )
            where = "a.user_EK = \'" + id + "\' and ";
        if (inizio == null)
            inizio = new DateTime(0);
        if (fine == null)
            fine = DateTime.Now.AddDays(365);
        if (targa == null)
            targa = "";
        if (numero == null)
            numero = "";
		if (struttura_cod == null)
			struttura_cod = "";
        where += "((partenza between cast(\'" + inizio.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) and cast(\'" + fine.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp)) or ";
        where += "(arrivo between cast(\'" + inizio.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) and cast(\'" + fine.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp)) or ";
        where += "(partenza <= cast(\'" + inizio.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) and arrivo >= cast(\'" + fine.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp))) and ";
        if (nome.Trim().Length > 0)
            where += "upper(nome) like \'%" + nome.Trim().ToUpper() + "%\' and ";
        if (cognome.Trim().Length > 0)
            where += "upper(cognome) like \'%" + cognome.Trim().ToUpper() + "%\' and ";
		if (nikname.Trim().Length > 0)
			where += "upper(nikname) like \'%" + nikname.Trim().ToUpper() + "%\' and ";
		if (matricola.Trim().Length > 0)
            where += "upper(matricola) like \'%" + matricola.Trim().ToUpper() + "%\' and ";
        if (ubi_ek.Trim().Length > 0)
            where += "a.ubicazione_ek='" + ubi_ek.Trim() + "\' and ";
        if (targa.Trim().Length > 0)
            where += "upper(targa) like \'%" + targa.Trim().ToUpper() + "%\' and ";
        if (numero.Trim().Length > 0)
            where += "numero = \'" + numero.Trim() + "\' and ";
		if (struttura_cod.Trim().Length > 0)
			where += "struttura_ek = \'" + struttura_cod.Trim() + "\' and ";
        if (dipendeda_ek.Trim().Length > 0)
            where += "dipendeda_ek = \'" + dipendeda_ek.Trim() + "\' and ";
        where = where.Trim().Length > 0 ? where.Substring(0, where.LastIndexOf(" and ")) : where; 
        where += " order by a.partenza desc";

        cds.Clear();
        cds = FBClass.getfromDSet((s + where), "prenotazioni", out msg);
		qry = "";
		if (msg != "") // msg viene già restituito dalla connessione
			return (null);
		qry = s + where;
		return (cds.Tables["prenotazioni"]);
    }
    public DataTable PrenotazioniGerarchiche(string id, DateTime inizio, DateTime fine, string nome, string cognome, string nikname, string matricola, string ubi_ek, string targa, string numero, string struttura_cod, string dipendeda_ek, out string qry, out string msg)
    {
        msg = "";
        s = "SELECT a.partenza, a.arrivo, a.dove_comune, b.cap, b.comune, c.ubicazione, d.telefono, d.cell, d.mail, d.cognome, d.nome, e.targa, ma.marca, mo.modello, e.numero, ";
        s += "a.id, a.user_ek, a.flotta_ek, eg.colore ";
        s += "FROM PRENOTAZIONI a ";
        s += "left join comuni b on a.DOVE_COMUNE_EK=b.comune_k ";
        s += "left join ubicazione c on a.ubicazione_EK=c.ID ";
        s += "left join utenti as d on a.user_ek=d.id ";
        s += "left join strutture as s on s.codice=d.STRUTTURA_ek ";
        s += "left join mezzi as e on a.mezzo_ek=e.id ";
        s += "left join marca as ma on e.marca_ek=ma.id ";
        s += "left join modello as mo on e.modello_ek=mo.id ";
        s += "left join etichetta_gruppo as eg on eg.id=a.flotta_ek ";
        string where = " where ";
        if (id.Trim().Length > 0)
            where = "a.user_EK = \'" + id + "\' and ";
        if (inizio == null)
            inizio = new DateTime(0);
        if (fine == null)
            fine = DateTime.Now.AddDays(365);
        if (targa == null)
            targa = "";
        if (numero == null)
            numero = "";
        if (struttura_cod == null)
            struttura_cod = "";
        where += "((partenza between cast(\'" + inizio.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) and cast(\'" + fine.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp)) or ";
        where += "(arrivo between cast(\'" + inizio.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) and cast(\'" + fine.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp)) or ";
        where += "(partenza <= cast(\'" + inizio.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) and arrivo >= cast(\'" + fine.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp))) and ";
        if (nome.Trim().Length > 0)
            where += "upper(nome) like \'%" + nome.Trim().ToUpper() + "%\' and ";
        if (cognome.Trim().Length > 0)
            where += "upper(cognome) like \'%" + cognome.Trim().ToUpper() + "%\' and ";
        if (nikname.Trim().Length > 0)
            where += "upper(nikname) like \'%" + nikname.Trim().ToUpper() + "%\' and ";
        if (matricola.Trim().Length > 0)
            where += "upper(matricola) like \'%" + matricola.Trim().ToUpper() + "%\' and ";
        if (ubi_ek.Trim().Length > 0)
            where += "a.ubicazione_ek='" + ubi_ek.Trim() + "\' and ";
        if (targa.Trim().Length > 0)
            where += "upper(targa) like \'%" + targa.Trim().ToUpper() + "%\' and ";
        if (numero.Trim().Length > 0)
            where += "numero = \'" + numero.Trim() + "\' and ";
        if (struttura_cod.Trim().Length > 0 )
            where += "(struttura_ek = \'" + struttura_cod.Trim() + "\' or dipendeda_ek = \'" + struttura_cod.Trim() + "\') and ";
        where = where.Trim().Length > 0 ? where.Substring(0, where.LastIndexOf(" and ")) : where;
        where += " order by a.partenza desc";

        cds.Clear();
        cds = FBClass.getfromDSet((s + where), "prenotazioni", out msg);
        qry = "";
        if (msg != "") // msg viene già restituito dalla connessione
            return (null);
        qry = s + where;
        return (cds.Tables["prenotazioni"]);
    }
    public int Cancella(string idp, string idu, out string msg)
    {
        msg = "";
		/*if ( _id.Trim().Length == 0 ) {
            msg = "";
            return (-1);
        }*/
        if (idp != "") tbl = cercaid(idp, out msg);
		DateTime dtp, dta;
		if (DateTime.TryParse(tbl.Rows[0]["partenza"].ToString(), out dtp)) partenza = dtp;
		if (DateTime.TryParse(tbl.Rows[0]["arrivo"].ToString(), out dta)) arrivo = dta;
		s = "delete from prenotazioni where id=\'" + idp + "\' and arrivo > cast('now' as timestamp) and user_ek=" + tbl.Rows[0]["user_ek"].ToString() + " and dove_comune = \'" + virgolette(tbl.Rows[0]["dove_comune"].ToString()) + "\' and mezzo_ek=" + tbl.Rows[0]["mezzo_ek"].ToString() + " and cast(partenza as timestamp)=cast(\'" + partenza.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) " + " and cast(arrivo as timestamp)=cast(\'" + arrivo.ToString("yyyy/MM/dd HH:mm:ss") + "\' as timestamp) ";

		int rr = FBClass.EsegueCmd(s, out msg);
        if (msg != "")
            return (-1);
		string msgl = "";
		//Int32 idui;
		//Int32.TryParse(idu, out idui);
		CLogga.logga("prenotazioni", s, 2, "Cancella prenotazione", idu, idp, out msgl);
		return (rr);
    }
    public bool carica(string idp, out string err) // aggiorno le proprietà della classe 
    {
		err = "";
		tbl = cercaid(idp, out err);
		if (err.Trim().Length > 0) return (false);
		bool ok = refresh(tbl);
		return (ok);
    }
}
public class ConnessioneSQL
{   // proprietà
    public SqlConnection SQLConn = null;
    public string SQLstrConn = "Data Source =xxxxxxxxxxxxxxxxxxxxxxxxxx; Initial Catalog = xxxxxxxxxx; User Id=xxxxxxxxxxxx; Password=xxxxxxxxxxxx;";
    public SqlCommand SQLc = new SqlCommand();
    public SqlDataAdapter SQLda = new SqlDataAdapter();
    public string _cnn;
    public SqlCommand c = new SqlCommand();
    public SqlDataAdapter da = new SqlDataAdapter();
    public DataSet ds = new DataSet();

    public string cnn { get { return _cnn; } set { _cnn = value; } }
    public string ComandoSetTesto { get { return c.CommandText; } set { c.CommandText = value; } }

    // metodi
    public ConnessioneSQL()
    {
        cnn = "Data Source =CL-SQL02-PAT01.it.dcad.infotn.it; Initial Catalog = APAC; User Id=apac_usr; Password=lucilla24;";
    }
    public ConnessioneSQL(string cnn)
    {
        if (cnn.Length >= 8)
        {
            this.cnn = "Data Source =xxxxxxxxxxxxxxxxxxxxx; Initial Catalog = xxxxxxxxxxxxxxxx; User Id=xxxxxxxxxxxxxxxxx; Password=xxxxxxxxxxxxxxxxxxx;";
        };
    }

    public SqlConnection openaSQLConn(out string msg)
    {
        msg = "";
        try
        {
            SQLConn = new SqlConnection(cnn);
            SQLConn.Open();
        }
        catch (Exception ex)
        {
            msg = ex.Message;
            return (null);
        }
        return (SQLConn);
    }

    public bool closeaSQLConn(SqlConnection ac, out string msg)
    {
        msg = "";
        try
        {
            ac.Close();
        }
        catch (Exception ex)
        {
            msg = "Chiusura connessione non riuscita! " + ex.ToString();
            return (false);
        }
        return (true);
    }

    public long EseguiComando()  // da implementare....
    {
        return (-1);
    }
}
public class ConnessioneFB
{   // proprietà
    //public static string FBConnectionString = "User=xxxxxxxxxxxxx;" + "Password=xxxxxxxxxxxxxxxxxxx;" + @"Database=D:\CarSharingxxxxxx;" + "DataSource=;" + "Port=3050;" + "Dialect=3;" + "Charset=UTF8;" + "Role=;" + "Connection lifetime=15;" + "Pooling=true;" + "MinPoolSize=0;" + "MaxPoolSize=50;" + "Packet Size=8192;" + "ServerType=0";
    public static string FBConnectionString = "User =xxxxxxxxxxxxxx; Password=xxxxxxxxxxxxxxxxxxx;" + @"Database=D:\carsharingxxxxxxxxxxxxx;" + " DataSource=xxxxxxxxxxxxxxxxxx; Port=3050;" + "Dialect=3;" + "Charset=UTF8;" + "Role=;" + "Connection lifetime=15;" + "Pooling=true;" + "MinPoolSize=0;" + "MaxPoolSize=50;" + "Packet Size=8192;" + "ServerType=0";
    public FbConnection FBconnessione = new FbConnection(FBConnectionString);
    public FbCommand FBc = new FbCommand();
    public FbParameter FBp = new FbParameter();
    public FbDataAdapter FBda = new FbDataAdapter();
    public string _cnn;
    public DataSet ds = new DataSet();
    private DataTable tbl = new DataTable();

    public string cnn { get { return _cnn; } set { _cnn = value; } }
    public string ComandoSetTesto { get { return FBc.CommandText; } set { FBc.CommandText = value; } }

    // costruttori
    public ConnessioneFB()
    {
        cnn = FBConnectionString;
    }
    public ConnessioneFB(string cnn)
    {
        if (cnn.Length >= 8)
        {
            this.cnn = FBConnectionString;
        };
    }

    // metodi
    public FbConnection openaFBConn(out string msg)
    {
        msg = "";
        try
        {
			if ( FBconnessione.State != ConnectionState.Open) //FBconnessione = new FbConnection(cnn);
				FBconnessione.Open();
        }
        catch (Exception ex)
        {
            msg = ex.Message;
            return (null);
        }
        return(FBconnessione);
    }
    public FbConnection FBConn()
    {
         return (FBconnessione);
    }
    public bool closeaFBConn(out string msg)
    {
        msg = "";
        try
        {
            FBconnessione.Close();
        }
        catch (Exception ex)
        {
            msg = "Chiusura connessione non riuscita! " + ex.ToString();
            return (false);
        }
        return (true);
    }
    public bool FBAddPar(FbCommand fbc, string nomeparametro, FbDbType fbtipo, string nome)
    {
        FBc.Parameters.Add(nomeparametro, fbtipo);        
        return (true);
    }
    public Int32 EsegueCmd(string comandotxt, out string message)  // potrei anche verificare lo stato della cnn ed eventualmente aprirla....
    {
		Int32 nr = -1;
		message = "";
		try
		{
			FBconnessione = openaFBConn(out message);
			if (FBconnessione.State != ConnectionState.Open)
			{
				FBconnessione.Close();
				FBconnessione.Open();
			}
			FBc.Connection = FBconnessione;
			FBc.CommandText = comandotxt;
			nr = FBc.ExecuteNonQuery();
		}
		catch (Exception ex)
		{
			message = "Comando non riuscito: " + ex.ToString();
		}
		// non so che ripercussione avrà su altri ambienti
		//if (nr <= 0) message = nr.ToString();
		FBconnessione.Close();
		return (nr);
    }
    public long getFBdata(string select, string strTableName, out string err)
    {
        long rr = -1;
        err = "";
        try
        {
            if (FBconnessione.State == ConnectionState.Closed) { FBconnessione.Open(); }
            FBda = new FbDataAdapter(select, FBconnessione);
            FBda.Fill(ds, strTableName);
            rr = ds.Tables[strTableName].Rows.Count;
        }
        catch (Exception ex)
        {
            Console.Write("Exception: {0}", ex);
            err = ex.Message;
        }
        FBconnessione.Close();
        return (rr);
    }
    public DataSet getfromDSet(string select, string strTableName, out string err)
    {
        err = "";
        try
        {
            if (FBconnessione.State != ConnectionState.Open) { FBconnessione.Open(); }
            FBda = new FbDataAdapter(select, FBconnessione);
            FBda.Fill(ds, strTableName);
        }
        catch (Exception ex)
        {
            err = ex.Message;
        }
        FBconnessione.Close();
        return (ds);
    }
    public DataTable getfromTbl(string select, out string err)
    {
        err = "";
        try
        {
            if (FBconnessione.State == ConnectionState.Closed) { FBconnessione.Open(); }
            FBda = new FbDataAdapter(select, FBconnessione);
            FBda.Fill(ds, "tbl");
        }
        catch (Exception ex)
        {
            err = ex.Message;
        }
        FBconnessione.Close();
        return (ds.Tables["tbl"]);
    }
    public System.Data.ConnectionState State()
    {
        return (FBconnessione.State);
    }
    public Int32 AddCmd(string comandotxt, out string message)  // ritorna l'id
    {
        Int32 nr = -1;
        message = "";
        try
        {
            FBconnessione = openaFBConn(out message);
            if (FBconnessione.State != ConnectionState.Open)
            {
                FBconnessione.Close();
                FBconnessione.Open();
            }
            FBc.Connection = FBconnessione;
            FBc.CommandText = comandotxt;
            nr = (Int32)FBc.ExecuteScalar();
        }
        catch (Exception ex)
        {
            message = "Comando non riuscito: " + ex.ToString();
        }
        // non so che ripercussione avrà su altri ambienti
        //if (nr <= 0) message = nr.ToString();
        FBconnessione.Close();
        return (nr);
    }
}
public class Connessione
{   // proprietà
    public OleDbConnection AConn = null;
    public string _cnn;
    public OleDbCommand c = new OleDbCommand();    
    public OleDbDataAdapter da = new OleDbDataAdapter();
    public DataSet ds = new DataSet();

    public string cnn { get { return _cnn; } set { _cnn = value; } }
    public string ComandoSetTesto { get { return c.CommandText; } set { c.CommandText = value; } }

    // metodi
    public Connessione()
    {
        cnn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=xxxxxxxxxxxxxx.mdb";
    }
    public Connessione(string cnn)
    {   if (cnn.Length >= 8)
        {   this.cnn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:/SVI/dati/GareDataBase.mdb";
        };
    }

    public OleDbConnection openaConn(out string msg)
    {
        msg = "";
        try
        {
            AConn = new OleDbConnection(cnn);
            AConn.Open();
        }
        catch (Exception ex)
        {
            msg = ex.Message;
            return (null);
        }
        return (AConn);
    }

    public bool closeaConn(OleDbConnection ac, out string msg)
    {
        msg = "";
        try
        {   
            ac.Close();
        }
        catch (Exception ex)
        {
            msg = "Chiusura connessione non riuscita! " + ex.ToString();
            return (false);
        }
        return (true);
    }

    public long EseguiComando()  // da implementare.... dovrebbe ritornare un dataset....
    {
        long nr = -1;
        return (nr);
    }
}
public class password
{
    public string _pwd;
    public string _pwdhashed;
    public int _nMaiuscole;
    public int _nMinuscole;
    public int _nNumeri;
    public int _lunghezzaminima;
    public int _durata;
    public DateTime _scadenza;
    public SHA256 cripto = SHA256.Create();
    private Random rnd;

    public string pwd { get { return _pwd; } set { _pwd = value; } }
    public string pwdhashed { get { return _pwdhashed; } set { _pwdhashed = value; } }
    public int nMaiuscole { get { return _nMaiuscole; } set { _nMaiuscole = value; } }
    public int nMinuscole { get { return _nMinuscole; } set { _nMinuscole = value; } }
    public int nNumeri { get { return _nNumeri; } set { _nNumeri = value; } }
    public int lunghezzaminima { get { return _lunghezzaminima; } set { _lunghezzaminima = value; } }
    public int durata { get { return _durata; } set { _durata = value; } }
    public DateTime scadenza { get { return _scadenza; } set { _scadenza = value; } }

    public password()
    {   lunghezzaminima = 8;
        nMaiuscole = 1;
        nMinuscole = 1;
        nNumeri = 1;
        string s = Convert.ToString(DateTime.Now);
        int r = Convert.ToInt16(s.Substring(s.Length - 1, 1) + s.Substring(s.Length - 4, 1));
        rnd = new Random(r);
    }
    public string CalcolaPassword(string n, string c, DateTime dt) // passo due stringhe (nome e cognome), una data (nascita, dtla, altro) e ritorno la nuova password
    {
        string pwd, sdtn, sdt;
        int l;

        sdtn = Convert.ToString(DateTime.Now);
        sdt = Convert.ToString(dt);
        l = sdt.Length;       

        if (sdt.Length > 10) sdt = sdt.Substring(l - 4, 1) + sdt.Substring(l - 1, 1); // i minuti e  i secondi dell'ultimo collegamento
        sdtn = sdtn.Substring(sdtn.Length - 1, 1) + sdtn.Substring(sdtn.Length - 4, 1); // i centesimi e i secondi attuali
        pwd = n.Substring(1, 1) + rnd.Next(999).ToString() + sdt + c.Substring(0, 2) + n.Substring(0, 1) + sdtn + rnd.Next(999).ToString();
        return (pwd);
    }
    public string CalcolaPasswordCasuale(int lmin, int cmaiuscole, int cminuscole, int nnumeri)
    {
        int limite = lmin > 5 ? lmin : lunghezzaminima;
        int nmaiu = 0, nminu = 0 , nnum = 0;
        nmaiu = cmaiuscole > nMaiuscole ? cmaiuscole : nMaiuscole;
        nminu = cminuscole > nMinuscole ? cminuscole : nMinuscole;
        nnum = nnumeri > nNumeri ? nnumeri : nNumeri;
        string pwd = ""; int ma = 0, mi = 0, nu = 0, i = 0;
        do
        {
            i++;
            switch (rnd.Next(1, 4))
            {
                case 1: pwd += maiuscola(); ma++; break;
                case 2: pwd += minuscola(); mi++; break;
                case 3: pwd += numero(); nu++; break;
            }
        } while (i <= limite && ma <= nmaiu && mi <= nminu && nu <= nnum);
        return (pwd);
    }
    public static string GetHash(HashAlgorithm hashAlgorithm, string input)
    {
        // Convert the input string to a byte array and compute the hash.
        byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        var sBuilder = new StringBuilder();
        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        // Return the hexadecimal string.
        return sBuilder.ToString();
    }
    private char maiuscola()
    {  
        return ( (char)rnd.Next(65,90) );
    }
    private char minuscola()
    {
        return ((char)rnd.Next(97, 112));
    }
    private char numero()
    {
        return ((char)rnd.Next(48, 57));
    }
}
