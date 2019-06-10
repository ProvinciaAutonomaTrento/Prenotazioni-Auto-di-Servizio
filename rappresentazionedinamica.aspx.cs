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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;

public partial class rappresentazionedinamica : System.Web.UI.Page
{
	public ConnessioneFB FBConn = new ConnessioneFB();
	public DataSet ds = new DataSet();
	public DataTable tbl = new DataTable();
	public string s, filtro, sqlstr, where, sqlconta;
	public string msg;
    public string formatotimestamp = "yyyy-MM-dd HH:mm:ss";
    public string formatoshowdata = "dd-MM-yyyy HH:mm:ss";
    public string formatoshowora = "HH:mm:ss";
    public string formatodata = "yyyy-MM-dd";
    public DateTime Data = DateTime.Now;

    protected void Page_Load(object sender, EventArgs e)
    {
		if (!Page.IsPostBack)
		{
			initChartCollection();
			initChartCollectionXData();
			initChartLiere();
			LeggiUbi();
		}
    }
    string titolo = "";

    protected void initChartCollection()
    {
        DateTime dada = DateTime.Now, ada = DateTime.Now;
        switch (rblist.SelectedValue)
        {
            case "Oggi": dada = DateTime.Now; ada = DateTime.Now; titolo = "Prenotazioni di oggi"; break;
            case "Resto della sttimana":
                dada = DateTime.Now; ada = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek ); titolo = "Prenotazioni resto della settimana";
                break;
            case "Prossima settimana":
                dada = DateTime.Now.AddDays( 7 -(int)DateTime.Now.DayOfWeek); titolo = "Prenotazioni prossima settimana";
				ada = dada.AddDays(7);
				break;
            case "Prossime due settimane":
				dada = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek); titolo = "Prenotazioni prossime due settimane";
				ada = dada.AddDays(14);
				break;
        }

        string dadata = dada.ToString(formatodata), adata = ada.ToString(formatodata);
        tStato.Text = "";

        //dadata = (da == null || da.ToShortDateString() == "1900-01-01") ? DateTime.Now.ToString(formatodata) : da.ToString(formatodata);
        //adata = (a == null || a.ToShortDateString() == "1900-01-01") ? DateTime.Now.ToString(formatodata) : a.ToString(formatodata);
        filtro = " (cast(p.partenza as date) between cast(\'" + dadata + "\' as date) and cast(\'" + adata + "\' as date) or ";
		filtro += "cast(p.arrivo as date) between cast(\'" + dadata + "\' as date) and cast(\'" + adata + "\' as date) or ";
		filtro += "cast(p.partenza as date) <= cast(\'" + dadata + "\' as date) and cast(p.arrivo as date) >= cast(\'" + adata + "\' as date) )) ";
		Giornaliera.Titles.Clear();

		sqlstr = "select  u.ubicazione, c.comune, ";
		sqlstr += "(SELECT COUNT(p.Id) as numerop ";
		sqlstr += "FROM prenotazioni as p ";
		sqlstr += "WHERE p.ubicazione_ek=u.id and ";
		sqlstr += filtro;
		sqlstr += "from ubicazione as u ";
		sqlstr += "left join comuni as c on u.comune_ek=c.comune_k ";
		sqlstr += "where u.abilitato=1 ";
		sqlstr += "order by u.ubicazione, c.comune ";

		tTitolo.Text = "Prenotazioni dal " + dada.ToString("dd-MM-yyyy") + " sino al " + ada.ToString("dd-MM-yyyy");
        tTitolo.Enabled = false;
		try
        {
			if ( FBConn.FBconnessione.State   != ConnectionState.Open)
            {
				FBConn.openaFBConn(out msg);
            }
			FBConn.FBc.Connection = FBConn.FBconnessione;
			FBConn.FBc.CommandText = sqlstr;
			FbDataReader reader = FBConn.FBc.ExecuteReader();
        }
        catch (Exception ex)
        {
            tStato.Text = string.Format("ERRORE: non è possibile leggere la tabella PRENOTAZIONI. {0}", ex.Message);
            return;
        }

		tbl = FBConn.getfromTbl(sqlstr, out msg);

        if (tbl.Rows.Count > 0)
        {
			//titolo += " " + tbl.Rows.Count.ToString();
            Giornaliera.Series.Clear();            
            Giornaliera.DataSource = tbl;
            Giornaliera.Titles.Add(titolo).Font = new System.Drawing.Font("Thaoma", 12);
            Giornaliera.Series.Add("Prenotazioni");
            Giornaliera.Series["Prenotazioni"].BorderWidth = 1;
            Giornaliera.Series["Prenotazioni"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
			//Giornaliera.Series["Prenotazioni"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.RangeBar;
			Giornaliera.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 7);
            Giornaliera.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            Giornaliera.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = false;
            Giornaliera.ChartAreas["ChartArea1"].AxisX.LabelStyle.TruncatedLabels = false;
            Giornaliera.ChartAreas["ChartArea1"].BackColor = System.Drawing.Color.FromName("AliceBlue");
            Giornaliera.ChartAreas["ChartArea1"].BackSecondaryColor = System.Drawing.Color.FromName("Red");
            Giornaliera.ChartAreas["ChartArea1"].BackGradientStyle = System.Web.UI.DataVisualization.Charting.GradientStyle.TopBottom;
			Giornaliera.Series["Prenotazioni"].XValueMember = "Ubicazione";
            Giornaliera.Series["Prenotazioni"].AxisLabel = "Ubicazione";
            Giornaliera.Series["Prenotazioni"].Color = System.Drawing.Color.FromName("RoyalBlue");
            Giornaliera.ChartAreas["ChartArea1"].AxisY.LabelStyle.TruncatedLabels = false;
            Giornaliera.Series["Prenotazioni"].YValueMembers = "numerop";
            Giornaliera.Series["Prenotazioni"].IsValueShownAsLabel = true;
            Giornaliera.Series["Prenotazioni"].Font = new System.Drawing.Font("Thaoma", 12);
            Giornaliera.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            //Giornaliera.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            //Giornaliera.ChartAreas["Serie Sedi"].
            Giornaliera.ChartAreas["ChartArea1"].AxisY2.Enabled = new System.Web.UI.DataVisualization.Charting.AxisEnabled();
            //Giornaliera.Series["Serie Sedi"].Label = Giornaliera.Series["Serie Sedi"].YValueMembers;
            Giornaliera.DataBind();
        }
	}

	protected void initChartCollectionXData()
	{
		DateTime dada = DateTime.Now, ada = DateTime.Now;
		string sede = ddlRitiroXData.SelectedIndex <= 0 ? "Nei punti di ritiro su Trento" : (ddlRitiroXData.SelectedItem.ToString() );
		titolo = sede;
		sqlconta = "select count(a.id) as \"num.\" ";
		sqlconta += "from mezzi as a ";
		sqlconta += "left join ubicazione as u on u.id = a.ubicazione_ek ";
		sqlconta += "left join comuni as l on u.comune_ek = l.comune_k ";
		sqlconta += "where l.comune='Trento' and " + (ddlRitiroXData.SelectedIndex <= 0 ? "a.ubicazione_ek != 0 " : "a.ubicazione_ek=" + ddlRitiroXData.SelectedValue.ToString() ) + " ";
		sqlconta += "and a.abilitato = 1  and a.scadenza > cast(\'" + DateTime.Now.Date.ToString("yyyy/MM/dd") + "\' as date) ";
		//sqlconta += "group by u.ubicazione ";
		tbl.Clear();
		tbl.Columns.Clear();
		tbl = FBConn.getfromTbl(sqlconta, out msg);
		int sostanza = 0;
		s = (tbl != null && tbl.Rows.Count > 0) ? tbl.Rows[0]["num."].ToString() : "0";
		int.TryParse(s, out sostanza);
		
		titolo += " (" + sostanza.ToString() + " veicoli) - ";
		//if ( tbl.Rows.Count > 0)
		//{ 
		switch (rdPeriodi.SelectedValue)
		{
			case "Resto della sttimana":
				ada = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek); titolo +=  "resto della settimana";
				break;
			case "Fine prossima settimana":
				ada = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek); ada = ada.AddDays(7); titolo += "prossima settimana";				
				break;
			case "Fine prossime due settimane":
				ada = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek); ada = ada.AddDays(14); titolo += "prossime due settimane";				
				break;
			case "Fine prossime tre settimane":
				ada = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek); ada = ada.AddDays(21); titolo += "prossime tre settimane";
				break;
			case "Fine prossime quattro settimane":
				ada = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek); ada = ada.AddDays(28); titolo += "prossime quattro settimane";
				break;
		}

		string dadata = dada.ToString(formatodata), adata = ada.ToString(formatodata);
		tStato.Text = "";

		//dadata = (da == null || da.ToShortDateString() == "1900-01-01") ? DateTime.Now.ToString(formatodata) : da.ToString(formatodata);
		//adata = (a == null || a.ToShortDateString() == "1900-01-01") ? DateTime.Now.ToString(formatodata) : a.ToString(formatodata);
		filtro = " (cast(a.partenza as date) between cast(\'" + dadata + "\' as date) and cast(\'" + adata + "\' as date) or ";
		filtro += "cast(a.arrivo as date) between cast(\'" + dadata + "\' as date) and cast(\'" + adata + "\' as date) or ";
		filtro += "cast(a.partenza as date) <= cast(\'" + dadata + "\' as date) and cast(a.arrivo as date) >= cast(\'" + adata + "\' as date) ) ";
		//filtro += "cast(a.partenza as date) <= cast(\'" + dadata + "\' as date) and cast(a.arrivo as date) >= cast(\'" + adata + "\' as date) ) ";
		filtro += (ddlRitiroXData.SelectedValue == "" ? "" : " and a.ubicazione_ek = \'" + ddlRitiroXData.SelectedValue.ToString() + "\' ");
		//filtro = " cast(a.partenza as date) >= cast('now' as date) and cast(a.partenza as date) <= cast('now' as date) + 15 ";
		//filtro += "cast(p.arrivo as date) between cast(\'" + dadata + "\' as date) and cast(\'" + adata + "\' as date) or ";
		//filtro += "cast(p.partenza as date) <= cast(\'" + dadata + "\' as date) and cast(p.arrivo as date) >= cast(\'" + adata + "\' as date) )) ";

		XData.Titles.Clear();
		//sqlstr = "select ";
		//sqlstr += "(trim(iif(EXTRACT(DAY FROM a.PARTENZA) > 9, '', '0') || EXTRACT(DAY FROM a.PARTENZA)) || '.' ||";
		//sqlstr += "trim(iif(EXTRACT(MONTH FROM a.PARTENZA) > 9, '', '0') || EXTRACT(MONTH FROM a.PARTENZA))) as DATA1, ";
		//sqlstr += "EXTRACT(MONTH FROM A.PARTENZA)) AS DATA1, ";

		sqlstr = "select a.id, a.partenza, a.arrivo ";
		sqlstr += "from prenotazioni as a ";
		sqlstr += "left join mezzi as b on b.id = a.mezzo_ek ";
		sqlstr += "left join ubicazione as u on u.id = a.ubicazione_ek ";
		sqlstr += "left join comuni as c on c.comune_k = u.comune_ek ";
		sqlstr += "where c.comune='Trento' and u.abilitato = 1 and ";
		sqlstr += filtro;		
		sqlstr += "order by a.partenza ";

		tTitolo.Text = "Prenotazioni dal " + dada.ToString("dd-MM-yyyy") + " sino al " + ada.ToString("dd-MM-yyyy");
		tTitolo.Enabled = false;

		tbl.Clear();
		tbl.Columns.Clear();
		tbl = FBConn.getfromTbl(sqlstr, out msg);

		if (tbl.Rows.Count > 0)
		{
			//tbl.Rows[0].Delete(); // cancello la prima riga
			//titolo += " " + tbl.Rows.Count.ToString();
			int giorni = (int)(ada - dada).TotalDays + 1; // compresi estremi 
			giorni = giorni <= 2 ? 1 : giorni; // (a meno che non sia un giorno)
			int[] numero = new int[giorni];
			String[] dates = new String[giorni];
			for (int i = 0; i < giorni; i++) { numero[i] = 0; }// inizializzo
			int y, m, d, H, min, sec;
			DateTime dd, ad, oggi = dada;
			for (int gg = 0; gg < giorni; gg++)
			{
				dates[gg] = oggi.ToString("dd/MM");
				for (int n = 0; n < tbl.Rows.Count; n++)
				{
					s = tbl.Rows[n][1].ToString(); dd = DateTime.ParseExact(s, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
					s = tbl.Rows[n][2].ToString(); ad = DateTime.ParseExact(s, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
					if (dd.Date <= oggi.Date && ad.Date >= oggi.Date) numero[gg]++;
				}
				oggi = oggi.AddDays(1);
			}
			//fr = ds.Tables["sovrapposte"].Select("id='" + idp + "'");
			//DataRowCollection drc = ds.Tables["sovrapposte"].Rows;
			//contiene = fr.Length > 0 ? 1 : 0;
			
			XData.Series.Clear();
			XData.DataSource = tbl;
			XData.Titles.Add(titolo).Font = new System.Drawing.Font("Thaoma", 14);
			XData.BackColor = System.Drawing.Color.White; // colore dello sfondo lo sfondo del grafico
			XData.ChartAreas["ChartArea1"].AxisY.Title = "n. prenotazioni\nper giorno";
			XData.ChartAreas["ChartArea1"].AxisY.TitleFont = new System.Drawing.Font("Thaoma", 14);
			XData.ChartAreas["ChartArea1"].AxisY.TextOrientation = System.Web.UI.DataVisualization.Charting.TextOrientation.Rotated270;
			XData.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 7);
			XData.ChartAreas["ChartArea1"].AxisX.Interval = 1;
			XData.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = false;
			XData.ChartAreas["ChartArea1"].AxisX.LabelStyle.TruncatedLabels = false;
			XData.ChartAreas["ChartArea1"].BackColor = System.Drawing.Color.FromName("AliceBlue");
			XData.ChartAreas["ChartArea1"].BackSecondaryColor = System.Drawing.Color.FromName("Red");
			XData.ChartAreas["ChartArea1"].BackGradientStyle = System.Web.UI.DataVisualization.Charting.GradientStyle.TopBottom;
			XData.ChartAreas["ChartArea1"].AxisY.LabelStyle.TruncatedLabels = true;
			XData.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
			XData.ChartAreas["ChartArea1"].AxisY2.Enabled = new System.Web.UI.DataVisualization.Charting.AxisEnabled();
			XData.Series.Add("Prenotazionixdata");
			XData.Series["Prenotazionixdata"].BorderWidth = 3;
			XData.Series["Prenotazionixdata"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.SplineArea;
			//XData.Series["Prenotazionixdata"].XValueMember = "Data1";
			XData.Series["Prenotazionixdata"].LabelAngle = 33;
			XData.Series["Prenotazionixdata"].Font = new System.Drawing.Font("Thaoma", 14);
			XData.Series["Prenotazionixdata"].Color = System.Drawing.Color.Orange;
			//XData.Series["Prenotazionixdata"].YValueMembers = "numerop";
			//XData.ChartAreas["ChartArea1"].AxisY.Interval = 30;
			XData.Series["Prenotazionixdata"].IsValueShownAsLabel = true;
			XData.Series["Prenotazionixdata"].BorderWidth = 5;
			XData.Series["Prenotazionixdata"].BackSecondaryColor = System.Drawing.Color.Orange; // ?
			XData.Series["Prenotazionixdata"].LabelForeColor = System.Drawing.Color.Black; // colore dei Marker
			XData.Series["Prenotazionixdata"].LabelBorderWidth = 2;
			XData.Series["Prenotazionixdata"].MarkerSize = 17; // distanza dalla serie delle etichette (Marker)
			XData.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Thaoma", giorni > 28 ? 8 : 10);
			XData.ChartAreas[0].AxisX.TextOrientation = System.Web.UI.DataVisualization.Charting.TextOrientation.Rotated270;
			XData.Series["Prenotazionixdata"].Points.DataBindXY(dates, numero);
		}
		//}
	}

	protected void initChartLiere()
	{
		DateTime dada = DateTime.Now, ada = DateTime.Now;
		string sede = ddlRitiroXData.SelectedIndex <= 0 ? "Nei punti di ritiro su Trento" : (ddlRitiroXData.SelectedItem.ToString());
		titolo = sede;
		sqlconta = "select count(a.id) as \"num.\" ";
		sqlconta += "from mezzi as a ";
		sqlconta += "left join ubicazione as u on u.id = a.ubicazione_ek ";
		sqlconta += "left join comuni as l on u.comune_ek = l.comune_k ";
		sqlconta += "where l.comune='Trento' and " + (ddlRitiroXData.SelectedIndex <= 0 ? "a.ubicazione_ek != 0 " : "a.ubicazione_ek=" + ddlRitiroXData.SelectedValue.ToString()) + " ";
		sqlconta += "and a.abilitato = 1  and a.scadenza > cast(\'" + DateTime.Now.Date.ToString("yyyy/MM/dd") + "\' as date) ";
		//sqlconta += "group by u.ubicazione ";
		tbl.Clear();
		tbl.Columns.Clear();
		tbl = FBConn.getfromTbl(sqlconta, out msg);
		int sostanza = 0;
		s = (tbl != null && tbl.Rows.Count > 0) ? tbl.Rows[0]["num."].ToString() : "0";
		int.TryParse(s, out sostanza);

		titolo += " (" + sostanza.ToString() + " veicoli) - ";
		//if ( tbl.Rows.Count > 0)
		//{ 
		switch (rdPeriodi.SelectedValue)
		{
			case "Resto della sttimana":
				ada = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek); titolo += "resto della settimana";
				break;
			case "Fine prossima settimana":
				ada = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek); ada = ada.AddDays(7); titolo += "prossima settimana";
				break;
			case "Fine prossime due settimane":
				ada = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek); ada = ada.AddDays(14); titolo += "prossime due settimane";
				break;
			case "Fine prossime tre settimane":
				ada = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek); ada = ada.AddDays(21); titolo += "prossime tre settimane";
				break;
			case "Fine prossime quattro settimane":
				ada = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek); ada = ada.AddDays(28); titolo += "prossime quattro settimane";
				break;
		}

		string dadata = dada.ToString(formatodata), adata = ada.ToString(formatodata);
		tLiberi.Text = "Prenotazioni dal " + dada.ToString("dd-MM-yyyy") + " sino al " + ada.ToString("dd-MM-yyyy");

		//dadata = (da == null || da.ToShortDateString() == "1900-01-01") ? DateTime.Now.ToString(formatodata) : da.ToString(formatodata);
		//adata = (a == null || a.ToShortDateString() == "1900-01-01") ? DateTime.Now.ToString(formatodata) : a.ToString(formatodata);
		filtro = " (cast(a.partenza as date) between cast(\'" + dadata + "\' as date) and cast(\'" + adata + "\' as date) or ";
		filtro += "cast(a.arrivo as date) between cast(\'" + dadata + "\' as date) and cast(\'" + adata + "\' as date) or ";
		filtro += "cast(a.partenza as date) <= cast(\'" + dadata + "\' as date) and cast(a.arrivo as date) >= cast(\'" + adata + "\' as date) ) ";
		//filtro += "cast(a.partenza as date) <= cast(\'" + dadata + "\' as date) and cast(a.arrivo as date) >= cast(\'" + adata + "\' as date) ) ";
		filtro += (ddlRitiroXData.SelectedValue == "" ? "" : " and a.ubicazione_ek = \'" + ddlRitiroXData.SelectedValue.ToString() + "\' ");
		//filtro = " cast(a.partenza as date) >= cast('now' as date) and cast(a.partenza as date) <= cast('now' as date) + 15 ";
		//filtro += "cast(p.arrivo as date) between cast(\'" + dadata + "\' as date) and cast(\'" + adata + "\' as date) or ";
		//filtro += "cast(p.partenza as date) <= cast(\'" + dadata + "\' as date) and cast(p.arrivo as date) >= cast(\'" + adata + "\' as date) )) ";

		XData.Titles.Clear();
		//sqlstr = "select ";
		//sqlstr += "(trim(iif(EXTRACT(DAY FROM a.PARTENZA) > 9, '', '0') || EXTRACT(DAY FROM a.PARTENZA)) || '.' ||";
		//sqlstr += "trim(iif(EXTRACT(MONTH FROM a.PARTENZA) > 9, '', '0') || EXTRACT(MONTH FROM a.PARTENZA))) as DATA1, ";
		//sqlstr += "EXTRACT(MONTH FROM A.PARTENZA)) AS DATA1, ";

		sqlstr = "select a.id, a.partenza, a.arrivo ";
		sqlstr += "from prenotazioni as a ";
		sqlstr += "left join mezzi as b on b.id = a.mezzo_ek ";
		sqlstr += "left join ubicazione as u on u.id = a.ubicazione_ek ";
		sqlstr += "left join comuni as c on c.comune_k = u.comune_ek ";
		sqlstr += "where c.comune='Trento' and u.abilitato = 1 and ";
		sqlstr += filtro;
		sqlstr += "order by a.partenza ";

		tTitolo.Text = "Prenotazioni dal " + dada.ToString("dd-MM-yyyy") + " sino al " + ada.ToString("dd-MM-yyyy");
		tTitolo.Enabled = false;

		tbl.Clear();
		tbl.Columns.Clear();
		tbl = FBConn.getfromTbl(sqlstr, out msg);

		if (tbl.Rows.Count > 0)
		{
			//tbl.Rows[0].Delete(); // cancello la prima riga
			//titolo += " " + tbl.Rows.Count.ToString();
			int giorni = (int)(ada - dada).TotalDays + 1; // compresi estremi 
			giorni = giorni <= 2 ? 1 : giorni; // (a meno che non sia un giorno)
			int[] numero = new int[giorni];
			String[] dates = new String[giorni];
			for (int i = 0; i < giorni; i++) { numero[i] = 0; }// inizializzo
			int y, m, d, H, min, sec;
			DateTime dd, ad, oggi = dada;
			for (int gg = 0; gg < giorni; gg++)
			{
				dates[gg] = oggi.ToString("dd/MM");
				for (int n = 0; n < tbl.Rows.Count; n++)
				{
					s = tbl.Rows[n][1].ToString(); dd = DateTime.ParseExact(s, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
					s = tbl.Rows[n][2].ToString(); ad = DateTime.ParseExact(s, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
					if (dd.Date <= oggi.Date && ad.Date >= oggi.Date) numero[gg]++;
				}
				numero[gg] = sostanza - numero[gg];
				oggi = oggi.AddDays(1);
			}
			//fr = ds.Tables["sovrapposte"].Select("id='" + idp + "'");
			//DataRowCollection drc = ds.Tables["sovrapposte"].Rows;
			//contiene = fr.Length > 0 ? 1 : 0;

			cLibere.Series.Clear();
			//cLibere.DataSource = tbl;
			cLibere.Titles.Add(titolo).Font = new System.Drawing.Font("Thaoma", 14);
			cLibere.BackColor = System.Drawing.Color.White; // colore dello sfondo lo sfondo del grafico
			cLibere.ChartAreas["ChartArea1"].AxisY.Title = "veicoli senza preno_\ntazione per data";
			cLibere.ChartAreas["ChartArea1"].AxisY.TitleFont = new System.Drawing.Font("Thaoma", 14);
			cLibere.ChartAreas["ChartArea1"].AxisY.TextOrientation = System.Web.UI.DataVisualization.Charting.TextOrientation.Rotated270;
			cLibere.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 7);
			cLibere.ChartAreas["ChartArea1"].AxisX.Interval = 1;
			cLibere.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = false;
			cLibere.ChartAreas["ChartArea1"].AxisX.LabelStyle.TruncatedLabels = false;
			cLibere.ChartAreas["ChartArea1"].BackColor = System.Drawing.Color.FromName("AliceBlue");
			cLibere.ChartAreas["ChartArea1"].BackSecondaryColor = System.Drawing.Color.FromName("Red");
			cLibere.ChartAreas["ChartArea1"].BackGradientStyle = System.Web.UI.DataVisualization.Charting.GradientStyle.TopBottom;
			cLibere.ChartAreas["ChartArea1"].AxisY.LabelStyle.TruncatedLabels = true;
			cLibere.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
			cLibere.ChartAreas["ChartArea1"].AxisY2.Enabled = new System.Web.UI.DataVisualization.Charting.AxisEnabled();
			cLibere.Series.Add("Prenotazionixdata");
			cLibere.Series["Prenotazionixdata"].BorderWidth = 3;
			cLibere.Series["Prenotazionixdata"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.SplineArea;
			//XData.Series["Prenotazionixdata"].XValueMember = "Data1";
			cLibere.Series["Prenotazionixdata"].LabelAngle = 33;
			cLibere.Series["Prenotazionixdata"].Font = new System.Drawing.Font("Thaoma", 14);
			cLibere.Series["Prenotazionixdata"].Color = System.Drawing.Color.Orange;
			//XData.Series["Prenotazionixdata"].YValueMembers = "numerop";
			//XData.ChartAreas["ChartArea1"].AxisY.Interval = 30;
			cLibere.Series["Prenotazionixdata"].IsValueShownAsLabel = true;
			cLibere.Series["Prenotazionixdata"].BorderWidth = 5;
			cLibere.Series["Prenotazionixdata"].BackSecondaryColor = System.Drawing.Color.Orange; // ?
			cLibere.Series["Prenotazionixdata"].LabelForeColor = System.Drawing.Color.Black; // colore dei Marker
			cLibere.Series["Prenotazionixdata"].LabelBorderWidth = 2;
			cLibere.Series["Prenotazionixdata"].MarkerSize = 17; // distanza dalla serie delle etichette (Marker)
			cLibere.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Thaoma", giorni > 28 ? 8 : 10);
			cLibere.ChartAreas[0].AxisX.TextOrientation = System.Web.UI.DataVisualization.Charting.TextOrientation.Rotated270;
			cLibere.Series["Prenotazionixdata"].Points.DataBindXY(dates, numero);
		}
		//}
	}

	protected void Display(System.Web.UI.DataVisualization.Charting.Chart s)
    {
        string ss = "";
        for (int i = 0; i < s.Titles.Count(); i++)
        {
            ss += s.Titles[i].Text + "; ";
        }
        tStato.Text = ss.ToString();
    }

    protected void rblist_SelectedIndexChanged(object sender, EventArgs e)
    {
        initChartCollection();
    }
	protected void rbPeriodi_SelectedIndexChanged(object sender, EventArgs e)
	{
		initChartCollectionXData();
		initChartLiere();
	}
	protected void bExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("pre.aspx");
    }
	protected void LeggiUbi()    // devo leggere la tabella ubicazini
	{
		msg = "";
		FBConn.openaFBConn(out msg);
		if (msg.Length >= 1)
			tStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
		else
		{
			msg = "";
			ds.Clear();
			ds = FBConn.getfromDSet("select a.*, b.comune from ubicazione as a left join comuni as b on a.comune_ek=b.comune_k where a.abilitato = 1 order by b.comune, a.ubicazione", "ubicazioni", out msg);
			FBConn.closeaFBConn(out s);
			if (msg.Length >= 1)
				tStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
			else
			{
				if (ds.Tables["ubicazioni"].Rows.Count > 0)
				{
					ddlRitiro.Items.Clear();
					ddlRitiroXData.Items.Clear();
					string ss = "";
					s = "";
					ddlRitiro.Items.Insert(0, new ListItem("", ""));
					ddlRitiroXData.Items.Insert(0, new ListItem("", ""));
					for (int i = 0; i < ds.Tables["ubicazioni"].Rows.Count; i++)
					{
						s = ds.Tables["ubicazioni"].Rows[i]["comune"] == DBNull.Value ? "" : ds.Tables["ubicazioni"].Rows[i]["comune"].ToString();
						s += ds.Tables["ubicazioni"].Rows[i]["via"] == DBNull.Value ? "" : ", " + ds.Tables["ubicazioni"].Rows[i]["via"].ToString();
						s += ds.Tables["ubicazioni"].Rows[i]["civico"] == DBNull.Value ? "" : " " + ds.Tables["ubicazioni"].Rows[i]["civico"].ToString();
						s += ds.Tables["ubicazioni"].Rows[i]["ubicazione"] == DBNull.Value ? "" : ", " + ds.Tables["ubicazioni"].Rows[i]["ubicazione"].ToString();
						ss = ds.Tables["ubicazioni"].Rows[i]["id"] == DBNull.Value ? "" : ds.Tables["ubicazioni"].Rows[i]["id"].ToString().Trim();
						ddlRitiro.Items.Insert(i + 1, new ListItem(s, ss));
						ddlRitiroXData.Items.Insert(i + 1, new ListItem(s, ss));
					}
				}
				else
					tStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella UBICAZIONI. Contattare l'assistenza al numero " + (string)Session["assistenza"];
			}
		}
	}
}
