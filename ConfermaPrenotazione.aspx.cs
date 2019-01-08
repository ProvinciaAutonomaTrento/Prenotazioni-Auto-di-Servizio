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
using System.Data;
using System.Collections;

using System.IO;
using System.Drawing;
using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using iTextSharp.text;
using System.Threading;

public partial class ConfermaPrenotazione : System.Web.UI.Page
{    
    public ConnessioneFB FBConn = new ConnessioneFB();
    public DataSet ds = new DataSet();
    public DataTable tbl = new DataTable();
    public string msg;
    public string formatodata = "dd-MM-yyyy";
    public user utenti = new user();
    public static classeprenota pre = new classeprenota();
    public string rtf = "";
    public string rtfuff = "";
    gmail gm = new gmail();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
        {
            Int32 id = Session["iduser"] != null ? System.Convert.ToInt32(Session["iduser"].ToString()) : -1;
            if (id <= 0 || !utenti.cercaid(id))
            {
                Session.Abandon();
                Response.Redirect("default.aspx");
            }

            // carico dati prenotazione
            int test = -1;
            if (Request.QueryString["c"] != null || test == 1)
            {
                msg = "";
                if (test == 1) tbl = pre.cercaid("44", out msg); // da togliere
                else
                    tbl = pre.cercaid(Request.QueryString["c"].ToString(), out msg);
                if (msg != "")
                {
                    sStato.Text = "ERRORE durante la ricerca della prenotazine. Errore su prenotazione: " + Request.QueryString["c"].ToString() + ". Contattare il servizio assistenza al n. " + (string)Session["assistenza"].ToString();
                    return;
                }
                else
                {
                    if (!pre.refresh(tbl))
                    {
                        sStato.Text = "ERRORE durante la lettura dei dati riguardanti la prenotazione. Contattare il servizio assistenza al n. " + (string)Session["assistenza"].ToString();
                        return;
                    }
                    npartenza.Attributes.Add("value", (pre.ubicazione_Via + " " + pre.ubicazione_Civico + ", " + pre.ubicazione_Città));
                    narrivo.Attributes.Add("value", pre.dove_comune + " " + pre.dove_provincia + " " + pre.dove_sigla );
                }
            }
            else
            {
                //sStato.Text = "ERRORE durante la ricerca della prenotazine. Contattare il servizio assistenza al n. " + (string)Session["assistenza"].ToString();
                return;
            }
        }

        lRientro.ForeColor = System.Drawing.Color.Black;
        lDestinazione.Text = pre.dove_comune + " (" + pre.dove_sigla + ")";
        lPartenza.Text = string.Format("{0} alle ore {1:00}", pre.partenza.ToString("dd-MM-yyyy"), pre.partenza.ToString("HH:mm"));
        lRientro.Text = string.Format("{0} alle ore {1:00}", pre.arrivo.ToString("dd-MM-yyyy"), pre.arrivo.ToString("HH:mm"));
        lRitiro.Text = string.Format("{0} aperto dalle {1}-{2}", pre.ubicazione, pre.ubicazione_dalle.ToString("HH:mm"), pre.ubicazione_alle.ToString("HH:mm"));
        lPasseggeri.Text = string.Format("{0}", System.Convert.ToInt16(pre.passeggeri) + System.Convert.ToInt16(pre.aggregati));
        lVeicolo.Text = string.Format("{0} - {1}  targa {2}{3}", pre.marca, pre.modello, pre.targa, pre.blackbox == "1" ? ", black box a bordo" : "");
        lNumero.Text = pre.numero.ToString();
        lDriver.Text = string.Format("{0}", pre.nome + " " + pre.cognome + " tel. " + pre.tel);
        lAggregato.Text = string.Format("{0} {1}", pre.aggregato_nome, pre.aggregato_cognome);
        lData.Text = string.Format("{0}", DateTime.Now.Date.ToShortDateString());
        trVeicolo.Visible = true;
        tRiepilogo.Visible = true;
        string nome_base;
        string SaveLocation = Server.MapPath("Data") + "\\";
        nome_base = SaveLocation + "moduli\\modulo_prenotazione";
        string dest = nome_base + "_" + pre.user_ek + "_";
        if (pre.dove_stato_ek == "0") // Italia
        {
            msg = "";
            int trovato = 0;
            for (int i = 20; i >= 0; i--)
            {
                try
                {
                    System.IO.File.Delete(nome_base + "_" + pre.user_ek + "_" + i.ToString() + ".pdf");
                    trovato = i;
                }
                catch (Exception ex)
                {
                    // Non Posso cancellare il file perchè è occupato
                }
            }
            dest += trovato.ToString() + ".pdf";
            try
            {   
                if (System.IO.File.Exists((nome_base + ".pdf")))
                {
                    PdfReader pdfReader = new PdfReader(nome_base + ".pdf");
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, new System.IO.FileStream(dest, System.IO.FileMode.Create));
                    AcroFields form = pdfStamper.AcroFields;
                    string[] keys = form.Fields.Keys.ToArray();
                    string s = DateTime.Now.Year.ToString();
                    form.SetField(keys[0], string.Format("{0} alle ore {1}", pre.partenza.ToString("dd-MM-yyyy"), pre.partenza.ToString("HH:mm"))); form.SetFieldProperty(keys[0], "setfflags", PdfFormField.FF_READ_ONLY, null);
                    form.SetField(keys[1], string.Format("{0} alle ore {1}", pre.arrivo.ToString("dd-MM-yyyy"), pre.arrivo.ToString("HH:mm"))); form.SetFieldProperty(keys[1], "setfflags", PdfFormField.FF_READ_ONLY, null);
                    form.SetField(keys[2], string.Format("{0}\n{1} {2} {3}\nOrario apertura: {4}-{5}", pre.ubicazione, pre.ubicazione_Via, pre.ubicazione_Civico, pre.ubicazione_Città, pre.ubicazione_dalle.ToString("HH:mm"), pre.ubicazione_alle.ToString("HH:mm"))); form.SetFieldProperty(keys[2], "setfflags", PdfFormField.FF_READ_ONLY, null);
                    form.SetField(keys[3], string.Format("{0} {1} {2}", pre.marca, pre.modello, (pre.blackbox == "1" ? " black box a bordo" : ""))); form.SetFieldProperty(keys[3], "setfflags", PdfFormField.FF_READ_ONLY, null);
                    form.SetField(keys[4], string.Format("{0}   targa {1}", pre.numero, pre.targa)); form.SetFieldProperty(keys[4], "setfflags", PdfFormField.FF_READ_ONLY, null);
                    form.SetField(keys[5], pre.nome + " " + pre.cognome + "  tel. " + pre.tel); form.SetFieldProperty(keys[5], "setfflags", PdfFormField.FF_READ_ONLY, null);
                    form.SetField(keys[6], pre.aggregati.Trim() == "0" ? "" : pre.aggregati.Trim()); form.SetFieldProperty(keys[6], "setfflags", PdfFormField.FF_READ_ONLY, null);
                    form.SetField(keys[7], pre.aggregato_nome + " " + pre.aggregato_cognome); form.SetFieldProperty(keys[7], "setfflags", PdfFormField.FF_READ_ONLY, null);
                    form.SetField(keys[8], DateTime.Now.Date.ToString("dd-MM-yyyy")); form.SetFieldProperty(keys[8], "setfflags", PdfFormField.FF_READ_ONLY, null);
                    pdfStamper.FormFlattening = false;
                    pdfStamper.Close();
                    pdfReader.Close();
                }
            }
            catch (Exception ex)
            {
                sStato.Text = "ATTENZIONE: scrittura file di prenotazione non riuscita: " + ex.ToString() + ". Contattare il servizio assistenza al n. " + Session["assistenza"].ToString();
                return;
            }
            gm.file = dest;
            gm.subject = string.Format("Foglio di prenotazione per viaggi con autovettura di servizio. ({0}, {1} - {2})", pre.dove_comune, pre.partenza, pre.arrivo);
            gm.body = "Buongiorno gentile " + pre.nome + " " + pre.cognome + "\n\n";
            gm.body += "    in allegato, troverà il foglio di prenotazione per viaggi con i mezzi di servizio. ";
            gm.body += "La preghiamo di stamparlo e di portarlo con sè durante la missione. Potrà essere esibito su richiesta delle autorità. ";
            gm.body += "Dovrà presentare il modulo all'addetto di portineria/custodia chiavi, come segno di riconoscimento/identificazione della persona prenotante.\n";
            gm.body += "Qualora la prenotazione non Le sia più necessaria, per qualsiasi motivazione (malattia, annullamento, cambiamento di orario, altro...), Le chiediamo di provvedere a cancellare/modificare la prenotazione prima possibile!\n";
            gm.body += "Grazie.\n\n\n";
			gm.body += "La presente mail è stata inoltrata da un sistema automatico. Non saranno prese in considerazioni eventuali risposte. Grazie.\n\n";
			gm.body += "Allegato: foglio di prenotazione.\n";
        }
        else
        {
            msg = "";
            nome_base = nome_base = SaveLocation + "moduli\\modulo_delega per estero tron";
            /*pdftron.PDFNet.Initialize();
            try
            {
                PDFDoc doc = null;
                doc = new PDFDoc((nome_base + ".pdf"));
                doc.InitSecurityHandler();
                ContentReplacer replacer = new ContentReplacer();
                pdftron.PDF.Page page = doc.GetPage(1);
                replacer.AddString("mm345678901234567890", (pre.marca + " " + pre.modello));
                replacer.AddString("targa67", pre.targa);
                replacer.AddString("driver789012345678901234567890", pre.nome + " " + pre.cognome);
                replacer.AddString("matricola", utenti.matricola);
                replacer.AddString("data", DateTime.Now.Date.ToShortDateString());
                replacer.Process(page); // process page
                doc.Save(nome_base + "_" + utenti.iduser + ".pdf", 0);
                doc.Close();
                doc.Dispose();
            }
            catch (Exception ex)
            {
                sStato.Text = "ATTENZIONE: scrittura file delega personalizzata non riuscita: " + ex.ToString() + " Contattare il servizio assistenza al n. " + Session["assistenza"].ToString();
                return;
            }*/
            gm.file = nome_base + "_" + pre.user_ek + ".pdf";
            gm.subject = "DELEGA per viaggi all'estero con autovettura di servizio.";
            gm.body = "Buongiorno sig./sig.ra " + pre.nome + " " + pre.cognome + "\n\n";
            gm.body += "    in allegato, troverà il modulo di delega dell'amministrazione per le missioni all'estero.\n";
            gm.body += "La preghiamo di stamparlo e di portarlo con sè durante la missione. Potrà essere esibito su richiesta delle autorità dei paesi esteri sui quali transiterà con l'automezzo di servizio.\n";
            gm.body += "Grazie.\n\n\n";
			gm.body += "La presente mail è stata inoltrata da un sistema automatico. Non saranno prese in considerazioni eventuali risposte. Grazie.\n\n";
			gm.body += "Allegato: delega per viaggi di servizio paesi esteri.\n";
        }

        string[] achi = { pre.mail };
        gm.achi = achi;
        string[] achicc = { "car.sharing@provincia.tn.it" };
        //gm.achiccn = achicc; // PER ORA NON INVIO A NESSUN ALTRO
        gm.dachi = "car.scharing@provincia.tn.it";
        gm.mandamail("",0,"","", out msg);            
        if (msg.Trim() != "")
            sStato.Text = "ATTENZIONE: conferma prenotazione non inviata correttamente. Contatare il servizio assistenza al n. " + Session["assistenza"].ToString();

	// provo a cancellare l'eventuale il file di destinazione
	/*try
	{
		System.IO.File.Delete(nome_base + "_" + utenti.iduser + ".pdf");
		//System.IO.File.Delete(nome_base + "_" + utenti.iduser + ".pdf");
	}
	catch (IOException ex)
	{
		// forse non c'è il file e quindi mi viene sollevata l'eccezione
		sStato.Text = "ATTENZIONE: impossibile cancellare il file di destinazione: " + ex.ToString() + ". Contattare il servizio assistenza al n. " + Session["assistenza"].ToString();
	}*/
}

	private void DownloadAsPDF(MemoryStream ms, string nome_file)
	{
		Response.Clear();
		Response.ClearContent();
		Response.ClearHeaders();
		Response.ContentType = "application/pdf";
		Response.AppendHeader("Content-Disposition", "attachment; filename=" + nome_file);
		Response.AddHeader("Pragma", "no-cache");
		Response.AddHeader("Cache-Control", "no-cache");
		Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
		// Sends the response buffer
		//Response.OutputStream.Flush();
		//Response.OutputStream.Close();
		// Prevents any other content from being sent to the browser
		//Response.SuppressContent = true;
		// Directs the thread to finish, bypassing additional processing
		try
		{
			HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
			HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
			HttpContext.Current.ApplicationInstance.CompleteRequest();
			// Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
			//HttpContext.Current.Response.End();
			//Response.End();
			Thread.Sleep(1);
		}
		catch (Exception ex)
		{
			sStato.Text = "Errore nel mandare il registro al client: " + ex.ToString();
		}
		finally
		{
			/*
			//Sends the response buffer
			HttpContext.Current.Response.Flush();
			// Prevents any other content from being sent to the browser
			HttpContext.Current.Response.SuppressContent = true;
			//Directs the thread to finish, bypassing additional processing
			HttpContext.Current.ApplicationInstance.CompleteRequest();
			*/
			//Suspends the current thread
		}
	}

	private bool rtf_replace(string cerca, string sostituisci)
    {
        try
        {
            //Int32 i = rtf.IndexOf(cerca);
            rtf = rtf.Replace(cerca, sostituisci);
            //Int32 f = rtf.IndexOf(sostituisci);
        }
        catch (Exception ex)
        {
            return (false);
        }
        return (true);
    }
    private bool LeggiRtf(string nf, out string msg)
    {
        msg = "";
        try
        {
            rtf = System.IO.File.ReadAllText(nf);
        }
        catch (Exception ex)
        {
            msg = ex.ToString();
            return (false);
        }
        return (true);
    }

    protected void btOKMappa_Click(object sender, EventArgs e)
    {
        PanelMappa.Visible = !PanelMappa.Visible;
        if (PanelMappa.Visible) btOKMappa.Text = "Nascondi mappa ed indicazioni stradali"; else btOKMappa.Text = "Mostra mappa ed indicazioni stradali";
    }
}