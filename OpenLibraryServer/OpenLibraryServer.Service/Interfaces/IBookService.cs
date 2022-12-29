using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenLibraryServer.Models;
using OpenLibraryServer.Models.DTOs;

namespace OpenLibraryServer.Service.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Genre>> GetBookGenres();
        Task<BookTO> GetById(Guid guid);
        Task<FilterResponse> GetByFilter(BookFilter filter);
        Task<BookTO> AddBook(CreateBookTO bookTo, Guid chatId);
        Task ChangeBookStatus(Guid bookId, string status);
    }
}