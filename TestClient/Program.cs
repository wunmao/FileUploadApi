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

requestToken.Headers.Add("cache-control", "no-cache");
requestToken.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

var response1 = httpClient.Send(requestToken);
var token     = response1.Content.ReadFromJsonAsync<Token>().Result!;

Console.WriteLine(response1.StatusCode);
Console.WriteLine(token.accessToken);
#endregion

#region 上傳檔案
using var fs = File.Open(@"C:\Users\wunma\Desktop\nugetpush.txt", FileMode.Open);
var content = new MultipartFormDataContent
              {
                  { new StringContent("TestUpload"), "folderId" },
                  { new StreamContent(fs), "file", "test.txt" }
              };

var requestFile = new HttpRequestMessage
                  {
                      Method     = HttpMethod.Post,
                      RequestUri = new Uri("UploadServlet", UriKind.Relative),
                      Content    = content
                  };

requestFile.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.accessToken);
requestToken.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

var response2 = httpClient.Send(requestFile);

Console.WriteLine(response2.StatusCode);
#endregion

Console.ReadKey();