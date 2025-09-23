using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSXMLLib
{
    public interface ITSFileFactory<T> where T : TSFile, ITSFileFactory<T>
    {
        public static async Task<T?> CreateAsync(Uri source, CancellationToken cancellationToken)
        {
            FileInfo? file = source.IsFile ? new(source.LocalPath) : await DownloadAsync(source, cancellationToken);

            if (file is not null)
            {
                T? result = await T.CreateCoreAsync(source, file, cancellationToken);
                if (result is not null) return result;
            }

            return null;
        }

        internal static abstract Task<T?> CreateCoreAsync(Uri source, FileInfo localFile, CancellationToken cancellationToken);

        internal static abstract string Extension { get; }

        internal static sealed async Task<FileInfo?> DownloadAsync(Uri uri, CancellationToken cancellationToken)
        {
            FileInfo? file = null;

            using HttpClient client = new();
            HttpResponseMessage headerResponse = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri), cancellationToken);

            if (headerResponse.IsSuccessStatusCode)
            {
                string? filename = headerResponse.Content.Headers.ContentDisposition?.FileName;
                filename ??= $"{uri!.Segments[^1]}{T.Extension}";

                DirectoryInfo directory = new(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
                if (!directory.Exists) Directory.CreateDirectory(directory.FullName);
                string path = Path.Combine(directory.FullName, filename);

                using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, true);
                using HttpResponseMessage contentResponse = await client.GetAsync(uri, HttpCompletionOption.ResponseContentRead, cancellationToken);
                using Stream contentStream = await contentResponse.Content.ReadAsStreamAsync(cancellationToken);

                await contentStream.CopyToAsync(fileStream, cancellationToken);
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