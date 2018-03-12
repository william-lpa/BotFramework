﻿using System.Threading.Tasks;

namespace Maratona_Bot.Services.Abstractions
{
    public interface IStoreFileProvider
    {
        Task StoreFile(byte[] image);
    }
}