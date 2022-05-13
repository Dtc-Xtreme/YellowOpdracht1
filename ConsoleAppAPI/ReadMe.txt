-------------------------------------------
## Token aanvragen - PostGetTokenAsync() ##
-------------------------------------------

Eerst ben ik opzoek gegaan hoe je een request verstuurd. Hier ben ik op HttpClient getoten.
	HttpClient client = new HttpClient();					// Instanciate HttpClient().
	client.BaseAddress= new Uri("http://localhost:5002");	// Destination address.
    
Aan een HttpClient kan je content meegeven. Dus hier gegeven we gegevens mee voor de token aan te vraqen
	HttpContent content = new FormUrlEncodedContent(
                    new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string>("client_id", "console_app"),
                        new KeyValuePair<string,string>("client_secret","console-app"),
                        new KeyValuePair<string,string>("grant_type","client_credentials"),
                        new KeyValuePair<string,string>("scope", "krc-genk")
                });

Dan kunnen we een Post request uitvoeren naar de identity api met deze content. En zo onze token verkrijgen.
    HttpResponseMessage response = await client.PostAsync("connect/token", content);
    string result = await response.Content.ReadAsStringAsync();             // Get response.
    token = System.Text.Json.JsonSerializer.Deserialize<Token>(result);     // Deserialize response to Token object.


--------------------------------------------------------
## Get Seatholders for city x - GetSeatholdersAsync() ##
--------------------------------------------------------

Hier gaan we ook gebruik maken van de HttpClient maar deze keer gaan we een get request uitvoeren.
Maar we moeten ook nog een Header voor Authorization meegeven onze verkregen token.
    HttpClient client = new HttpClient();					                                                        // Instanciate HttpClient().
    client.BaseAddress = new Uri("http://localhost:5000");                                                          // Destination address
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.access_token);      // Header with Auth token that we requested earlier.

Get request uitvoeren en response verkrijgen.
    HttpResponseMessage response = await client.GetAsync("api/seatholders/" + input);   // Get request with our input (city)
    var x = await response.Content.ReadAsStringAsync();                                 // Get the response