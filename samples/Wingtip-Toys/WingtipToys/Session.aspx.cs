using System;
using System.Web.UI;

namespace WingtipToys
{
    public partial class Session : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var sessionText = Request.QueryString["SessionText"];
            sessionTextLabel.Text = sessionText;

            var pulledFromSession = Session["TestSessionText"];
            pullFromSession.Text = pulledFromSession?.ToString();
        }
    }
}