using System.IO;
using System.Web;

namespace Elevel.Api.Handlers
{
    public class FileHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)  
        {  
            if (context.Request.Files.Count > 0)  
            {  
                HttpFileCollection SelectedFiles = context.Request.Files;  
                for (int i = 0; i < SelectedFiles.Count; i++)  
                {  
                    HttpPostedFile PostedFile = SelectedFiles[i];  
                    string FileName = context.Server.MapPath("~/wwwroot/" + PostedFile.FileName);  
                    PostedFile.SaveAs(FileName);                      
                }  
            }  
            else  
            {  
                context.Response.ContentType = "text/plain";  
                context.Response.Write("Please Select Files");  
            }  
            context.Response.ContentType = "text/plain";  
            context.Response.Write("Files Uploaded Successfully!");  
        }  
        public bool IsReusable  
        {  
            get  
            {  
                return false;  
            }  
        }  
    }  
} 
