using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Facebook.Models;

namespace Facebook.Controllers
{
    public class FriendsController : Controller
    {
        private DemoSMS_OnlienEntities1 db = new DemoSMS_OnlienEntities1();
        static String sql_con = @"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=DemoSMS_Onlien;Integrated Security=True";
        SqlConnection con = new SqlConnection(sql_con);

        // GET: Friends
        public ActionResult Index()
        {
            var user = db.Users.ToList();
            return View(user.ToList());
        }

        // GET: Friends/Details/5
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

        // GET: Friends/Create
        public ActionResult Create()
        {
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "UserName");
            ViewBag.UserFriend_Id = new SelectList(db.Users, "User_Id", "UserName");
            return View();
        }

        // POST: Friends/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Friend_Id,FriendName,UserFriend_Id,User_Id")] Friend friend)
        {
            if (ModelState.IsValid)
            {
                db.Friends.Add(friend);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "UserName", friend.User_Id);
            ViewBag.UserFriend_Id = new SelectList(db.Users, "User_Id", "UserName", friend.UserFriend_Id);
            return View(friend);
        }

        // GET: Friends/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friend friend = db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "UserName", friend.User_Id);
            ViewBag.UserFriend_Id = new SelectList(db.Users, "User_Id", "UserName", friend.UserFriend_Id);
            return View(friend);
        }

        // POST: Friends/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Friend_Id,FriendName,UserFriend_Id,User_Id")] Friend friend)
        {
            if (ModelState.IsValid)
            {
                db.Entry(friend).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "UserName", friend.User_Id);
            ViewBag.UserFriend_Id = new SelectList(db.Users, "User_Id", "UserName", friend.UserFriend_Id);
            return View(friend);
        }

        // GET: Friends/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friend friend = db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        // POST: Friends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Friend friend = db.Friends.Find(id);
            db.Friends.Remove(friend);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult notications()
        {
            var fen = db.Friends.ToList();
            return View(fen.ToList());
        }

        public String Accept(int User_Id, int UserFriend_Id)
        {
            con.Open();
            string updateSql = "update Friend set Status = 'Accepted' where User_Id = @senderId and UserFriend_Id = @receiverId";
            SqlCommand command = new SqlCommand(updateSql, con);
            command.Parameters.AddWithValue("@senderId", User_Id);
            command.Parameters.AddWithValue("@receiverId", UserFriend_Id);

            command.ExecuteNonQuery();
            return "abc";
        }

        public String Decline(int User_Id, int UserFriend_Id)
        {
            con.Open();
            string updateSql = "update Friend set Status = 'Refused' where User_Id = @senderId and UserFriend_Id = @receiverId";
            SqlCommand command = new SqlCommand(updateSql, con);
            command.Parameters.AddWithValue("@senderId", User_Id);
            command.Parameters.AddWithValue("@receiverId", UserFriend_Id);

            command.ExecuteNonQuery();
            return "abc";
        }

        //[HttpPost]
        public String addFriend(int UserFriend_Id, string FriendName)
        {
            con.Open();
            var a = Session["UserID"];

            String sql = "insert into Friend values(@FriendName, @UserFriend_Id, @UserId,@Status)";
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@FriendName", FriendName);
            command.Parameters.AddWithValue("@UserFriend_Id", UserFriend_Id);
            command.Parameters.AddWithValue("@UserId", a);
            command.Parameters.AddWithValue("@Status", "null");
            command.ExecuteNonQuery();

            return "abc";
        }

        //[HttpPost]
        public String Like(int idLiked, int Status)
        {
            //string b = a;
            con.Open();
            var a = Session["UserID"];


            //0 là dislike
            //1 là like
            //null là không có gì
            //id là người dùng nhận like

            if (Status == 1)
            {
                //var sqlLike = "Select * from Emoji where User_Sender = " + a + " And User_Receiver = " + idLiked + " And Status = " + 1;
                var checkFalse = db.Emojis.Where(s => s.User_Sender == (int)a && s.User_Receiver == idLiked && s.Status == false).ToList();
                var checkTrue = db.Emojis.Where(s => s.User_Sender == (int)a && s.User_Receiver == idLiked && s.Status == true).ToList();
                if (checkTrue.Count() > 0)
                {
                    String sqlDislikeUpdate = "Update Emoji Set Status = 0 where User_Sender = " + a + " And User_Receiver = " + idLiked;
                    SqlCommand commandLike = new SqlCommand(sqlDislikeUpdate, con);
                    commandLike.ExecuteNonQuery();
                }
                else if (checkFalse.Count() > 0)
                {
                    String sqlLike = "Update Emoji Set Status = 1 where User_Sender = " + a + " And User_Receiver = " + idLiked;
                    SqlCommand commandLike = new SqlCommand(sqlLike, con);
                    commandLike.ExecuteNonQuery();
                }
                else
                {
                    String sql = "insert into Emoji values(@Sender, @Receiver, @Status)";
                    SqlCommand command = new SqlCommand(sql, con);
                    command.Parameters.AddWithValue("@Sender", a);
                    command.Parameters.AddWithValue("@Receiver", idLiked);
                    command.Parameters.AddWithValue("@Status", Status);

                    command.ExecuteNonQuery();
                }
            }
            else if (Status == 0)
            {
                var checkFalse = db.Emojis.Where(s => s.User_Sender == (int)a && s.User_Receiver == idLiked && s.Status == false).ToList();
                var checkTrue = db.Emojis.Where(s => s.User_Sender == (int)a && s.User_Receiver == idLiked && s.Status == true).ToList();
                //var likeToDislike = db.Emojis.Where(s => s.User_Sender == (int)a && s.User_Receiver == idLiked && s.Status == true).ToList();
                if (checkFalse.Count() > 0)
                {
                    String sqlLike = "Update Emoji Set Status = 1 where User_Sender = " + a + " And User_Receiver = " + idLiked;
                    SqlCommand commandLike = new SqlCommand(sqlLike, con);
                    commandLike.ExecuteNonQuery();
                }
                else if(checkTrue.Count() > 0)
                {
                    String sqlLike = "Update Emoji Set Status = 0 where User_Sender = " + a + " And User_Receiver = " + idLiked;
                    SqlCommand commandLike = new SqlCommand(sqlLike, con);
                    commandLike.ExecuteNonQuery();
                }
                else
                {
                    String sql = "insert into Emoji values(@Sender, @Receiver, @Status)";
                    SqlCommand command = new SqlCommand(sql, con);
                    command.Parameters.AddWithValue("@Sender", a);
                    command.Parameters.AddWithValue("@Receiver", idLiked);
                    command.Parameters.AddWithValue("@Status", Status);

                    command.ExecuteNonQuery();
                }
            }
            con.Close();

            return "abc";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
