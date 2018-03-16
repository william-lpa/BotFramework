using System;
using System.Collections.Generic;

namespace Maratona_Bot.Model
{
    public class FilesRetrieved
    {
        public IEnumerable<File> Files { get; set; }
    }

    public class File
    {
        public string AlbumName { get; set; }
        public Content Data { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Content
    {
        public byte[] Data { get; set; }
    }
}