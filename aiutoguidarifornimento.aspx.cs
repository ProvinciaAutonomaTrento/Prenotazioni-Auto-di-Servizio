using System;
using System.Web.UI;
using System.Data;
using System.Text;
using System.Drawing;

public partial class aiutoguidarifornimento : System.Web.UI.Page
{
    public ConnessioneFB FBConn = new ConnessioneFB();
    public DataSet ds = new DataSet();
    public string msg;
    public string formatodata = "dd-MM-yyyy";
    public static user utenti = new user();
    public Color rosso = Color.Red;
    public Color nero = Color.Black;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
        {
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
}




