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
                       Content = new FormUrlEncodedContent(new[]
                                                           {
                                                               new KeyValuePair<string, string>("clientType", "20"),
                                                               new KeyValuePair<string, string>("serverId",   "0"),
                                                               new KeyValuePair<string, string>("userName",   userName),
                                                               new KeyValuePair<string, string>("password",   password)
                                                           })
                   };

requestToken.Headers.Add("Cache-Control", "no-cache");
requestToken.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

var responseToken = httpClient.Send(requestToken);
var token         = responseToken.Content.ReadFromJsonAsync<Token>().Result!;

Console.WriteLine(responseToken.StatusCode);
Console.WriteLine(token.accessToken);
#endregion

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

Console.ReadKey();