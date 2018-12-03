using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class schedaveicolo : System.Web.UI.Page
{
    public ConnessioneFB FBConn = new ConnessioneFB();
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
            if (Request.QueryString["p"] != null)
            {
                if (Request.QueryString["p"] == "a")
                {
                    bDel.Visible = false;
                    bInsert.Visible = false;
                }
            }
			checkSession();
			LeggiAutomezzi();
            LeggiTarga();
            LeggiNumero();  
            LeggiMarca();
            LeggiModello();
            LeggiAlimentazione();
            LeggiDotazioni();
            LeggiClassificazioni();
            LeggiCambio();
            LeggiGomme();
            LeggiTrazione();
            LeggiUbi();
            ddlEuro.Items.Clear();
            for (int i = 0; i < 7; i++)
                ddlEuro.Items.Insert(i, new ListItem(i.ToString(), i.ToString()));
            // ddlEuro.SelectedValue = "6";
            ddlPosti.Items.Clear();
            for (int i = 0; i < 10; i++)
                ddlPosti.Items.Insert(i, new ListItem((i+1).ToString(), i.ToString()));
            LeggiClassificazioni();
            // ddlPosti.SelectedValue = "4";
            if (Request.QueryString["v"] != null)
            {
                ddlMezzi.SelectedValue = Request.QueryString["v"].ToString();
                ddlMezzi_SelectedIndexChanged(this, null);
            }
            else
                CaricaMezzo(ddlMezzi.SelectedValue);
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

    protected void bSalva_Click(object sender, EventArgs e) // MODIFICA
    {
        bool cplibretto = false, cpdx = false, cpfr = false, cpint = false;         
        if ((fuLibretto.PostedFile != null) && (fuLibretto.PostedFile.ContentLength > 0))
            cplibretto = controllavalidita(System.IO.Path.GetExtension(fuLibretto.PostedFile.FileName));
        if ((fuFotodx.PostedFile != null) && (fuFotodx.PostedFile.ContentLength > 0))
            cpdx = controllavalidita(System.IO.Path.GetExtension(fuFotodx.PostedFile.FileName));
        if ((fuFronte.PostedFile != null) && (fuFronte.PostedFile.ContentLength > 0))
            cpfr = controllavalidita(System.IO.Path.GetExtension(fuFronte.PostedFile.FileName));
        if ((fuInt.PostedFile != null) && (fuInt.PostedFile.ContentLength > 0))
            cpint = controllavalidita(System.IO.Path.GetExtension(fuInt.PostedFile.FileName));

		checkSession();
		// copio il file in locale
		msg = "";
        string sqlfoto = "";
        string SaveLocation = Server.MapPath("Data") + "\\";
        try
        {
            if (cplibretto)
            {
                fuLibretto.PostedFile.SaveAs(SaveLocation + ddlMezzi.SelectedValue + "_libretto" + System.IO.Path.GetExtension(fuLibretto.PostedFile.FileName)); // copio il file sul server.....
                filesfoto[0] = ddlMezzi.SelectedValue + "_libretto" + System.IO.Path.GetExtension(fuLibretto.PostedFile.FileName);
                sqlfoto = ", libretto=\'" + filesfoto[0] + "\'";
                iLibretto.ImageUrl = "DATA//" + filesfoto[0];
            }
            if (cpdx)
            {
                fuFotodx.PostedFile.SaveAs(SaveLocation + ddlMezzi.SelectedValue + "_fotodx" + System.IO.Path.GetExtension(fuFotodx.PostedFile.FileName)); // copio il file sul server.....
                filesfoto[1] = ddlMezzi.SelectedValue + "_fotodx" + System.IO.Path.GetExtension(fuFotodx.PostedFile.FileName);
                sqlfoto += ", foto1=\'" + filesfoto[1] + "\'";
                iFotodx.ImageUrl = "DATA//" + filesfoto[1] + "\'";
            }
            if (cpfr)
            {
                fuFronte.PostedFile.SaveAs(SaveLocation + ddlMezzi.SelectedValue + "_fotofr" + System.IO.Path.GetExtension(fuFronte.PostedFile.FileName)); // copio il file sul server.....
                filesfoto[2] = ddlMezzi.SelectedValue + "_fotofr" + System.IO.Path.GetExtension(fuFronte.PostedFile.FileName);
                sqlfoto += ", foto2=\'" + filesfoto[2] + "\'";
                iFotofr.ImageUrl = "DATA//" + filesfoto[2] + "\'";
            }
            if (cpint)
            {
                fuInt.PostedFile.SaveAs(SaveLocation + ddlMezzi.SelectedValue + "_fotoint" + System.IO.Path.GetExtension(fuInt.PostedFile.FileName)); // copio il file sul server.....
                filesfoto[3] = ddlMezzi.SelectedValue + "_fotoint" + System.IO.Path.GetExtension(fuInt.PostedFile.FileName);
                sqlfoto += ", foto3=\'" + filesfoto[3] + "\'";
                iFotoint.ImageUrl = "DATA//" + filesfoto[3] + "\'";
            }
        }
        catch (Exception ex)
        {
            sStato.Text = "Copia file/s non riuscita! Error: " + ex.Message;
            return;
        }
        if (cbLibretto.Checked)
        {
            sqlfoto = ", libretto=\'\'";
        }
        if (cbdx.Checked)
        {
            sqlfoto = ", foto1=\'\'";
        }
        if (cbfr.Checked)
        {
            sqlfoto = ", foto2=\'\'";
        }
        if (cbint.Checked)
        {
            sqlfoto = ", foto3=\'\'";
        }

        // devo fare tutti i controlli per cedere la validità dei campi obbligatori        
        // devo controllare se ci ubicazioni già caricate (stessa via, stesso civico, stesso palazzo, stessa città
        msg = "";
        FBConn.openaFBConn(out msg);
        if (msg.Length >= 1)
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
        else
        {  
            msg = "";
            ds.Clear();
            string where; //int i = ddlMezzi.SelectedIndex;
            // devoontrollare se ci sono già records con quella targa e quel numero
            s = "select id, targa from mezzi ";
            where = " where targa=\'" + tTarga.Text + "\' and id != \'" + ddlMezzi.SelectedValue + "\'";
            Int32 rr = FBConn.EsegueCmd(s + where, out msg);
            if (rr > 0)
            {
                s = "ATTENZIONE: hai cercato di cambiare targa, impostando i valori uguali a veicoli già esistenti! Verifica che non ci siano veicoli con targa uguali a quelli inseriti!"; 
                sStato.Text = s;
                ShowPopUpMsg(s);
                return;                
            }
            DateTime dt;
            dt = Convert.ToDateTime(tScadenza.Text);
            // targa, numero e id..... non serve salvarli
            s = "update mezzi set abilitato=\'" + (cbAbilitata.Checked ? "1" : "0") + "\', blackbox=\'" + (cbBlackBox.Checked ? "1" : "0") + "\', marca_ek=\'" + ddlProduttore.SelectedValue + "\', modello_ek=\'" + ddlModello.SelectedValue + "\'" + sqlfoto;
            s += ", trazione_ek =\'" + ddlTrazione.SelectedValue + "\', alimentazione_ek=\'" + ddlAlimentazione.SelectedValue + "\', posti=\'" + ddlPosti.SelectedValue + "\', euro=\'" + ddlEuro.SelectedValue + "\', cambio_ek=\'" + ddlCambio.SelectedValue;
            s += "\', cavalli=\'" + (tCavalli.Text.Trim() == "" ? "0" : tCavalli.Text.Trim()) + "\', dotazione1_ek=\'" + ddlDotazione1.SelectedValue + "\', dotazione2_ek=\'" + ddlDotazione2.SelectedValue + "\', portata=\'" + (tPortata.Text.Trim() == "" ? "0" : tPortata.Text.Trim()) + "\', colore=\'" + tColore.Text + "\', descrizione=\'" + tDescrizione.Text;
            s += "\', ubicazione_ek=\'" + ddlPosteggio.SelectedValue + "\', posteggio=\'" + tPosteggio.Text.Trim() + "\', ritirochiavi_ek=\'" + ddlRitiro.SelectedValue + "\', classificazione_EK=\'" + ddlCla.SelectedValue + "\', gomme_ek=\'" + ddlGomme.SelectedValue + "\', scadenza=cast(\'" + dt.Year.ToString() +"/" + dt.Month.ToString() + "/" + dt.Day.ToString() + "\' as date) ";
            where = " where id=" + ddlMezzi.SelectedValue;
            rr = FBConn.EsegueCmd(s + where, out msg);
			if (rr == 1)
			{
				sStato.Text = "Modifica eseguita!";
				idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
				CLogga.logga("mezzi", s, 2, "Modifica mezzi", idu, out msgl);
			}
			else
			{
				sStato.Text = "ATTENZIONE: modifica non eseguitaeseguita! " + msg;
				return;
			}
        }
        string sv = ddlMezzi.SelectedValue;
        LeggiNumero();
        LeggiAutomezzi();
        CaricaMezzo(sv);
        ddlMezzi.SelectedValue = sv;
        //Response.Redirect("schedaveicolo.aspx?v=" + ddlMezzi.SelectedValue); // ricarico la pagina con visualizzazione dell'ubicazione desiderata
    }

    protected void bInsert_Click(object sender, EventArgs e)
    {
		checkSession();
		// devo fare tutti i controlli per cedere la validità dei campi obbligatori        
		// devo controllare se ci sono veicoli già caricate (stessa targa)
		msg = "";
        FBConn.openaFBConn(out msg);
        if (msg.Length >= 1)
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
        else
        {
            msg = "";
            string where;
            s = "SELECT * FROM mezzi ";
            where = "where targa =\'" + tTarga.Text + "\'";
            ds.Clear();
            ds = FBConn.getfromDSet(s + where, "cègià", out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
				int posteggio = 0;
				int.TryParse(tPosteggio.Text, out posteggio);

                if (ds.Tables["cègià"].Rows.Count > 0)
                {
                    sStato.Text = "ATTENZIONE: un veicolo con stessa targa è già presente nel database!";
                }
                else //posso inserire nuova sede
                {
                    bool cplibretto = false, cpdx = false, cpfr = false, cpint = false;
                    if ((fuLibretto.PostedFile != null) && (fuLibretto.PostedFile.ContentLength > 0))
                        cplibretto = controllavalidita(System.IO.Path.GetExtension(fuLibretto.PostedFile.FileName));
                    if ((fuFotodx.PostedFile != null) && (fuFotodx.PostedFile.ContentLength > 0))
                        cpdx = controllavalidita(System.IO.Path.GetExtension(fuFotodx.PostedFile.FileName));
                    if ((fuFronte.PostedFile != null) && (fuFronte.PostedFile.ContentLength > 0))
                        cpfr = controllavalidita(System.IO.Path.GetExtension(fuFronte.PostedFile.FileName));
                    if ((fuInt.PostedFile != null) && (fuInt.PostedFile.ContentLength > 0))
                        cpint = controllavalidita(System.IO.Path.GetExtension(fuInt.PostedFile.FileName));

                    // copio il file in locale
                    msg = "";
                    string sqlfoto = "", sqlvalue="";
                    string SaveLocation = Server.MapPath("Data") + "\\";
                    try
                    {
                        if (cplibretto)
                        {
                            fuLibretto.PostedFile.SaveAs(SaveLocation + ddlMezzi.SelectedValue + "_libretto" + System.IO.Path.GetExtension(fuLibretto.PostedFile.FileName)); // copio il file sul server.....
                            filesfoto[0] = "_libretto" + System.IO.Path.GetExtension(fuLibretto.PostedFile.FileName);
                            sqlfoto = ", libretto"; sqlvalue = ", \'" + filesfoto[0] + "\'";
                            iLibretto.ImageUrl = "DATA//" + filesfoto[0];
                        }
                        if (cpdx)
                        {
                            fuFotodx.PostedFile.SaveAs(SaveLocation + ddlMezzi.SelectedValue + "_fotodx" + System.IO.Path.GetExtension(fuFotodx.PostedFile.FileName)); // copio il file sul server.....
                            filesfoto[1] = "_fotodx" + System.IO.Path.GetExtension(fuFotodx.PostedFile.FileName);
                            sqlfoto += ", foto1" ; sqlvalue += ", \'" + filesfoto[1] + "\'";
                            iFotodx.ImageUrl = "DATA//" + filesfoto[1] + "\'";
                        }
                        if (cpfr)
                        {
                            fuFronte.PostedFile.SaveAs(SaveLocation + ddlMezzi.SelectedValue + "_fotofr" + System.IO.Path.GetExtension(fuFronte.PostedFile.FileName)); // copio il file sul server.....
                            filesfoto[2] = "_fotofr" + System.IO.Path.GetExtension(fuFronte.PostedFile.FileName);
                            sqlfoto += ", foto2";  sqlvalue += ", \'" + filesfoto[2] + "\'";
                            iFotofr.ImageUrl = "DATA//" + filesfoto[2] + "\'";
                        }
                        if (cpint)
                        {
                            fuInt.PostedFile.SaveAs(SaveLocation + ddlMezzi.SelectedValue + "_fotoint" + System.IO.Path.GetExtension(fuInt.PostedFile.FileName)); // copio il file sul server.....
                            filesfoto[3] = "_fotoint" + System.IO.Path.GetExtension(fuInt.PostedFile.FileName);
                            sqlfoto += ", foto3"; sqlvalue += ", \'" + filesfoto[3] + "\'";
                            iFotoint.ImageUrl = "DATA//" + filesfoto[3] + "\'";
                        }
                    }
                    catch (Exception ex)
                    {
                        sStato.Text = "Copia file/s non riuscita! Error: " + ex.Message;
                        return;
                    }
                    DateTime dt;
                    dt = Convert.ToDateTime(tScadenza.Text);
                    s = "insert into mezzi (id, targa, marca_ek, modello_ek, cavalli, kilowat, portata, euro, alimentazione_ek, colore, dotazione1_ek, dotazione2_ek, dotazione3_ek, ubicazione_ek, ritirochiavi_ek, numero, dotazioni, descrizione, abilitato, blackbox, trazione_ek, gomme_EK, classificazione_EK, cambio_EK, scadenza, posti, posteggio" + sqlfoto + ") values (null, ";
                    s += "\'" + tTarga.Text + "\', \'" + ddlProduttore.SelectedValue + "\', \'" + ddlModello.SelectedValue + "\', \'" + (tCavalli.Text.Trim() == "" ? "0" : tCavalli.Text.Trim()) + "\', \'0\', \'" + ( tPortata.Text.Trim() == "" ? "0" : tPortata.Text.Trim() ) + "\', \'" + ddlEuro.SelectedValue + "\', \'" + ddlAlimentazione.SelectedValue + "\'";
                    s += ", \'" + tColore.Text + "\', \'" + ddlDotazione1.SelectedValue + "\', \'" + ddlDotazione2.SelectedValue + "\', null, \'" + ddlPosteggio.SelectedValue + "\', \'" + ddlRitiro.SelectedValue + "\', \'" + tNumero.Text + "\', \'\', \'" + tDescrizione.Text + "\', " + (cbAbilitata.Checked ? "1" : "0") + ", " + (cbBlackBox.Checked ? "1" : "0") + ", \'" + ddlTrazione.SelectedValue + "\'";
                    s += ", \'" + ddlGomme.SelectedValue + "\', \'" + ddlCla.SelectedValue + "\', \'" + ddlCambio.SelectedValue + "\', cast(\'" + dt.Year.ToString() + "/" + dt.Month.ToString() + "/" + dt.Day.ToString() + "\' as date), \'" + ddlPosti.SelectedValue + "\', '" + posteggio.ToString().Trim() + "\'" +  sqlvalue + ")";
                    msg = "";
                    string id = "";
                    Int32 rr = FBConn.EsegueCmd(s, out msg);
                    if (rr == 1) // devo cambiare il contenuto dei due campi foto1 e foto2 e il nome dei due file foto
                    {
						idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
						CLogga.logga("mezzi", s, 1, "Insert mezzi", idu, out msgl);

						s = "select id, libretto, foto1, foto2, foto3 from mezzi where targa=\'" + tTarga.Text.Trim() + "\'"; // se serve....
                        ds.Clear();
                        ds = FBConn.getfromDSet(s, "mezzi", out msg);
                        if (msg == "" && ds.Tables["mezzi"].Rows.Count == 1) id = ds.Tables["mezzi"].Rows[0]["id"].ToString();
                        if (msg == "" && ds.Tables["mezzi"].Rows.Count == 1 && (cplibretto || cpdx || cpfr || cpint)) // ok ora update se ci sono foto
                        {
                            filesfoto[0] = id + (ds.Tables["mezzi"].Rows[0]["libretto"] != DBNull.Value ? ds.Tables["mezzi"].Rows[0]["libretto"].ToString() : "");
                            filesfoto[1] = id + (ds.Tables["mezzi"].Rows[0]["foto1"] != DBNull.Value ? ds.Tables["mezzi"].Rows[0]["foto1"].ToString() : "");
                            filesfoto[2] = id + (ds.Tables["mezzi"].Rows[0]["foto2"] != DBNull.Value ? ds.Tables["mezzi"].Rows[0]["foto2"].ToString() : "");
                            filesfoto[3] = id + (ds.Tables["mezzi"].Rows[0]["foto3"] != DBNull.Value ? ds.Tables["mezzi"].Rows[0]["foto3"].ToString() : "");
                            s = "update mezzi set libretto=\'" + filesfoto[0] + "\', foto1=\'" + filesfoto[1] + "\', foto2=\'" + filesfoto[2] + "\', foto3=\'" + filesfoto[3] + "\' where id=\'" + id + "\'";
                            rr = FBConn.EsegueCmd(s, out msg);
                            FBConn.closeaFBConn(out msg);
                            if (rr == 1)
                            {
                                try
                                {   // rinomino i files su server
                                    foreach (string f in filesfoto)
                                        if (f != "") System.IO.File.Move(SaveLocation + f.Substring(f.IndexOf("_")), SaveLocation + f);
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
                        sStato.Text = "ERRORE: inserimento automezzo non riuscito. Contattare il servizio assistenza al numero " + (string)Session["assistenza"];
                        return;
                    }
                    sStato.Text = "Inserimento eseguito.";
                    LeggiAutomezzi();
                    LeggiTarga();
                    LeggiNumero();
                    ddlMezzi.SelectedValue = id.ToString();
                    ddlMezzi_SelectedIndexChanged(this, null);  // o carica mezzo(id)
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
            s = "delete FROM mezzi ";
            where = "where targa=\'" + tTarga.Text + "\'";
            msg = "";
            Int32 rr = FBConn.EsegueCmd(s + where, out msg);

            string SaveLocation = Server.MapPath("Data") + "\\";
            foreach (string f in filesfoto)
                if (f != "" && f != null) System.IO.File.Delete(SaveLocation + f);

            if (rr != 1)
            {
                sStato.Text = "ATTENZIONE: automezzo nontrovato o si è verificato un\'errore durante la cancellazione delle immagini: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
                return;
            }
            else
                sStato.Text = "Cancellazione di " + tTarga.Text + " - " + tNumero.Text + " avvenuta con successo!";
			idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
			CLogga.logga("mezzi", s + where, 3, "Delete mezzi", idu, out msgl);
			LeggiAutomezzi();
            LeggiTarga();
            LeggiNumero();
            CaricaMezzo("");
        }
    }

    public void LeggiAutomezzi() // devo leggere la tabella Modello
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
            s = "SELECT a.id, a.targa, b.marca, c.modello FROM MEZZI a left join marca as b on a.marca_ek = b.id left join modello as c on a.modello_ek = c.id order by b.marca, c.modello, a.targa";
            ds.Clear();
            ds = FBConn.getfromDSet(s, "mezzicerca", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                ddlMezzi.Items.Clear();
                if (ds.Tables["mezzicerca"].Rows.Count > 0)
                {
                    string s = "", ss = "";
                    for (int i = 0; i < ds.Tables["mezzicerca"].Rows.Count; i++)
                    {
                        s = ds.Tables["mezzicerca"].Rows[i]["marca"] == DBNull.Value ? "" : ds.Tables["mezzicerca"].Rows[i]["marca"].ToString();
                        s += " " + (ds.Tables["mezzicerca"].Rows[i]["modello"] == DBNull.Value ? "" : ds.Tables["mezzicerca"].Rows[i]["modello"].ToString());
                        s += " " + (ds.Tables["mezzicerca"].Rows[i]["targa"] == DBNull.Value ? "" : ds.Tables["mezzicerca"].Rows[i]["targa"].ToString());
                        ss = ds.Tables["mezzicerca"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["mezzicerca"].Rows[i]["id"].ToString();
                        ddlMezzi.Items.Insert(i, new ListItem(s, ss));
                    }
                }
                else
                {
                    if (ds.Tables["mezzicerca"].Rows.Count != 0)
                        sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella MEZZI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
                }
            }
        }
    }

    public void LeggiMarca() // devo leggere la tabella Marca
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
            ds = FBConn.getfromDSet("select * from marca order by marca", "marca", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                ddlProduttore.Items.Clear();
                if (ds.Tables["marca"].Rows.Count > 0)
                {
                    string s = "", ss = ""; int k = 0;
                    for (int i = 0; i < ds.Tables["marca"].Rows.Count; i++)
                    {
                        s = ds.Tables["marca"].Rows[i]["marca"] == DBNull.Value ? "" : ds.Tables["marca"].Rows[i]["marca"].ToString();
                        ss = ds.Tables["marca"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["marca"].Rows[i]["id"].ToString();
                        ddlProduttore.Items.Insert(i, new ListItem(s, ss));                        
                    }
                }
                else
                    if (ds.Tables["marca"].Rows.Count != 0)
                        sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella PRODUTTORI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }

    public void LeggiModello() // devo leggere la tabella Modello
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
            ds = FBConn.getfromDSet("select * from modello order by modello", "modello", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                ddlModello.Items.Clear();
                if (ds.Tables["modello"].Rows.Count > 0)
                {
                    string s = "", ss = ""; int k = 0;
                    for (int i = 0; i < ds.Tables["modello"].Rows.Count; i++)
                    {
                        s = ds.Tables["modello"].Rows[i]["modello"] == DBNull.Value ? "" : ds.Tables["modello"].Rows[i]["modello"].ToString();
                        ss = ds.Tables["modello"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["modello"].Rows[i]["id"].ToString();
                        ddlModello.Items.Insert(i, new ListItem(s, ss));
                    }
                }
                else
                    if (ds.Tables["modello"].Rows.Count != 0)
                        sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella MODELLO. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }

    public void LeggiAlimentazione() // devo leggere la tabella Alimentazione
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
            ds = FBConn.getfromDSet("select * from alimentazione order by alimentazione", "alimentazione", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["alimentazione"].Rows.Count > 0)
                {
                    ddlAlimentazione.Items.Clear();
                    int k = 0;
                    for (int i = 0; i < ds.Tables["alimentazione"].Rows.Count; i++)
                    {
                        s = ds.Tables["alimentazione"].Rows[i]["alimentazione"] == DBNull.Value ? "" : ds.Tables["alimentazione"].Rows[i]["alimentazione"].ToString();
                        ss = ds.Tables["alimentazione"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["alimentazione"].Rows[i]["id"].ToString();
                        ddlAlimentazione.Items.Insert(i, new ListItem(s, ss));
                    }
                }
                else
                    if (ds.Tables["alimentazione"].Rows.Count != 0)
                        sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella ALIMENTAZIONE. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }

    public void LeggiUbi() // devo leggere la tabella Ubicazione e Ritiro chiavi
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
            ds = FBConn.getfromDSet("SELECT a.ID, a.UBICAZIONE, a.VIA, a.CIVICO, a.COMUNE_EK, a.DESCRIZIONE, a.FOTO1, a.FOTO2, a.ABILITATO, b.comune FROM UBICAZIONE a left join comuni b on a.comune_ek = b.comune_k order by comune, ubicazione, via, civico", "Posteggio", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                ddlPosteggio.Items.Clear();
                ddlRitiro.Items.Clear();
                if (ds.Tables["Posteggio"].Rows.Count > 0)
                {
                    int k = 0;
                    for (int i = 0; i < ds.Tables["Posteggio"].Rows.Count; i++)
                    {
                        s = ds.Tables["Posteggio"].Rows[i]["comune"] == DBNull.Value ? "" : ds.Tables["Posteggio"].Rows[i]["comune"].ToString();
                        s += " " + (ds.Tables["Posteggio"].Rows[i]["ubicazione"] == DBNull.Value ? "" : ds.Tables["Posteggio"].Rows[i]["ubicazione"].ToString());
                        s += " " + (ds.Tables["Posteggio"].Rows[i]["via"] == DBNull.Value ? "" : ds.Tables["Posteggio"].Rows[i]["via"].ToString());
                        s += " " + (ds.Tables["Posteggio"].Rows[i]["civico"] == DBNull.Value ? "" : ds.Tables["Posteggio"].Rows[i]["civico"].ToString());
                        ss = ds.Tables["Posteggio"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["Posteggio"].Rows[i]["id"].ToString();
                        ddlPosteggio.Items.Insert(i, new ListItem(s, ss));
                        ddlRitiro.Items.Insert(i, new ListItem(s, ss));
                    }
                }
                else
                    if (ds.Tables["Posteggio"].Rows.Count != 0)
                        sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella UBICAZIONE. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }

    public void LeggiDotazioni() // devo leggere la tabella Ubicazione e Ritiro chiavi
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
            ds = FBConn.getfromDSet("SELECT * FROM DOTAZIONE order by id", "Dotazione", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["Dotazione"].Rows.Count > 0)
                {
                    ddlDotazione1.Items.Clear();
                    ddlDotazione2.Items.Clear();
                    for (int i = 0; i < ds.Tables["Dotazione"].Rows.Count; i++)
                    {
                        s = ds.Tables["Dotazione"].Rows[i]["Dotazione"] == DBNull.Value ? "1" : ds.Tables["Dotazione"].Rows[i]["Dotazione"].ToString();
                        ss = ds.Tables["Dotazione"].Rows[i]["id"] == DBNull.Value ? "1" : ds.Tables["Dotazione"].Rows[i]["id"].ToString();
                        ddlDotazione1.Items.Insert(i, new ListItem(s, ss));
                        ddlDotazione2.Items.Insert(i, new ListItem(s, ss));
                    }
                }
                else
                    if (ds.Tables["Dotazione"].Rows.Count != 0)
                        sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella DOTAZIONE. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }

    public void LeggiTrazione() // devo leggere la tabella Ubicazione e Ritiro chiavi
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
            ds = FBConn.getfromDSet("SELECT * FROM TRAZIONE order by id", "Trazione", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["Trazione"].Rows.Count > 0)
                {
                    ddlTrazione.Items.Clear();
                    for (int i = 0; i < ds.Tables["Trazione"].Rows.Count; i++)
                    {
                        s = ds.Tables["Trazione"].Rows[i]["Trazione"] == DBNull.Value ? "1" : ds.Tables["Trazione"].Rows[i]["Trazione"].ToString();
                        ss = ds.Tables["Trazione"].Rows[i]["id"] == DBNull.Value ? "1" : ds.Tables["Trazione"].Rows[i]["id"].ToString();
                        ddlTrazione.Items.Insert(i, new ListItem(s, ss));
                    }
                }
                else
                    if (ds.Tables["Trazione"].Rows.Count != 0)
                        sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella TRAZIONE. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }

    public void LeggiGomme() // devo leggere la tabella Ubicazione e Ritiro chiavi
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
            ds = FBConn.getfromDSet("SELECT * FROM GOMME order by id", "GOMME", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["GOMME"].Rows.Count > 0)
                {
                    ddlGomme.Items.Clear();
                    for (int i = 0; i < ds.Tables["GOMME"].Rows.Count; i++)
                    {
                        s = ds.Tables["GOMME"].Rows[i]["GOMME"] == DBNull.Value ? "1" : ds.Tables["GOMME"].Rows[i]["GOMME"].ToString();
                        ss = ds.Tables["GOMME"].Rows[i]["id"] == DBNull.Value ? "1" : ds.Tables["GOMME"].Rows[i]["id"].ToString();
                        ddlGomme.Items.Insert(i, new ListItem(s, ss));
                    }
                }
                else
                    if (ds.Tables["GOMME"].Rows.Count != 0)
                        sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella PNEUMATICI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }
    public void LeggiCambio() // devo leggere la tabella Ubicazione e Ritiro chiavi
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
            ds = FBConn.getfromDSet("SELECT * FROM CAMBIO order by id", "CAMBIO", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["CAMBIO"].Rows.Count > 0)
                {
                    ddlCambio.Items.Clear();
                    for (int i = 0; i < ds.Tables["CAMBIO"].Rows.Count; i++)
                    {
                        s = ds.Tables["CAMBIO"].Rows[i]["CAMBIO"] == DBNull.Value ? "1" : ds.Tables["CAMBIO"].Rows[i]["CAMBIO"].ToString();
                        ss = ds.Tables["CAMBIO"].Rows[i]["id"] == DBNull.Value ? "1" : ds.Tables["CAMBIO"].Rows[i]["id"].ToString();
                        ddlCambio.Items.Insert(i, new ListItem(s, ss));
                    }
                }
                else
                    if (ds.Tables["CAMBIO"].Rows.Count != 0)
                        sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella CAMBIO. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }

    public void LeggiTarga()
    {
        // ddltarga
        ds.Clear();
        s = "SELECT a.id, a.targa FROM MEZZI as a order by a.targa";
        ds = FBConn.getfromDSet(s, "targa", out msg);
        if (msg.Length >= 1)
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
        else
        {
            ddlTarga.Items.Clear();
            if (ds.Tables["targa"].Rows.Count > 0)
            {
                s = "";
                for (int i = 0; i < ds.Tables["targa"].Rows.Count; i++)
                {
                    //s = ds.Tables["mezzi"].Rows[i]["targa"] == DBNull.Value ? "" : ds.Tables["mezzi"].Rows[i]["targa"].ToString();
                    s = (ds.Tables["targa"].Rows[i]["targa"] == DBNull.Value ? "" : ds.Tables["targa"].Rows[i]["targa"].ToString());
                    ss = ds.Tables["targa"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["targa"].Rows[i]["id"].ToString();
                    ddlTarga.Items.Insert(i, new ListItem(s, ss));
                }
                ddlTarga.SelectedValue = ddlMezzi.SelectedValue;
            }
            else
                if (ds.Tables["targa"].Rows.Count != 0)
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella AUTOMEZZI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
        }
    }

    public void LeggiClassificazioni() // devo leggere la tabella Marca
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
            ds = FBConn.getfromDSet("select * from classificazione order by classificazione", "cla", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                ddlCla.Items.Clear();
                if (ds.Tables["cla"].Rows.Count > 0)
                {
                    string s = "", ss = ""; int k = 0;
                    for (int i = 0; i < ds.Tables["cla"].Rows.Count; i++)
                    {
						if (ds.Tables["cla"].Rows[i]["abilitato"] != DBNull.Value && ds.Tables["cla"].Rows[i]["abilitato"].ToString() == "1")
						{
							s = ds.Tables["cla"].Rows[i]["classificazione"] == DBNull.Value ? "" : ds.Tables["cla"].Rows[i]["classificazione"].ToString();
							ss = ds.Tables["cla"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["cla"].Rows[i]["id"].ToString();
							ddlCla.Items.Insert(i, new ListItem(s, ss));
						}
                    }
                }
                else
                    if (ds.Tables["cla"].Rows.Count != 0)
                        sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella CLASSIFICAZIONE. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }

    public void LeggiNumero()
    {
        // ddltarga
        ds.Clear();
        s = "SELECT a.id, a.numero FROM MEZZI as a order by a.numero";
        ds = FBConn.getfromDSet(s, "numero", out msg);
        if (msg.Length >= 1)
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
        else
        {
            ddlNumero.Items.Clear();
            if (ds.Tables["numero"].Rows.Count > 0)
            {
                s = "";
                for (int i = 0; i < ds.Tables["numero"].Rows.Count; i++)
                {
                    //s = ds.Tables["mezzi"].Rows[i]["targa"] == DBNull.Value ? "" : ds.Tables["mezzi"].Rows[i]["targa"].ToString();
                    s = (ds.Tables["numero"].Rows[i]["numero"] == DBNull.Value ? "" : ds.Tables["numero"].Rows[i]["numero"].ToString());
                    ss = ds.Tables["numero"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["numero"].Rows[i]["id"].ToString();
                    ddlNumero.Items.Insert(i, new ListItem(s, ss));
                }
                ddlNumero.SelectedValue = ddlMezzi.SelectedValue;
            }
            else
                if (ds.Tables["numero"].Rows.Count != 0)
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella AUTOMEZZI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
        }
    }

    protected void ddlMezzi_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaMezzo(ddlMezzi.SelectedValue.ToString());
        ddlTarga.SelectedValue = ddlMezzi.SelectedValue.ToString();
        ddlNumero.SelectedValue = ddlMezzi.SelectedValue.ToString();
        sStato.Text = "";
    }

    protected void ddlTarga_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaMezzo(ddlTarga.SelectedValue.ToString());
        ddlMezzi.SelectedValue = ddlTarga.SelectedValue.ToString();
        ddlNumero.SelectedValue = ddlTarga.SelectedValue.ToString();
        sStato.Text = "";
    }

    protected void ddlNumero_SelectedIndexChanged(object sender, EventArgs e)
    {
        CaricaMezzo(ddlNumero.SelectedValue.ToString());
        ddlTarga.SelectedValue = ddlNumero.SelectedValue.ToString();
        ddlMezzi.SelectedValue = ddlNumero.SelectedValue.ToString();
        sStato.Text = "";
    }

    public void CaricaMezzo(string id)
    {
        if (id == "")
            if (ddlMezzi.Items.Count > 0) { ddlMezzi.SelectedIndex = 0; id = ddlMezzi.SelectedValue; }
            else
                return;
            
        // devo leggere la tabella mezzi
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
            if (id == "")
                if (ddlMezzi.SelectedValue != "") id = ddlMezzi.SelectedIndex.ToString();

            s = "SELECT a.*, b.*, c.*, d.*, e.dotazione, f.* FROM MEZZI a left join marca as b on a.marca_ek = b.id left join modello as c on a.modello_ek = c.id left join ALIMENTAZIONE as d on a.ALIMENTAZIONE_EK = d.id left join DOTAZIONE as e on a.DOTAZIONE1_EK = e.id left join UBICAZIONE as f on a.UBICAZIONE_EK = f.id ";
            s += " " + (id == "" ? "" : " where a.id=" + id) + " order by a.targa, b.marca, c.modello ";
            ds.Clear();
            ds = FBConn.getfromDSet(s, "mezzi", out msg);
            FBConn.closeaFBConn(out msg);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (ds.Tables["mezzi"].Rows.Count >= 1)
                {
                    tTarga.Text = ds.Tables["mezzi"].Rows[0]["targa"] == DBNull.Value ? "" : ds.Tables["mezzi"].Rows[0]["targa"].ToString();
                    tNumero.Text = ds.Tables["mezzi"].Rows[0]["numero"] == DBNull.Value ? "" : ds.Tables["mezzi"].Rows[0]["numero"].ToString();
                    cbAbilitata.Checked = ds.Tables["mezzi"].Rows[0]["abilitato"] == DBNull.Value ? false : ds.Tables["mezzi"].Rows[0]["abilitato"].ToString() == "1" ? true : false;
                    cbBlackBox.Checked = ds.Tables["mezzi"].Rows[0]["blackbox"] == DBNull.Value ? false : ds.Tables["mezzi"].Rows[0]["blackbox"].ToString() == "1" ? true : false;
                    string marca = ds.Tables["mezzi"].Rows[0]["marca_ek"] == DBNull.Value ? "1" : ds.Tables["mezzi"].Rows[0]["marca_ek"].ToString();
                    string modello = ds.Tables["mezzi"].Rows[0]["modello_ek"] == DBNull.Value ? "1" : ds.Tables["mezzi"].Rows[0]["modello_ek"].ToString();
                    ddlProduttore.SelectedValue = marca;
                    ddlModello.SelectedValue = modello;
                    ddlTrazione.SelectedValue = ds.Tables["mezzi"].Rows[0]["trazione_ek"] == DBNull.Value ? "1" : ds.Tables["mezzi"].Rows[0]["trazione_ek"].ToString();
                    ddlAlimentazione.SelectedValue = ds.Tables["mezzi"].Rows[0]["alimentazione_ek"] == DBNull.Value ? "1" : ds.Tables["mezzi"].Rows[0]["alimentazione_ek"].ToString();
                    ddlGomme.SelectedValue = ds.Tables["mezzi"].Rows[0]["gomme_ek"] == DBNull.Value ? "1" : ds.Tables["mezzi"].Rows[0]["gomme_ek"].ToString();
                    ddlPosti.SelectedValue = ds.Tables["mezzi"].Rows[0]["posti"] == DBNull.Value ? "1" : ds.Tables["mezzi"].Rows[0]["posti"].ToString();
                    ddlEuro.SelectedValue = ds.Tables["mezzi"].Rows[0]["euro"] == DBNull.Value ? "1" : ds.Tables["mezzi"].Rows[0]["euro"].ToString();
                    ddlCambio.SelectedValue = ds.Tables["mezzi"].Rows[0]["cambio_ek"] == DBNull.Value ? "1" : ds.Tables["mezzi"].Rows[0]["cambio_ek"].ToString();
                    tCavalli.Text = ds.Tables["mezzi"].Rows[0]["cavalli"] == DBNull.Value ? "" : ds.Tables["mezzi"].Rows[0]["cavalli"].ToString();
                    ddlDotazione1.SelectedValue = ds.Tables["mezzi"].Rows[0]["dotazione1_ek"] == DBNull.Value ? "1" : ds.Tables["mezzi"].Rows[0]["dotazione1_ek"].ToString();
                    ddlDotazione2.SelectedValue = ds.Tables["mezzi"].Rows[0]["dotazione2_ek"] == DBNull.Value ? "1" : ds.Tables["mezzi"].Rows[0]["dotazione2_ek"].ToString();
                    tPortata.Text = ds.Tables["mezzi"].Rows[0]["portata"] == DBNull.Value ? "" : ds.Tables["mezzi"].Rows[0]["portata"].ToString();
                    tColore.Text = ds.Tables["mezzi"].Rows[0]["colore"] == DBNull.Value ? "" : ds.Tables["mezzi"].Rows[0]["colore"].ToString();
                    tDescrizione.Text = ds.Tables["mezzi"].Rows[0]["descrizione"] == DBNull.Value ? "" : ds.Tables["mezzi"].Rows[0]["descrizione"].ToString();
                    tScadenza.Text = ds.Tables["mezzi"].Rows[0]["scadenza"] == DBNull.Value ? "" : string.Format("{0:dd/MM/yyyy}", ds.Tables["mezzi"].Rows[0]["scadenza"]);
                    ddlPosteggio.SelectedValue = ds.Tables["mezzi"].Rows[0]["ubicazione_ek"] == DBNull.Value ? "1" : ds.Tables["mezzi"].Rows[0]["ubicazione_ek"].ToString();
                    ddlRitiro.SelectedValue = ds.Tables["mezzi"].Rows[0]["ritirochiavi_ek"] == DBNull.Value ? "1" : ds.Tables["mezzi"].Rows[0]["ritirochiavi_ek"].ToString();
					ddlCla.SelectedValue = ds.Tables["mezzi"].Rows[0]["classificazione_ek"] == DBNull.Value ? "1" : ds.Tables["mezzi"].Rows[0]["classificazione_ek"].ToString();
					tPosteggio.Text = ds.Tables["mezzi"].Rows[0]["posteggio"] == DBNull.Value ? "0" : ds.Tables["mezzi"].Rows[0]["posteggio"].ToString();
					string nfoto;
                    nfoto = ds.Tables["mezzi"].Rows[0]["libretto"] == DBNull.Value || ds.Tables["mezzi"].Rows[0]["libretto"].ToString() == "" ? "" : "DATA\\" + ds.Tables["mezzi"].Rows[0]["libretto"].ToString();
                    if (nfoto.Length > 0) { iLibretto.ImageUrl = nfoto; iLibretto.Visible = true; cbLibretto.Visible = true; }
                        else { iLibretto.Visible = false; cbLibretto.Visible = false; }
                    nfoto = ds.Tables["mezzi"].Rows[0]["foto1"] == DBNull.Value || ds.Tables["mezzi"].Rows[0]["foto1"].ToString() == "" ? "" : "DATA\\" + ds.Tables["mezzi"].Rows[0]["foto1"].ToString();
                    if (nfoto.Length > 0) { iFotodx.ImageUrl = nfoto; iFotodx.Visible = true; cbdx.Visible = true; }
                        else { iFotodx.Visible = false; cbdx.Visible = false; }
                    nfoto = ds.Tables["mezzi"].Rows[0]["foto2"] == DBNull.Value || ds.Tables["mezzi"].Rows[0]["foto2"].ToString() == "" ? "" : "DATA\\" + ds.Tables["mezzi"].Rows[0]["foto2"].ToString();
                    if (nfoto.Length > 0) { iFotofr.ImageUrl = nfoto; iFotofr.Visible = true; cbfr.Visible = true; }
                        else { iFotofr.Visible = false; cbfr.Visible = false; }
                    nfoto = ds.Tables["mezzi"].Rows[0]["foto3"] == DBNull.Value || ds.Tables["mezzi"].Rows[0]["foto3"].ToString() == "" ? "" : "DATA\\" + ds.Tables["mezzi"].Rows[0]["foto3"].ToString();
                    if (nfoto.Length > 0) { iFotoint.ImageUrl = nfoto; iFotoint.Visible = true; cbint.Visible = true; }
                        else { iFotoint.Visible = false; cbint.Visible = false; }
                }
                else
                    if (ds.Tables["mezzi"].Rows.Count != 0)  // se non è > di 0 e è diverso da 0 allora deve essere per forza negativo!!!!
                        sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella AUTOMEZZI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }
}

