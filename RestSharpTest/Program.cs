using System.Text.Json;
using RestSharp;
using RestSharpTest;

#region GetToken
using var    clientToken = new RestClient("http://localhost:7000/getAccessToken");
const string userName    = "wunmao";
const string password    = "1234";

var requestToken = new RestRequest { Method = Method.Post };
requestToken.AddHeader("Cache-Control", "no-cache");
requestToken.AddHeader("Content-Type",  "application/x-www-form-urlencoded");
requestToken.AddParameter("clientType", "20");
requestToken.AddParameter("serverId",   "0");
requestToken.AddParameter("userName",   userName);
requestToken.AddParameter("password",   password);

var responseToken = clientToken.Execute(requestToken);
Console.WriteLine(responseToken.StatusCode);

Token? token = null;
if (!string.IsNullOrEmpty(responseToken.Content))
{
    try
    {
        //! 此處用System.Text.Json反序列化，也可以改用Json.Net
        token = JsonSerializer.Deserialize<Token>(responseToken.Content);
        Console.WriteLine(token?.accessToken);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
#endregion

if (token != null)
{
    #region 上傳檔案
    using var clientFile  = new RestClient("http://localhost:7000/UploadServlet");
    var       requestFile = new RestRequest { Method = Method.Post };
    requestFile.AddHeader("Authorization", $"Bearer {token.accessToken}");
    requestFile.AddHeader("Content-Type",  "multipart/form-data");
    requestFile.AddParameter("folderId", "TestUpload", ParameterType.RequestBody); //! TestUpload是資料夾名稱，隨便打
    requestFile.AddFile("ooxx", @"C:\Users\wunmao\Desktop\test.txt");              //! ooxx是name可以隨便替換
    var responseFile = clientFile.Execute(requestFile);
    Console.WriteLine(responseFile.StatusCode);
    #endregion
}

Console.ReadKey();