using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BloodDonationApp.Models;
using DatabaseLayer;

namespace BloodDonationApp.Controllers
{
    public class UserTypeController : Controller
    {
        // GET: UserType
        OnlineBloodBankDbEntities DB = new OnlineBloodBankDbEntities();
        public ActionResult AllUserType()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var usertypes = DB.UserTypeTables.ToList();
            var listusertype1 = new List<UserTypeMV>();
            foreach (var usertype in usertypes)
            {
                var addusertypes = new UserTypeMV();
                addusertypes.UserTypeID = usertype.UserTypeID;
                addusertypes.UserType = usertype.UserType;
                listusertype1.Add(addusertypes);
            }
            return View(listusertype1);
        }

        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var usertypes = new UserTypeMV();
            return View(usertypes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserTypeMV usertypeMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var userTypeTable = new UserTypeTable();
                userTypeTable.UserTypeID = usertypeMV.UserTypeID;
                userTypeTable.UserType = usertypeMV.UserType;
                DB.UserTypeTables.Add(userTypeTable);
                DB.SaveChanges();
                return RedirectToAction("AllUserType");
            }
            return View(usertypeMV);
        }

        public ActionResult Edit(int id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var usertype = DB.UserTypeTables.Find(id);
            if (usertype == null)
            {
                return HttpNotFound();
            }
            var usertypemv = new UserTypeMV();
            usertypemv.UserTypeID = usertype.UserTypeID;
            usertypemv.UserType = usertype.UserType;

            return View(usertypemv);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserTypeMV userTypeMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var userTypeTable = new UserTypeTable();
                userTypeTable.UserTypeID = userTypeMV.UserTypeID;
                userTypeTable.UserType = userTypeMV.UserType;

                DB.Entry(userTypeTable).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("AllUserType");
            }
            return View(userTypeMV);
        }

        public ActionResult Delete(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var usertype = DB.UserTypeTables.Find(id);
            if (usertype == null)
            {
                return HttpNotFound();
            }
            var usertypemv = new UserTypeMV();
            usertypemv.UserTypeID = usertype.UserTypeID;
            usertypemv.UserType = usertype.UserType;

            return View(usertypemv);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var usertype = DB.UserTypeTables.Find(id);
            DB.UserTypeTables.Remove(usertype);
            DB.SaveChanges();
            return RedirectToAction("AllUserType");
        }


    }
}