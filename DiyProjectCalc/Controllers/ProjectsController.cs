using DiyProjectCalc.Models;
using DiyProjectCalc.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DiyProjectCalc.Controllers;
public class ProjectsController : Controller
{
    private readonly IProjectRepository _repository;
    public ProjectsController(IProjectRepository repository)
    {
        this._repository = repository;
    }
    //TODO: I wrote this controller along with a tutorial - upgrade it to use practices recommended by MS scaffolded code 

    public async Task<IActionResult> Index()
    {
        var projects = await _repository.GetAllProjectsAsync();
        return View(projects);
    }

    // GET: Projects/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var project = await _repository.GetProjectAsync(Convert.ToInt32(id));
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Project obj)
    {
        if (obj.Name == "42")
        {
            ModelState.AddModelError("Name", "Name cannot be 42. That number is reserved.");
        }
        if (ModelState.IsValid)
        {
            await _repository.AddAsync(obj);
            //TODO: TempData didn't work with unit tests
 //           TempData["success"] = "Project created successfully.";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        //TODO: should controller methods be async? this tutorial was really inconsistent about that
        if (id == null || id.Value == 0) 
            return NotFound();

        var projectFromDb = await _repository.GetProjectAsync(Convert.ToInt32(id));
        if (projectFromDb == null) 
            return NotFound();

        return View(projectFromDb);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Project obj)
    {
        if (obj.Name == "42")
        {
            ModelState.AddModelError("Name", "Name cannot be 42. That number is reserved.");
        }
        if (ModelState.IsValid) 
        {
            _repository.UpdateAsync(obj);
   //         TempData["success"] = "Project updated successfully.";
            return RedirectToAction("Index");
        }
        return View(obj);
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id.Value == 0)
            return NotFound();

        var projectFromDb = await _repository.GetProjectAsync(Convert.ToInt32(id));
        if (projectFromDb == null)
            return NotFound();

        return View(projectFromDb);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePOST(int? id)
    {
        if (id == null || id.Value == 0)
            return NotFound();

        var projectFromDb = await _repository.GetProjectAsync(Convert.ToInt32(id)); 
        if (projectFromDb == null)
            return NotFound();

        await _repository.DeleteAsync(projectFromDb);

  //      TempData["success"] = "Project deleted successfully.";
        return RedirectToAction("Index");
        
        //return View(obj);
    }

}
