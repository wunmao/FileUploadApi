using System.Text.Json;
using RestSharp;
using RestSharpTest;

#region GetToken
using var    clientToken = new RestClient("http://localhost:7000/getAccessToken");
const string userName    = "wunmao";
const string password    = "1234";

var requestToken = new RestRequest { Method = Method.Post };
requestToken.AddHeader("cache-control", "no-cache");
requestToken.AddHeader("content-type",  "application/x-www-form-urlencoded");
requestToken.AddParameter("clientType", "20");
requestToken.AddParameter("serverId",   "0");
requestToken.AddParameter("userName",   userName);
requestToken.AddParameter("password",   password);

var response1 = clientToken.Execute(requestToken);
var token     = JsonSerializer.Deserialize<Token>(response1.Content!)!;

Console.WriteLine(response1.StatusCode);
Console.WriteLine(token.accessToken);
#endregion

#region 上傳檔案
using var clientFile  = new RestClient("http://localhost:7000/UploadServlet");
var       requestFile = new RestRequest { Method = Method.Post };
requestFile.AddHeader("Authorization", $"Bearer {token.accessToken}");
requestFile.AddHeader("Content-Type",  "multipart/form-data");
requestFile.AddParameter("folderId", "TestUpload", ParameterType.RequestBody);
requestFile.AddFile("file", @"C:\Users\wunma\Desktop\nugetpush.txt");
var response2 = clientFile.Execute(requestFile);

Console.WriteLine(response2.StatusCode);
#endregion

Console.ReadKey();