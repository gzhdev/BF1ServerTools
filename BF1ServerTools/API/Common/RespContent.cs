namespace BF1ServerTools.API;

public class RespContent
{
    public bool IsSuccess { get; set; }
    public HttpStatusCode HttpCode { get; set; }

    public string Content { get; set; }
    public double ExecTime { get; set; }
}
