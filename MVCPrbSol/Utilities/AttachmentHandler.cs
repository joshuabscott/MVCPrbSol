using MVCPrbSol.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Utilities
{
    public class AttachmentHandler
    {
        public TicketAttachment Attach(IFormFile attachment)
        {
            TicketAttachment ticketAttachment = new TicketAttachment();

            // Create a variable ms that is a new instance of Memory Stream, a built-in back-end "ram". We move the attachment data into a byte array 
            // and close the memory stream
            var ms = new MemoryStream();
            attachment.CopyTo(ms);
            byte[] bytes = ms.ToArray();
            ms.Close();
            ms.Dispose();

            // This is creating a variable binary, which is going to be converting our previously stored image into base64string (long list of characters)
            var binary = Convert.ToBase64String(bytes);
            var ext = Path.GetExtension(attachment.FileName);

            ticketAttachment.FilePath = $"data:image/{ext};base64,{binary}";
            ticketAttachment.FileData = bytes;
            ticketAttachment.Created = DateTime.Now;

            return ticketAttachment;
        }
    }
}