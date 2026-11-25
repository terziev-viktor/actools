extern alias Original;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Dialogs;
using FirstFloor.ModernUI.Windows.Controls;
using Newtonsoft.Json;
using Orig = Original::AcManager.Internal;

namespace AcManager.Internal {
    /// <summary>
    /// Wrapper over the closed-source <c>AcManager.Internal.InternalUtils</c> from
    /// Libraries/AcManager.Internal/AcManager.Internal.dll. It has the same public members, and each
    /// one currently forwards to the original. Change a member here to customise it.
    /// </summary>
    public static class InternalUtils {
        public const string ChecksumHeader = Orig.InternalUtils.ChecksumHeader;
        public const string AppReporterNotesKeyHeader = Orig.InternalUtils.AppReporterNotesKeyHeader;
        public const string ShareContentTypeHeader = Orig.InternalUtils.ShareContentTypeHeader;
        public const string ShareContentNameHeader = Orig.InternalUtils.ShareContentNameHeader;
        public const string ShareContentTargetHeader = Orig.InternalUtils.ShareContentTargetHeader;
        public const string ShareAuthorNameHeader = Orig.InternalUtils.ShareAuthorNameHeader;
        public const string ShareRemovalKeyHeader = Orig.InternalUtils.ShareRemovalKeyHeader;

        #region Nested types
        // Values must match the original enum: commands are cast to it by value.
        public enum AcControlPointCommand {
            FocusNextCar = 0,
            FocusPlayerCar = 1,
            FocusPreviousCar = 2,
            GentleStop = 3,
            ChangeCamera = 4,
            ChangeExternalCamera = 5,
            ChangeExtraCemera = 6,
            RestartSession = 7,
            Shutdown = 8,
            SkipSession = 9,
            StartGame = 10,
            TeleportToPits = 11,
            TeleportToPitsWithConfig = 12,
        }

        public class AcControlPoint : IDisposable {
            private readonly Orig.InternalUtils.AcControlPoint _original = new Orig.InternalUtils.AcControlPoint();

            public bool IsAvailable() {
                return _original.IsAvailable();
            }

            public void Send(AcControlPointCommand cmd) {
                _original.Send((Orig.InternalUtils.AcControlPointCommand)cmd);
            }

            public void Dispose() {
                _original.Dispose();
            }
        }

        public class ShareResult {
            [JsonProperty("id")]
            public string Id;

            [JsonProperty("removalkey")]
            public string RemovalKey;

            [JsonIgnore]
            public string Url => GetUrl(Id);

            public static string GetUrl(string id) {
                return Orig.InternalUtils.ShareResult.GetUrl(id);
            }
        }

        public class SharedEntryLoaded {
            public byte[] Data;
            public string EntryType;
            public string Name;
            public string Target;
            public string Author;
        }
        #endregion

        #region Key and initialization
        public static void Initialize(string filename, string privateValue) {
            Orig.InternalUtils.Initialize(filename, privateValue);
        }

        public static void SetKey(string value, string privateValue) {
            Orig.InternalUtils.SetKey(value, privateValue);
        }

        public static Task<bool?> CheckKeyAsync(string value, string privateValue, string userAgent,
                CancellationToken cancellation = default(CancellationToken)) {
            return Orig.InternalUtils.CheckKeyAsync(value, privateValue, userAgent, cancellation);
        }

        public static void Launch(ModernWindow mainWindow) {
            Orig.InternalUtils.Launch(mainWindow);
        }

        public static void SetSteamId(string steamId) {
            Orig.InternalUtils.SetSteamId(steamId);
        }

        public static bool VerifySignature(string filename) {
            return Orig.InternalUtils.VerifySignature(filename);
        }

        public static string Revoked => Orig.InternalUtils.Revoked;
        // always alright
        public static bool IsAllRight => true;
        public static string Key => Orig.InternalUtils.Key;
        public static string KeySafe => Orig.InternalUtils.KeySafe;
        #endregion

        #region CM API
        public static string MainApiDomain => Orig.InternalUtils.MainApiDomain;
        public static string PatchDataApiDomain => Orig.InternalUtils.PatchDataApiDomain;

        public static string CmGetApiAddress(string path) {
            return Orig.InternalUtils.CmGetApiAddress(path);
        }

        public static byte[] CmGetData_v3(string url, string userAgent) {
            return Orig.InternalUtils.CmGetData_v3(url, userAgent);
        }

        public static Task<Tuple<byte[], DateTime?>> CmGetDataAsync(string url, string userAgent, DateTime? ifModifiedSince,
                IProgress<AsyncProgressEntry> progress = null, CancellationToken cancellation = default(CancellationToken)) {
            return Orig.InternalUtils.CmGetDataAsync(url, userAgent, ifModifiedSince, progress, cancellation);
        }

        public static Task<byte[]> CmGetDataAsync(string url, string userAgent,
                IProgress<AsyncProgressEntry> progress = null, CancellationToken cancellation = default(CancellationToken)) {
            return Orig.InternalUtils.CmGetDataAsync(url, userAgent, progress, cancellation);
        }

        public static Task<Tuple<byte[], DateTime?>> CmGetPaintShopDataAsync(string carId, string userAgent, DateTime? ifModifiedSince,
                IProgress<AsyncProgressEntry> progress = null, CancellationToken cancellation = default(CancellationToken)) {
            return Orig.InternalUtils.CmGetPaintShopDataAsync(carId, userAgent, ifModifiedSince, progress, cancellation);
        }

        public static Task<byte[]> CmGetContentDataAsync(string url, string userAgent,
                IProgress<AsyncProgressEntry> progress = null, CancellationToken cancellation = default(CancellationToken)) {
            return Orig.InternalUtils.CmGetContentDataAsync(url, userAgent, progress, cancellation);
        }

        public static string GetLobbyRegistryUrl(bool dev) {
            return Orig.InternalUtils.GetLobbyRegistryUrl(dev);
        }

        public static string GetContentRegistryUrl() {
            return Orig.InternalUtils.GetContentRegistryUrl();
        }

        public static string GetEmojiUrl() {
            return Orig.InternalUtils.GetEmojiUrl();
        }

        public static IReadOnlyCollection<WebDownloadRule> LoadWebDownloadRules(string userAgent, string domain = null) {
            return Orig.InternalUtils.LoadWebDownloadRules(userAgent, domain)?
                    .Select(x => new WebDownloadRule(x.SiteUrl, x.Rule, x.Data)).ToList();
        }

        public static IReadOnlyList<ChangelogEntry> LoadChangelog(string userAgent, bool recentOnly) {
            return Orig.InternalUtils.LoadChangelog(userAgent, recentOnly)?
                    .Select(x => new ChangelogEntry(x.Version, x.Changes, x.Date)).ToList();
        }
        #endregion

        #region Reports
        public static void SendAppReport(byte[] data, string notes, string userAgent) {
            Orig.InternalUtils.SendAppReport(data, notes, userAgent);
        }

        public static void SendData(byte[] data, string notes, string userAgent) {
            Orig.InternalUtils.SendData(data, notes, userAgent);
        }

        public static void SendLocale(byte[] data, string notes, string userAgent) {
            Orig.InternalUtils.SendLocale(data, notes, userAgent);
        }
        #endregion

        #region Online data and sharing
        public static Task<string> PostOnlineDataAsync(string data, string userAgent, CancellationToken cancellation = default(CancellationToken)) {
            return Orig.InternalUtils.PostOnlineDataAsync(data, userAgent, cancellation);
        }

        public static Task<string> GetOnlineDataAsync(string id, string userAgent, CancellationToken cancellation = default(CancellationToken)) {
            return Orig.InternalUtils.GetOnlineDataAsync(id, userAgent, cancellation);
        }

        public static Task DeleteSharedEntryAsync(string id, string removalKey, string userAgent) {
            return Orig.InternalUtils.DeleteSharedEntryAsync(id, removalKey, userAgent);
        }

        public static async Task<ShareResult> ShareEntryAsync(string typeString, string name, string target, string author, byte[] data,
                string userAgent, string customId, CancellationToken cancellation = default(CancellationToken)) {
            var result = await Orig.InternalUtils.ShareEntryAsync(typeString, name, target, author, data, userAgent, customId, cancellation)
                    .ConfigureAwait(false);
            return result == null ? null : new ShareResult { Id = result.Id, RemovalKey = result.RemovalKey };
        }

        public static async Task<SharedEntryLoaded> GetSharedEntryAsync(string id, string userAgent,
                CancellationToken cancellation = default(CancellationToken)) {
            var result = await Orig.InternalUtils.GetSharedEntryAsync(id, userAgent, cancellation).ConfigureAwait(false);
            return result == null ? null : new SharedEntryLoaded {
                Data = result.Data,
                EntryType = result.EntryType,
                Name = result.Name,
                Target = result.Target,
                Author = result.Author
            };
        }
        #endregion

        #region AC and Kunos servers
        public static bool AcControlPointExecute(AcControlPointCommand cmd) {
            return Orig.InternalUtils.AcControlPointExecute((Orig.InternalUtils.AcControlPointCommand)cmd);
        }

        public static int KunosServersNumber => Orig.InternalUtils.KunosServersNumber;

        public static string GetKunosUserAgent() {
            return Orig.InternalUtils.GetKunosUserAgent();
        }

        public static string GetKunosServerUri(int offset) {
            return Orig.InternalUtils.GetKunosServerUri(offset);
        }

        public static string GetKunosServerProxyUri() {
            return Orig.InternalUtils.GetKunosServerProxyUri();
        }

        public static string GetKunosServerCompressedProxyUri() {
            return Orig.InternalUtils.GetKunosServerCompressedProxyUri();
        }

        public static void MoveToNextKunosServer() {
            Orig.InternalUtils.MoveToNextKunosServer();
        }

        public static string GetRaceUPassword(string trackId, string ip, int port) {
            return Orig.InternalUtils.GetRaceUPassword(trackId, ip, port);
        }

        public static string GetRaceUChecksum(string steamId) {
            return Orig.InternalUtils.GetRaceUChecksum(steamId);
        }

        public static string GetPackedSteamId(string packed, string additionalSalt) {
            return Orig.InternalUtils.GetPackedSteamId(packed, additionalSalt);
        }

        public static Tuple<string, string> GetLibrariesOptimizationTweak(int buildVersion) {
            return Orig.InternalUtils.GetLibrariesOptimizationTweak(buildVersion);
        }
        #endregion

        #region Third-party keys
        public static string GetDiscordClientId() {
            return Orig.InternalUtils.GetDiscordClientId();
        }

        public static string GetSteamApiCode() {
            return Orig.InternalUtils.GetSteamApiCode();
        }

        public static string GetGoogleMapsApiCode() {
            return Orig.InternalUtils.GetGoogleMapsApiCode();
        }

        public static string GetOpenWeatherApiCode() {
            return Orig.InternalUtils.GetOpenWeatherApiCode();
        }

        public static string GetTimeZoneDbApiCode() {
            return Orig.InternalUtils.GetTimeZoneDbApiCode();
        }

        public static string GetValuesStorageEncryptionKey() {
            return Orig.InternalUtils.GetValuesStorageEncryptionKey();
        }

        public static string GetOnlineInviteEncryptionKey() {
            return Orig.InternalUtils.GetOnlineInviteEncryptionKey();
        }

        public static Tuple<string> GetPositionStackApiKey() {
            return Orig.InternalUtils.GetPositionStackApiKey();
        }

        public static Tuple<string> GetYouTubeApiKey() {
            return Orig.InternalUtils.GetYouTubeApiKey();
        }

        public static Tuple<string> GetMegaAppKey() {
            return Orig.InternalUtils.GetMegaAppKey();
        }

        public static Tuple<string, string> GetGoogleDriveCredentials() {
            return Orig.InternalUtils.GetGoogleDriveCredentials();
        }

        public static Tuple<string, string> GetGoogleDriveAppFolderCredentials() {
            return Orig.InternalUtils.GetGoogleDriveAppFolderCredentials();
        }

        public static Tuple<string, string> GetOneDriveCredentials() {
            return Orig.InternalUtils.GetOneDriveCredentials();
        }

        public static Tuple<string, string> GetOneDriveAppFolderCredentials() {
            return Orig.InternalUtils.GetOneDriveAppFolderCredentials();
        }

        public static Tuple<string, string> GetYandexDiskAppFolderCredentials() {
            return Orig.InternalUtils.GetYandexDiskAppFolderCredentials();
        }

        public static Tuple<string, string> GetDropboxCredentials() {
            return Orig.InternalUtils.GetDropboxCredentials();
        }
        #endregion
    }
}
