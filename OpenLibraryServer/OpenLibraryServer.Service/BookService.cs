using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OpenLibraryServer.DataAccess;
using OpenLibraryServer.Models;
using OpenLibraryServer.Models.DTOs;
using OpenLibraryServer.Service.Exceptions;
using OpenLibraryServer.Service.Interfaces;

namespace OpenLibraryServer.Service
{
    public class BookService : IBookService
    {
        private readonly OpenLibraryServerDBContext _dbContext;

        private static string _fullFilterQuery =
            "select * from \"Books\" b where LOWER(b.\"Title\") like '%TITLE%' and LOWER(b.\"Author\") like '%AUTHOR%' and EXISTS (select bg.\"BookId\" from \"BookGenres\" bg right join \"Genres\" g on bg.\"GenreId\" = g.\"GenreId\" where g.\"GenreId\" in (GENRES) and bg.\"BookId\" = b.\"BookId\") order by b.\"Title\" offset OFFSET limit LIMIT";
        private static string _filterWithoutGenresQuery =
            "select * from \"Books\" b where LOWER(b.\"Title\") like '%TITLE%' and LOWER(b.\"Author\") like '%AUTHOR%' order by b.\"Title\" offset OFFSET limit LIMIT";
        public BookService(OpenLibraryServerDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Genre>> GetBookGenres()
        {
            return await _dbContext.Genres.OrderBy(g => g.GenreName).ToListAsync();
        }

        public async Task<BookTO> GetById(Guid guid)
        {
            var book = await GetDBBookById(guid);
            return ConvertToTO(book);
        }

        private async Task<Book> GetDBBookById(Guid guid)
        {
            var book = await _dbContext.Books
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .FirstOrDefaultAsync(b => b.BookId == guid);
            if (book == null)
            {
                throw new NotFoundException($"Book with Id {guid} not found");
            }

            return book;
        }

        public async Task<FilterResponse> GetByFilter(BookFilter filter)
        {
            var request = filter.Genres.Length > 0
                ? SetFilterParams(_fullFilterQuery, filter)
                : SetFilterParams(_filterWithoutGenresQuery, filter);
            var books = await _dbContext.Books.FromSqlRaw(request).Include(b => b.BookGenres).ThenInclude(bg => bg.Genre).ToListAsync();
            var count = await _dbContext.Books.FromSqlRaw(request.Substring(0, request.IndexOf("order by"))).CountAsync();
            var results = books.Select(ConvertToTO);
            return new FilterResponse()
            {
                Count = count,
                BookTOs = results.OrderBy(r => r.Title)
            };
        }

        private string SetFilterParams(string query, BookFilter bookFilter)
        {
            return query.Replace("TITLE", bookFilter.Title.ToLower())
                .Replace("AUTHOR", bookFilter.Author.ToLower())
                .Replace("GENRES", string.Join(',', bookFilter.Genres))
                .Replace("OFFSET",bookFilter.Offset.ToString())
                .Replace("LIMIT",bookFilter.Count.ToString());
        }

        public async Task<BookTO> AddBook(CreateBookTO bookTo, Guid chatId)
        {
            var book = new Book()
            {
                Author = bookTo.Author,
                Description = bookTo.Description,
                Status = "Active",
                Title = bookTo.Title,
                FileURL = bookTo.FileURL,
                PictureURL = bookTo.ImageLink,
                ChatId = chatId
            };
            var newBook = await _dbContext.Books.AddAsync(book);
            var bookEntity = newBook.Entity;
            await _dbContext.SaveChangesAsync();
            var genres = await _dbContext.Genres.Where(g => bookTo.Genres.Contains(g.GenreId)).ToListAsync();
            var bookGenres = genres.Select(g => new BookGenres()
            {
                BookId = bookEntity.BookId,
                Book = bookEntity,
                GenreId = g.GenreId,
                Genre = g
            });
            await _dbContext.BookGenres.AddRangeAsync(bookGenres);
            await _dbContext.SaveChangesAsync();
            return ConvertToTO(_dbContext.Books.Where(b => b.BookId == bookEntity.BookId).Include(b => b.BookGenres).ThenInclude(bg => bg.Genre).First());
        }

        public async Task ChangeBookStatus(Guid bookId, string status)
        {
            var book = await GetDBBookById(bookId);
            book.Status = status;
            await _dbContext.SaveChangesAsync();
        }

        private static BookTO ConvertToTO(Book book)
        {
            return new BookTO()
            {
                Author = book.Author,
                Description = book.Description,
                Genres = book.BookGenres.Select(bg => bg.Genre).ToList(),
                Status = book.Status,
                Title = book.Title,
                BookId = book.BookId,
                FileURL = book.FileURL,
                ImageLink = book.PictureURL,
                ChatId = book.ChatId
            };
        }
    }
}