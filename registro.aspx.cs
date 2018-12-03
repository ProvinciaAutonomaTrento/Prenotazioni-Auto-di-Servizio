using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using FirebirdSql.Data.FirebirdClient;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Threading;

public partial class registro : System.Web.UI.Page
{
    public ConnessioneFB FBConn = new ConnessioneFB();
    public gmail gm;
    public string msg;
    public string formatodata = "dd-MM-yyyy";
    public static user utenti = new user();

    private static FbConnection cAFbConn = null;
    private ConnessioneFB FBClass = new ConnessioneFB();
    private FbDataAdapter cda = new FbDataAdapter();
    private FbCommand cc = new FbCommand();
    private DataSet cds = new DataSet();
    private DataTable tbl = new DataTable();
    public string s = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
        {
            CheckSession();
            LBenvenuto.Text = " Benvenuto " + utenti.nome + " " + utenti.cognome;
            Session.Add("potere", utenti.potere);
            LeggiUbi();
			//ddlSedi.SelectedIndex = 4;  // da togliere
            cldData.SelectedDate = DateTime.Now.Date;
			lData.Width = cldData.Width;
        }
    }

    protected void LeggiUbi()    // devo leggere la tabella ubicazini
    {
        msg = "";
        FBConn.openaFBConn(out msg);
        if (msg.Length >= 1)
            sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
        else
        {
            msg = "";
            cds.Clear();
            cds = FBConn.getfromDSet("select a.*, b.comune from ubicazione as a left join comuni as b on a.comune_ek=b.comune_k where a.abilitato > 0 order by b.comune, a.ubicazione", "ubicazioni", out msg);
            FBConn.closeaFBConn(out s);
            if (msg.Length >= 1)
                sStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (cds.Tables["ubicazioni"].Rows.Count > 0)
                {
                    ddlSedi.Items.Clear();
                    string s = "", ss = "";
                    ddlSedi.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));
                    for (int i = 0; i < cds.Tables["ubicazioni"].Rows.Count; i++)
                    {
                        s = cds.Tables["ubicazioni"].Rows[i]["comune"] == DBNull.Value ? "" : cds.Tables["ubicazioni"].Rows[i]["comune"].ToString();
                        s += cds.Tables["ubicazioni"].Rows[i]["via"] == DBNull.Value ? "" : ", " + cds.Tables["ubicazioni"].Rows[i]["via"].ToString();
                        s += cds.Tables["ubicazioni"].Rows[i]["civico"] == DBNull.Value ? "" : " " + cds.Tables["ubicazioni"].Rows[i]["civico"].ToString();
                        s += cds.Tables["ubicazioni"].Rows[i]["ubicazione"] == DBNull.Value ? "" : ", " + cds.Tables["ubicazioni"].Rows[i]["ubicazione"].ToString();
                        ss = cds.Tables["ubicazioni"].Rows[i]["id"] == DBNull.Value ? "" : cds.Tables["ubicazioni"].Rows[i]["id"].ToString().Trim();
                        ddlSedi.Items.Insert(i + 1, new System.Web.UI.WebControls.ListItem(s, ss));
                    }
                }
                else
                    sStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella UBICAZIONI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
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
    protected bool CheckSession()
    {
        Int32 id = Session["iduser"] != null ? Convert.ToInt32(Session["iduser"].ToString()) : -1;
        if (id <= 0 || !utenti.cercaid(id))
        {
            string s = "Sessione scaduta. Prego ricollegarsi.";
            ShowPopUpMsg(s);
            Response.Redirect("default.aspx?session=0");
        }
        return (true);
    }

    protected void cbStampa_Click(object sender, EventArgs e)
    {
        // controllo sessione....
        sStato.Text = "";
        CheckSession();
        if (ddlSedi.SelectedValue == "")
        {
            sStato.Text = "ATTENZIONE: indicare la sede per la quale si desidere ottenere il registro!";
            return;
        }
        s = "select a.partenza, a.arrivo, b.cognome, b.nome, c.targa, c.numero, d.ubicazione, d.via, d.civico ";
        s += "from PRENOTAZIONI a ";
        s += "left join utenti b on a.user_ek = b.id ";
        s += "left join mezzi c on a.mezzo_ek = c.id ";
        s += "left join ubicazione d on a.UBICAZIONE_EK = d.id ";
		s += "where cast(a.partenza as date) <= Cast(\'" + cldData.SelectedDate.ToString("yyyy/MM/dd") + "\' as date) and ";
		s += "cast(a.arrivo as date) >= cast('" + cldData.SelectedDate.ToString("yyyy/MM/dd") + "\' as date) AND ";
        s += "a.ubicazione_ek = " + ddlSedi.SelectedValue + " ";
        s += "order by a.arrivo, c.numero ";
        tbl.Clear();
        tbl = FBConn.getfromTbl(s, out msg);
		string[] listafile = new string[(int)(tbl.Rows.Count/20) + 1];
		
		if (tbl.Rows.Count > 0)
        {
            msg = "";
            DateTime dt;
            int foglio = 0, campo = 0, trovato = 0;
            int righeXpagina = 20;
            string SaveLocation = Server.MapPath("Data") + "\\";
            string nome_base = SaveLocation + "moduli\\Registro";
            string dest = nome_base + "_";
            string pagina = "";
			
            int vuote = 0;
            msg = "";
            trovato = 0;
            for (int r = 20; r >= 0; r--) // scorro i 20 registri possibili
            {
                try
                {
                    for (int i = 0; i <= (int)(tbl.Rows.Count / righeXpagina); i++) // scorro le pagine del registro attuale
                        System.IO.File.Delete(nome_base + "_" + r.ToString() + "_" + utenti.iduser.ToString() + "_" + (i + 1).ToString() + ".pdf");
                    trovato = r;
                }
                catch (Exception ex)
                {
                    // Non Posso cancellare il file perchè è occupato... passo al prossimo
                }
            }
            dest += trovato.ToString() + "_" + utenti.iduser.ToString() + "_";

            try
            {
                int records = 0;
                while (records < tbl.Rows.Count)
                {
                    foglio++;
                    pagina = dest + foglio.ToString() + ".pdf";
                    if (System.IO.File.Exists((nome_base + ".pdf")))
                    {
						listafile[foglio-1] = pagina;
                        PdfReader pdfReader = new PdfReader(nome_base + ".pdf");
                        PdfStamper pdfStamper = new PdfStamper(pdfReader, new System.IO.FileStream(pagina, System.IO.FileMode.Create));
                        AcroFields form = pdfStamper.AcroFields;
						string[] keys = form.Fields.Keys.ToArray();
						string s;
						form.GenerateAppearances = true;
						// rinomino tutti i campi
						for (int k = 0; k < form.Fields.Count; k++)
						{
							s = String.Format("{0}_{1}_{2}", "Campo", foglio, k);   // nomi univoci
							form.RenameField(keys[k], s);
						}
						keys = form.Fields.Keys.ToArray(); // una volta rinominati.... li rileggo						
						
                        // intesto la pagina
                        campo = 0;
					
						s = DateTime.Now.Year.ToString();
						form.SetField(keys[campo], string.Format("{0}", cldData.SelectedDate.ToString("dd-MM-yyyy")));
						form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.APPEARANCE_NORMAL, null);
						form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
						campo++;
						form.SetField(keys[campo], string.Format("{0}", ddlSedi.SelectedItem.Text));
						form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
						//form.SetFieldProperty(keys[campo++], "setfflags", PdfFormField.FLAGS_LOCKED, null);
						campo++;
						vuote = righeXpagina;
						
						// ciclo pagina
                        for (int i = 0; (i < righeXpagina && records < tbl.Rows.Count); i++)
                        {
                            DateTime.TryParse(tbl.Rows[records]["partenza"].ToString(), out dt);
							form.SetField(keys[campo], string.Format("{0}", dt.ToString("dd/MM/yyyy HH:mm")));
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							DateTime.TryParse(tbl.Rows[records]["arrivo"].ToString(), out dt);
							if (dt.Date == cldData.SelectedDate)
								form.SetField(keys[campo], string.Format("{0}", dt.ToString("dd/MM/yyyy HH:mm")));
							else
								form.SetField(keys[campo], "__/__/_____");
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							form.SetField(keys[campo], string.Format("{0}", tbl.Rows[records]["targa"].ToString()));
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							form.SetField(keys[campo], string.Format("{0}", tbl.Rows[records]["numero"].ToString()));
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							form.SetField(keys[campo], string.Format("{0}", tbl.Rows[records]["Cognome"].ToString() + " " + tbl.Rows[records]["Nome"].ToString()));
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							records++;
							vuote--;
                        }
						
                        for (int i = 0; i < vuote; i++) // fill sino in fondo alla pagina
                        {
                            form.SetField(keys[campo], "");
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							form.SetField(keys[campo], "");
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							form.SetField(keys[campo], "");
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							form.SetField(keys[campo], "");
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							form.SetField(keys[campo], "");
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							records++;
						}
						form.GenerateAppearances = true;
						pdfStamper.FormFlattening = false;  // dovrebbe trasformate il contenuto dei campi in testo
						pdfStamper.Close();
						pdfReader.Close();
					}
                }

                //PdfStamper pdfStamper = new PdfStamper(pdfReader, new System.IO.FileStream(pagina, System.IO.FileMode.Create));
                //PdfContentByte cb;
                // cb = pdfStamper.GetOverContent(pagina2));
            }
            catch (Exception ex)
            {
                sStato.Text = "ATTENZIONE: scrittura file di registro non riuscita: " + ex.ToString() + ". Contattare il servizio assistenza al n. " + Session["assistenza"].ToString();
                return;
            }
			
			string risultato = SaveLocation + "moduli\\Registro_" + cldData.SelectedDate.ToString("yyyy-MM-dd") + "_" + ddlSedi.SelectedValue.ToString().Trim()+".pdf";
			string risultatoa = SaveLocation + "moduli\\risultatoUnisci.pdf";
			MemoryStream ms;
			ms = CreaUnicoPDF(listafile); // mi crea il pdf e mi importa i dati.... ma mi lascia scuri i campi
			//SendEmail(ms, utenti.mail, ddlSedi.SelectedItem.Text, cldData.SelectedDate.ToString("dd-MM-yyyy"));
			sStato.Text = string.Format("Registro per {0} del {1} inviato via email e messo a disposizione nel browser. 1", ddlSedi.SelectedItem.Text, cldData.SelectedDate.ToString("dd-MM-yyyy"));
			DownloadAsPDF(ms, "Registro " + ddlSedi.SelectedValue + "_" + cldData.SelectedDate.ToString("yyyy-MM-dd") + ".pdf");
			sStato.Text = string.Format("Registro per {0} del {1} messo a disposizione nel browser. 2", ddlSedi.SelectedItem.Text, cldData.SelectedDate.ToString("dd-MM-yyyy"));
			
		}
		else
		{
			sStato.Text = "Non ci sono prenotazioni per i filtri impostati.";
		}
	}
	//Copy a set of form fields from an existing PDF template/doc
	//and keep appending to a brand new PDF file.
	//The copied set of fields will have different values.
	private void AppendSetOfFormFields(string[] files, string dest)
	{
		FileStream fs = new FileStream(dest, FileMode.Create);
		PdfCopyFields _copy = new PdfCopyFields(fs);
		PdfReader reader = null;
		foreach (string f in files)
		{
			reader = new PdfReader(files[0]);
			_copy.AddDocument(reader);
		}
		_copy.Close();
	}
	//ConcatenateForms
	private void flattaidoc (String[] files)
	{
		Document document = new Document();
		FileStream fss; //= new FileStream(dest, FileMode.Create);
		PdfReader reader = null;
		document.Open();
		PdfStamper stamper;
		//using (var existingFileStream = new FileStream(Server.MapPath(P_InputStream), FileMode.Open))
		//using (MemoryStream stream = new MemoryStream())
		// Open existing PDF
		foreach (string f in files)
		{
			s = f.Substring(0, f.LastIndexOf(".pdf")) + "flatten.pdf";
			fss = new FileStream(s, FileMode.Create);
			reader = new PdfReader(files[0]);
			stamper = new PdfStamper(reader, fss);
			AcroFields form = stamper.AcroFields;
			// "Flatten" the form so it wont be editable/usable anymore
			stamper.FormFlattening = true;
			stamper.Close();
			fss.Close();
			reader.Close();
		}
		document.Close();
	}
	protected MemoryStream CreaUnicoPDF(String[] files)
	{
		Document document = new Document();
		MemoryStream fss = new MemoryStream();
		PdfReader reader = null;
		PdfCopy copy = new PdfCopy(document, fss);   // ok scrivo sul file risultato3
		document.Open();
		// Istanzio un nuovo reader che ci servirà per leggere i singoli file
		// Per ogni file passato
		
		for (int i = 0; i < files.Length; i++)
		{
			//Leggo il file
			reader = new PdfReader(files[i]);
			//fss.Position = 0;
			//Prendo il numero di pagine
			int n = reader.NumberOfPages;
			// e per ogni pagina
			for (int page = 0; page < n; page++)
			{
				copy.AddPage(copy.GetImportedPage(reader, page + 1)); // devo prendere la pagina 1 e non la 0
			}
			// con questo metodo faccio il flush del contenuto e libero il rader
			reader.Close();
			copy.FreeReader(reader);
		}
		// Chiudo il documento			
		copy.CloseStream = false;
		document.Close();
		copy.Close();
		fss.Position = 0;
		return (fss);
	}
	protected void UnisciDuePDF(String[] files, string dest)
	{
		Document document = new Document();
		FileStream fss = new FileStream(dest, FileMode.Create);
		PdfCopy copy = new PdfCopy(document, fss);              // ok scrivo sul file risultato3
		PdfReader reader;
		document.Open();
		copy.SetMergeFields();
		// Istanzio un nuovo reader che ci servirà per leggere i singoli file
		// Per ogni file passato
		for (int i = 0; i < files.Length; i++)
		{
			//Leggo il file
			
			reader = new PdfReader(files[i]);	// leggo l'ennesimo file...
			copy.SetMergeFields();				// GLI DICO DI COPIARMI I CAMPI
			copy.AddDocument(reader);			// aggiungo....			
		}
		// Chiudo il documento			
		document.Close();
		copy.Close();
		fss.Close();
		//return (fss);
	}
	public byte[] renameFields(String src, int i) 
	{
		MemoryStream baos = new MemoryStream();
		PdfReader reader = new PdfReader(src);
		PdfStamper stamper = new PdfStamper(reader, baos);
		AcroFields form = reader.AcroFields;
		string[] keys = form.Fields.Keys.ToArray();
		stamper.Close();
		reader.Close();
		return baos.ToArray();
	}
	private void SendEmail(MemoryStream ms, string richiedente, string sede, string dataregistro)
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("uff.gestionigenerali@provincia.tn.it", "Uff. Gestioni generali - Gestione autoveicoli di servizio.");
        mail.To.Add(new MailAddress(richiedente));		
		Attachment data = new Attachment(ms, "Registro.pdf", "application/pdf");
		mail.Attachments.Add(data);
		
		mail.Subject = "REGISTRO: " + sede + ", " + dataregistro;
		mail.Body = "Buongiorno,\n";
		mail.Body += "In allegato il registro per la sede e per la data richiesta.\n\n";
		mail.Body += "Cordiali saluti.\n";
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
			sStato.Text = "ATTENZIONE: problema inoltro registro. Contattare l'assistenza al n. " + Session["asssistenza"].ToString();
		}
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
    public void creanuovopdf(string nomefile)
    {
		/*
        Document document = new Document();
        PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileOutputStream(nomefile));
        document.open();
        Paragraph paragraph = new Paragraph();
        paragraph.add("Hello World!");
        document.add(paragraph);
        document.close();
		*/
    }
	protected void cbtest1_Click(object sender, EventArgs e)
	{
		string SaveLocation = Server.MapPath("Data") + "\\";
		string nome_base = SaveLocation + "moduli\\Registro";
		string[] sf = { nome_base+ "_0_1_1.pdf", nome_base+ "_0_1_2.pdf" };
		string dest = nome_base + "outcb.pdf";
		MergeFiles(dest, sf);
	}
	public static void MergeFiles(string destinationFile, string[] sourceFiles)
	{
		try
		{
			int f = 0;
			// we create a reader for a certain document
			PdfReader reader = new PdfReader(sourceFiles[f]);
			// we retrieve the total number of pages
			int n = reader.NumberOfPages;
			Console.WriteLine("There are " + n + " pages in the original file.");
			// step 1: creation of a document-object
			Document document = new Document(reader.GetPageSizeWithRotation(1));
			// step 2: we create a writer that listens to the document
			PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationFile, FileMode.Create));
			// step 3: we open the document
			document.Open();
			PdfContentByte cb = writer.DirectContent;
			PdfImportedPage page;
			int rotation;
			// step 4: we add content
			while (f < sourceFiles.Length)
			{
				int i = 0;
				while (i < n)
				{
					i++;
					document.SetPageSize(reader.GetPageSizeWithRotation(i));
					document.NewPage();
					page = writer.GetImportedPage(reader, i);
					rotation = reader.GetPageRotation(i);
					if (rotation == 90 || rotation == 270)
					{
						cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
					}
					else
					{
						cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
					}
					Console.WriteLine("Processed page " + i);
				}
				f++;
				if (f < sourceFiles.Length)
				{
					reader = new PdfReader(sourceFiles[f]);
					// we retrieve the total number of pages
					n = reader.NumberOfPages;
					Console.WriteLine("There are " + n + " pages in the original file.");
				}
			}
			// step 5: we close the document
			document.Close();
		}
		catch (Exception e)
		{
			Console.Error.WriteLine(e.Message);
			Console.Error.WriteLine(e.StackTrace);
		}
	}
	protected void cldData_SelectionChanged(object sender, EventArgs e)
	{
		lData.Text = cldData.SelectedDate.ToString("dd-MM-yyyy");
	}
	protected void cbMail_Click(object sender, EventArgs e)
	{
		// controllo sessione....
		sStato.Text = "";
		CheckSession();
		if (ddlSedi.SelectedValue == "")
		{
			sStato.Text = "ATTENZIONE: indicare la sede per la quale si desidere ottenere il registro!";
			return;
		}
		s = "select a.partenza, a.arrivo, b.cognome, b.nome, c.targa, c.numero, d.ubicazione, d.via, d.civico ";
		s += "from PRENOTAZIONI a ";
		s += "left join utenti b on a.user_ek = b.id ";
		s += "left join mezzi c on a.mezzo_ek = c.id ";
		s += "left join ubicazione d on a.UBICAZIONE_EK = d.id ";
		s += "where cast(a.partenza as date) <= Cast(\'" + cldData.SelectedDate.ToString("yyyy/MM/dd") + "\' as date) and ";
		s += "cast(a.arrivo as date) >= cast('" + cldData.SelectedDate.ToString("yyyy/MM/dd") + "\' as date) AND ";
		s += "a.ubicazione_ek = " + ddlSedi.SelectedValue + " ";
		s += "order by a.arrivo, c.numero ";
		tbl.Clear();
		tbl = FBConn.getfromTbl(s, out msg);
		string[] listafile = new string[(int)(tbl.Rows.Count / 20) + 1];

		if (tbl.Rows.Count > 0)
		{
			msg = "";
			DateTime dt;
			int foglio = 0, campo = 0, trovato = 0;
			int righeXpagina = 20;
			string SaveLocation = Server.MapPath("Data") + "\\";
			string nome_base = SaveLocation + "moduli\\Registro";
			string dest = nome_base + "_";
			string pagina = "";

			int vuote = 0;
			msg = "";
			trovato = 0;
			for (int r = 20; r >= 0; r--) // scorro i 20 registri possibili
			{
				try
				{
					for (int i = 0; i <= (int)(tbl.Rows.Count / righeXpagina); i++) // scorro le pagine del registro attuale
						System.IO.File.Delete(nome_base + "_" + r.ToString() + "_" + utenti.iduser.ToString() + "_" + (i + 1).ToString() + ".pdf");
					trovato = r;
				}
				catch (Exception ex)
				{
					// Non Posso cancellare il file perchè è occupato... passo al prossimo
				}
			}
			dest += trovato.ToString() + "_" + utenti.iduser.ToString() + "_";

			try
			{
				int records = 0;
				while (records < tbl.Rows.Count)
				{
					foglio++;
					pagina = dest + foglio.ToString() + ".pdf";
					if (System.IO.File.Exists((nome_base + ".pdf")))
					{
						listafile[foglio - 1] = pagina;
						PdfReader pdfReader = new PdfReader(nome_base + ".pdf");
						PdfStamper pdfStamper = new PdfStamper(pdfReader, new System.IO.FileStream(pagina, System.IO.FileMode.Create));
						AcroFields form = pdfStamper.AcroFields;
						string[] keys = form.Fields.Keys.ToArray();
						string s;
						form.GenerateAppearances = true;
						// rinomino tutti i campi
						for (int k = 0; k < form.Fields.Count; k++)
						{
							s = String.Format("{0}_{1}_{2}", "Campo", foglio, k);   // nomi univoci
							form.RenameField(keys[k], s);
						}
						keys = form.Fields.Keys.ToArray(); // una volta rinominati.... li rileggo						

						// intesto la pagina
						campo = 0;

						s = DateTime.Now.Year.ToString();
						form.SetField(keys[campo], string.Format("{0}", cldData.SelectedDate.ToString("dd-MM-yyyy")));
						form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.APPEARANCE_NORMAL, null);
						form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
						campo++;
						form.SetField(keys[campo], string.Format("{0}", ddlSedi.SelectedItem.Text));
						form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
						//form.SetFieldProperty(keys[campo++], "setfflags", PdfFormField.FLAGS_LOCKED, null);
						campo++;
						vuote = righeXpagina;

						// ciclo pagina
						for (int i = 0; (i < righeXpagina && records < tbl.Rows.Count); i++)
						{
							DateTime.TryParse(tbl.Rows[records]["partenza"].ToString(), out dt);
							form.SetField(keys[campo], string.Format("{0}", dt.ToString("dd/MM/yyyy HH:mm")));
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							DateTime.TryParse(tbl.Rows[records]["arrivo"].ToString(), out dt);
							if (dt.Date == cldData.SelectedDate)
								form.SetField(keys[campo], string.Format("{0}", dt.ToString("dd/MM/yyyy HH:mm")));
							else
								form.SetField(keys[campo], "__/__/_____");
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							form.SetField(keys[campo], string.Format("{0}", tbl.Rows[records]["targa"].ToString()));
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							form.SetField(keys[campo], string.Format("{0}", tbl.Rows[records]["numero"].ToString()));
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							form.SetField(keys[campo], string.Format("{0}", tbl.Rows[records]["Cognome"].ToString() + " " + tbl.Rows[records]["Nome"].ToString()));
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							records++;
							vuote--;
						}

						for (int i = 0; i < vuote; i++) // fill sino in fondo alla pagina
						{
							form.SetField(keys[campo], "");
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							form.SetField(keys[campo], "");
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							form.SetField(keys[campo], "");
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							form.SetField(keys[campo], "");
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							form.SetField(keys[campo], "");
							form.SetFieldProperty(keys[campo], "setfflags", PdfFormField.FLAGS_LOCKED, null);
							campo++;
							records++;
						}
						form.GenerateAppearances = true;
						pdfStamper.FormFlattening = false;  // dovrebbe trasformate il contenuto dei campi in testo
						pdfStamper.Close();
						pdfReader.Close();
					}
				}

				//PdfStamper pdfStamper = new PdfStamper(pdfReader, new System.IO.FileStream(pagina, System.IO.FileMode.Create));
				//PdfContentByte cb;
				// cb = pdfStamper.GetOverContent(pagina2));
			}
			catch (Exception ex)
			{
				sStato.Text = "ATTENZIONE: scrittura file di registro non riuscita: " + ex.ToString() + ". Contattare il servizio assistenza al n. " + Session["assistenza"].ToString();
				return;
			}

			string risultato = SaveLocation + "moduli\\Registro_" + cldData.SelectedDate.ToString("yyyy-MM-dd") + "_" + ddlSedi.SelectedValue.ToString().Trim() + ".pdf";
			string risultatoa = SaveLocation + "moduli\\risultatoUnisci.pdf";
			MemoryStream ms;
			ms = CreaUnicoPDF(listafile); // mi crea il pdf e mi importa i dati.... ma mi lascia scuri i campi
			SendEmail(ms, utenti.mail, ddlSedi.SelectedItem.Text, cldData.SelectedDate.ToString("dd-MM-yyyy"));
			sStato.Text = string.Format("Registro per {0} del {1} inviato via email e messo a disposizione nel browser. 1", ddlSedi.SelectedItem.Text, cldData.SelectedDate.ToString("dd-MM-yyyy"));
			DownloadAsPDF(ms, "Registro " + ddlSedi.SelectedValue + "_" + cldData.SelectedDate.ToString("yyyy-MM-dd") + ".pdf");
			sStato.Text = string.Format("Registro per {0} del {1} inviato via email e messo a disposizione nel browser. 2", ddlSedi.SelectedItem.Text, cldData.SelectedDate.ToString("dd-MM-yyyy"));
		}
		else
		{
			sStato.Text = "Non ci sono prenotazioni per i filtri impostati.";
		}
		
	}

	protected void cbShow_Click(object sender, EventArgs e)
	{
		// controllo sessione....
		sStato.Text = "";
		CheckSession();
		if (ddlSedi.SelectedValue == "")
		{
			sStato.Text = "ATTENZIONE: indicare la sede per la quale si desidere ottenere il registro!";
			return;
		}
		s = "select a.partenza, a.arrivo, b.cognome, b.nome, c.targa, c.numero, d.ubicazione, d.via, d.civico ";
		s += "from PRENOTAZIONI a ";
		s += "left join utenti b on a.user_ek = b.id ";
		s += "left join mezzi c on a.mezzo_ek = c.id ";
		s += "left join ubicazione d on a.UBICAZIONE_EK = d.id ";
		s += "where cast(a.partenza as date) <= Cast(\'" + cldData.SelectedDate.ToString("yyyy/MM/dd") + "\' as date) and ";
		s += "cast(a.arrivo as date) >= cast('" + cldData.SelectedDate.ToString("yyyy/MM/dd") + "\' as date) AND ";
		s += "a.ubicazione_ek = " + ddlSedi.SelectedValue + " ";
		s += "order by a.arrivo, c.numero ";
		tbl.Clear();
		tbl = FBConn.getfromTbl(s, out msg);
		string[] listafile = new string[(int)(tbl.Rows.Count / 20) + 1];

		if (tbl.Rows.Count > 0)
		{
			msg = "";
			DateTime dt;
			RiempiRegistro(tbl, 0);
			pRegistro.Visible = true;
			GWRegistro.Visible = true;
			sStato.Text = string.Format("Registro per {0} del {1}. Numero prenotazioni attive: {2}", ddlSedi.SelectedItem.Text, cldData.SelectedDate.ToString("dd-MM-yyyy"), tbl.Rows.Count);
		}
		else
		{
			sStato.Text = "Non ci sono prenotazioni per i filtri impostati.";
		}
	}

	public void RiempiRegistro(DataTable gwds, int comando) // missioni
	{
		try
		{
			if (GWRegistro.Rows.Count > 0)
			{
				GWRegistro.DataBind();
			}
			GWRegistro.DataSource = gwds;
			gwds.DefaultView.Sort = "Arrivo DESC";
			GWRegistro.DataBind();
			//if (GWRegistro.Columns.Count > 0) GWRegistro.Columns[0].Visible = comando == 0 ? true : false; // mon serve +
			//if (GWRegistro.Columns.Count > 1) GWRegistro.Columns[1].Visible = comando == 1 ? true : false;
			//GWDD.Sort(GWDD.Columns[2].HeaderText, SortDirection.Descending);
			GWRegistro.Visible = true;
		}
		catch (Exception ex)
		{
			sStato.Text = "Riscontrato errore durante la ricerca delle missioni. Errore: " + ex.ToString() + " Avvertire l'amministratore al n. " + (string)Session["assistenza"].ToString();
		}
	}

	protected void ddlSedi_SelectedIndexChanged(object sender, EventArgs e)
	{
		pElenco.Visible = false;
		GWRegistro.Visible = false;
	}
}