using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    [Route("api/authors/{authorId}/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public CoursesController(ICourseLibraryRepository courseLibraryRepository,
            IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                                       throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
                NotFound();

            var coursesForAuthorFromDto = _courseLibraryRepository.GetCourses(authorId);

            var coursesToReturn = _mapper.Map<IEnumerable<CourseDto>>(coursesForAuthorFromDto);

            return Ok(coursesToReturn);
        }

        [HttpGet("{courseId}")]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
                NotFound();

            var courseForAuthorFromDto = _courseLibraryRepository.GetCourse(authorId, courseId);

            if (courseForAuthorFromDto == null)
                return NotFound();

            var courseToReturn = _mapper.Map<CourseDto>(courseForAuthorFromDto);

            return Ok(courseToReturn);
        }
    }
}