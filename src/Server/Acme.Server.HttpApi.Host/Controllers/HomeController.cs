namespace Acme.Server.Controllers;

public class HomeController : AbpController
{
    public ActionResult Index()
    {
        return this.Redirect("~/swagger");
    }
}
