using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SharedStuff;
using WingtipToys.Models;

namespace WingtipToys
{
  public partial class About : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
        var foo = Session["foo"] as Foo ?? new Foo {Bar = "server"};
        Session["foo"] = foo;
        FooLabel.Text = foo.Bar;
    }
  }
}