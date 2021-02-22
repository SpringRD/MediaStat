using BlazorInputFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Services
{
    public interface IFileUpload
    {
        Task UploadAsync(string myPath,IFileListEntry file);
        Task UploadProfilesAsync(string myPath,IFileListEntry file);
    }
}
