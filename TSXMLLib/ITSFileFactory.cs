using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSXMLLib
{
    public interface ITSFileFactory<T> where T : TSFile, ITSFileFactory<T>
    {
        internal static abstract Task<T?> CreateFromLocalFileAsyncCore(FileInfo file);
        public static sealed async Task<T?> CreateFromLocalFileAsync(FileInfo file) => await T.CreateFromLocalFileAsyncCore(file);
        public static sealed async Task<T?> CreateFromRemoteFileAsync(Uri uri, DirectoryInfo? destination = null)
        {
            FileInfo? file = await DownloadAsync(uri, destination);
            T? result = file is not null ? await T.CreateFromLocalFileAsyncCore(file) : null;
            if (result is not null) result.URI = uri;

            return result;
        }

        internal static abstract string Extension { get; }

        internal static sealed async Task<FileInfo?> DownloadAsync(Uri uri, DirectoryInfo? destination = null)
        {
            FileInfo? file = null;

            using HttpClient client = new();
            HttpResponseMessage headerResponse = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri));

            if (headerResponse.IsSuccessStatusCode)
            {
                string? filename = headerResponse.Content.Headers.ContentDisposition?.FileName;
                filename ??= $"{uri!.Segments[^1]}{T.Extension}";

                DirectoryInfo directory = destination ?? new(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
                if (!directory.Exists) Directory.CreateDirectory(directory.FullName);
                string path = Path.Combine(directory.FullName, filename);

                using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, true);
                using HttpResponseMessage contentResponse = await client.GetAsync(uri, HttpCompletionOption.ResponseContentRead);
                using Stream contentStream = await contentResponse.Content.ReadAsStreamAsync();

                await contentStream.CopyToAsync(fileStream);
                file = new(path);
            }

            else
            {
                // TODO: do something if it does not respond successfully
            }

            return file;
        }
    }
}