using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DXC.Technology.Gis.Here
{
    #region GeoResult Class
    public class GeoResult
    {
        #region Public Properties

        /// <summary>
        /// Longitude of the geocoded location.
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// Latitude of the geocoded location.
        /// </summary>
        public decimal Latitude { get; set; }

        #endregion
    }
    #endregion

    #region GeoCodeLocationHereHelper Class
    public static class GeoCodeLocationHereHelper
    {
        #region Static Fields

        /// <summary>
        /// Template for the geocoding request URL.
        /// </summary>
        private static readonly string requestTemplate = "https://geocoder.api.here.com/6.2/geocode.json?app_id=hGRrOFP8FVs59mIXX4sf&app_code=duoDDtAIUvPnMhNkT5nTfg&searchtext= {3} {4},{1} {2},{0}";

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets the geocoded location based on the provided address details.
        /// </summary>
        /// <param name="country">Country name.</param>
        /// <param name="zip">Postal code.</param>
        /// <param name="city">City name.</param>
        /// <param name="street">Street name.</param>
        /// <param name="houseNumber">House number.</param>
        /// <returns>A GeoResult object containing latitude and longitude.</returns>
        public static GeoResult GetGeoCodeLocation(string country, string zip, string city, string street, string houseNumber)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            string jsonUrl = string.Format(DXC.Technology.Utilities.StringFormatProvider.Default,
                requestTemplate,
                country, zip, city, street, houseNumber);

            double latitude = 0;
            double longitude = 0;

            using (var webClient = new WebClient())
            {
                var jsonData = string.Empty;

                try
                {
                    jsonData = webClient.DownloadString(jsonUrl);
                }
                catch (Exception)
                {
                    // Handle exceptions if necessary
                }

                HereResponse response = null;
                if (!string.IsNullOrEmpty(jsonData))
                {
                    response = JsonSerializer.Deserialize<HereResponse>(jsonData);
                }

                if (response != null && response.Response.View.Any() && response.Response.View[0].Result.Any())
                {
                    latitude = response.Response.View[0].Result[0].Location.DisplayPosition.Latitude;
                    longitude = response.Response.View[0].Result[0].Location.DisplayPosition.Longitude;
                }
            }

            return new GeoResult { Latitude = Convert.ToDecimal(latitude), Longitude = Convert.ToDecimal(longitude) };
        }

        #endregion
    }
    #endregion

    #region HereResponse Class
    public partial class HereResponse
    {
        #region Public Properties

        /// <summary>
        /// Response object containing geocoding results.
        /// </summary>
        [JsonPropertyName("Response")]
        public Response Response { get; set; }

        #endregion
    }
    #endregion

    #region Response Class
    public partial class Response
    {
        #region Public Properties

        /// <summary>
        /// Metadata information about the response.
        /// </summary>
        [JsonPropertyName("MetaInfo")]
        public MetaInfo MetaInfo { get; set; }

        /// <summary>
        /// Array of views containing geocoding results.
        /// </summary>
        [JsonPropertyName("View")]
        public View[] View { get; set; }

        #endregion
    }
    #endregion

    #region MetaInfo Class
    public partial class MetaInfo
    {
        #region Public Properties

        /// <summary>
        /// Timestamp of the response.
        /// </summary>
        [JsonPropertyName("Timestamp")]
        public string Timestamp { get; set; }

        #endregion
    }
    #endregion

    #region View Class
    public partial class View
    {
        #region Public Properties

        /// <summary>
        /// Type of the view.
        /// </summary>
        [JsonPropertyName("_type")]
        public string Type { get; set; }

        /// <summary>
        /// Identifier for the view.
        /// </summary>
        [JsonPropertyName("ViewId")]
        public long ViewId { get; set; }

        /// <summary>
        /// Array of results within the view.
        /// </summary>
        [JsonPropertyName("Result")]
        public Result[] Result { get; set; }

        #endregion
    }
    #endregion

    #region Result Class
    public partial class Result
    {
        #region Public Properties

        /// <summary>
        /// Relevance score of the result.
        /// </summary>
        [JsonPropertyName("Relevance")]
        public double Relevance { get; set; }

        /// <summary>
        /// Level of match for the result.
        /// </summary>
        [JsonPropertyName("MatchLevel")]
        public string MatchLevel { get; set; }

        /// <summary>
        /// Quality of the match.
        /// </summary>
        [JsonPropertyName("MatchQuality")]
        public MatchQuality MatchQuality { get; set; }

        /// <summary>
        /// Type of match for the result.
        /// </summary>
        [JsonPropertyName("MatchType")]
        public string MatchType { get; set; }

        /// <summary>
        /// Location details of the result.
        /// </summary>
        [JsonPropertyName("Location")]
        public Location Location { get; set; }

        #endregion
    }
    #endregion

    #region Location Class
    public partial class Location
    {
        #region Public Properties

        /// <summary>
        /// Identifier for the location.
        /// </summary>
        [JsonPropertyName("LocationId")]
        public string LocationId { get; set; }

        /// <summary>
        /// Type of the location.
        /// </summary>
        [JsonPropertyName("LocationType")]
        public string LocationType { get; set; }

        /// <summary>
        /// Display position of the location.
        /// </summary>
        [JsonPropertyName("DisplayPosition")]
        public DisplayPosition DisplayPosition { get; set; }

        /// <summary>
        /// Navigation positions for the location.
        /// </summary>
        [JsonPropertyName("NavigationPosition")]
        public DisplayPosition[] NavigationPosition { get; set; }

        /// <summary>
        /// Map view details for the location.
        /// </summary>
        [JsonPropertyName("MapView")]
        public MapView MapView { get; set; }

        /// <summary>
        /// Address details of the location.
        /// </summary>
        [JsonPropertyName("Address")]
        public Address Address { get; set; }

        #endregion
    }
    #endregion

    #region Address Class
    public partial class Address
    {
        #region Public Properties

        /// <summary>
        /// Label for the address.
        /// </summary>
        [JsonPropertyName("Label")]
        public string Label { get; set; }

        /// <summary>
        /// Country of the address.
        /// </summary>
        [JsonPropertyName("Country")]
        public string Country { get; set; }

        /// <summary>
        /// State of the address.
        /// </summary>
        [JsonPropertyName("State")]
        public string State { get; set; }

        /// <summary>
        /// County of the address.
        /// </summary>
        [JsonPropertyName("County")]
        public string County { get; set; }

        /// <summary>
        /// City of the address.
        /// </summary>
        [JsonPropertyName("City")]
        public string City { get; set; }

        /// <summary>
        /// Street of the address.
        /// </summary>
        [JsonPropertyName("Street")]
        public string Street { get; set; }

        /// <summary>
        /// House number of the address.
        /// </summary>
        [JsonPropertyName("HouseNumber")]
        public string HouseNumber { get; set; }

        /// <summary>
        /// Postal code of the address.
        /// </summary>
        [JsonPropertyName("PostalCode")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Additional data for the address.
        /// </summary>
        [JsonPropertyName("AdditionalData")]
        public AdditionalDatum[] AdditionalData { get; set; }

        #endregion
    }
    #endregion

    #region AdditionalDatum Class
    public partial class AdditionalDatum
    {
        #region Public Properties

        /// <summary>
        /// Value of the additional data.
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }

        /// <summary>
        /// Key of the additional data.
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }

        #endregion
    }
    #endregion

    #region DisplayPosition Class
    public partial class DisplayPosition
    {
        #region Public Properties

        /// <summary>
        /// Latitude of the display position.
        /// </summary>
        [JsonPropertyName("Latitude")]
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude of the display position.
        /// </summary>
        [JsonPropertyName("Longitude")]
        public double Longitude { get; set; }

        #endregion
    }
    #endregion

    #region MapView Class
    public partial class MapView
    {
        #region Public Properties

        /// <summary>
        /// Top-left position of the map view.
        /// </summary>
        [JsonPropertyName("TopLeft")]
        public DisplayPosition TopLeft { get; set; }

        /// <summary>
        /// Bottom-right position of the map view.
        /// </summary>
        [JsonPropertyName("BottomRight")]
        public DisplayPosition BottomRight { get; set; }

        #endregion
    }
    #endregion

    #region MatchQuality Class
    public partial class MatchQuality
    {
        #region Public Properties

        /// <summary>
        /// Match quality for the country.
        /// </summary>
        [JsonPropertyName("Country")]
        public double Country { get; set; }

        /// <summary>
        /// Match quality for the city.
        /// </summary>
        [JsonPropertyName("City")]
        public double City { get; set; }

        /// <summary>
        /// Match quality for the street.
        /// </summary>
        [JsonPropertyName("Street")]
        public double[] Street { get; set; }

        /// <summary>
        /// Match quality for the house number.
        /// </summary>
        [JsonPropertyName("HouseNumber")]
        public double HouseNumber { get; set; }

        /// <summary>
        /// Match quality for the postal code.
        /// </summary>
        [JsonPropertyName("PostalCode")]
        public double PostalCode { get; set; }

        #endregion
    }
    #endregion
}