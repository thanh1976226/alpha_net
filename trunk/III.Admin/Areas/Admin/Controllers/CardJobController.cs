using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ESEIM.Models;
using ESEIM.Utils;
using FTU.Utils.HelperNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class CardJobCOntroller : BaseController
    {
        private readonly EIMDBContext _context;
        private readonly IActionLogService _actionLog;
        private readonly IUploadService _upload;
        private readonly IHostingEnvironment _hostingEnvironment;


        public IActionResult Index()
        {
            return View();
        }

        public CardJobCOntroller(EIMDBContext context, IUploadService upload, IActionLogService actionLog, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _upload = upload;
            _actionLog = actionLog;
            _hostingEnvironment = hostingEnvironment;

        }

        #region Team
        [HttpPost]
        public JsonResult InsertTeam([FromBody]WORKOSTeam data)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                string code = "TEAM_" + (_context.WORKOSTeams.Count() > 0 ? _context.WORKOSTeams.Last().Id + 1 : 1);
                data.TeamCode = code;

                _context.WORKOSTeams.Add(data);
                _context.SaveChanges();

                msg.Title = "Thêm nhóm mới thành công!";
                msg.Error = false;
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra khi thêm!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult EditTeam([FromBody]WORKOSTeam data)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var currentData = _context.WORKOSTeams.FirstOrDefault(x => x.Id == data.Id);
                currentData.TeamName = data.TeamName;
                currentData.Member = data.Member;
                currentData.Leader = data.Leader;

                _context.WORKOSTeams.Update(currentData);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Cập nhật thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult GetTeams()
        {
            var data = _context.WORKOSTeams.Where(x => x.Flag == false).OrderBy(x => x.TeamCode).ToList();
            return Json(data);
        }
        [HttpPost]
        public JsonResult GetTeam(string TeamCode)
        {
            var data = _context.WORKOSTeams.FirstOrDefault(x => x.TeamCode.Equals(TeamCode));
            return Json(data);
        }
        [HttpPost]
        public JsonResult DeleteTeam(string TeamCode)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.WORKOSTeams.FirstOrDefault(x => x.TeamCode.Equals(TeamCode));
                data.Flag = true;

                _context.WORKOSTeams.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Xóa thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult GetTeamMember(string TeamCode)
        {
            var data = _context.WORKOSTeams.FirstOrDefault(x => x.TeamCode.Equals(TeamCode));
            var member = data.Leader + ";" + data.Member;
            return Json(member);
        }
        #endregion

        #region Board
        [HttpPost]
        public JsonResult GetBoards()
        {
            var data = _context.WORKOSBoards.Where(x =>
                                                    x.IsDeleted == false &&
                                                    _context.WORKOSTeams.FirstOrDefault(y => y.TeamCode.Equals(x.TeamCode)).Flag == false)
                                            .OrderBy(x => x.TeamCode).ToList();
            return Json(data);
        }
        [HttpPost]
        public JsonResult GetBoardDetail(string BoardCode)
        {
            var data = _context.WORKOSBoards.FirstOrDefault(x => x.BoardCode.Equals(BoardCode));
            return Json(data);
        }
        [HttpPost]
        public JsonResult InsertBoard([FromBody]WORKOSBoard data)
        {
            if (data.BoardCode == "" || data.BoardName == "")
            {
                return Json(new JMessage() { Error = true, Title = "Chưa nhập nội dung!" });
            }

            //if (_context.WORKOSBoard.Where(x => x.BoardCode.ToLower().Contains(data.BoardCode)).Count() > 0)
            //{
            //    return Json(new JMessage() { Error = true, Title = "Mã đã tồn tại!" });
            //}

            if (string.IsNullOrEmpty(data.TeamCode))
            {
                return Json(new JMessage() { Error = true, Title = "Chưa chọn nhóm!" });
            }

            var msg = new JMessage() { Error = true };
            try
            {
                data.BoardCode = "BOARD_" + (_context.WORKOSBoards.Count() > 0 ? _context.WORKOSBoards.Last().BoardID + 1 : 1);
                //data.BoardCode = data.BoardCode.ToUpper();
                if (data.Background == "")
                {
                    data.Background = "#ccc";
                }
                _context.WORKOSBoards.Add(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Thêm bảng mới thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Title = "Có lỗi khi thêm bảng mới!";
                msg.Object = ex;
                return Json(ex);
            }
        }
        [HttpPost]
        public JsonResult EditBoard([FromBody]WORKOSBoard data)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var editData = _context.WORKOSBoards.FirstOrDefault(x => x.BoardID == data.BoardID);
                editData.BoardName = data.BoardName;
                editData.Visibility = data.Visibility;
                editData.Background = data.Background;
                editData.Avatar = data.Avatar;
                editData.TeamCode = data.TeamCode;

                _context.WORKOSBoards.Update(editData);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Cập nhật thành công!";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi khi cập nhật bảng!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult DeleteBoard(int id)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.WORKOSBoards.FirstOrDefault(x => x.BoardID == id);
                data.IsDeleted = true;
                _context.WORKOSBoards.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Xóa bảng thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi khi xóa bảng!";
                return Json(msg);
            }
        }
        #endregion

        #region List
        [HttpPost]
        public JsonResult GetLists(string BoardCode)
        {
            try
            {
                var data = _context.WORKOSLists.Where(x => x.BoardCode.Equals(BoardCode) && x.IsDeleted == false).OrderBy(x => x.Order);
                return Json(data);
            }
            catch (Exception ex)
            {
                var msg = new JMessage() { Error = true, Title = "Có lỗi xảy ra!", Object = ex };
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult InsertList([FromBody]WORKOSList data)
        {
            var msg = new JMessage() { Error = true };
            if (data.ListName == "")
            {
                return Json(new JMessage() { Error = true, Title = "Nhập nội dung" });
            }

            //var a = _context.WORKOSList.Where(x => x.ListCode.ToUpper().Equals(data.ListCode.ToUpper())).Count();
            //if (a > 0)
            //{
            //    return Json(new JMessage() { Error = true, Title = "Mã danh sách đã tồn tại!" });
            //}            

            try
            {
                data.ListCode = "LIST_" + (_context.WORKOSLists.Count() > 0 ? (_context.WORKOSLists.Last().ListID + 1) : 1);
                if (data.Status == null)
                {
                    data.Status = 1;
                }
                //data.ListCode = "L-" + _context.WORKOSList.Count() + 1;
                data.ListCode = data.ListCode.ToUpper();
                //data.IsDeleted = false;
                data.Order = _context.WORKOSLists.Count() + 1;
                _context.WORKOSLists.Add(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Thêm danh sách mới thành công";
                msg.Object = data;
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi khi thêm danh sách mới!";
                return Json(msg);
                throw;
            }
        }
        [HttpPost]
        public JsonResult UpdateListName(int ListID, string NewName)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.WORKOSLists.FirstOrDefault(x => x.ListID == ListID);
                data.ListName = NewName;
                _context.WORKOSLists.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Cập nhật thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
                throw;
            }
        }
        [HttpPost]
        public JsonResult UpdateOrder(string Orther, string Entry)
        {
            var msg = new JMessage() { Error = true };
            List<string> orther = Orther.Split(',').ToList();
            List<string> entry = Entry.Split(',').ToList();
            List<string> a = new List<string>();
            List<string> b = new List<string>();

            for (int i = 0; i < orther.Count; i++)
            {
                if (orther[i] != entry[i])
                {
                    a.Add(orther[i]);
                    b.Add(entry[i]);
                }
            }

            //List<WORKOSList> data = new List<WORKOSList>();
            for (int i = 0; i < a.Count; i++)
            {
                var data = _context.WORKOSLists.FirstOrDefault(x => x.Order == int.Parse(b[i]));
                data.Order = int.Parse(a[i]);
            }

            _context.SaveChanges();

            return Json("");
        }
        [HttpPost]
        public JsonResult SortListByStatus(string BoardCode, int Orther)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                List<WORKOSList> data = new List<WORKOSList>();
                List<int> orther = new List<int>();

                if (Orther == 0)
                {
                    data = _context.WORKOSLists.Where(x => x.BoardCode.Equals(BoardCode)).OrderBy(x => x.Status).ToList();
                }
                else
                {
                    data = _context.WORKOSLists.Where(x => x.BoardCode.Equals(BoardCode)).OrderByDescending(x => x.Status).ToList();
                }

                for (int i = 0; i < data.Count; i++)
                {
                    orther.Add(data[i].Order);
                }
                orther.Sort();
                for (int i = 0; i < data.Count; i++)
                {
                    data[i].Order = orther[i];
                    _context.WORKOSLists.Update(data[i]);
                }

                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Sắp xếp thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult ChangeListStatus(int ListID, int Status)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.WORKOSLists.FirstOrDefault(x => x.ListID == ListID);
                data.Status = Status;
                _context.WORKOSLists.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Cập nhật trạng thái thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra khi cập nhật trạng thái!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult ChangeBoard(int ListID, string BoardCode)
        {
            var msg = new JMessage() { Error = true };

            try
            {
                var data = _context.WORKOSLists.FirstOrDefault(x => x.ListID == ListID);
                data.BoardCode = BoardCode;
                _context.WORKOSLists.Update(data);
                _context.SaveChanges();

                msg.Title = "Di chuyển danh sách thành công!";
                msg.Error = false;
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra khi di chuyển danh sách!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult DeleteList(int id)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.WORKOSLists.FirstOrDefault(x => x.ListID == id);
                data.IsDeleted = true;
                _context.WORKOSLists.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Xóa danh sách thành công!";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi khi xóa danh sách!";
                return Json(msg);
            }
        }

        #endregion

        #region Card
        public class GridCard : JTableModel
        {
            public string BoardCode { get; set; }
            public string CardName { get; set; }
            public string Fromdate { get; set; }
            public string ListCode { get; set; }
            public string Member { get; set; }
            public string Todate { get; set; }
        }
        public class DescriptionModel
        {
            public string CardCode { get; set; }
            public string Description { get; set; }
        }
        [HttpPost]
        public JsonResult GetCards(string BoardCode)
        {
            var ListCodes = _context.WORKOSLists.Where(x => x.BoardCode.Equals(BoardCode) && x.IsDeleted == false).Select(x => x.ListCode).Distinct();
            var data = _context.WORKOSCards.Where(x => ListCodes.Contains(x.ListCode) && x.IsDeleted == false).Select(x => new { x.CardCode, x.CardName, x.ListCode, x.CreatedDate });
            return Json(data);
        }
        [HttpPost]
        public JsonResult GetCardsByList(string ListCode)
        {
            var data = _context.WORKOSCards.Where(x => x.ListCode.Equals(ListCode) && x.IsDeleted == false);
            return Json(data);
        }
        [HttpPost]
        public JsonResult GetCardDetail(string CardCode)
        {
            var data = _context.WORKOSCards.FirstOrDefault(x => x.CardCode.Equals(CardCode));
            return Json(data);
        }
        [HttpPost]
        public JsonResult InsertCard([FromBody]dynamic data)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                WORKOSCard card = new WORKOSCard();
                if (data.CardName.Value == "")
                {
                    return Json(new JMessage() { Error = true, Title = "Nhập nội dung" });
                }
                card.CardName = data.CardName.Value;
                card.CardCode = "CARD_" + (_context.WORKOSCards.Count() > 0 ? _context.WORKOSCards.Last().CardID + 1 : 1);
                card.ListCode = data.ListCode.Value;
                card.CreatedBy = ESEIM.AppContext.UserName;
                card.CreatedDate = DateTime.Now;
                card.IsDeleted = false;
                _context.WORKOSCards.Add(card);

                if (data.Setting != null)
                {
                    CardMember cardMember;
                    for (int i = 0; i < data.Setting.Member.Count; i++)
                    {
                        string username = data.Setting.Member[i].UserName.Value;
                        cardMember = new CardMember()
                        {
                            CardCode = card.CardCode,
                            MemberId = username,
                            Flag = false,
                        };
                        _context.CardMember.Add(cardMember);
                    }
                    for (int i = 0; i < data.Setting.Team.Count; i++)
                    {
                        string team = data.Setting.Team[i].TeamCode.Value;
                        cardMember = new CardMember()
                        {
                            CardCode = card.CardCode,
                            TeamCode = team,
                            Flag = false
                        };
                        _context.CardMember.Add(cardMember);
                    }
                    for (int i = 0; i < data.Setting.ListDependency.Count; i++)
                    {
                        string ObjCode = data.Setting.ListDependency[i].ObjCode.Value;
                        string Dependency = data.Setting.ListDependency[i].Dependency.Value;
                        string Relative = data.Setting.ListDependency[i].Relative.Value;
                        CardForWObj cardForObj = new CardForWObj()
                        {
                            CardCode = card.CardCode,
                            CatObjCode = Dependency,
                            ObjCode = ObjCode,
                            Relative = Relative,
                            CreatedBy = ESEIM.AppContext.UserName,
                            CreatedTime = DateTime.Now,
                            IsDeleted = false
                        };
                        _context.CardForWObj.Add(cardForObj);
                    }
                }

                _context.SaveChanges();

                msg.Title = "Thêm thành công";
                msg.Object = card;
                msg.Error = false;
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult DeleteCard(int Id)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.WORKOSCards.FirstOrDefault(x => x.CardID == Id);
                data.IsDeleted = true;

                _context.WORKOSCards.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Xóa thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Title = "Có lỗi xảy ra!";
                msg.Object = ex;
                return Json(msg);
            }

        }
        [HttpPost]
        public JsonResult UpdateCardName(int CardID, string NewName)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.WORKOSCards.FirstOrDefault(x => x.CardID == CardID);
                data.CardName = NewName;
                _context.WORKOSCards.Update(data);
                _context.SaveChanges();

                msg.Title = "Thay đổi thành công!";
                msg.Error = false;
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "CÓ lỗi xảy ra!";
                return Json(msg);
            }
        }


        /// <Member>
        ///  Member
        /// </Member>
        [HttpPost]
        public List<TreeViewResource> GetAllGroupUser()
        {
            var data = _context.AdGroupUsers.Where(x => x.IsEnabled == true).OrderByDescending(x => x.GroupUserId).AsNoTracking();
            var dataOrder = GetSubTreeData(data.ToList(), null, new List<TreeViewResource>(), 0);
            return dataOrder;
        }
        [NonAction]
        private List<TreeViewResource> GetSubTreeData(List<AdGroupUser> data, string parentid, List<TreeViewResource> lstCategories, int tab)
        {
            //tab += "- ";
            var contents = String.IsNullOrEmpty(parentid)
                ? data.Where(x => x.ParentCode == null).OrderByDescending(x => x.GroupUserId).AsParallel()
                : data.Where(x => x.ParentCode == parentid).OrderByDescending(x => x.GroupUserCode).AsParallel();
            foreach (var item in contents)
            {
                var category = new TreeViewResource
                {
                    Id = item.GroupUserId,
                    Code = item.GroupUserCode,
                    Title = item.Title,
                    Level = tab,
                    HasChild = data.Any(x => x.ParentCode == item.GroupUserCode),
                    ParentCode = item.ParentCode,
                };
                lstCategories.Add(category);
                if (category.HasChild) GetSubTreeData(data, item.GroupUserCode, lstCategories, tab + 1);
            }
            return lstCategories;
        }

        //[HttpPost]
        //public object GetListUserInGroup(List<string> listGroup)
        //{
        //    var list = listGroup[0] != null ? listGroup[0].Split(',') : new string[0];
        //    var userPermission = _context.AdGroupUsers.FirstOrDefault(x => list.Any(y => y == x.GroupUserCode) && !string.IsNullOrEmpty(x.Leader));
        //    if (userPermission != null)
        //    {
        //        var listMember = (from a in _context.AdUserInGroups
        //                          join b in _context.Users on a.UserId equals b.Id
        //                          where list.Any(y => y == a.GroupUserCode)
        //                          && b.Active == true
        //                          select new
        //                          {
        //                              a.UserId,
        //                              Name = b.GivenName,
        //                              a.GroupUserCode,
        //                              IsPermision = a.UserId == userPermission.Leader ? true : false,
        //                          });
        //        return listMember;
        //    }
        //    else
        //    {
        //        var listMember = (from a in _context.AdUserInGroups
        //                          join b in _context.Users on a.UserId equals b.Id
        //                          where list.Any(y => y == a.GroupUserCode)
        //                          && b.Active == true
        //                          select new
        //                          {
        //                              a.UserId,
        //                              Name = b.GivenName,
        //                              a.GroupUserCode,
        //                              IsPermision = false,
        //                          });
        //        return listMember;
        //    }
        //}

        [HttpPost]
        public JsonResult AddMember([FromBody]dynamic data)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                string cardCode = data.cardcode.Value;
                var currentData = _context.CardMember.Where(x => x.CardCode.Equals(cardCode)).ToList();
                for (int i = 0; i < currentData.Count; i++)
                {
                    currentData[i].Flag = true;
                    _context.CardMember.Update(currentData[i]);
                }
                CardMember cardMember;
                for (int i = 0; i < data.listmember.Count; i++)
                {
                    string username = data.listmember[i].UserName.Value;
                    cardMember = new CardMember()
                    {
                        CardCode = cardCode,
                        MemberId = username,
                        Flag = false,
                    };
                    _context.CardMember.Add(cardMember);
                }

                for (int i = 0; i < data.listteam.Count; i++)
                {
                    string team = data.listteam[i].TeamCode.Value;
                    cardMember = new CardMember()
                    {
                        CardCode = cardCode,
                        TeamCode = team,
                        Flag = false
                    };
                    _context.CardMember.Add(cardMember);
                }

                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Thêm thành viên thành công!";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi khi thêm thành viên!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult GetCardMember(string CardCode)
        {
            var data = _context.CardMember.Where(x => x.CardCode.Equals(CardCode) && x.MemberId != null && x.Flag == false).ToList();
            return Json(data);
        }
        /// <summary>
        /// End Member
        /// </summary>
        /// <param name="CardCode"></param>
        /// <returns></returns>



        [HttpPost]
        public JsonResult GetCardTeam(string CardCode)
        {
            var data = _context.CardMember.Where(x => x.CardCode.Equals(CardCode) && x.TeamCode != null && x.Flag == false).ToList();
            List<WORKOSTeam> listTeam = new List<WORKOSTeam>();
            for (int i = 0; i < data.Count; i++)
            {
                var team = _context.WORKOSTeams.FirstOrDefault(x => x.TeamCode.Equals(data[i].TeamCode));
                listTeam.Add(team);
            }
            return Json(listTeam);
        }
        [HttpPost]
        public JsonResult UpdateCardDescription([FromBody]DescriptionModel obj)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                if (string.IsNullOrEmpty(obj.CardCode))
                {
                    return null;
                }
                var data = _context.WORKOSCards.FirstOrDefault(x => x.CardCode.Equals(obj.CardCode));
                if (data.Description != null && data.Description.Equals(obj.Description))
                {
                    return null;
                }

                data.Description = obj.Description;

                _context.WORKOSCards.Update(data);
                _context.SaveChanges();

                msg.Title = "Cập nhật thành công";
                msg.Error = false;
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult UpdateCardLabel(string CardCode, string Label)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.WORKOSCards.FirstOrDefault(x => x.CardCode.Equals(CardCode));
                data.Labels = Label;

                _context.WORKOSCards.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Cập nhật thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult ChangeListCard(string CardCode, string ListCode)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.WORKOSCards.FirstOrDefault(x => x.CardCode.Equals(CardCode));
                data.ListCode = ListCode;

                _context.WORKOSCards.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Di chuyển thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult UpdateDuedate(string CardCode, DateTime Duedate)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.WORKOSCards.FirstOrDefault(x => x.CardCode.Equals(CardCode));
                data.DueDate = Duedate;

                _context.WORKOSCards.Update(data);
                _context.SaveChanges();

                msg.Title = "Cập nhật thành công";
                msg.Error = false;
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult ChangeWorkType(string CardCode, string Type)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.WORKOSCards.FirstOrDefault(x => x.CardCode.Equals(CardCode));
                data.WorkType = Type;

                _context.WORKOSCards.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Cập nhật thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Title = "Có lỗi xảy ra!";
                msg.Object = ex;
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult GetGridCard([FromBody]GridCard jtablePara)
        {
            int intBegin = (jtablePara.CurrentPage - 1) * jtablePara.Length;
            DateTime? fromDate = string.IsNullOrEmpty(jtablePara.Fromdate) ? (DateTime?)null : DateTime.ParseExact(jtablePara.Fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime? toDate = string.IsNullOrEmpty(jtablePara.Todate) ? (DateTime?)null : DateTime.ParseExact(jtablePara.Todate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var ListCodes = _context.WORKOSLists.Where(x => x.BoardCode.Equals(jtablePara.BoardCode) && x.IsDeleted == false).Select(x => x.ListCode).Distinct();
            var query = _context.WORKOSCards.Where(x =>
                            ListCodes.Contains(x.ListCode) &&
                            x.IsDeleted == false &&
                            (string.IsNullOrEmpty(jtablePara.CardName) || x.CardName.ToUpper().Contains(jtablePara.CardName)) &&
                            (string.IsNullOrEmpty(jtablePara.ListCode) || x.ListCode.Equals(jtablePara.ListCode)) &&
                            (fromDate == null || x.CreatedDate >= fromDate) &&
                            (toDate == null || x.CreatedDate <= toDate) &&
                            (string.IsNullOrEmpty(jtablePara.Member) ||
                                (_context.CardMember.Where(y =>
                                                                y.CardCode.Equals(x.CardCode) &&
                                                                y.MemberId.Equals(jtablePara.Member)).Count() > 0))

                        )
                        .Select(x => new
                        {
                            x.CardID,
                            x.CardCode,
                            x.CardName,
                            ListName = _context.WORKOSLists.FirstOrDefault(y => y.ListCode.Equals(x.ListCode)).ListName,
                            x.CreatedDate,
                            x.CreatedBy,
                            x.DueDate,
                            LevelCode = x.CardLevel,
                            StatusCode = x.Status,
                            Status = _context.CommonSettings.FirstOrDefault(y => y.AssetCode.Equals("CARDJOB") && y.Group.Equals("STATUS") && y.CodeSet.Equals(x.Status)).ValueSet,
                            CardLevel = _context.CommonSettings.FirstOrDefault(y => y.AssetCode.Equals("CARDJOB") && y.Group.Equals("LEVEL") && y.CodeSet.Equals(x.CardLevel)).ValueSet
                        });

            int count = query.Count();
            var data = query.OrderUsingSortExpression(jtablePara.QueryOrderBy).Skip(intBegin).Take(jtablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(
                                data,
                                jtablePara.Draw,
                                count,
                                "CardID", "CardCode", "CardName", "ListName", "CreatedDate",
                                "CreatedBy", "DueDate", "Status", "CardLevel", "LevelCode",
                                "StatusCode"
                                );
            return Json(jdata);
        }
        [HttpPost]
        public JsonResult GetWorkType()
        {
            var data = _context.CommonSettings.Where(x => x.AssetCode.Equals("CARDJOB") && x.Group.Equals("OBJ_WORKTYPE") && x.IsDeleted == false)
                        .Select(x => new { Code = x.CodeSet, Value = x.ValueSet }).ToList();
            return Json(data);
        }
        [HttpPost]
        public JsonResult GetStatus()
        {
            var data = _context.CommonSettings.Where(x => x.AssetCode.Equals("CARDJOB") && x.Group.Equals("STATUS") && x.IsDeleted == false)
                        .Select(x => new { Code = x.CodeSet, Value = x.ValueSet }).ToList();
            return Json(data);
        }
        [HttpPost]
        public JsonResult ChangeCardStatus(string CardCode, string Status)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.WORKOSCards.FirstOrDefault(x => x.CardCode.Equals(CardCode));
                data.Status = Status;

                _context.WORKOSCards.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Cập nhật trạng thái thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Title = "Có lỗi xảy ra!";
                msg.Object = ex;
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult GetLevels()
        {
            var data = _context.CommonSettings.Where(x => x.AssetCode.Equals("CARDJOB") && x.Group.Equals("LEVEL") && x.IsDeleted == false)
                        .Select(x => new { Code = x.CodeSet, Value = x.ValueSet }).ToList();
            return Json(data);
        }
        [HttpPost]
        public JsonResult ChangeCardLevel(string CardCode, string Level)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.WORKOSCards.FirstOrDefault(x => x.CardCode.Equals(CardCode));
                data.CardLevel = Level;

                _context.WORKOSCards.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Cập nhật cấp độ thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Title = "Có lỗi xảy ra!";
                msg.Object = ex;
                return Json(msg);
            }
        }
        #endregion

        #region CheckList
        [HttpPost]
        public JsonResult AddCheckList(string CardCode, string Title)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                string code = "CHECK_LIST_" + (_context.CardCheckList.Count() > 0 ? _context.CardCheckList.Last().Id + 1 : 0);
                var data = new CardCheckList()
                {
                    CardCode = CardCode,
                    CheckTitle = Title,
                    ChkListCode = code
                };

                _context.CardCheckList.Add(data);
                _context.SaveChanges();

                msg.Title = "Thêm thành công!";
                msg.Error = false;
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra khi thêm!";
                return Json(msg);
            }
        }

        [HttpPost]
        public JsonResult GetCheckLists(string CardCode)
        {
            var data = _context.CardCheckList.Where(x => x.CardCode.Equals(CardCode) && x.Flag == false).ToList();
            return Json(data);
        }
        [HttpPost]
        public JsonResult AddCheckItem(string CheckCode, string Title)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                CardChkItem data = new CardChkItem()
                {
                    ChkListCode = CheckCode,
                    Title = Title,
                    MemberId = ESEIM.AppContext.UserName,
                    Status = 0
                };

                _context.CardChkItem.Add(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Thêm thành công!";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra khi thêm!";
                return Json(msg);
                throw;
            }
        }
        [HttpPost]
        public JsonResult GetCheckItem(string CheckCode)
        {
            var data = _context.CardChkItem.Where(x => x.ChkListCode.Equals(CheckCode) && x.Flag == false).ToList();
            return Json(data);
        }
        [HttpPost]
        public JsonResult DeleteCheckItem(int Id)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.CardChkItem.FirstOrDefault(x => x.Id == Id);
                data.Flag = true;
                _context.CardChkItem.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Xóa thành công";

                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult ChangeCheckTitle(string CheckCode, string Title)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.CardCheckList.FirstOrDefault(x => x.ChkListCode.Equals(CheckCode));
                if (data.CheckTitle.Equals(Title))
                {
                    return null;
                }

                data.CheckTitle = Title;

                _context.CardCheckList.Update(data);
                _context.SaveChanges();

                msg.Title = "Cập nhật thành công!";
                msg.Error = false;
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult ChangeItemStatus(int Id)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.CardChkItem.FirstOrDefault(x => x.Id == Id);
                if (data.Status != 1)
                {
                    data.Status = 1;
                    data.MemberChecked = ESEIM.AppContext.UserName;
                    data.TimeChecked = DateTime.Now;
                }
                else
                {
                    data.Status = 0;
                    data.MemberChecked = null;
                    data.TimeChecked = null;
                }

                data.UpdatedTime = DateTime.Now;
                _context.CardChkItem.Update(data);
                _context.SaveChanges();

                msg.Title = "Cập nhật thành công";
                msg.Error = false;
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult ChangeItemTitle(int Id, string Title)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.CardChkItem.FirstOrDefault(x => x.Id == Id);
                if (data.Title.Equals(Title))
                {
                    return null;
                }
                data.Title = Title;
                data.UpdatedTime = DateTime.Now;
                _context.CardChkItem.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Cập nhật thành công!";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult DeleteCheckList(string CheckCode)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var data = _context.CardCheckList.FirstOrDefault(x => x.ChkListCode.Equals(CheckCode));
                data.Flag = true;

                _context.CardCheckList.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Xóa thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi khi xóa!";
                return Json(msg);
            }
        }
        #endregion

        #region Comment
        [HttpPost]
        public JsonResult AddComment(string CardCode, string Content)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                string code = "COMMENT_" + CardCode + "_" + (_context.CardCommentList.Count() > 0 ? _context.CardCommentList.Last().Id + 1 : 0);
                CardCommentList data = new CardCommentList()
                {
                    CardCode = CardCode,
                    CmtId = code,
                    CmtContent = Content,
                    MemberId = ESEIM.AppContext.UserName,
                    CreatedTime = DateTime.Now
                };

                _context.CardCommentList.Add(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Thêm bình luận thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult GetComments(string CardCode)
        {
            var data = _context.CardCommentList.Where(x => x.CardCode.Equals(CardCode) && x.Flag == false).ToList();
            return Json(data);
        }
        [HttpPost]
        public JsonResult DeleteComment(string CommentId)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.CardCommentList.FirstOrDefault(x => x.CmtId.Equals(CommentId));
                data.Flag = true;

                _context.CardCommentList.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Xóa thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi khi xóa!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult UpdateComment(string CmtId, string Content)
        {
            var msg = new JMessage() { Error = true };
            if (string.IsNullOrEmpty(Content))
            {
                msg.Title = "Nhập nội dung!";
                return Json(msg);
            }
            else
            {
                if (Content.Length > 255)
                {
                    msg.Title = "255 ký tự!";
                    return Json(msg);
                }
            }
            try
            {
                var data = _context.CardCommentList.First(x => x.CmtId.Equals(CmtId));
                if (data.CmtContent.Equals(Content))
                {
                    return null;
                }

                data.CmtContent = Content;
                data.UpdatedBy = ESEIM.AppContext.UserName;
                data.UpdatedTime = DateTime.Now;

                _context.CardCommentList.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Cập nhật thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi khi cập nhật!";
                return Json(msg);
            }
        }
        #endregion

        #region Attachment
        [HttpPost]
        public JsonResult AddAttachment([FromBody]CardAttachment data)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                string code = "ATTACHMENT_" + data.CardCode + "_" + (_context.CardAttachment.Count() > 0 ? _context.CardAttachment.Last().Id + 1 : 0);
                data.FileCode = code;
                data.CreatedTime = DateTime.Now;
                data.MemberId = ESEIM.AppContext.UserName;
                _context.CardAttachment.Add(data);
                _context.SaveChanges();

                msg.Title = "Thêm thành công";
                msg.Error = false;
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult GetAttachment(string CardCode)
        {
            var data = _context.CardAttachment.Where(x => x.CardCode.Equals(CardCode) && x.Flag == false).ToList();
            return Json(data);
        }
        [HttpPost]
        public object UploadFile(IFormFile fileUpload)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var upload = _upload.UploadFile(fileUpload, Path.Combine(_hostingEnvironment.WebRootPath, "uploads\\files"));
                if (upload.Error)
                {
                    msg.Error = true;
                    msg.Title = upload.Title;
                }
                else
                {
                    msg.Object = upload.Object;
                }

            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi upload file!";
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult DeleteAttachment(string FileCode)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.CardAttachment.FirstOrDefault(x => x.FileCode.Equals(FileCode));
                data.Flag = true;
                data.UpdatedTime = DateTime.Now;
                _context.CardAttachment.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Xóa thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }
        #endregion

        //#region ExecutiveRelative
        //[HttpPost]
        //public JsonResult GetContractProject(int type)
        //{
        //    if (type == 0)
        //    {
        //        var data = _context.ContractHeaders.Select(x => new { Id = "CONTRACT_" + x.ContractHeaderID, Title = x.Title }).ToList();
        //        return Json(data);
        //    }
        //    if (type == 1)
        //    {
        //        var data = _context.Projects.Select(x => new { Id = "PROJECT_" + x.Id, Title = x.ProjectTitle }).ToList();
        //        return Json(data);
        //    }
        //    return null;
        //}
        //public class ExecutiveRelative
        //{
        //    public string Id { get; set; }
        //    public string Title { get; set; }
        //}
        //[HttpPost]
        //public JsonResult GetExecutiveRelative(string ListCode)
        //{
        //    List<ExecutiveRelative> obj = new List<ExecutiveRelative>();
        //    string data = _context.WorkExecutiveObjectrelative.FirstOrDefault(x => x.ListCode.Equals(ListCode)).ObjectModule;
        //    foreach (string item in data.Split(';'))
        //    {
        //        string title = "";
        //        if (item.Split('_')[0].Equals("CONTRACT"))
        //        {
        //            title = _context.ContractHeaders.FirstOrDefault(x => x.ContractHeaderID == int.Parse(item.Split('_')[1])).Title;
        //        }
        //        else
        //        {
        //            title = _context.Projects.FirstOrDefault(x => x.Id == int.Parse(item.Split('_')[1])).ProjectTitle;
        //        }
        //        ExecutiveRelative a = new ExecutiveRelative
        //        {
        //            Id = item,
        //            Title = title
        //        };
        //        obj.Add(a);
        //    }
        //    return Json(obj);
        //}
        //[HttpPost]
        //public JsonResult InsertExecuteRelative([FromBody]WorkExecutiveObjectrelative data)
        //{
        //    var msg = new JMessage() { Error = true };
        //    try
        //    {

        //        if (_context.WorkExecutiveObjectrelative.Select(x => x.ListCode.Equals(data.ListCode)).Count() > 0)
        //        {
        //            var obj = _context.WorkExecutiveObjectrelative.FirstOrDefault(x => x.ListCode.Equals(data.ListCode));
        //            obj.UpdatedBy = ESEIM.AppContext.UserName;
        //            obj.UpdatedTime = DateTime.Now;

        //            foreach (string item in data.ObjectModule.Replace(obj.ObjectModule, "").Split(';'))
        //            {
        //                if (item.Split('_')[0].Equals("CONTRACT"))
        //                {
        //                    AddContractNotication(int.Parse(item.Split('_')[1]), data.ListCode);
        //                }
        //            }

        //            obj.ObjectModule = data.ObjectModule;
        //            obj.Note = data.Note;
        //            _context.WorkExecutiveObjectrelative.Update(obj);
        //            _context.SaveChanges();



        //            msg.Title = "Cập nhật thành công";
        //            msg.Error = false;

        //            return Json(msg);
        //        }


        //        data.ObjectCode = "EXEREL_" + data.ListCode + "_" + (_context.WorkExecutiveObjectrelative.Count() > 0 ? _context.WorkExecutiveObjectrelative.Last().Id + 1 : 1);
        //        data.CreatedBy = ESEIM.AppContext.UserName;
        //        data.CreatedTime = DateTime.Now;
        //        data.Flag = false;

        //        _context.WorkExecutiveObjectrelative.Add(data);
        //        _context.SaveChanges();

        //        foreach (string item in data.ObjectModule.Split(';'))
        //        {
        //            if (item.Split('_')[0].Equals("CONTRACT"))
        //            {
        //                AddContractNotication(int.Parse(item.Split('_')[1]), data.ListCode);
        //            }
        //        }
        //        msg.Title = "Thêm thành công";
        //        msg.Error = false;

        //        return Json(msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Title = "CÓ lỗi xảy ra!";
        //        msg.Object = ex;
        //        return Json(msg);
        //    }
        //}

        //private void AddContractNotication(int ContractId, string listCode)
        //{
        //    try
        //    {
        //        var contract = _context.ContractHeaders.FirstOrDefault(x => x.ContractHeaderID == ContractId);
        //        var listTitle = _context.WORKOSList.FirstOrDefault(x => x.ListCode.Equals(listCode)).ListName;
        //        var contractActivity = new EDMSContractActivity()
        //        {
        //            ActivityCode = "ACT_" + contract.ContractCode + "_" + (_context.EDMSContractActivitys.Count() > 0 ? _context.EDMSContractActivitys.Last().ContractActivityId + 1 : 1),
        //            Title = "Hợp đồng được thêm vào list " + listTitle,
        //            ContractCode = contract.ContractCode
        //        };

        //        _context.EDMSContractActivitys.Add(contractActivity);
        //        _context.SaveChanges();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //#endregion

        #region Advance Search
        public class AdvanceSearchObj
        {
            public string CardTitle { get; set; }
            public string Member { get; set; }
            public string Fromdate { get; set; }
            public string Todate { get; set; }
            public string Status { get; set; }
            public string ObjDependency { get; set; }
            public string ObjCode { get; set; }
        }

        [HttpPost]
        public JsonResult GetObjDependency()
        {
            var data = _context.CommonSettings.Where(x => x.AssetCode.Equals("CARDJOB") && x.Group.Equals("OBJ_DEPENDENCY") && x.IsDeleted == false)
                            .Select(x => new { Code = x.CodeSet, Value = x.ValueSet }).ToList();
            return Json(data);
        }

        [HttpPost]
        public JsonResult AdvanceSearch([FromBody]AdvanceSearchObj data)
        {
            DateTime? fromDate = string.IsNullOrEmpty(data.Fromdate) ? (DateTime?)null : DateTime.ParseExact(data.Fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime? toDate = string.IsNullOrEmpty(data.Todate) ? (DateTime?)null : DateTime.ParseExact(data.Todate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var query = from a in _context.WORKOSCards

                        where a.IsDeleted == false &&
                              (fromDate == null || a.CreatedDate >= fromDate) &&
                              (toDate == null || a.CreatedDate <= toDate) &&
                              (string.IsNullOrEmpty(data.CardTitle) || a.CardName.ToUpper().Contains(data.CardTitle.ToUpper())) &&
                              (string.IsNullOrEmpty(data.Status) || a.DueDate < DateTime.Now) &&
                              (string.IsNullOrEmpty(data.Member) ||
                                (from b in _context.CardMember
                                 where b.CardCode.Equals(a.CardCode) && b.MemberId.Equals(data.Member)
                                 select b).Count() > 0) &&
                              (string.IsNullOrEmpty(data.ObjCode) || (
                                from c in _context.CardForWObj
                                where c.CardCode.Equals(a.CardCode) && c.CatObjCode.Equals(data.ObjDependency) && c.ObjCode.Equals(data.ObjCode)
                                select c
                              ).Count() > 0)
                        select a.CardCode;


            return Json(query.ToList());
        }
        #endregion

        #region Obj Dependency
        public JsonResult JtableObj([FromBody]JTableModel jtablePara)
        {
            int intBegin = (jtablePara.CurrentPage - 1) * jtablePara.Length;

            var query = from a in _context.CardForWObj
                        where (a.IsDeleted == false)
                        select new
                        {
                            Dependency = a.CatObjCode,
                            ObjCode = a.ObjCode,
                            Relative = a.Relative,
                            Id = a.Id
                        };
            int count = query.Count();
            var data = query.OrderUsingSortExpression(jtablePara.QueryOrderBy).Skip(intBegin).Take(jtablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jtablePara.Draw, count, "Id", "Dependency", "ObjCode", "Relative");
            return Json(jdata);
        }
        [HttpPost]
        public JsonResult GetObjCode(string Dependency)
        {
            switch (Dependency)
            {
                case "PROJECT":
                    var project = _context.Projects.Where(x => x.FlagDeleted == false).Select(x => new { Code = x.ProjectCode, Value = x.ProjectTitle }).ToList();
                    return Json(project);
                case "CONTRACT":
                    var contract = _context.ContractHeaders.Where(x => x.IsDeleted == false).Select(x => new { Code = x.ContractCode, Value = x.Title }).ToList();
                    return Json(contract);
                case "CUSTOMER":
                    var customer = _context.Customers.Where(x => x.IsDeleted == false).Select(x => new { Code = x.CusCode, Value = x.CusName }).ToList();
                    return Json(customer);
                case "SUPPLIER":
                    var supplier = _context.Suppliers.Where(x => x.IsDeleted == false).Select(x => new { Code = x.SupCode, Value = x.SupName }).ToList();
                    return Json(supplier); ;
                case "COMMON":
                    List<Object> list = new List<Object>();
                    dynamic obj = new JObject();
                    obj.Code = "ADMIN";
                    obj.Value = "Admin";
                    list.Add(obj);
                    obj = new JObject();
                    obj.Code = "HR";
                    obj.Value = "Tuyển dụng";
                    list.Add(obj);
                    obj = new JObject();
                    obj.Code = "ACOUNTING";
                    obj.Value = "Kế toán";
                    list.Add(obj);

                    return Json(list);
            }
            return Json("");
        }
        [HttpPost]
        public JsonResult SetObjectRelative([FromBody]dynamic data)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                string cardCode = data.CardCode.Value;
                var currentData = _context.CardForWObj.Where(x => x.CardCode.Equals(cardCode)).ToList();
                for (int i = 0; i < currentData.Count; i++)
                {
                    currentData[i].IsDeleted = true;
                    _context.CardForWObj.Update(currentData[i]);
                }
                for (int i = 0; i < data.listDependency.Count; i++)
                {
                    string ObjCode = data.listDependency[i].ObjCode.Value;
                    string Dependency = data.listDependency[i].Dependency.Value;
                    string Relative = data.listDependency[i].Relative.Value;
                    CardForWObj cardForObj = new CardForWObj()
                    {
                        CardCode = cardCode,
                        CatObjCode = Dependency,
                        ObjCode = ObjCode,
                        Relative = Relative,
                        CreatedBy = ESEIM.AppContext.UserName,
                        CreatedTime = DateTime.Now,
                        IsDeleted = false
                    };
                    _context.CardForWObj.Add(cardForObj);
                }

                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Cập nhật thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }
        [HttpPost]
        public JsonResult GetObjectRelative(string CardCode)
        {
            var data = _context.CardForWObj.Where(x => x.CardCode.Equals(CardCode) && x.IsDeleted == false).ToList();
            return Json(data);
        }
        [HttpPost]
        public JsonResult DeleteCardDependency(int Id)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.CardForWObj.FirstOrDefault(x => x.Id == Id);
                data.IsDeleted = true;
                _context.CardForWObj.Update(data);
                _context.SaveChanges();

                msg.Title = "Xóa thành công";
                msg.Error = false;
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Title = "Có lỗi xảy ra!";
                msg.Object = ex;
                return Json(msg);
            }

        }
        [HttpPost]
        public JsonResult GetRelative()
        {
            var data = _context.CommonSettings.Where(x => x.AssetCode.Equals("CARDJOB") && x.Group.Equals("OBJ_RELATIVE") && x.IsDeleted == false)
                            .Select(x => new { Code = x.CodeSet, Value = x.ValueSet }).ToList();
            return Json(data);
        }
        #endregion
    }
}
