﻿using MusicManager.Domain.Dtos.Album;
using MusicManager.Domain.Dtos.Artist;
using MusicManager.Domain.Dtos.Genre;
using MusicManager.Domain.Dtos.Track;
using System.Threading.Tasks;

namespace MusicManager.Domain.Services
{
    public interface IDataWriteService
    {
        Task<int> CreateArtist(ArtistEditDto dto);
        Task<bool> UpdateArtist(int id, ArtistEditDto dto);
        Task<bool> DeleteArtist(int id);
        
        Task<int> CreateAlbum(AlbumEditDto dto);
        Task<bool> UpdateAlbum(int id, AlbumEditDto dto);
        Task<bool> DeleteAlbum(int id);

        Task<int> CreateGenre(GenreEditDto dto);
        Task<bool> UpdateGenre(int id, GenreEditDto dto);
        Task<bool> DeleteGenre(int id);
        
        Task<int> CreateTrack(TrackEditDto dto);
        Task<bool> UpdateTrack(int id, TrackEditDto dto);
        Task<bool> DeleteTrack(int id);
    }
}
