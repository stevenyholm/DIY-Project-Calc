using AutoMapper;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiyProjectCalc.Controllers.API;

[Route("api/[controller]")]
[ApiController]
public class BasicShapesController : ControllerBase 
{
    private readonly IMapper _mapper;
    private readonly IBasicShapeRepository _repository;
    private readonly IProjectRepository _projectRepository;

    public BasicShapesController(IMapper mapper, IBasicShapeRepository repository, IProjectRepository projectRepository)
    {
        this._mapper = mapper;
        this._repository = repository;
        this._projectRepository = projectRepository;
    }

    // GET: api/projects/5/basicshapes
    [Route("~/api/projects/{projectId}/[controller]")]
    [HttpGet]
    public async Task<ProjectDTOWithBasicShapes> GetAllForProject(int projectId)
    {
        var entity = await _projectRepository.GetProjectWithBasicShapesAsync(projectId);
        var dto = _mapper.Map<ProjectDTOWithBasicShapes>(entity);
        return dto;
    }

    // GET api/basicshapes/5
    [HttpGet("{id}")]
    public async Task<BasicShapeDTO> Get(int id)
    {
        var entity = await _repository.GetBasicShapeAsync(id);
        var dto = _mapper.Map<BasicShapeDTO>(entity);
        return dto;
    }

    // POST api/basicshapes
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] BasicShapeDTO model)
    {
        try
        {
            var basicShape = _mapper.Map<BasicShape>(model);
            basicShape.Project = (await _projectRepository.GetProjectAsync(basicShape.ProjectId))!;
            await _repository.AddAsync(basicShape);
            return CreatedAtAction(nameof(BasicShapeDTO), new { BasicShapeId = basicShape.Id }, 
                _mapper.Map<BasicShapeDTO>(basicShape));
        }
        catch 
        {
            return BadRequest();
        }
    }

    // PUT api/basicshapes/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] BasicShapeDTO model)
    {
        try
        {
            var basicShape = _mapper.Map<BasicShape>(model);
            basicShape.Project = (await _projectRepository.GetProjectAsync(basicShape.ProjectId))!;
            await _repository.UpdateAsync(basicShape);
            return Ok();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _repository.BasicShapeExists(model.Id))
            {
                return NotFound();
            }
            else
            {
                return BadRequest();
            }
        }
    }

    // DELETE api/basicshapes/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<int>> Delete(int id)
    {
        try
        {
            var basicShape = await _repository.GetBasicShapeAsync(id);
            var projectId = basicShape?.ProjectId ?? -1;
            await _repository.DeleteAsync(basicShape!);
            return projectId;
        }
        catch
        {
            return BadRequest();
        }
    }
}
