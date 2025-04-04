using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DXC.Technology.Gis
{
    public class GeoCodeLocationGoogleHelper
    {
        #region Public Static Methods

        /// <summary>
        /// Gets the geocode location based on the provided address details.
        /// </summary>
        /// <param name="country">The country name.</param>
        /// <param name="zip">The zip code.</param>
        /// <param name="city">The city name.</param>
        /// <param name="street">The street name.</param>
        /// <param name="houseNumber">The house number.</param>
        public static Location GetGeoCodeLocation(string country, string zip, string city, string street, string houseNumber)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object sender, X509Certificate certificate,
                         X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

            const string requestTemplate = "https://maps.googleapis.com/maps/api/geocode/json?address={4}+{3},+{1}+{2},+{0}&key=AIzaSyCY66KKfunc9EM3ahZq4zDx9PUP83YX3FY";

            string jsonUrl = string.Format(DXC.Technology.Utilities.StringFormatProvider.Default,
                requestTemplate,
                country, zip, city, street, houseNumber);

            var request = (HttpWebRequest)HttpWebRequest.Create(jsonUrl);
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GeoCodeLocationGoogleResult));
            GeoCodeLocationGoogleResult result = (GeoCodeLocationGoogleResult)serializer.ReadObject(request.GetResponse().GetResponseStream());

            if (result.Results.Count == 0) return new Location { Lat = 0, Lng = 0 };

            return result.Results[0].Geometry.Location;
        }

        #endregion
    }

    public class AddressComponent
    {
        #region Public Properties

        /// <summary>
        /// The long name of the address component.
        /// </summary>
        public string LongName { get; set; }

        /// <summary>
        /// The short name of the address component.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// The types of the address component.
        /// </summary>
        public IList<string> Types { get; set; }

        #endregion
    }

    public class Location
    {
        #region Public Properties

        /// <summary>
        /// Latitude of the location.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Longitude of the location.
        /// </summary>
        public double Lng { get; set; }

        #endregion
    }

    public class Northeast
    {
        #region Public Properties

        /// <summary>
        /// Latitude of the northeast corner.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Longitude of the northeast corner.
        /// </summary>
        public double Lng { get; set; }

        #endregion
    }

    public class Southwest
    {
        #region Public Properties

        /// <summary>
        /// Latitude of the southwest corner.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Longitude of the southwest corner.
        /// </summary>
        public double Lng { get; set; }

        #endregion
    }

    public class Viewport
    {
        #region Public Properties

        /// <summary>
        /// Northeast corner of the viewport.
        /// </summary>
        public Northeast Northeast { get; set; }

        /// <summary>
        /// Southwest corner of the viewport.
        /// </summary>
        public Southwest Southwest { get; set; }

        #endregion
    }

    public class Geometry
    {
        #region Public Properties

        /// <summary>
        /// Location details.
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Type of the location.
        /// </summary>
        public string LocationType { get; set; }

        /// <summary>
        /// Viewport details.
        /// </summary>
        public Viewport Viewport { get; set; }

        #endregion
    }

    public class Result
    {
        #region Public Properties

        /// <summary>
        /// Address components of the result.
        /// </summary>
        public IList<AddressComponent> AddressComponents { get; set; }

        /// <summary>
        /// Formatted address of the result.
        /// </summary>
        public string FormattedAddress { get; set; }

        /// <summary>
        /// Geometry details of the result.
        /// </summary>
        public Geometry Geometry { get; set; }

        /// <summary>
        /// Place ID of the result.
        /// </summary>
        public string PlaceId { get; set; }

        /// <summary>
        /// Types of the result.
        /// </summary>
        public IList<string> Types { get; set; }

        #endregion
    }

    public class GeoCodeLocationGoogleResult
    {
        #region Public Properties

        /// <summary>
        /// List of results from the geocode API.
        /// </summary>
        public IList<Result> Results { get; set; }

        /// <summary>
        /// Status of the geocode API response.
        /// </summary>
        public string Status { get; set; }

        #endregion
    }
}