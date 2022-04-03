using AutoMapper;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiyProjectCalc.SharedKernel.Interfaces;

namespace DiyProjectCalc.Controllers.API;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRepository<Project> _projectRepository;
    public ProjectsController(IMapper mapper, IRepository<Project> projectRepository)
    {
        this._mapper = mapper;
        this._projectRepository = projectRepository;
    }

    // GET: ~/api/projects
    [HttpGet]
    public async Task<IEnumerable<ProjectDTO>> Get()
    {
        var projects = await _projectRepository.ListAsync();
        var projectDTOs = _mapper.Map<IEnumerable<Project>, IEnumerable<ProjectDTO>>(projects);
        return projectDTOs;
    }

    // GET: ~/api/projects/5
    [HttpGet("{projectId:int}")]
    public async Task<ProjectDTO> Get(int projectId)
    {
        var project = await _projectRepository.GetByIdAsync<int>(projectId);
        var projectDTO = _mapper.Map<ProjectDTO>(project);
        return projectDTO;
    }

    // POST ~/api/projects
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] ProjectDTO newProjectDTO)
    {
        try
        {
            var projectToAdd = _mapper.Map<Project>(newProjectDTO);
            var newProject = await _projectRepository.AddAsync(projectToAdd);
            return CreatedAtAction(nameof(ProjectDTO), new { ProjectId = newProject.Id },
                _mapper.Map<ProjectDTO>(newProject));
        }
        catch
        {
            return BadRequest();
        }
    }

    // PUT ~/api/projects/5
    [HttpPut("{projectId:int}")]
    public async Task<ActionResult> Put(int projectId, [FromBody] ProjectDTO updatedProjectDTO)
    {
        try
        {
            var project = _mapper.Map<Project>(updatedProjectDTO);
            await _projectRepository.UpdateAsync(project);
            return Ok();
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest();
        }
    }

    // DELETE ~/api/projects/5
    [HttpDelete("{projectIdToDelete:int}")]
    public async Task<ActionResult> Delete(int projectIdToDelete)
    {
        try
        {
            var projectToDelete = await _projectRepository.GetByIdAsync<int>(projectIdToDelete);
            if (projectToDelete == null)
                return NotFound();

            await _projectRepository.DeleteAsync(projectToDelete);
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

}
