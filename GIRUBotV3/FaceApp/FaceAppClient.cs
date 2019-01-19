using GIRUBotV3.Personality;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FaceApp
{
    public class FaceAppClient
    {
        private const string BaseUrl = "https://node-01.faceapp.io/api/v3.0/photos";
        private const string UserAgent = "FaceApp/1.0.229 (Linux; Android 4.4)";
        private const int IdLength = 8;

        private string _deviceId;
        private HttpClient _client;

        private readonly ImmutableArray<FilterType> ProFilters = ImmutableArray.Create(
        
            FilterType.Bangs,
            FilterType.Female,
            FilterType.Female_2,
            FilterType.Glasses,
            FilterType.Goatee,
            FilterType.Heisenberg,
            FilterType.Hipster,
            FilterType.Hitman,
            FilterType.Hollywood,
            FilterType.Impression,
            FilterType.Lion,
            FilterType.Makeup,
            FilterType.Male,
            FilterType.Mustache,
            FilterType.Pan,
            FilterType.Wave
        );

        public FaceAppClient(HttpClient client)
        {
            _client = client;
            _deviceId = GenerateDeviceId();
        }


       
        /// <summary>
        /// Applies the filter type provided using the image code.
        /// </summary>
        /// <param name="code"></param>
        /// Image code provided by the API.
        /// <param name="filter"></param>
        /// Type of filter to be applied.
        /// <returns></returns>
        public async Task<Stream> ApplyFilterAsync(string code, FilterType filter, CancellationToken ct = default(CancellationToken))
        {
            bool cropped = false;
            if (ProFilters.Any(x => x == filter))
                cropped = true;
            var reqUrl = $"{BaseUrl}/{code}/filters/{filter.ToString().ToLower()}?cropped={cropped}";
            var request = new HttpRequestMessage(HttpMethod.Get, reqUrl);
            request.Headers.Add("User-Agent", UserAgent);
            request.Headers.Add("X-FaceApp-DeviceID", _deviceId);
            ct.ThrowIfCancellationRequested();
            var response = await _client.SendAsync(request, ct).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                string errorCode = null;
                if (response.Headers.TryGetValues("X-FaceApp-ErrorCode", out var codes))
                    errorCode = codes.First();
                var exp = await HandleException(errorCode);
                throw exp;
            }
            return await response.Content.ReadAsStreamAsync();           
        }

        /// <summary>
        /// Retrieves the image code from the image's url.
        /// </summary>
        /// <param name="uri"></param>
        /// The valid uri of the image.
        /// <returns></returns>
        public async Task<string> GetCodeAsync(Uri uri, CancellationToken ct = default(CancellationToken))
        {
            using (var imageStream = await _client.GetStreamAsync(uri).ConfigureAwait(false))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl);
                request.Headers.Add("User-Agent", UserAgent);
                request.Headers.Add("X-FaceApp-DeviceID", _deviceId);
                var streamContent = new StreamContent(imageStream);
                var mutipartContent = new MultipartFormDataContent();
                mutipartContent.Add(streamContent, "file", "file");
                request.Content = mutipartContent;
                ct.ThrowIfCancellationRequested();
                var response = await _client.SendAsync(request, ct).ConfigureAwait(false);
                var jsonStr = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    string errorCode = null;
                    if (response.Headers.TryGetValues("X-FaceApp-ErrorCode", out var codes))
                        errorCode = codes.First();
                    var exp = HandleException(errorCode);
                    throw await exp;
                }
                return JObject.Parse(jsonStr)["code"].ToString();              
            }
        }

        /// <summary>
        /// Retrieves the image code from the image's path.
        /// </summary>
        /// <param name="path"></param>
        /// Valid path of the file.
        /// <returns></returns>
        public async Task<string> GetCodeAsync(string path, CancellationToken ct = default(CancellationToken))
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("The file specified was not found.");
            using (var imageStream = File.Open(path, FileMode.Open))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl);
                request.Headers.Add("User-Agent", UserAgent);
                request.Headers.Add("X-FaceApp-DeviceID", _deviceId);
                var fileName = Path.GetFileName(path);
                var streamContent = new StreamContent(imageStream);
                var mutipartContent = new MultipartFormDataContent();
                mutipartContent.Add(streamContent, "file", fileName);
                request.Content = mutipartContent;
                ct.ThrowIfCancellationRequested();
                var response = await _client.SendAsync(request, ct).ConfigureAwait(false);
                var jsonStr = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    string errorCode = null;
                    if (response.Headers.TryGetValues("X-FaceApp-ErrorCode", out var codes))
                        errorCode = codes.First();
                    var exp = await HandleException(errorCode);
                    throw exp;
                }
                return JObject.Parse(jsonStr)["code"].ToString();
            }
        }

        public async Task<FaceException> HandleException(string errorCode)
        {
            var insult = await Insults.GetInsult();
            switch (errorCode)
            {
                case "photo_bad_type":
                     return new FaceException(ExceptionType.BadImageType, "the shit api wont acccept image in that format");
                case "photo_no_faces":
                    return new FaceException(ExceptionType.NoFacesDetected, "i dont see a face in that pic " + insult);
                default:
                    return new FaceException(ExceptionType.Unknown, "think we got banned boys."); 
            }
        }
        //Something only a madman would do. :^)
        private string GenerateDeviceId()
            => Guid.NewGuid().ToString().Replace("-", "").Substring(0, IdLength);
    }
}
