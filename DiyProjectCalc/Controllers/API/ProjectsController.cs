using AutoMapper;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiyProjectCalc.Controllers.API;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IProjectRepository _repository;
    public ProjectsController(IMapper mapper, IProjectRepository repository)
    {
        this._mapper = mapper;
        this._repository = repository;
    }

    // GET: api/projects/
    [Route("~/api/[controller]")]
    [HttpGet]
    public async Task<IEnumerable<ProjectDTO>> Get()
    {
        var projects = await _repository.GetAllProjectsAsync();
        var projectDTOs = _mapper.Map<IEnumerable<Project>, IEnumerable<ProjectDTO>>(projects);
        return projectDTOs;
    }

    // GET: api/projects/
    [HttpGet("{id}")]
    public async Task<ProjectDTO> Get(int id)
    {
        var project = await _repository.GetProjectAsync(id);
        var projectDTO = _mapper.Map<ProjectDTO>(project);
        return projectDTO;
    }

    // POST api/projects
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] ProjectDTO model)
    {
        try
        {
            var project = _mapper.Map<Project>(model);
            await _repository.AddAsync(project);
            return CreatedAtAction(nameof(ProjectDTO), new { ProjectId = project.ProjectId },
                _mapper.Map<ProjectDTO>(project));
        }
        catch
        {
            return BadRequest();
        }
    }

    // PUT api/projects/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] ProjectDTO model)
    {
        try
        {
            var project = _mapper.Map<Project>(model);
            await _repository.UpdateAsync(project);
            return Ok();
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest();
        }
    }

    // DELETE api/projects/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var projectFromDb = await _repository.GetProjectAsync(id);
            if (projectFromDb == null)
                return NotFound();

            await _repository.DeleteAsync(projectFromDb);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

}
