using System.Data;
using DAL;
using JewelleryStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Microsoft.AspNet.SignalR;

namespace JewelleryStore.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [LogAttribute(LogType.Home,false)]
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Title = "Index";
            using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
            {
                var data = dbContext.tblProduct.ToList();
                return View(data);
            }
        }
        [HttpPost]
        public bool SendConfirmMail(string email)
        {
            try
            {
                string Guid = string.Empty;
                using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
                {
                    Guid = dbContext.tblUser.FirstOrDefault(us => us.Email == email).GuidID.ToString();
                }
                SmtpClient sc = new SmtpClient();
                sc.Port = 587;
                sc.Host = "smtp.gmail.com";
                sc.EnableSsl = true;
                sc.Credentials = new NetworkCredential("JewelleryStore78@gmail.com", "Sahibinden2015");
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("JewelleryStore78@gmail.com", "Jewellery Store");
                mail.To.Add(email);
                //mail.Subject = "JewelleryStore Login Confirm Link"; mail.IsBodyHtml = true; mail.Body = "<a href='http://localhost:17716/ConfirmJewelleryLogin/" + Guid + "' TARGET = '_blank'>Please Confirm Your Mail!</a>";
                mail.Subject = "JewelleryStore Login Confirm Link"; mail.IsBodyHtml = true; mail.Body = "<a href='jewellerystore.azurewebsites.net/ConfirmJewelleryLogin/" + Guid + "' TARGET = '_blank'>Please Confirm Your Mail!</a>";                
                sc.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void SendConfirmMail(string Guid, string email)
        {
            try
            {
                SmtpClient sc = new SmtpClient();
                sc.Port = 587;
                sc.Host = "smtp.gmail.com";
                sc.EnableSsl = true;
                sc.Credentials = new NetworkCredential("JewelleryStore78@gmail.com", "Sahibinden2015");
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("JewelleryStore78@gmail.com", "Jewellery Store");
                mail.To.Add(email);
                //mail.Subject = "JewelleryStore Login Confirm Link"; mail.IsBodyHtml = true; mail.Body = "<a href='http://localhost:17716/ConfirmJewelleryLogin/" + Guid + "' TARGET = '_blank'>Please Confirm Your Mail!</a>";
                mail.Subject = "JewelleryStore Login Confirm Link"; mail.IsBodyHtml = true; mail.Body = "<a href='jewellerystore.azurewebsites.net/ConfirmJewelleryLogin/" + Guid + "' TARGET = '_blank'>Please Confirm Your Mail!</a>";                
                sc.Send(mail);
            }
            catch (Exception ex)
            {
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            HttpCookie userCookie = Request.Cookies["IsRemmber"];
            if (userCookie != null) { ViewBag.Email = userCookie.Value; }

            ViewBag.Title = "Login";
            return View();
        }

        [HttpPost]
        [LogAttribute(LogType.LogIn,false)]
        public int Login(User user)
        {
            //Cookie İşlemleri
            if (user.IsRemmber)
            {
                HttpCookie userCookie = new HttpCookie("IsRemmber");
                userCookie.Value = user.Email;
                userCookie.Expires = DateTime.Now.AddDays(1d);
                Response.Cookies.Add(userCookie);
            }
            //Bitti--------------

            using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
            {
                var usr =
                    dbContext.tblUser.FirstOrDefault(us => us.Email == user.Email && us.Password == user.Password);
                if (usr == null || usr.isChecked == false)
                {
                    if (usr != null && usr.isChecked == false)
                    {
                        return (int)LoginState.WaitForApprove;//Kayıt var Onaylanmamış
                    }
                    else if (dbContext.tblUser.FirstOrDefault(us => us.Email == user.Email) != null)
                    {
                        if (dbContext.tblUser.FirstOrDefault(us => us.Email == user.Email).isChecked == false)
                        {
                            return (int)LoginState.WaitForApproveWrongPassword;//Şifre Yanlış Onaylanmamış Mail;
                        }
                        return (int)LoginState.WrongPassword;//Şifre yanlış 
                    }
                    else
                    {
                        return (int)LoginState.NoRecord;//Kayıt yok 
                    }
                }
                else
                {
                    Session["UserID"] = usr.ID;
                }

            }
            return 2;//Kayıtlı
        }

        [HttpGet]
        public ActionResult SignIn(string Password, string Email)
        {
            tblUser user = new tblUser() { Password = Password, Email = Email };
            return View(user);
        }
        [HttpPost]
        [LogAttribute(LogType.WaitForConfirm,false)]
        public int SignIn(tblUser user)
        {
            try
            {
                Guid _Guid = System.Guid.NewGuid();
                user.GuidID = _Guid;
                user.isChecked = false;
                SendConfirmMail(_Guid.ToString(), user.Email);
                using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
                {
                    if (dbContext.tblUser.FirstOrDefault(us => us.Email == user.Email) != null)
                    {
                        return 3;
                    }
                    dbContext.tblUser.Add(user);
                    dbContext.SaveChanges();
                }
                return 1;
            }
            catch (Exception)
            {

                return 2;
            }
        }

        [LogAttribute(LogType.SignIn,false)]
        public ActionResult ConfirmJewelleryLogin(Guid guid)
        {
            using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
            {
                var user = dbContext.tblUser.First(us => us.GuidID == guid);
                user.isChecked = true;
                dbContext.SaveChanges();
                Session["UserID"] = user.ID;
            }
            return RedirectToAction("Admin");

        }
        [LogAttribute(LogType.LogOut,true)]
        public ActionResult LogOut()
        {
            Session.Remove("UserID");
            return RedirectToAction("Login");
        }
        
        [LogAttribute(LogType.Admin,false)]
        public ActionResult Admin()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            List<DAL.tblProduct> data;
            using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
            {
                data = dbContext.tblProduct.ToList();
            }
            ViewBag.Title = "Admin";
            return View(data);
        }
        [HttpPost]
        [LogAttribute(LogType.ProductInsert,false)]
        public int Admin(tblProduct product)
        {
            try
            {
                using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
                {
                    dbContext.tblProduct.Add(product);
                    product.CreatedDate = DateTime.Now;
                    dbContext.SaveChanges();
                    Session["ProductID"] = product.ID;
                }
                return 1;
            }
            catch
            {
                return 2;
            }
        }
        [LogAttribute(LogType.ProductUpdate,false)]
        public int AdminUpdate(tblProduct product)
        {
            try
            {
                using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
                {
                    tblProduct tp = dbContext.tblProduct.FirstOrDefault(t => t.ID == product.ID);
                    tp.ImageUrl = product.ImageUrl;
                    tp.Name = product.Name;
                    tp.Price = product.Price;
                    tp.Description = product.Description;
                    dbContext.SaveChanges();
                    Session["ProductID"] = product.ID;
                }
                return 1;
            }
            catch
            {
                return 2;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string FileUpload(IEnumerable<HttpPostedFileBase> files)
        {
            string fileName = string.Empty;
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        file.SaveAs(path);
                    }
                }
            }
            return fileName;
        }
        public PartialViewResult ProductList()
        {
            List<DAL.tblProduct> data;
            using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
            {
                data = dbContext.tblProduct.ToList();
            }
            return PartialView(data);
        }
        public PartialViewResult ProductDetail()
        {
            return PartialView("ProductDetail", new tblProduct());
        }
        [HttpPost]
        public PartialViewResult ProductDetail(int DetailID)
        {
            tblProduct data;
            using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
            {
                data = dbContext.tblProduct.FirstOrDefault(pro => pro.ID == DetailID);
            }
            return PartialView("ProductDetail", data);
        }
        [HttpPost]
        [LogAttribute(LogType.ProductDelete,false)]
        public PartialViewResult ProductDelete(int DetailID)
        {
            using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
            {
                Session["ProductID"] = DetailID;
                tblProduct product = dbContext.tblProduct.FirstOrDefault(pro => pro.ID == DetailID);
                dbContext.tblProduct.Remove(product);
                dbContext.SaveChanges();
                var data = dbContext.tblProduct.ToList();
                return PartialView("ProductList", data);
            }
        }

        [LogAttribute(LogType.Log,true)]
        public ActionResult Log()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Title = "Log";
            List<DAL.tblLog> data;
            List<Log> LogData = new List<Log>();
            using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
            {
                data = dbContext.tblLog.OrderByDescending(tg=>tg.ID).ToList();

                foreach (var item in data)
                {
                    Log lg = new Log();
                    lg.ID = item.ID;
                    lg.LogType = item.LogType;
                    var product = dbContext.tblProduct.FirstOrDefault(tp => tp.ID == item.ProductID);
                    var user = dbContext.tblUser.FirstOrDefault(us => us.ID == item.UserID);
                    string prdoct=item.ProductID==null?"-":item.ProductID.ToString();
                    lg.ProductName = product == null ? prdoct : product.Name;
                    lg.UserName = user == null ? item.UserID.ToString() : user.Name;
                    lg.CreatedDate = item.CreatedDate;
                    LogData.Add(lg);
                }
            }
            return View(LogData);
        }
        public class Jewellery : Hub
        {
            public void RefreshProduct()
            {
                List<DAL.tblProduct> data;
                using (JewelleryStoreDB dbContext = new JewelleryStoreDB())
                {
                    data = dbContext.tblProduct.ToList();
                }
                string viewName = @"~/Views/Shared/ProductScreen.cshtml";
                var productScreenPrint = Helper.GetRazorViewAsString(data, viewName);

                Clients.All.reloadProduct(productScreenPrint);
            }
        }
    }
}