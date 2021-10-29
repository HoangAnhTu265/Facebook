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
            var users = db.Users.ToList();
           
            var id = Session["UserID"];
            

            //đây là  những người bạn và những người yêu cầu kết bạn
            //List<int> sql3 = db.Friends.Where(f => f.User_Id == (int)id).Select(f => f.Friend_Id).ToList();

            var sql3 = db.Friends.Where(f => f.User_Id == (int)id).ToList();

            var sqlUser = db.Users.ToList();
            var sqlFriend = db.Friends.ToList();
           // lấy id của những người bạn cho vào cái list
            List<int> listIdFriend = new List<int>();
            List<int> listIdUserFriend = new List<int>();
            List<User> listUser_ChuaAddFriend = new List<User>();

            List<int> idNhungNguoiDangyeuCauKetBan = new List<int>();
            List<int> idNhungNguoiDaDongYKetBan = new List<int>();
            List<int> listNhungNguoiTuChoiKetBan = new List<int>();
            // list int lấy tất cả thằng đã là friend add vào list
            // foreach 
            //foreach(var f in sql3)
            //{
            //    idFriend.Add((int)f.UserFriend_Id);
            //    idFriend.Add((int)f.User_Id);
            //} 
            // lấy ở trong bảng user lấy id của những thằng đã là bạn gán vào một biến nào đấy
            // lấy ở trong bảng friend lấy tất cả ngoại trừ những thằng đã có cái biến kia 
            //var sql4 = db.Friends.Where()

            //var userUnfriend = db.Users.ToList();
            //foreach (int a in sql3)
            //{
            //     userUnfriend = db.Users.Where(u => u.User_Id != a).ToList();
            //}


            List<User> listUserChuaKetBan = new List<User>();

            List<int> idUserFriend = new List<int>();
            foreach (var f in db.Friends)
            {
                idUserFriend.Add((int)f.UserFriend_Id);
                if (f.Status == null || f.Status.Contains("null") && f.User_Id == (int)id)
                {
                    
                    idNhungNguoiDangyeuCauKetBan.Add((int)f.UserFriend_Id);
                }
                else if (f.Status.Contains("Accepted") && f.User_Id == (int)id)
                    {
                    idNhungNguoiDaDongYKetBan.Add((int)f.UserFriend_Id);
                }
                else if(f.User_Id == (int)id)
                {
                    listNhungNguoiTuChoiKetBan.Add((int)f.UserFriend_Id);
                }
            }
            int count1 = idNhungNguoiDaDongYKetBan.Count();
            int count2 = idNhungNguoiDangyeuCauKetBan.Count();
            int count3 = idUserFriend.Count();
        
            //foreach(var i in idUserFriend) // chứa id userfriend , hiện tại đang count = 14
            //{
            //    foreach(var j in idNhungNguoiDaDongYKetBan) // chứa id của những người đã đồng ý kết bạn , hiện count = 3 đăng nhập bằng nick tuan
            //    {
            //        if (idUserFriend.Contains(j) && ) {

            //        }
            //    }
                
            //}

            foreach(var u in db.Users)
            {
                
                //if(!(idNhungNguoiDaDongYKetBan.Contains(u.User_Id)) && !(idNhungNguoiDangyeuCauKetBan.Contains(u.User_Id)))
                if (!(idNhungNguoiDaDongYKetBan.Contains(u.User_Id)))
                {
                    listUserChuaKetBan.Add(u);
                }
                 else
                {
                    //listUserChuaKetBan.Add(u);
                }
            }
            return View(listUserChuaKetBan);
        }
        //var sqlGet = db.Users.Where(s => s.User_Id != f.UserFriend_Id).ToList();
        //var sqlGet = db.Users.Where(s => s.User_Id != f.UserFriend_Id).ToList();
        //var sqlGet2 = db.Users.Where(s => s.Friends.UserFriend_Id != f.UserFriend_Id).ToList();
        //if (f.User_Id != (int)id && idFriend.Contains(f.User_Id))
        //{
        //    listUser_ChuaAddFriend.Add(f);
        //}
        //foreach(var b in sqlGet)
        //{

        //    if(b.User_Id == f.UserFriend_Id)
        //    {
        //        continue;
        //    } 
        //        listUser_ChuaAddFriend.Add(b);


        //}
        //foreach (var b in sqlGet)
        //foreach (var b in sqlUser) // lấy tất cả user
        //{

        //    if (b.User_Id == f.UserFriend_Id)
        //    {
        //        c = false;
        //        continue;
        //    }
        //    else
        //    {
        //        listUser_ChuaAddFriend.Add(b);
        //        c = true;
        //    }
        //}

        //foreach(var i in listUser_ChuaAddFriend)
        //{
        //    if()
        //}


        //List<Friend> listUser_ChuaAddFriend = new List<Friend>();

        //lấy danh sách những người có id không nằm trong list id người bạn và chính mình
        //foreach (var u in db.Users)
        //{
        //    //if (u.User_Id != (int)id && !idFriend.Contains(u.User_Id))
        //    //{
        //    //    listUser_ChuaAddFriend.Add(u);
        //    //}
        //    if (u.User_Id != (int)id && idFriend.Contains(u.User_Id) && idFriend.Contains((int)u.Friend.UserFriend_Id))
        //    {
        //        //listUser_ChuaAddFriend.Add(u);
        //    } else
        //    {
        //        listUser_ChuaAddFriend.Add(u);
        //    }
        //}
        //}




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

        //public String checkBtnAdd(int UserFriend_Id)
        //{
        //    con.Open();
        //    var a = Session["UserID"];
        //    //string sqlCheck = "select * from Friend where UserFriend_Id = " + UserFriend_Id + " and User_Id = " + a;
        //    var checkTrue = db.Friends.Where(s => s.UserFriend_Id == UserFriend_Id && s.User_Id == (int)a).ToList();
        //    //SqlCommand command = new SqlCommand(checkTrue, con);
        //    //command.ExecuteNonQuery();
        //    int b = checkTrue.Count();
        //    if (checkTrue.Count() > 0)
        //    {
        //        return ViewBag.CheckStatusFriend = "hai bạn đã kết bạn rồi";
        //    }
        //    else
        //    {
        //        return ViewBag.CheckStatusFriend = "hai bạn chưa kết bạn";
        //    }
        //    //return "abc";
        //}

        public ActionResult notications()
        {
            var fen = db.Friends.ToList();
            return View(fen.ToList());
        }

        public String Accept(int User_Id, int UserFriend_Id)
        {
            var a = Session["UserID"];
            con.Open();

            //String sql = "insert into Friend values(@FriendName, @UserFriend_Id, @UserId,@Status)";
            //SqlCommand command = new SqlCommand(sql, con);
            //command.Parameters.AddWithValue("@FriendName", FriendName);
            //command.Parameters.AddWithValue("@UserFriend_Id", UserFriend_Id);
            //command.Parameters.AddWithValue("@UserId", a);
            //command.Parameters.AddWithValue("@Status", "Accepted");
            //command.ExecuteNonQuery();
            //var name = from c in db.Friends where c.User_Id == User_Id select c;

            //var name = Session;

            //string insertSql = "insert into Friend values(@FriendName, @UserFriend_Id, @UserId,@Status)";
            //SqlCommand command2 = new SqlCommand(insertSql, con);
            //command2.Parameters.AddWithValue("@FriendName", UserName);
            //command2.Parameters.AddWithValue("@UserFriend_Id", UserFriend_Id);
            //command2.Parameters.AddWithValue("@UserId", a);
            //command2.Parameters.AddWithValue("@Status", "Accepted");
            //command2.ExecuteNonQuery();


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

            string acceptBtn = null;
            string dislikeBtn = null;

            //check xem đã kết bạn chưa
            var checkTrue = db.Friends.Where(s => s.UserFriend_Id == UserFriend_Id && s.User_Id == (int)a).ToList();

            //để nguời nhân đc thông báo không gửi lại thông báo cho nguời gửi nữa 
            var checkTrue2 = db.Friends.Where(s => s.UserFriend_Id == (int)a && s.User_Id == UserFriend_Id).ToList();

            if (checkTrue.Count() > 0)
            {
                ViewBag.check = "bạn và người này đã kết bạn rồi";
            }
            else if (checkTrue2.Count() > 0)
            {
                ViewBag.check = "bạn và người này đã kết bạn rồi";
            }
            else
            {
                String sql = "insert into Friend values(@FriendName, @UserFriend_Id, @UserId,@Status)";
                SqlCommand command = new SqlCommand(sql, con);
                command.Parameters.AddWithValue("@FriendName", FriendName);
                command.Parameters.AddWithValue("@UserFriend_Id", UserFriend_Id);
                command.Parameters.AddWithValue("@UserId", a);
                command.Parameters.AddWithValue("@Status", "null");
                command.ExecuteNonQuery();
            }

            

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
