 using ESEIM.Models;
using III.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESEIM.Utils
{
    public interface IParameterService
    {
        int GetPagingLength();
        int GetCountNotification();
        int GetIncommingDocumentPending(string userId);
        int GetOutDocumentPending(string userId);
        double GetSessionTimeout();
        int GetCountCandidateInterview();
        bool CheckAuthority(string userId);
    }

    public class ParameterService : IParameterService
    {
        private EIMDBContext _context;

        public ParameterService(EIMDBContext context)
        {
            _context = context;
        }

        public int GetPagingLength()
        {
            //var para = _context.AdParameters.SingleOrDefault(x => x.ParameterCode == "ADMIN_NUM_PER_PAGE");
            int paging;
            /*if (para == null || !int.TryParse(para.Value, out paging) || paging <= 0)*/
            paging = 10;
            return paging;
        }
        public int GetCountNotification()
        {
            int notification = _context.Notifications.Where(x => !x.IsDeleted).Count();
            return notification;
        }

        public int GetCountCandidateInterview()
        {
            var today = DateTime.Today;
            var count = (from a in _context.CandidateInterviews
                         where a.InterviewDate.Date >= today.Date
                         select new
                         {
                             a
                         }).AsNoTracking().Count();
            return count;
        }
        public int GetIncommingDocumentPending(string userId)
        {
            var inBoxType = EnumHelper<DocumentTypeEnum>.GetDisplayValue(DocumentTypeEnum.InBoxType);
            var count = (from a in _context.DispatchesHeaders
                         from b in _context.DispatchTrackingProcesss.Where(o => o.DispatchCode == a.DispatchCode).Take(1)
                         join c in _context.DispatchesMemberActivitys on b.ProcessCode equals c.ProcessCode
                         where a.Type == inBoxType && c.UserId == userId && c.AssigneeConfirm != EnumHelper<DocumentStatusEnum>.GetDisplayValue(DocumentStatusEnum.Done)
                         && c.AssigneeConfirm != EnumHelper<DocumentStatusEnum>.GetDisplayValue(DocumentStatusEnum.Send)
                         && c.AssigneeConfirm != EnumHelper<DocumentStatusEnum>.GetDisplayValue(DocumentStatusEnum.SendCoordinated)
                         && c.AssigneeConfirm != EnumHelper<DocumentStatusEnum>.GetDisplayValue(DocumentStatusEnum.Coordinated)
                         && (c.Role != DocumentRoleEnum.ReView.GetHashCode() || (c.AssigneeConfirm != EnumHelper<DocumentStatusEnum>.GetDisplayValue(DocumentStatusEnum.Review) && c.Role == DocumentRoleEnum.ReView.GetHashCode()))
                         select a).AsNoTracking().DistinctBy(x => x.Id).Count();
            return count;
        }
        public int GetOutDocumentPending(string userId)
        {
            var outBoxType = EnumHelper<DocumentTypeEnum>.GetDisplayValue(DocumentTypeEnum.OBT);
            var count = (from a in _context.DispatchesHeaders
                         from b in _context.DispatchTrackingProcesss.Where(o => o.DispatchCode == a.DispatchCode).Take(1)
                         join c in _context.DispatchesMemberActivitys on b.ProcessCode equals c.ProcessCode
                         where a.Type == outBoxType && c.UserId == userId && c.AssigneeConfirm != (EnumHelper<DocumentStatusEnum>.GetDisplayValue(DocumentStatusEnum.Review))
                         select new
                         {
                             a.Id,
                         }).AsNoTracking().DistinctBy(x => x.Id).Count();
            return count;
        }
        public bool CheckAuthority(string userId)
        {
            bool isValid = false;
            var checkAuthority = _context.AdAuthorings.FirstOrDefault(x => x.ToUser == userId && x.Confirm == "N");
            if (checkAuthority != null)
            {
                isValid = true;
            }
            return isValid;
        }

        public double GetSessionTimeout()
        {
            //var para = _context.VIBParameter.SingleOrDefault(x => x.ParameterCode == "SYSTEM_SESSION_TIMEOUT");
            double timeout;
            /* if (para == null || !double.TryParse(para.Value, out timeout) || timeout <= 0)*/
            timeout = 60; // Minutes
            return timeout;
        }
    }
}
