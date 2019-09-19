﻿using System.Threading.Tasks;

namespace Host.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task QueueSendEmailAsync(string email, string subject, string message);
    }
}
