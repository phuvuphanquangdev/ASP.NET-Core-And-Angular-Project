using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository,
            IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ?? 
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }


        [HttpGet(Name = "GetAuthors")]
        [HttpHead]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery]AuthorsResourceParameters authorsResourceParameters)
        {
            var authorsFromRepo = _courseLibraryRepository.GetAuthors(authorsResourceParameters);

            var paginationMetadata = new
            {
                totalCount = authorsFromRepo.TotalCount,
                pageSize = authorsFromRepo.PageSize,
                currentPage = authorsFromRepo.CurrentPage,
                totalPages = authorsFromRepo.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
            
            return Ok(authorsToReturn);
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public IActionResult GetAuthor(Guid authorId)
        {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null)
                return NotFound();

            var authorToReturn = _mapper.Map<AuthorDto>(authorFromRepo);

            return Ok(authorToReturn);
        }

        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthor(AuthorForCreationDto authorForCreationDto)
        {
            var authorEntity = _mapper.Map<Author>(authorForCreationDto);
            _courseLibraryRepository.AddAuthor(authorEntity);
            _courseLibraryRepository.Save();

            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);

            return CreatedAtRoute("GetAuthor", new {authorId = authorToReturn.Id}, authorToReturn);
        }

        [HttpOptions]
        public IActionResult GetAuthorOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        [HttpDelete("{authorId}")]
        public ActionResult DeleteAuthor(Guid authorId)
        {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null)
                return NotFound();

            _courseLibraryRepository.DeleteAuthor(authorFromRepo);
            _courseLibraryRepository.Save();

            return NoContent();
        }

        private string CreateAuthorsResourceUri(
           AuthorsResourceParameters authorsResourceParameters,
           ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetAuthors",
                      new
                      {
                          //fields = authorsResourceParameters.Fields,
                          //orderBy = authorsResourceParameters.OrderBy,
                          pageNumber = authorsResourceParameters.PageNumber - 1,
                          pageSize = authorsResourceParameters.PageSize,
                          mainCategory = authorsResourceParameters.MainCategory,
                          searchQuery = authorsResourceParameters.SearchQuery
                      });
                case ResourceUriType.NextPage:
                    return Url.Link("GetAuthors",
                      new
                      {
                          //fields = authorsResourceParameters.Fields,
                          //orderBy = authorsResourceParameters.OrderBy,
                          pageNumber = authorsResourceParameters.PageNumber + 1,
                          pageSize = authorsResourceParameters.PageSize,
                          mainCategory = authorsResourceParameters.MainCategory,
                          searchQuery = authorsResourceParameters.SearchQuery
                      });
                //case ResourceUriType.Current:
                default:
                    return Url.Link("GetAuthors",
                    new
                    {
                        //fields = authorsResourceParameters.Fields,
                        //orderBy = authorsResourceParameters.OrderBy,
                        pageNumber = authorsResourceParameters.PageNumber,
                        pageSize = authorsResourceParameters.PageSize,
                        mainCategory = authorsResourceParameters.MainCategory,
                        searchQuery = authorsResourceParameters.SearchQuery
                    });
            }

        }





    }
}