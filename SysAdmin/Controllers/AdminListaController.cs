using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SysAdmin.Controllers
{
    public class AdminListaController : Controller
    {
        // GET: AdminLista
        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("Test:" + Session["anvandarId"]);
            AdminServerRefferens.InloggningServiceClient klient = new AdminServerRefferens.InloggningServiceClient();
            List<AdminServerRefferens.Anvandare> lista = new List<AdminServerRefferens.Anvandare>();
            
            lista = klient.HamtaAllaAnvandare().ToList();

            int aId = int.Parse(Session["anvandarId"].ToString());
            if (klient.VerifieraInloggning(aId))
            {
                System.Diagnostics.Debug.WriteLine("Vi är inloggad!");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Vi är inte inloggad!");
            }

            return View(lista);
        }
        public ActionResult Andra(int? id) // Ändrar värdet i "admin behörighet"
        {
            AdminServerRefferens.InloggningServiceClient klient = new AdminServerRefferens.InloggningServiceClient();
            AdminServerRefferens.Anvandare anvandare = klient.VisaAnvandarInfoId(id.Value);
            System.Diagnostics.Debug.WriteLine(anvandare.Id + ":" + anvandare.Behorighet);
            return View(anvandare);
        }
        [HttpPost]
        public ActionResult Andra([Bind(Include = "Id,Behorighet")] AdminServerRefferens.Anvandare anvandare)
        {
            System.Diagnostics.Debug.WriteLine("Användare: " + anvandare.Id + " - " + anvandare.Behorighet);
            AdminServerRefferens.InloggningServiceClient klient = new AdminServerRefferens.InloggningServiceClient();
            klient.AndraBehorighet(anvandare.Id, anvandare.Behorighet);
            return RedirectToAction("Index");
        }
        public ActionResult TaBort(int? id)
        {
            AdminServerRefferens.InloggningServiceClient klient = new AdminServerRefferens.InloggningServiceClient();
            AdminServerRefferens.Anvandare anvandare = klient.VisaAnvandarInfoId(id.Value);

            return View(anvandare);

        }
        [HttpPost, ActionName("TaBort")]
        public ActionResult TaBortAccepterat(int id)
        {

            AdminServerRefferens.InloggningServiceClient klient = new AdminServerRefferens.InloggningServiceClient();
            klient.TaBortAnvandare(id);
            System.Diagnostics.Debug.WriteLine("Ta bort ID: " + id);

            return RedirectToAction("Index");
        }
       
        public ActionResult LaggTillAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LaggTillAdmin(string anvandarnamn, string email, string behorighet, string losenord, string losenordRepeat)
        {
            if (losenord == losenordRepeat)
            {
                AdminServerRefferens.InloggningServiceClient klient = new AdminServerRefferens.InloggningServiceClient();
                if (klient.VerifieraInloggning(12))
                {
                    System.Diagnostics.Debug.WriteLine("Du är inloggad");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Du är inte inloggad");
                }
                klient.RegistreraAdmin(anvandarnamn, losenord, email, behorighet);
                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.VisaFelMeddelande = "De inmatade lösenorden överenstämmer inte med varandra";
            }

            return View();    
        }
    }
}