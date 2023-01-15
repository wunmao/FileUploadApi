|**專案**|**說明**|
|-|-|
|FileUploadApi|基於.NET 7.0最小WebAPI的上傳檔案Server實作|
|RestSharpTest|使用RestSharp的Client實作Demo|
|TestClient|使用HttpClient的Clien實作Demo|

# API說明
|**Function**|**說明**|**參數**|
|-|-|-|
|getAccessToken|使用帳號密碼獲得檔案上傳Token|clientType、serverId、userName、password|
|UploadServlet|使用Token執行檔案上傳|token、folderId|
