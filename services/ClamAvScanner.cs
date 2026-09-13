using Amazon.S3.Model;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace movie_booking.services
{
    public class ClamAvScanner
    {
        public readonly string _clamAvServer;
        public readonly int _clamAvPort;

        public ClamAvScanner(IConfiguration config)
        {
            this._clamAvServer = config["ClamAV:Server"];
            this._clamAvPort = int.Parse(config["ClamAV:Port"]);
        }

        // returns bool value of whether malicious thing is included or not
        public async Task<bool> ScanFileAsync(Stream R2ObjectStream)
        {
            // first get file data from the r2 object file storage from the stream

            // set the maximum number of bytes can be read from the stream at a time
            byte[] buffer = new byte[8192];
            int readedBytes;
            bool notSecure = false;

            // first extablish tcp connection to the clamAv service ie running as the docker containter
            using (var client = new TcpClient(this._clamAvServer, this._clamAvPort))
            {
                using (var netWorkStream = client.GetStream())
                {
                    // read the bytes sequently until the bytes are zero ie all the bytes are readed from the file stream
                    while ((readedBytes = await R2ObjectStream.ReadAsync(buffer)) > 0)
                    {

                        //) over a network socket. The zINSTREAM command literally tells the scanner: "Ge" +
                        //"t ready, because I am about to stream the bytes of a file directly to you over this connection so you can scan it."[1]
                        //(https://docs.clamav.net/manual/Usage/ClamdProtocol.html), [2] (https://stackoverflow.com/questions/59377579/using-netty-with-clamav-instream)
                        //, [3] (https://docs.clamav.net/manual/Usage/ClamdProtocol.html)
                        var command = Encoding.UTF8.GetBytes("zINSTREAM\0");
                        // so the command ie "zINSTREAM\0" is ready ie accepted by the clamAv damenon that now file will be sending via bytes  and now then writes into
                        // the stream ie the command and the starting position of the bytes is 0 and the also the length of the bytes
                        await netWorkStream.WriteAsync(command, 0, command.Length);
                        await netWorkStream.WriteAsync(buffer, 0, readedBytes);
                        // End of stream
                    }
                    // after the end of the writing to the stream manually should stop the bytes writing to the stream by passing 0
                    await netWorkStream.WriteAsync(new byte[] { 0, 0, 0, 0 });
                    byte[] responseBuffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = await netWorkStream.ReadAsync(responseBuffer)) > 0)
                    {
                        var response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
                        if (response.Contains("FOUND")) { notSecure = true; }
                    }

                }
            }
            return notSecure;
        }
    }
}
