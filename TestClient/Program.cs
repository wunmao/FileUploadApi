using System.Net.Http.Headers;
using System.Net.Http.Json;
using TestClient;

#region GetToken
using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:7000");

const string userName = "wunmao";
const string password = "1234";
var requestToken = new HttpRequestMessage
                   {
                       Method     = HttpMethod.Post,
                       RequestUri = new Uri("getAccessToken", UriKind.Relative),
                       Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                                                           {
                                                               new("clientType", "20"),
                                                               new("serverId", "0"),
                                                               new("userName", userName),
                                                               new("password", password)
                                                           })
                   };

requestToken.Headers.Add("Cache-Control", "no-cache");
requestToken.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

var responseToken = httpClient.Send(requestToken);
Console.WriteLine(responseToken.StatusCode);

Token? token = null;
try
{
    token = responseToken.Content.ReadFromJsonAsync<Token>().Result;
    Console.WriteLine(token?.accessToken);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
#endregion

if (token != null)
{
    #region 上傳檔案
    using var fs = File.Open(@"C:\Users\wunmao\Desktop\test.txt", FileMode.Open);
    var content = new MultipartFormDataContent
                  {
                      { new StringContent("TestUpload"), "folderId" }, //! TestUpload是資料夾名稱，隨便打
                      { new StreamContent(fs), "ooxx", "test.txt" }    //! ooxx是name可以隨便替換，test.txt是filename也可以隨便替換
                  };

    var requestFile = new HttpRequestMessage
                      {
                          Method     = HttpMethod.Post,
                          RequestUri = new Uri("UploadServlet", UriKind.Relative),
                          Content    = content
                      };

    requestFile.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.accessToken);
    requestFile.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

    var responseFile = httpClient.Send(requestFile);

    Console.WriteLine(responseFile.StatusCode);
    #endregion
}

Console.ReadKey();