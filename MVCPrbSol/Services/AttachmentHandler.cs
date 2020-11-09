using MVCPrbSol.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Services
{
    public class AttachmentHandler
    {
        public TicketAttachment Attach(IFormFile attachment)
        {
            TicketAttachment ticketAttachment = new TicketAttachment();

            var ms = new MemoryStream();
            attachment.CopyTo(ms);
            byte[] bytes = ms.ToArray();
            ms.Close();
            ms.Dispose();

            var binary = Convert.ToBase64String(bytes);
            var ext = Path.GetExtension(attachment.FileName);

            ticketAttachment.FilePath = $"data:image/{ext};base64,{binary}";
            ticketAttachment.FileData = bytes;
            ticketAttachment.Created = DateTime.Now;

            return ticketAttachment;
        }
    }
}