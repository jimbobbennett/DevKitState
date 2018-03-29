using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using Microsoft.Azure.Devices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;

static RegistryManager registryManager;
static string connectionString = Environment.GetEnvironmentVariable("iotHubConnectionString");

// Modify the device name for your environment
static string deviceName = "";

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    HttpResponseMessage response;
    registryManager = RegistryManager.CreateFromConnectionString(connectionString);
    // parse query parameter
    string action = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "action", true) == 0)
        .Value;

    if (action == "get")
    {
        string callback = req.GetQueryNameValuePairs()
            .FirstOrDefault(q => string.Compare(q.Key, "callback", true) == 0)
            .Value;

        if (String.IsNullOrEmpty(callback))
        {
            callback = "callback";
        }
        var twin = await registryManager.GetTwinAsync(deviceName);
        var json = JsonConvert.SerializeObject(twin.Properties);
        response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = new StringContent(callback + "(" + json + ");", System.Text.Encoding.UTF8, "application/javascript");
    }
    else if (action == "set")
    {
        string _key = req.GetQueryNameValuePairs()
            .FirstOrDefault(q => string.Compare(q.Key, "key", true) == 0)
            .Value;
        string _state = req.GetQueryNameValuePairs()
            .FirstOrDefault(q => string.Compare(q.Key, "state", true) == 0)
            .Value;

        SetDeviceTwin(_state, _key).Wait();
        response = new HttpResponseMessage(HttpStatusCode.NoContent);
    }
    else
    {
        response = new HttpResponseMessage(HttpStatusCode.BadRequest);
    }

    return response;
}

public static async Task SetDeviceTwin(string _state, string _key)
{
    var twin = await registryManager.GetTwinAsync(deviceName);
    int state;
    bool parsed;
    switch(_key) {
        case "userLED":
            parsed = Int32.TryParse(_state, out state);
            if (parsed) {
                var patch = new {
                    properties = new {
                        desired = new {
                            userLEDState = state
                        }
                    }
                };
                await registryManager.UpdateTwinAsync(twin.DeviceId, JsonConvert.SerializeObject(patch), twin.ETag);
            }
            break;
        case "rgbLED":
            parsed = Int32.TryParse(_state, out state);
            if (parsed) {
                var patch = new {
                    properties = new {
                        desired = new {
                            rgbLEDState = state
                        }
                    }
                };
                await registryManager.UpdateTwinAsync(twin.DeviceId, JsonConvert.SerializeObject(patch), twin.ETag);
            }
            break;
        case "rgbLEDColor":
            var color = Regex.Split(_state, ",");
            int r, g, b;
            bool parsedR = Int32.TryParse(color[0], out r);
            bool parsedG = Int32.TryParse(color[1], out g);
            bool parsedB = Int32.TryParse(color[2], out b);
            if (parsedR && parsedG && parsedB) {
                var patch = new {
                    properties = new {
                        desired = new {
                            rgbLEDR = r,
                            rgbLEDG = g,
                            rgbLEDB = b
                        }
                    }
                };
                await registryManager.UpdateTwinAsync(twin.DeviceId, JsonConvert.SerializeObject(patch), twin.ETag);
            }
            break;
        default:
            break;
    }
}