using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Facebook.Models;

namespace Facebook.Controllers
{
    public class UsersController : Controller
    {
        private DemoSMS_OnlienEntities1 db = new DemoSMS_OnlienEntities1();

        

        
        

       

        

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "User_Id,UserName,PassWord,Email,DOB,Gender,WorkStatus,Address,Status")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_Id,UserName,PassWord,Email,DOB,Gender,WorkStatus,Address,Status")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        String sql_con = @"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=DemoSMS_Onlien;Integrated Security=True";
        public ActionResult Index()
        {

            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                ViewBag.error = "Login failed";
                return RedirectToAction("Login");
            }
        }


        public ActionResult Login()
        {
            return View();
        }

        static int a;
        SqlConnection con;
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password, string status)
        {
            if (ModelState.IsValid)
            {
                var f_password = EncodePassword(password);
                var data = db.Users.Where(s => s.Email.Equals(email) && s.PassWord.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().UserName;

                    if (data.FirstOrDefault().Gender == true)
                    {
                        Session["Gender"] = "Male";
                    }
                    else if (data.FirstOrDefault().Gender == false)
                    {
                        Session["Gender"] = "Female";
                    }



                    if (data.FirstOrDefault().WorkStatus == true)
                    {
                        Session["WorkStatus"] = "Students";
                    }
                    else if (data.FirstOrDefault().WorkStatus == false)
                    {
                        Session["WorkStatus"] = "Employees";
                    }

                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["Address"] = data.FirstOrDefault().Address;
                    Session["DOB"] = data.FirstOrDefault().DOB;
                    Session["UserID"] = data.FirstOrDefault().User_Id;
                    a = data.FirstOrDefault().User_Id;
                    Session["UserName"] = data.FirstOrDefault().UserName;


                    con = new SqlConnection(sql_con);
                    String sql = "update Users set Status = @Status where User_Id =" + data.FirstOrDefault().User_Id;
                    SqlCommand command = new SqlCommand(sql, con);

                    command.Parameters.AddWithValue("@Status", "Logged In");

                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                    return RedirectToAction("Index");
                }


                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            { // check xem email đã tồn tại trên đb chưa
                var checkEmail = db.Users.FirstOrDefault(s => s.Email == _user.Email);
                var checkUserName = db.Users.FirstOrDefault(s => s.UserName == _user.UserName);
                if (checkEmail == null && checkUserName == null)
                {
                    _user.PassWord = EncodePassword(_user.PassWord);
                    //_user.Password = _user.Password;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Users.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return RedirectToAction("Register");
                }
            }
            return View();
        }

        //Logout
        public ActionResult Logout()
        {
            var data = Session["User_Id"];
            //var data = Session["Email"].ToString();

            con = new SqlConnection(sql_con);
            //String sql = "update Users set Status = 'Logged Out' where Email = " + " ' " + data + " ' ";
            String sql = "update Users set Status = 'Logged Out' where User_Id = " + a;
            SqlCommand command = new SqlCommand(sql, con);

            //command.Parameters.AddWithValue("@Status", "Logged Out");

            con.Open();
            command.ExecuteNonQuery();
            con.Close();

            //Session.Clear();
            return RedirectToAction("Login");
        }

        //create md5 string
        public static string EncodePassword(string originalPassword)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }
    }
}
