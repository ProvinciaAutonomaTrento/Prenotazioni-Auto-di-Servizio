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
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

public partial class ConfermaPrenotazionedn : System.Web.UI.Page
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
	public static string dest = "";
	public string aid = ""; // id altro utente... 
	public string idp = ""; // ex static; è una modifica e contiene l'id della prenotazione che voglio modificare
	public Int32 idu = -1;

	protected void Page_Load(object sender, EventArgs e)
    {
		checkSession();
		
		if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
		{
			// carico dati prenotazione
			//int test = -1;
			string msg = "";
			if (pre.carica(Session["idp"].ToString(), out msg)) // campi nascosti per recupero dati javascript
			{
				msg = "";
				npartenza.Attributes.Add("value", (pre.ubicazione_Via + " " + pre.ubicazione_Civico + ", " + pre.ubicazione_Città));
				narrivo.Attributes.Add("value", pre.dove_comune + " " + pre.dove_provincia + " " + pre.dove_sigla);
				// la prima volta.... invio la mail e carico documento....
			}
			else
			{
				sStato.Text = "ERRORE durante la ricerca della prenotazine. Contattare il servizio assistenza al n. " + (string)Session["assistenza"].ToString();
				return;
			}
			lRientro.ForeColor = System.Drawing.Color.Black;
			lDestinazione.Text = pre.dove_comune + " (" + pre.dove_sigla + ")";
			lPartenza.Text = string.Format("{0} alle ore {1:00}", pre.partenza.ToString("dd-MM-yyyy"), pre.partenza.ToString("HH:mm"));
			lRientro.Text = string.Format("{0} alle ore {1:00}", pre.arrivo.ToString("dd-MM-yyyy"), pre.arrivo.ToString("HH:mm"));
			lRitiro0.Text = string.Format("{0} aperto dalle {1}-{2}", pre.ubicazione0, pre.ubicazione0_dalle.ToString("HH:mm"), pre.ubicazione0_alle.ToString("HH:mm"));
            lRitiro.Text = string.Format("{0} aperto dalle {1}-{2}", pre.ubicazione, pre.ubicazione_dalle.ToString("HH:mm"), pre.ubicazione_alle.ToString("HH:mm"));
            lPasseggeri.Text = string.Format("{0}", System.Convert.ToInt16(pre.passeggeri) + System.Convert.ToInt16(pre.aggregati));
			lVeicolo.Text = string.Format("{0} - {1}  targa {2}{3}", pre.marca, pre.modello, pre.targa, pre.blackbox == "1" ? ", black box a bordo" : "");
			lNumero.Text = string.Format("{0}", pre.numero);
			lDriver.Text = string.Format("{0}", pre.nome + " " + pre.cognome + " tel. " + pre.tel);
			lAggregato.Text = string.Format("{0} {1}", pre.aggregato_nome, pre.aggregato_cognome);
			//lData.Text = string.Format("{0}", DateTime.Now.Date.ToShortDateString());
			trVeicolo.Visible = true;
			tRiepilogo.Visible = true;
			string nome_base;
			string SaveLocation = Server.MapPath("Data") + "\\";
            nome_base = SaveLocation + "moduli\\" + pre.foglio.Trim();
            //nome_base = SaveLocation + "moduli\\modulo_prenotazione";
            dest = nome_base + "_" + pre.user_ek;
			msg = "";
			int trovato = 0;
			dest += trovato.ToString() + ".pdf";
			MemoryStream ms = new MemoryStream();
			try
			{
				if (System.IO.File.Exists((nome_base + ".pdf")))
				{
					PdfReader pdfReader = new PdfReader(nome_base + ".pdf");
					//PdfStamper pdfStamper = new PdfStamper(pdfReader, new System.IO.FileStream(dest, System.IO.FileMode.Create));
					PdfStamper pdfStamper = new PdfStamper(pdfReader, ms);
					AcroFields form = pdfStamper.AcroFields;
					// cambio nome ai campi
					string[] keys = form.Fields.Keys.ToArray();
					int i = 0;
					foreach (string k in keys)  // rinomino i campi
						form.RenameField(k, "Campo_" + (i++).ToString());
					keys = form.Fields.Keys.ToArray();
					string s = DateTime.Now.Year.ToString();
					string[] dati = new string[10];
					dati[0] = string.Format("{0} ({1})", pre.dove_comune, pre.dove_sigla);
					dati[1] = string.Format("{0} alle ore {1}", pre.partenza.ToString("dd-MM-yyyy"), pre.partenza.ToString("HH:mm"));
					dati[2] = string.Format("{0} alle ore {1}", pre.arrivo.ToString("dd-MM-yyyy"), pre.arrivo.ToString("HH:mm"));
					dati[3] = string.Format("{0}\n{1} {2} {3}\nOrario apertura: {4}-{5}", pre.ubicazione0, pre.ubicazione0_Via, pre.ubicazione0_Civico, pre.ubicazione0_Città, pre.ubicazione0_dalle.ToString("HH:mm"), pre.ubicazione0_alle.ToString("HH:mm"));
					dati[4] = string.Format("{0} {1} {2}", pre.marca, pre.modello, (pre.blackbox == "1" ? " black box a bordo" : ""));
					dati[5] = string.Format("{0},  targa {1}{2}", pre.numero, pre.targa, (pre.posteggio.Trim() != "" ? (", posteggio " + pre.posteggio) : ""));
					dati[6] = pre.nome + " " + pre.cognome + "  tel. " + pre.tel;
					dati[7] = pre.aggregati.Trim() == "0" ? "" : pre.aggregati.Trim();
					dati[8] = pre.aggregato_nome + " " + pre.aggregato_cognome;
					dati[9] = DateTime.Now.Date.ToString("dd-MM-yyyy");
					ms = FillPdfStream(nome_base, dati);

					MailMessage mail = new MailMessage();
					mail.From = new MailAddress("car.scharing@provincia.tn.it", "Uff. Gestioni generali - Gestione autoveicoli di servizio.");
					mail.To.Add(new MailAddress(pre.mail));
					//mail.Bcc.Add(new MailAddress("tiziano.donati@provincia.tn.it"));
					Attachment data = new Attachment(ms, "PrenotazioneAutoDiServizio_" + pre.partenza.ToString("dd-MM-yyyy") + ".pdf", "application/pdf");
					mail.Attachments.Add(data);
					// preparo modulo per estero
					long aidl;
					long.TryParse(aid, out aidl);
					utenti.cercaid(aidl);
					if (pre.dove_stato_ek != "0") // Italia
					{
						btEstero.Enabled = true;
						btEstero.Visible = true;
						dati[0] = pre.marca + " " + pre.modello;
						dati[1] = pre.targa;
						dati[2] = pre.nome + " " + pre.cognome;
						dati[3] = utenti.matricola;
						dati[4] = string.Format("{0}", DateTime.Now.Date.ToString("dd-MM-yyyy"));
						dati[5] = pre.marca + " " + pre.modello;
						dati[6] = pre.targa;
						dati[7] = pre.nome + " " + pre.cognome;
						dati[8] = string.Format("{0}", DateTime.Now.Date.ToString("dd-MM-yyyy"));
						ms = FillPdfStream((SaveLocation + "moduli\\" + pre.delega.Trim()), dati);
						Attachment dataestero = new Attachment(ms, "Modulo_delega_estero_" + pre.partenza.ToString("dd-MM-yyyy") + ".pdf", "application/pdf");
						mail.Attachments.Add(dataestero);
					}
					mail.Subject = string.Format("Foglio di prenotazione per viaggi con autovettura di servizio. ({0}, {1} - {2})", pre.dove_comune, pre.partenza, pre.arrivo);
					mail.Body = "Buongiorno gentile " + pre.nome + " " + pre.cognome + "\n\n";
					mail.Body += "    in allegato, trovera\' il foglio di prenotazione per viaggi con i mezzi di servizio. ";
					mail.Body += "La preghiamo di stamparlo e di portarlo con se\' durante la missione. Potra\' essere esibito su richiesta delle autorita\'. ";
					mail.Body += "Dovra\' presentare il modulo all'addetto di portineria/custodia chiavi, come segno di riconoscimento/identificazione della persona prenotante.\n";
					mail.Body += "Qualora la prenotazione non Le sia piu\' necessaria, per qualsiasi motivazione (malattia, annullamento, cambiamento di orario, altro...), Le chiediamo di provvedere a cancellare/modificare la prenotazione prima possibile!\n";
					mail.Body += "Grazie.\n\n";
					mail.Body += "La presente mail e\' stata inoltrata da un sistema automatico. Non saranno prese in considerazioni eventuali risposte. Grazie.\n\n";
					mail.Body += "Allegato:\tfoglio di prenotazione.\n";
					if (pre.dove_stato_ek != "0") // Italia
					{
						mail.Body += "         \tmodulo delega estero.\n";
					}
					SmtpClient mailSender = new SmtpClient("smtp.gmail.com", 587);
					mailSender.UseDefaultCredentials = true;
					mailSender.EnableSsl = true;
					mailSender.Credentials = new System.Net.NetworkCredential("provinciaautonomatn@gmail.com", "ProvinciaPAT");
					try
					{
						mailSender.Send(mail);
					}
					catch (Exception ex)
					{
						sStato.Text = "ATTENZIONE: conferma prenotazione non inviata correttamente. Contatare il servizio assistenza al n. 0461-496415";
					}
					sStato.Text = "Inoltrata mail con foglio di prenotazione";
				}
			}
			catch (Exception ex)
			{
				sStato.Text = "ATTENZIONE: scrittura file di prenotazione non riuscita: " + ex.ToString() + ". Contattare il servizio assistenza al n. " + Session["assistenza"].ToString();
				return;
			}
		}
	}

	private bool checkSession()
	{
		idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
		aid = Session["aid"] != null ? Session["aid"].ToString() : "";
		idp = Session["idp"] != null ? Session["idp"].ToString() : "";

		if (idu < 0 || !utenti.cercaid(idu))
		{
			string s = "Sessione scaduta. Prego ricollegarsi.";
			Session.Clear();
			Session.Abandon();
			ShowPopUpMsg(s);
			Response.Redirect("default.aspx?session=0");
		}
		Session.Timeout = 30; // ritacco il conteggio!!!
		idu = utenti.iduser;
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
	private void DownloadAsPDF(MemoryStream ms, string nome_file, bool header)
	{
		Response.Clear();
		Response.ClearContent();
		if (header) Response.ClearHeaders();
		Response.ContentType = "application/pdf";
		if (header) Response.AppendHeader("Content-Disposition", "attachment; filename=" + nome_file);
		if (header) Response.AddHeader("Pragma", "no-cache");
		if (header) Response.AddHeader("Cache-Control", "no-cache");
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

	protected void btCarica_Click(object sender, EventArgs e)
	{
		checkSession();
		if (!pre.carica(Session["idp"].ToString(), out msg))
		{
			sStato.Text = "ATTENZIONE: problema nel caricamento della prenotazione! Contattare servizio assistenza al numero " + Session["assistenza"].ToString();
			return;
		}		
		string nome_base;
		string SaveLocation = Server.MapPath("Data") + "\\";
		nome_base = SaveLocation + "moduli\\" + pre.foglio.Trim(); 
		dest = nome_base;
		MemoryStream ms = new MemoryStream();
		string s = DateTime.Now.Year.ToString();
		string[] dati = new string[10];
		dati[0] = string.Format("{0} ({1})", pre.dove_comune, pre.dove_sigla);
		dati[1] = string.Format("{0} alle ore {1}", pre.partenza.ToString("dd-MM-yyyy"), pre.partenza.ToString("HH:mm"));
		dati[2] = string.Format("{0} alle ore {1}", pre.arrivo.ToString("dd-MM-yyyy"), pre.arrivo.ToString("HH:mm"));
		dati[3] = string.Format("{0}\n{1} {2} {3}\nOrario apertura: {4}-{5}", pre.ubicazione0, pre.ubicazione0_Via, pre.ubicazione0_Civico, pre.ubicazione0_Città, pre.ubicazione0_dalle.ToString("HH:mm"), pre.ubicazione0_alle.ToString("HH:mm"));
		dati[4] = string.Format("{0} {1} {2}", pre.marca, pre.modello, (pre.blackbox == "1" ? " black box a bordo" : ""));
		dati[5] = string.Format("{0}, targa {1}{2}", pre.numero, pre.targa, (pre.posteggio.Trim() != "" ? (", posteggio " + pre.posteggio) : ""));
		dati[6] = pre.nome + " " + pre.cognome + "  tel. " + pre.tel;
		dati[7] = pre.aggregati.Trim() == "0" ? "" : pre.aggregati.Trim();
		dati[8] = pre.aggregato_nome + " " + pre.aggregato_cognome;
		dati[9] = DateTime.Now.Date.ToString("dd-MM-yyyy");
		ms = FillPdfStream(nome_base, dati);
		DownloadAsPDF(ms, "Prenotazione.PDF", true);
		ms.Close();
	}
	protected void btEstero_Click(object sender, EventArgs e)
	{
		checkSession();
		if (!pre.carica(Session["idp"].ToString(), out msg))
		{
			sStato.Text = "ATTENZIONE: problema nel caricamento della prenotazione! Contattare servizio assistenza al numero " + Session["assistenza"].ToString();
			return;
		}
		long aidl; // ricarico user destinatario
		long.TryParse(aid, out aidl);
		utenti.cercaid(aidl);
		string nome_base;
		string SaveLocation = Server.MapPath("Data") + "\\";
		nome_base = SaveLocation + "moduli\\"+ pre.delega.Trim();
		dest = nome_base;
		MemoryStream ms = new MemoryStream();
		string s = DateTime.Now.Year.ToString();
		string[] dati = new string[9];
		dati[0] = pre.marca + " " + pre.modello;
		dati[1] = pre.targa;
		dati[2] = pre.nome + " " + pre.cognome;
		dati[3] = utenti.matricola;
		dati[4] = string.Format("{0}", DateTime.Now.Date.ToString("dd-MM-yyyy"));
		dati[5] = pre.marca + " " + pre.modello;
		dati[6] = pre.targa;
		dati[7] = pre.nome + " " + pre.cognome;
		dati[8] = string.Format("{0}", DateTime.Now.Date.ToString("dd-MM-yyyy"));
		ms = FillPdfStream(nome_base, dati);
		DownloadAsPDF(ms, "Modulo_estero.PDF", true);
		ms.Close();
	}

	// devo passare il file master e i dati
	private MemoryStream FillPdfStream(string nome_base, string[] campi)
	{
		MemoryStream ms = new MemoryStream();
		PdfReader pdfReader = new PdfReader(nome_base + ".pdf"); // forse basterebbe anche solo questa istanza
		try
		{
			if (System.IO.File.Exists((nome_base + ".pdf")))
			{
				pdfReader = new PdfReader(nome_base + ".pdf"); // rileggo il pdf master
				// creo il destinario dal master
				PdfStamper pdfStamper = new PdfStamper(pdfReader, ms);
				AcroFields form = pdfStamper.AcroFields;
				string[] keys = form.Fields.Keys.ToArray();
				// cambio nome ai campi
				int i = 0;
				foreach (string k in keys)  // rinomino i campi
					form.RenameField(k, "Campo_" + (i++).ToString());
				keys = form.Fields.Keys.ToArray();
				// riempi master con dati
				for (int j = 0; j < form.Fields.Count; j++)
				{
					form.SetField(keys[j], campi[j].ToString()); form.SetFieldProperty(keys[j], "setfflags", PdfFormField.FF_READ_ONLY, null);
				}
				pdfStamper.Writer.CloseStream = false;
				pdfStamper.FormFlattening = false;
				pdfStamper.Close();
				pdfReader.Close();
				ms.Position = 0;
			}
		}
		catch (Exception ex)
		{
			return (null);
		}
		return (ms);
	}

	protected void btNuova_Click(object sender, EventArgs e)
	{
		idp = "";
		//aid = "";
		//Session.Add("aid", "");
		Session.Add("idp", ""); // azzero la prenotazione, ne vado a fare una nuova
        if (aid != "")
            Response.Redirect("pre.aspx?aid=" + aid + "&nome=" + utenti.nome + "&cognome=" + utenti.cognome + "&p=" + utenti.potere);
        else
            Response.Redirect("pre.aspx");
	}
}
