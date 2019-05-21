using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SysAdmin.Controllers
{
    public class HomeController : Controller
    {
        public string alertMeddelande;
        // GET: Home
        public ActionResult Index()
        {
            // AdminServerRefferens.InloggningServiceClient kollaOmDuÄrInloggad = new AdminServerRefferens.InloggningServiceClient();
            //kollaOmDuÄrInloggad.LoggaIn(); 
            if(Session["LoginError"] != null)
            {
                ViewBag.alert = Session["LoginError"];
            }
            
            return View();
        }

        [HttpPost]
        [ActionName("LoggaIn")]
        public ActionResult LoggaIn(string anvandarnamn, string losenord)
        {
            AdminServerRefferens.InloggningServiceClient kollaOmDuÄrInloggad = new AdminServerRefferens.InloggningServiceClient();
            AdminServerRefferens.Anvandare anvandare = null;
            try
            {
                anvandare = kollaOmDuÄrInloggad.LoggaIn(anvandarnamn, losenord, "sysadmin");

            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);

            }

            if (anvandare != null)
            {
                //Inloggningen lyckades
                Session["anvandarId"] = anvandare.Id;
                System.Diagnostics.Debug.WriteLine("Loggar in som: " + anvandarnamn);
                return RedirectToAction("Index", "AdminLista");
            }

            Session["LoginError"] = "Inloggningen misslyckades, försök igen!";
            System.Diagnostics.Debug.WriteLine("Inloggningen misslyckades: " + anvandarnamn);
            return RedirectToAction("Index");
        }
        
    }
}
