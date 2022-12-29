using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenLibraryServer.Models;
using OpenLibraryServer.Models.DTOs;
using OpenLibraryServer.Service.Exceptions;
using OpenLibraryServer.Service.HelperFunctions;
using OpenLibraryServer.Service.Interfaces;

namespace OpenLibraryServer.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IChatService _chatService;

        public BooksController(IBookService bookService, IChatService chatService)
        {
            _bookService = bookService;
            _chatService = chatService;
        }
        [HttpGet("genres")]
        public async Task<IEnumerable<Genre>> GetBooKGenres()
        {
            return await _bookService.GetBookGenres();
        }
        [HttpGet("{bookId}")]
        public async Task<BookTO> GetById([FromRoute] string bookId)
        {
            var bookGuid = EntityHelpers.TryParseGuid(bookId);
            return await _bookService.GetById(bookGuid);
        }
        [HttpPost("filter")]
        public async Task<FilterResponse> GetByFilter([FromBody] BookFilter filter)
        {
            return await _bookService.GetByFilter(filter);
        }

        [HttpPost]
        public async Task<BookTO> CreateBook([FromBody] CreateBookTO bookTo)
        {
            var chat = await _chatService.CreateChat(new ChatTO()
            {
                Name = $"{bookTo.Title} - Chat",
                ImageURL = bookTo.ImageLink,
                LastMessage = null
            });
            var book = await _bookService.AddBook(bookTo, chat.ChatId);
            return book;
        }
    }
}