using System.Net;

namespace TY.Hiring.Fleet.Management.Model.Models
{
    // scheme for successful responses
    public class DataResult<T>
    {
        public DataResult()
        {
            this.ResultCode = HttpStatusCode.OK;
            Messages = new List<Message>();
        }

        public T? Result { get; set; }
        public HttpStatusCode ResultCode { get; set; }
        public List<Message> Messages { get; }

        public bool IsSucceeded
        {
            get
            {
                return !Messages.Any();
            }
        }

        public void AddMessage(string code, string messageText = "An unexpected error has occurred")
        {
            Messages.Add(new Message { Code = code, Text = messageText });
        }
    }

    // scheme for faulty responses. used by exceptionhanglermiddleware.
    public class DataResult
    {
        public DataResult()
        {
            this.ResultCode = HttpStatusCode.InternalServerError;
            Messages = new List<Message>();
        }

        public HttpStatusCode ResultCode { get; set; }
        public List<Message> Messages { get; }

        public bool IsSucceeded
        {
            get
            {
                return !Messages.Any();
            }
        }

        public void AddMessage(string code, string messageText = "An unexpected error has occurred")
        {
            Messages.Add(new Message { Code = code, Text = messageText });
        }

    }

}
