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
```

Due to there being multiple different types of 'INFO' you can request you can do the following:

```c#
    private async void GetInfo()
    {
        List<SolusClient.eInfoAction> actionList = new List<SolusClient.eInfoAction>() {
            SolusClient.eInfoAction.INFO_BW,
            SolusClient.eInfoAction.INFO_HDD,
        };
        string resultXML = await client.Action(SolusClient.eAction.INFO, actionList);
    }
```

Or to avoid this and get all of the INFO options:
```c#
    private async void GetInfo()
    {
        string resultXML = await client.Action(SolusClient.eAction.INFO, SolusClient.eInfoAction.INFO_ALL);
    }
```
