using Facebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Facebook.Controllers
{
    public class ChatController : Controller
    {
        DemoSMS_OnlienEntities1 db = new DemoSMS_OnlienEntities1();
        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Chat()
        {
            var userMessages = from usersChatBoard in db.ChatBoards
                               orderby usersChatBoard.DateTimeOfMessage ascending
                               select usersChatBoard;

            ViewBag.ChatBoardList = userMessages.ToList();


            var onlineUsers = from user in db.Users
                              where user.Status == "Logged In"
                              select user;

            ViewBag.UsersOnlineList = onlineUsers.ToList();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Chat(Facebook.Models.ChatBoard chatModel)
        {
            Models.User user = new Models.User();
            Models.ChatBoard chatBoard = new ChatBoard();
            chatBoard.FromUser = Session["UserName"].ToString();
            chatBoard.Message = chatModel.Message;
            chatBoard.DateTimeOfMessage = DateTime.Now;

            db.ChatBoards.Add(chatBoard);
            await db.SaveChangesAsync();

            return RedirectToAction("Chat", "Chat");
        }
    }
}