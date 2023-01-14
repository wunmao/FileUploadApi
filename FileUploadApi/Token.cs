namespace FileUploadApi;

public class Token
{
    public long    expiresIn    { get; set; }
    public string? accessToken  { get; set; }
    public string? refreshToken { get; set; }
}