using FileUploadApi;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(option => option.Limits.MaxRequestBodySize = 100 * 1024 * 1024);
var app     = builder.Build();
app.Urls.Add("http://localhost:7000");

app.MapPost("getAccessToken",
            async context =>
            {
                var request    = context.Request;
                var clientType = request.Form["clientType"];
                var serverId   = request.Form["serverId"];
                var userName   = request.Form["userName"];
                var password   = request.Form["password"];

                Console.WriteLine($"clientType = {clientType}");
                Console.WriteLine($"serverId = {serverId}");
                Console.WriteLine($"userName = {userName}");
                Console.WriteLine($"password = {password}");

                context.Response.StatusCode = 200;

                var token = new Token
                            { 
                                accessToken  = "ooxx",
                                refreshToken = "ooxx",
                                expiresIn    = long.MaxValue
                            };

                await context.Response.WriteAsJsonAsync(token);
            });

app.MapPost("UploadServlet",
            async context =>
            {
                var request  = context.Request;
                var token    = request.Headers["Authorization"].ToString().Split(' ')[1];
                var folderId = request.Form["folderId"];

                Console.WriteLine($"token = {token}");
                Console.WriteLine($"folderId = {folderId}");

                context.Response.StatusCode = 200;

                foreach (var file in request.Form.Files)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            var path                       = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\\{folderId}";
                            if (!Directory.Exists(path)) _ = Directory.CreateDirectory(path);
                            await using var inputStream    = new FileStream($"{path}\\{file.FileName}", FileMode.Create);
                            await file.CopyToAsync(inputStream);

                            Console.WriteLine($"¤w¦¬¨ì{file.FileName}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            });

app.Run();