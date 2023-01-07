using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Weather;

[Category("LogiX/Weather")]
[NodeName("Current Weather")]
public class CurrentWeather : LogixNode
{
    public readonly Input<float> Latitude;
    public readonly Input<float> Longitude;
    public readonly Input<WeatherUnits> Units;
    public readonly Input<string> APIKeyOverride;
        
    public readonly Output<DateTime> CurrentTime; //current time of position
    public readonly Output<DateTime> SunriseTime; //sunrise time of position
    public readonly Output<DateTime> SunsetTime; //sunset time of position
    public readonly Output<float> Temperature; //using units described below
    public readonly Output<float> FeelsLike; //same as above
    public readonly Output<float> Pressure; //hPa, sea level
    public readonly Output<float> Humidity; //percentage
    public readonly Output<float> DewPoint; //using units described below
    public readonly Output<float> Cloudiness; //percentage
    public readonly Output<float> UVIndex; //i dont know what this is
    public readonly Output<float> Visibility; //meters, maximum of 10k
    public readonly Output<float> WindSpeed; //using units described below
    public readonly Output<float> WindDegrees; //direction in meteorological degrees
    public readonly Output<float> Rain; //volume in mm in 1h
    public readonly Output<float> Snow; //volume in mm in 1h

    public readonly Impulse OnSent;
    public readonly Impulse OnRetrivedWeather;
    public readonly Impulse OnError;
    
    private const string APIKey = "bd5e378503939ddaee76f12ad7a97608";
    public enum WeatherUnits
    {
        Standard, //kelvin, meters/sec
        Metric, //celcius, meters/sec
        Imperial //fahrenheit, miles/hour
    }
    [ImpulseTarget]
    public void RetriveWeather() => StartTask(GetWeather);
    private async Task GetWeather() 
    {
        var lat = Latitude.Evaluate();
        var lon = Longitude.Evaluate();
        var unit = Units.Evaluate();
        var key = APIKeyOverride.Evaluate();
        if (string.IsNullOrWhiteSpace(key)) key = APIKey;
        key = Regex.Replace(key, "^[a-z0-9]*$", "",RegexOptions.Compiled);
        var url = new Uri($"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lon}&exclude=minutely,hourly,daily,alerts&units={unit.ToString().ToLower()}&appid={key}");
            
        OnSent.Trigger();
            
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.UserAgent.Add(Cloud.UserAgent);
            using var response = await Engine.Cloud.SafeHttpClient.SendAsync(request);
            var jsonIn = JObject.Parse(await response.Content.ReadAsStringAsync());
            var cur = jsonIn["current"];
            CurrentTime.Value = new DateTime((long) cur["dt"]);
            SunriseTime.Value = new DateTime((long) cur["sunrise"]);
            SunsetTime.Value = new DateTime((long) cur["sunset"]);
            Temperature.Value = (float) cur["temp"];
            FeelsLike.Value = (float) cur["feels"];
            Pressure.Value = (float) cur["pressure"];
            Humidity.Value = (float) cur["humidity"];
            DewPoint.Value = (float) cur["dew_point"];
            UVIndex.Value = (float) cur["uvi"];
            Cloudiness.Value = (float) cur["clouds"];
            Visibility.Value = (float) cur["visibility"];
            WindSpeed.Value = (float) cur["wind_speed"];
            WindDegrees.Value = (float) cur["wind_deg"];
            Rain.Value = cur["rain"] != null ? (float) cur["rain"]["1h"] : 0;
            Snow.Value = cur["snow"] != null ? (float) cur["snow"]["1h"] : 0;
            OnRetrivedWeather.Trigger();
        }
        catch
        {
            ClearOutputs();
            OnError.Trigger();
        }
        ClearOutputs();
    }

    private void ClearOutputs()
    {
        CurrentTime.Value = default;
        SunriseTime.Value = default;
        SunsetTime.Value = default;
        Temperature.Value = default;
        FeelsLike.Value = default;
        Pressure.Value = default;
        Humidity.Value = default;
        DewPoint.Value = default;
        Cloudiness.Value = default;
        UVIndex.Value = default;
        Visibility.Value = default;
        WindSpeed.Value = default;
        WindDegrees.Value = default;
        Rain.Value = default;
        Snow.Value = default;
    }
}