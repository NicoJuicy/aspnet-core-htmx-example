﻿using Microsoft.EntityFrameworkCore;
using MusicManager.Domain.DataAccess;
using MusicManager.Domain.Dtos.Album;
using MusicManager.Domain.Dtos.Artist;
using MusicManager.Domain.Dtos.Genre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Domain.Services
{
    public class DataReadService : IDataReadService
    {
        private readonly MusicManagerContext _dbContext;

        public DataReadService(MusicManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Artists
        public async Task<ArtistViewDto> GetArtistView(int id)
        {
            return await _dbContext.Artists
                                    .Select(a => new ArtistViewDto
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                        AlbumCount = a.Albums.Count,
                                        Created = a.Created,
                                        Updated = a.Updated
                                    })
                                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<ArtistEditDto> GetArtistEdit(int id)
        {
            return await _dbContext.Artists
                                    .Select(a => new ArtistEditDto
                                    {
                                        Id = a.Id,
                                        Name = a.Name
                                    })
                                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<(IList<ArtistViewDto> pageItems, int totalCount)> GetArtistsPage(string search, string sortField,
                                                                                    bool descending, int pageIndex, int pageSize)
        {
            var query = _dbContext.Artists.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(a => EF.Functions.Like(a.Name, $"%{search}%"));

            switch (sortField?.ToLowerInvariant())
            {
                case "albumcount":
                    if (descending)
                        query = query.OrderByDescending(a => a.Albums.Count);
                    else
                        query = query.OrderBy(a => a.Albums.Count);
                    break;
                case "created":
                    if (descending)
                        query = query.OrderByDescending(a => a.Created);
                    else
                        query = query.OrderBy(a => a.Created);
                    break;
                case "updated":
                    if (descending)
                        query = query.OrderByDescending(a => a.Updated);
                    else
                        query = query.OrderBy(a => a.Updated);
                    break;
                default:
                    if (descending)
                        query = query.OrderByDescending(a => a.Name);
                    else
                        query = query.OrderBy(a => a.Name);
                    break;
            }

            var totalCount = await query.CountAsync();
            var pageItems = await query
                                    .Select(a => new ArtistViewDto
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                        AlbumCount = a.Albums.Count,
                                        Created = a.Created,
                                        Updated = a.Updated
                                    })
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return (pageItems, totalCount);
        }

        public async Task<IList<(int artistId, string artistName)>> GetArtistNames()
        {
            var result = await _dbContext.Artists.OrderBy(a => a.Name)
                                                .Select(a => new { a.Id, a.Name })
                                                .ToListAsync();

            return result.Select(a => (a.Id, a.Name)).ToList();
        }


        // Albums
        public async Task<AlbumViewDto> GetAlbumView(int id)
        {
            return await _dbContext.Albums
                                    .Select(a => new AlbumViewDto
                                    {
                                        Id = a.Id,
                                        Title = a.Title,
                                        ReleaseYear = a.ReleaseYear,
                                        TrackCount = a.Tracks.Count,
                                        Genres = a.AlbumGenres.Select(g => g.Genre.Name).ToList(),
                                        ArtistId = a.ArtistId,
                                        ArtistName = a.Artist.Name,
                                        Created = a.Created,
                                        Updated = a.Updated
                                    })
                                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AlbumEditDto> GetAlbumEdit(int id)
        {
            return await _dbContext.Albums
                                    .Select(a => new AlbumEditDto
                                    {
                                        Id = a.Id,
                                        Title = a.Title,
                                        ReleaseYear = a.ReleaseYear,
                                        ArtistId = a.ArtistId,
                                        GenreIds = a.AlbumGenres.Select(g => g.GenreId).ToList()
                                    })
                                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<(IList<AlbumViewDto> pageItems, int totalCount)> GetAlbumsPage(int? artistId, string search, string sortField,
                                                                                        bool descending, int pageIndex, int pageSize)
        {
            var query = _dbContext.Albums.AsQueryable();

            if (artistId.HasValue)
                query = query.Where(a => a.ArtistId == artistId);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(a => EF.Functions.Like(a.Title, $"%{search}%")
                                        || EF.Functions.Like(a.Artist.Name, $"%{search}%"));

            switch (sortField?.ToLowerInvariant())
            {
                case "trackcount":
                    if (descending)
                        query = query.OrderByDescending(a => a.Tracks.Count);
                    else
                        query = query.OrderBy(a => a.Tracks.Count);
                    break;
                case "releaseyear":
                    if (descending)
                        query = query.OrderByDescending(a => a.ReleaseYear);
                    else
                        query = query.OrderBy(a => a.ReleaseYear);
                    break;
                case "artistname":
                    if (descending)
                        query = query.OrderByDescending(a => a.Artist.Name);
                    else
                        query = query.OrderBy(a => a.Artist.Name);
                    break;
                case "created":
                    if (descending)
                        query = query.OrderByDescending(a => a.Created);
                    else
                        query = query.OrderBy(a => a.Created);
                    break;
                case "updated":
                    if (descending)
                        query = query.OrderByDescending(a => a.Updated);
                    else
                        query = query.OrderBy(a => a.Updated);
                    break;
                default:
                    if (descending)
                        query = query.OrderByDescending(a => a.Title);
                    else
                        query = query.OrderBy(a => a.Title);
                    break;
            }

            var totalCount = await query.CountAsync();
            var pageItems = await query
                                    .Select(a => new AlbumViewDto
                                    {
                                        Id = a.Id,
                                        Title = a.Title,
                                        ReleaseYear = a.ReleaseYear,
                                        TrackCount = a.Tracks.Count,
                                        Genres = a.AlbumGenres.Select(g => g.Genre.Name).ToList(),
                                        ArtistId = a.ArtistId,
                                        ArtistName = a.Artist.Name,
                                        Created = a.Created,
                                        Updated = a.Updated
                                    })
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return (pageItems, totalCount);
        }


        // Genres

        public async Task<IList<(int genreId, string genreName)>> GetGenreNames()
        {
            var result = await _dbContext.Genres.OrderBy(g => g.Name)
                                                .Select(g => new { g.Id, g.Name })
                                                .ToListAsync();

            return result.Select(a => (a.Id, a.Name)).ToList();
        }
        
        public async Task<(IList<GenreViewDto> pageItems, int totalCount)> GetGenresPage(string search, string sortField, bool descending, int pageIndex, int pageSize)
        {
            var query = _dbContext.Genres.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(a => EF.Functions.Like(a.Name, $"%{search}%"));

            switch (sortField?.ToLowerInvariant())
            {
                case "albumcount":
                    if (descending)
                        query = query.OrderByDescending(a => a.AlbumGenres.Count);
                    else
                        query = query.OrderBy(a => a.AlbumGenres.Count);
                    break;
                case "created":
                    if (descending)
                        query = query.OrderByDescending(a => a.Created);
                    else
                        query = query.OrderBy(a => a.Created);
                    break;
                case "updated":
                    if (descending)
                        query = query.OrderByDescending(a => a.Updated);
                    else
                        query = query.OrderBy(a => a.Updated);
                    break;
                default:
                    if (descending)
                        query = query.OrderByDescending(a => a.Name);
                    else
                        query = query.OrderBy(a => a.Name);
                    break;
            }

            var totalCount = await query.CountAsync();
            var pageItems = await query
                                    .Select(a => new GenreViewDto
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                        AlbumCount = a.AlbumGenres.Count,
                                        Created = a.Created,
                                        Updated = a.Updated
                                    })
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return (pageItems, totalCount);
        }

        public async Task<GenreViewDto> GetGenreView(int id)
        {
            return await _dbContext.Genres
                                    .Select(a => new GenreViewDto
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                        AlbumCount = a.AlbumGenres.Count,
                                        Created = a.Created,
                                        Updated = a.Updated
                                    })
                                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<GenreEditDto> GetGenreEdit(int id)
        {
            return await _dbContext.Genres
                                    .Select(a => new GenreEditDto
                                    {
                                        Id = a.Id,
                                        Name = a.Name
                                    })
                                    .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
