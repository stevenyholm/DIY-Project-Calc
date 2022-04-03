using AutoMapper;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiyProjectCalc.SharedKernel.Interfaces;
using DiyProjectCalc.Core.Entities.ProjectAggregate.Specifications;

namespace DiyProjectCalc.Controllers.API;

[Route("~/api/projects/{projectId:int}/[controller]")]
[ApiController]
public class BasicShapesController : ControllerBase 
{
    private readonly IMapper _mapper;
    private readonly IRepository<Project> _projectRepository;

    public BasicShapesController(IMapper mapper, 
        IRepository<Project> projectRepository)
    {
        this._mapper = mapper;
        this._projectRepository = projectRepository;
    }

    // GET: ~/api/projects/5/basicshapes
    [HttpGet]
    public async Task<ProjectDTOWithBasicShapes> GetAllForProject(int projectId)
    {
        var project = await LoadProjectData(projectId);

        var projectDTOWithBasicShapes = _mapper.Map<ProjectDTOWithBasicShapes>(project);
        return projectDTOWithBasicShapes;
    }

    // GET ~/api/projects/5/basicshapes/5
    [HttpGet("{basicShapeId:int}")]
    public async Task<BasicShapeDTO> Get(int projectId, int basicShapeId)
    {
        var project = await LoadProjectData(projectId);
        var basicShape = project?.GetBasicShape(basicShapeId);

        var basicShapeDTO = _mapper.Map<BasicShapeDTO>(basicShape);
        return basicShapeDTO;
    }

    // POST ~/api/projects/5/basicshapes
    [HttpPost]
    public async Task<ActionResult> Post(int projectId, [FromBody] BasicShapeDTO newBasicShapeDTO)
    {
        try
        {
            var project = await LoadProjectData(projectId);
            var newBasicShape = _mapper.Map<BasicShape>(newBasicShapeDTO);
            project?.AddBasicShape(newBasicShape);
            
            await _projectRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(BasicShapeDTO), new { BasicShapeId = newBasicShape.Id }, 
                _mapper.Map<BasicShapeDTO>(newBasicShape));
        }
        catch 
        {
            return BadRequest();
        }
    }

    // PUT ~/api/projects/5/basicshapes/5
    [HttpPut("{basicShapeId:int}")]
    public async Task<ActionResult> Put(int projectId, int basicShapeId, [FromBody] BasicShapeDTO updatedBasicShapeDTO)
    {
        try
        {
            var project = await LoadProjectData(projectId);
            var detachedBasicShapeWithUpdate = _mapper.Map<BasicShape>(updatedBasicShapeDTO);
            project?.UpdateBasicShape(detachedBasicShapeWithUpdate, _mapper);

            await _projectRepository.SaveChangesAsync();

            return Ok();
        }
        catch (DbUpdateConcurrencyException)
        {
            var project = await LoadProjectData(projectId);
            if (project is null || !project.BasicShapeExists(updatedBasicShapeDTO.Id))
            {
                return NotFound();
            }
            else
            {
                return BadRequest();
            }
        }
    }

    // DELETE ~/api/projects/5/basicshapes/5
    [HttpDelete("{basicShapeIdToDelete:int}")]
    public async Task<ActionResult> Delete(int projectId, int basicShapeIdToDelete)
    {
        try
        {
            var project = await LoadProjectData(projectId);
            project?.RemoveBasicShape(basicShapeIdToDelete);

            await _projectRepository.SaveChangesAsync();

            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    //================== Helper functions ========================================================

    private async Task<Project> LoadProjectData(int projectId)
    {
        var projectSpec = new ProjectWithBasicShapesSpec(projectId);
        var project = await _projectRepository.GetBySpecAsync(projectSpec);
        return project!;
    }
}
