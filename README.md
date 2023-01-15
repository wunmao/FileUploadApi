# 某客戶案例

|**專案**|**說明**|
|-|-|
|FileUploadApi|基於.NET 7.0最小WebAPI的Server實作，用於測試|
|RestSharpTest|使用RestSharp的Client實作Demo（客戶範例修改）|
|TestClient|使用HttpClient的Clien實作Demo（推薦使用）|

## API說明
|**Function**|**說明**|**參數**|
|-|-|-|
|getAccessToken|使用帳號密碼獲得檔案上傳Token|clientType、serverId、userName、password|
|UploadServlet|使用Token執行檔案上傳|token、folderId|
