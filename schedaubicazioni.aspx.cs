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
using System.IO;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using System.Text;

public partial class schedaubicazioni : System.Web.UI.Page
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
    public string formatoora = "hh:mm";
    public static string[] filesfoto = new string[4];

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
        {
			checkSession();
			LeggiUbicazioni();
            ddlProvincia.Items.Clear();
            LeggiProvince();
            if (Request.QueryString["ubi"] != null)
                CaricaUbicazione(Request.QueryString["ubi"].ToString()); 
            else
                CaricaUbicazione(ddlUbi.SelectedValue.ToString());
            //string s = ddlMezzi.SelectedValue.ToString();
            //s = ddlMezzi.SelectedItem.ToString();
            // devo caricare i dati della prima ubicazione.... forse
        }
    }
	private bool checkSession()
	{
		idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
		string s;
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
	protected void ddlUbi_SelectedIndexChanged(object sender, EventArgs e)
    {
        // si deve fare una procedura che legga i dati dell'ubicazione selezionate.... quindi si deve 
        // in leggi ubicazioni tirar su anche il codice ubicazione
        CaricaUbicazione(ddlUbi.SelectedValue.ToString());
    }

    protected bool controllavalidita(string ext)
    {
        string[] validFileTypes = { "bmp", "gif", "png", "jpg", "jpeg"};        
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
    protected void bSalva_Click(object sender, EventArgs e)
    {
        // devo fare tutti i controlli per cedere la validità dei campi obbligatori        
        // devo controllare se ci ubicazioni già caricate (stessa via, stesso civico, stesso palazzo, stessa città
        string s = "";
        DateTime t;
        if (DateTime.TryParse(tDalle.Text, out t)) tDalle.Text = t.ToShortTimeString(); else s += "Ora apertura; ";
        if (tAlle.Text.Length >= 2 && tAlle.Text.Substring(0, 2) == "24") tAlle.Text = "23:59:59";
        if (DateTime.TryParse(tAlle.Text, out t)) tAlle.Text = t.ToShortTimeString(); else s += "Ora chiusura; ";
        if (s.Length > 0)
        {
            sStato.Text = "ATTENZIONE! I dati inseriti non sono corretti o sono inconsistenti: " + s; return;
        }
        string sqlfoto = "";
        bool copiomappa = false, copiofoto = false;
        string SaveLocation = Server.MapPath("Data") + "\\";
        if ((fuMappa.PostedFile != null) && (fuMappa.PostedFile.ContentLength > 0))
            copiomappa = controllavalidita(System.IO.Path.GetExtension(fuMappa.PostedFile.FileName));
        if ((fuFoto.PostedFile != null) && (fuFoto.PostedFile.ContentLength > 0))
            copiofoto = controllavalidita(System.IO.Path.GetExtension(fuFoto.PostedFile.FileName));

		checkSession();
		// copio il file in locale
		msg = "";
        try
        {
            if (copiomappa)
            {
                fuMappa.PostedFile.SaveAs(SaveLocation + ddlUbi.SelectedValue + "_mappa" + System.IO.Path.GetExtension(fuMappa.PostedFile.FileName)); // copio il file sul server.....
                filesfoto[0] = ddlUbi.SelectedValue + "_mappa" + System.IO.Path.GetExtension(fuMappa.PostedFile.FileName);
                sqlfoto = ", nomefoto1=\'" + filesfoto[0] + "\'";
                iMappa.ImageUrl = "DATA//" + filesfoto[0];
            }
            if (copiofoto)
            {
                fuFoto.PostedFile.SaveAs(SaveLocation + ddlUbi.SelectedValue + "_foto" + System.IO.Path.GetExtension(fuFoto.PostedFile.FileName)); // copio il file sul server.....
                filesfoto[1] = ddlUbi.SelectedValue + "_foto" + System.IO.Path.GetExtension(fuFoto.PostedFile.FileName);
                sqlfoto += ", nomefoto2=\'" + filesfoto[1] + "\'";
                iFoto.ImageUrl = "DATA//" + filesfoto[1] + "\'";
            }
        }
        catch (Exception ex)
        {
            sStato.Text = "Copia file/s non riuscita! Error: " + ex.Message;                
            return;
        }
        //if (filesfoto[0] == "") filesfoto[0] = "Data" +"\\" + Lmappa.Text;
        string where;
        string index = ddlUbi.SelectedValue;
        //---------------------------
        s = "update ubicazione set ubicazione=\'" + tUbi.Text + "\', via=\'" + tVia.Text + "\', civico=\'" + tCivico.Text + "\'" + sqlfoto; //, foto1=@f1 ";
        s += ", comune_ek=\'" + ddlComune.SelectedValue.ToString() + "\', descrizione=\'" + tDescrizione.Text + "\', apertodalle=cast(\'" + tDalle.Text + "\' as time), apertofino=Cast(\'" + tAlle.Text + "\' as time), abilitato=" + (cbAbilitata.Checked ? "1" : "0");
        where = " where id=" + ddlUbi.SelectedValue + " and ubicazione=\'" + tUbi.Text.Trim().ToString() + "\'";
        s += where;
        msg = "";
        FBConn.openaFBConn(out msg);
        if (msg.Length >= 1)
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
        else
        {
            try
            {
                FBConn.FBc.CommandText = s;
                FBConn.EsegueCmd(s, out msg);
				if (msg == "0")
					sStato.Text = "Attenzione! Non è stato salvato alcun records. NON è POSSIBILE CAMBIARE NOME ALL'UBICAZIONE. Eventualmente cancellala e rinseriscila.";
				else
				{
					id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
					CLogga.logga("Ubicazioni", s + where, 2, "Modifica ubicazioni", id.ToString(), ddlUbi.SelectedValue.ToString(), out msgl);
				}
			}
            catch (Exception ex)
            {
                sStato.Text = "ATTENZIONE: lettura file foto, non andata a buon fine! " + ex.ToString() + " - Riprovare o chiamare l'assistenza al numero " + (string)Session["assistenza"];
                FBConn.closeaFBConn(out msg);
                return;
            }
        }
        bool ok = FBConn.closeaFBConn(out msg);
		Response.Redirect("schedaubicazioni.aspx?ubi=" + ddlUbi.SelectedValue); // ricarico la pagina con visualizzazione dell'ubicazione desiderata
    }

    protected void bInsert_Click(object sender, EventArgs e)
    {
        // devo fare tutti i controlli per cedere la validità dei campi obbligatori        
        // devo controllare se ci ubicazioni già caricate (stessa via, stesso civico, stesso palazzo, stessa città
        string s = "";
        DateTime t;
		Int32 idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
		if (DateTime.TryParse(tDalle.Text, out t)) tDalle.Text = t.ToShortTimeString(); else s += "Ora apertura; ";
        if (tAlle.Text.Length >= 2 && tAlle.Text.Substring(0, 2) == "24") tAlle.Text = "23:59:59";
        if (DateTime.TryParse(tAlle.Text, out t)) tAlle.Text = t.ToShortTimeString(); else s += "Ora chiusura; ";
        if (s.Length > 0) {
            sStato.Text = "ATTENZIONE! I dati inseriti non sono corretti o sono inconsistenti: " + s; return; }
        msg = "";
		checkSession();
		FBConn.openaFBConn(out msg);
        if (msg.Length >= 1)
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
        else
        {
            msg = ""; 
            string where, id="";
            s = "SELECT a.*, b.* FROM ubicazione as a left join comuni as b on a.comune_ek = b.comune_k ";
            where = "where ubicazione =\'" + tUbi.Text + "\'";
            ds.Clear();
            ds = FBConn.getfromDSet(s + where, "cègià", out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["cègià"].Rows.Count > 0)
                {
                    sStato.Text = "ATTENZIONE: una sede con questo nome è già presente nel database!";
                }
                else //posso inserire nuova sede
                {
                    // devo controllare se almeno uno dei due controllo upload contengono valori.... se si devo copiare i files, ricavare il nome e salvare i dati
                    string sqlfoto = "", sqlvalue="";
                    bool copiomappa = false, copiofoto = false;
                    string SaveLocation = Server.MapPath("Data") + "\\";
                    if ((fuMappa.PostedFile != null) && (fuMappa.PostedFile.ContentLength > 0))
                        copiomappa = controllavalidita(System.IO.Path.GetExtension(fuMappa.PostedFile.FileName));
                    if ((fuFoto.PostedFile != null) && (fuFoto.PostedFile.ContentLength > 0))
                        copiofoto = controllavalidita(System.IO.Path.GetExtension(fuFoto.PostedFile.FileName));
                    msg = "";
                    try
                    {
                        if (copiomappa)
                        {
                            fuMappa.PostedFile.SaveAs(SaveLocation + "_mappa" + System.IO.Path.GetExtension(fuMappa.PostedFile.FileName)); // copio il file sul server.....
                            filesfoto[0] = "_mappa" + System.IO.Path.GetExtension(fuMappa.PostedFile.FileName);
                            sqlfoto = ", nomefoto1"; sqlvalue = ", " + filesfoto[0] + "\'";
                            iMappa.ImageUrl = "DATA//" + filesfoto[0];
                        }
                        if (copiofoto)
                        {
                            fuFoto.PostedFile.SaveAs(SaveLocation +  "_foto" + System.IO.Path.GetExtension(fuFoto.PostedFile.FileName)); // copio il file sul server.....
                            filesfoto[1] =  "_foto" + System.IO.Path.GetExtension(fuFoto.PostedFile.FileName);
                            sqlfoto += ", nomefoto2"; sqlvalue = ", " + filesfoto[0] + "\'";
                            iFoto.ImageUrl = "DATA//" + filesfoto[1] + "\'";
                        }
                    }
                    catch (Exception ex)
                    {
                        sStato.Text = "Copia file/s non riuscita! Error: " + ex.Message;
                        return;
                    }

                    s = "insert into ubicazione (id, ubicazione, via, civico, comune_ek, descrizione, apertodalle, apertofino, abilitato" + (copiomappa ? ", nomefoto1" : "") + (copiofoto ? ", nomefoto2" : "") + ") values (null, ";
                    s +="\'" + tUbi.Text + "\', \'" + tVia.Text + "\', \'" + tCivico.Text + "\', \'" + ddlComune.SelectedValue + "\', \'" + tDescrizione.Text + "\', cast(\'"+tDalle.Text+"\' as time), cast(\'" + tAlle.Text+"\' as time), " + (cbAbilitata.Checked ? "1" : "0") + (copiomappa ? ", \'" + filesfoto[0] + "\'" : "") + (copiofoto ? ", \'" + filesfoto[1] + "\'": "") + ")  returning id ";
                    msg = "";
                    Int32 rr = FBConn.AddCmd(s, out msg);
                    if (rr > 0) CLogga.logga("Ubicazioni", s, 1, "Inserimento ubicazioni", idu.ToString(), rr.ToString(), out msgl);
                    if (rr > 0 && (copiomappa || copiofoto)) // devo cambiare il contenuto dei due campi foto1 e foto2 e il nome dei due file foto
                    {
						//CLogga.logga("Ubicazioni", s, 1, "Inserimento ubicazioni", idu.ToString(), rr.ToString(), out msgl);
						s = "select id, nomefoto1, nomefoto2 from ubicazione where ubicazione=\'" + tUbi.Text + "\'"; // se serve....
                        ds.Clear();
                        ds = FBConn.getfromDSet(s, "ubi", out msg);

                        if (msg == "" && ds.Tables["ubi"].Rows.Count == 1) // ok ora update
                        {
                            id = ds.Tables["ubi"].Rows[0]["id"].ToString();
                            filesfoto[0] = id + (ds.Tables["ubi"].Rows[0]["nomefoto1"] != DBNull.Value ? ds.Tables["ubi"].Rows[0]["nomefoto1"].ToString() : "");
                            filesfoto[1] = id + (ds.Tables["ubi"].Rows[0]["nomefoto2"] != DBNull.Value ? ds.Tables["ubi"].Rows[0]["nomefoto2"].ToString() : "");
                            s = "update ubicazione set nomefoto1=\'" + filesfoto[0] + "\', nomefoto2=\'" + filesfoto[1] + "\' where id=\'" + id + "\'";
                            rr = FBConn.EsegueCmd(s, out msg);
                            FBConn.closeaFBConn(out msg);
                            if (rr == 1)
                            {
                                try
                                {
                                    if (filesfoto[0] != "") File.Move(SaveLocation + filesfoto[0].Substring(filesfoto[0].IndexOf("_")), SaveLocation + filesfoto[0]);
                                    if (filesfoto[1] != "") File.Move(SaveLocation + filesfoto[1].Substring(filesfoto[0].IndexOf("_")), SaveLocation + filesfoto[1]);
                                }
                                catch (Exception ex)
                                {
                                    sStato.Text = "ERRORE: file immagini non rinominati dopo insert. Contattare il servizio assistenza al numero " + (string)Session["assistenza"] + " errore: " + ex.ToString();
                                    return;
                                }
                            }
                            else
                            {
                                sStato.Text = "ERRORE: file immagini non aggiornate. Contattare il servizio assistenza al numero " + (string)Session["assistenza"];
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (rr < 0)
                        {
                            sStato.Text = "ERRORE: inserimento ubicazione non riusto. Contattare il servizio assistenza al numero " + (string)Session["assistenza"];
                            return;
                        }
                        sStato.Text = "Inserimento nuova sede avvenuto con successo";
                    }
                    LeggiUbicazioni();
                    CaricaUbicazione(id.Trim().Length > 0 ? id : "");
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
            s = "delete FROM ubicazione ";
            where = "where id=" + ddlUbi.SelectedValue + " and ubicazione =\'" + tUbi.Text + "\'";
            msg = "";
            Int32 rr = FBConn.EsegueCmd(s + where, out msg);
            if (rr != 1)
            {
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
                return;
            }
            else
                sStato.Text = "Cancellazione di " + tUbi.Text + " avvenuta con successo!";
			id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
			CLogga.logga("Ubicazioni", s + where, 3, "Cancellazione ubicazione", id.ToString(), ddlUbi.SelectedValue.ToString(), out msgl);
			string SaveLocation = Server.MapPath("Data") + "\\";
            foreach (string f in filesfoto)
                if (f != "" && f != null) File.Delete(SaveLocation + f);
            LeggiUbicazioni();
            CaricaUbicazione(ddlUbi.SelectedValue);
        }
    }
    protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
    {
        // devo leggere la tabella Comuni
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
            msg = ddlProvincia.SelectedValue;
            ds = FBConn.getfromDSet("SELECT a.* from comuni a where provincia_ek = " + msg + " order by comune", "comuni", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["comuni"].Rows.Count > 0)
                {
                    ddlComune.Items.Clear();
                    string s = "", ss = "";
                    for (int i = 0; i < ds.Tables["comuni"].Rows.Count; i++)
                    {
                        s = ds.Tables["comuni"].Rows[i]["comune"] == DBNull.Value ? "" : ds.Tables["comuni"].Rows[i]["comune"].ToString();
                        ss = ds.Tables["comuni"].Rows[i]["comune_k"] == DBNull.Value ? "" : ds.Tables["comuni"].Rows[i]["comune_k"].ToString();
                        ddlComune.Items.Insert(i, new ListItem(s, ss));
                    }
                }
                else
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella UBICAZIONI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
        ddlComune.Focus();
    }

    public void LeggiProvince() // devo leggere la tabella Province
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
            ds.Clear();
            ds = FBConn.getfromDSet("select * from province order by provincia", "province", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                ddlProvincia.Items.Clear();
                if (ds.Tables["province"].Rows.Count > 0)
                {
                    string s = "", ss = ""; int k = 0;
                    for (int i = 0; i < ds.Tables["province"].Rows.Count; i++)
                    {
                        s = ds.Tables["province"].Rows[i]["provincia"] == DBNull.Value ? "" : ds.Tables["province"].Rows[i]["provincia"].ToString();
                        ss = ds.Tables["province"].Rows[i]["provincia_k"] == DBNull.Value ? "" : ds.Tables["province"].Rows[i]["provincia_k"].ToString();
                        ddlProvincia.Items.Insert(i, new ListItem(s, ss));
                        if (s == "Trento") k = i;
                    }
                    ddlProvincia.SelectedIndex = k;
                    LeggiComuni();
                }
                else
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella UBICAZIONI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }

    public void LeggiUbicazioni()
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
            ds.Clear();
            string s;
            s = "SELECT a.*, b.* FROM ubicazione as a left join comuni as b on a.comune_ek = b.comune_k order by b.comune, a.ubicazione, a.via";
            ds = FBConn.getfromDSet(s, "ubi", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                ddlUbi.Items.Clear();
                if (ds.Tables["ubi"].Rows.Count > 0)
                {
                    string ss = "";
                    s = "";
                    for (int i = 0; i < ds.Tables["ubi"].Rows.Count; i++)
                    {
                        s = ds.Tables["ubi"].Rows[i]["comune"] == DBNull.Value ? "" : ds.Tables["ubi"].Rows[i]["comune"].ToString();
                        s += ds.Tables["ubi"].Rows[i]["ubicazione"] == DBNull.Value ? "" : " - " + ds.Tables["ubi"].Rows[i]["ubicazione"].ToString();
                        s += ds.Tables["ubi"].Rows[i]["via"] == DBNull.Value ? "" : " - " + ds.Tables["ubi"].Rows[i]["via"].ToString();
                        s += ds.Tables["ubi"].Rows[i]["civico"] == DBNull.Value ? "" : " " + ds.Tables["ubi"].Rows[i]["civico"].ToString();
                        ss = ds.Tables["ubi"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["ubi"].Rows[i]["id"].ToString();
                        ddlUbi.Items.Insert(i, new ListItem(s, ss));
                    }
                }
                else
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella UBICAZIONI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }

    public void CaricaUbicazione(string id)
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
            s = "SELECT a.*, b.*, c.* FROM ubicazione as a left join comuni as b on a.comune_ek = b.comune_k left join PROVINCE as c on b.provincia_ek = c.provincia_k " + (id == "" ? "" : " where a.id=" + id);
            ds.Clear();
            ds = FBConn.getfromDSet(s, "ubi", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["ubi"].Rows.Count >= 1)
                {
                    id = ds.Tables["ubi"].Rows[0]["id"] == DBNull.Value ? "" : ds.Tables["ubi"].Rows[0]["id"].ToString();
                    tUbi.Text = ds.Tables["ubi"].Rows[0]["ubicazione"] == DBNull.Value ? "" : ds.Tables["ubi"].Rows[0]["ubicazione"].ToString();
                    cbAbilitata.Checked = ds.Tables["ubi"].Rows[0]["abilitato"] == DBNull.Value ? false : ds.Tables["ubi"].Rows[0]["abilitato"].ToString() == "1" ? true : false;
                    string provincia = ds.Tables["ubi"].Rows[0]["provincia_ek"] == DBNull.Value ? "" : ds.Tables["ubi"].Rows[0]["provincia_ek"].ToString();
                    string comune = ds.Tables["ubi"].Rows[0]["comune_ek"] == DBNull.Value ? "" : ds.Tables["ubi"].Rows[0]["comune_ek"].ToString();
                    s = ds.Tables["ubi"].Rows[0]["via"] == DBNull.Value ? "" : ds.Tables["ubi"].Rows[0]["via"].ToString();
                    tVia.Text = s;
                    s = ds.Tables["ubi"].Rows[0]["civico"] == DBNull.Value ? "" : ds.Tables["ubi"].Rows[0]["civico"].ToString();
                    tCivico.Text = s;
                    s = ds.Tables["ubi"].Rows[0]["descrizione"] == DBNull.Value ? "" : ds.Tables["ubi"].Rows[0]["descrizione"].ToString();
                    tDescrizione.Text = s;
                    //TimeSpan oramin;
                    // String.Format("{0:t}", row("timereported"))
                    //s = String.Format("{0:HH:mm}", Convert.ToDateTime(ds.Tables["ubi"].Rows[0]["apertodalle"]));
                    //tDalle.Text = ds.Tables["ubi"].Rows[0]["apertodalle"] == DBNull.Value ? "" : s;
                    tDalle.Text = ds.Tables["ubi"].Rows[0]["apertodalle"] == DBNull.Value ? "" : string.Format("{0}", (ds.Tables["ubi"].Rows[0]["apertodalle"].ToString()));
                    tAlle.Text = ds.Tables["ubi"].Rows[0]["apertofino"] == DBNull.Value ? "" : string.Format("{0}", ds.Tables["ubi"].Rows[0]["apertofino"]);
                    Lmappa.Text = ds.Tables["ubi"].Rows[0]["nomefoto1"] == DBNull.Value ? "" : ds.Tables["ubi"].Rows[0]["nomefoto1"].ToString();
                    iMappa.ImageUrl = "DATA//" + Lmappa.Text;
                    //iMappa.ResolveClientUrl("DATA//" + Lmappa.Text);
                    Lfoto.Text = ds.Tables["ubi"].Rows[0]["nomefoto2"] == DBNull.Value ? "" : ds.Tables["ubi"].Rows[0]["nomefoto2"].ToString();
                    iFoto.ImageUrl = "DATA//" + Lfoto.Text;
                    //iFoto.ResolveClientUrl("DATA//" + Lfoto.Text);
                    filesfoto[0] = Lmappa.Text;
                    filesfoto[1] = Lfoto.Text;
                    ddlProvincia.SelectedValue = provincia;
                    LeggiComuni(); // una volta cambiata la provincia..... devo adeguare la ddl comuni
                    ddlComune.SelectedValue = comune;
                    ddlUbi.SelectedValue = id;
                }
                else
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella UBICAZIONI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }

    protected void LeggiComuni()
    {
        // devo leggere la tabella Comuni
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
            msg = ddlProvincia.SelectedValue;
            ds = FBConn.getfromDSet("SELECT a.* from comuni a where provincia_ek = \'" + msg + "\' order by comune", "comuni", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["comuni"].Rows.Count > 0)
                {
                    ddlComune.Items.Clear();
                    string s = "", ss="";
                    for (int i = 0; i < ds.Tables["comuni"].Rows.Count; i++)
                    {
                        s = ds.Tables["comuni"].Rows[i]["comune"] == DBNull.Value ? "" : ds.Tables["comuni"].Rows[i]["comune"].ToString();
                        ss = ds.Tables["comuni"].Rows[i]["comune_k"] == DBNull.Value ? "" : ds.Tables["comuni"].Rows[i]["comune_k"].ToString();
                        ddlComune.Items.Insert(i, new ListItem(s, ss));
                    }
                }
                else
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella UBICAZIONI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }

    protected void btnMappaUpload_Click(object sender, EventArgs e)
    {
        string[] validFileTypes = { "bmp", "gif", "png", "jpg", "jpeg", "pdf" };
        string ext = System.IO.Path.GetExtension(fuMappa.PostedFile.FileName);
        bool isValidFile = false;
        for (int i = 0; i < validFileTypes.Length; i++)
        {
            if (ext == "." + validFileTypes[i])
            {
                isValidFile = true;
                break;
            }
        }
        if (!isValidFile)
        {
            Lmappa.ForeColor = System.Drawing.Color.Red;
            Lmappa.Text = "File non valido. Selezionare un file con estensione " + string.Join(",", validFileTypes);
        }
        else
        {
            Lmappa.ForeColor = System.Drawing.Color.Green;
            Lmappa.Text = "File caricato correttamente.";
            sStato.Text = "Nome file = " + fuMappa.PostedFile.FileName.ToString();           
        }
    }

    protected void btnFotoUpload_Click(object sender, EventArgs e)
    {
        string[] validFileTypes = { "bmp", "gif", "png", "jpg", "jpeg", "pdf" };
        string ext = System.IO.Path.GetExtension(fuFoto.PostedFile.FileName);
        bool isValidFile = false;
        for (int i = 0; i < validFileTypes.Length; i++)
        {
            if (ext == "." + validFileTypes[i])
            {
                isValidFile = true;
                break;
            }
        }
        if (!isValidFile)
        {
            Lfoto.ForeColor = System.Drawing.Color.Red;
            Lfoto.Text = "File non valido. Selezionare un file con estensione " + string.Join(",", validFileTypes);
        }
        else
        {
            Lfoto.ForeColor = System.Drawing.Color.Green;
            Lfoto.Text = "File caricato correttamente.";
            filesfoto[1] = fuMappa.PostedFile.FileName.ToString();
        }
    }
}