# SolusVM.NET

Initialise as follows:

```c#
string API_KEY = "Example-Api-Key";
string API_HASH = "Example-Api-Hash";
string HOSTNAME = "www.example.com";
int Port = 443;
bool SSL = true;

SolusClient client = new SolusClient(API_KEY, API_HASH, HOSTNAME, Port, SSL);
```

To perform a request:

```c#
private async void BootVM()
{
    string resultXML = await client.Action(SolusClient.eAction.BOOT);
    
    // to convert resultXML to SolusResponse object
    SolusResponse obj = client.ParseResult(resultXML);
}
