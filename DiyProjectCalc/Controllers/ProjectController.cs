using DiyProjectCalc.Data;
using DiyProjectCalc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiyProjectCalc.Controllers;
public class ProjectController : Controller //TODO: rename the controller to be "ProjectsController", and fix all asp-controller="Project"
{
    private readonly ApplicationDbContext _db;
    public ProjectController(ApplicationDbContext db)
    {
        this._db = db;
    }
    //TODO: I wrote this controller along with a tutorial - upgrade it to use practices recommended by MS scaffolded code 
    public IActionResult Index()
    {
        var projects = _db.Projects.Include(p => p.BasicShapes).ToList();
        return View(projects);
    }

    // GET: Projects/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var project = await _db.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
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
    public IActionResult Create(Project obj)
    {
        if (obj.Name == "42")
        {
            ModelState.AddModelError("Name", "Name cannot be 42. That number is reserved.");
        }
        if (ModelState.IsValid)
        {
            _db.Projects.Add(obj);
            _db.SaveChanges();
            TempData["success"] = "Project created successfully.";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    public IActionResult Edit(int? id)
    {
        if (id == null || id.Value == 0) 
            return NotFound();

        var projectFromDb = _db.Projects.Find(id);
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
            _db.Projects.Update(obj);
            _db.SaveChanges();
            TempData["success"] = "Project updated successfully.";
            return RedirectToAction("Index");
        }
        return View(obj);
    }


    public IActionResult Delete(int? id)
    {
        if (id == null || id.Value == 0)
            return NotFound();

        var projectFromDb = _db.Projects.Find(id);
        if (projectFromDb == null)
            return NotFound();

        return View(projectFromDb);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        if (id == null || id.Value == 0)
            return NotFound();

        var projectFromDb = _db.Projects.Find(id);
        if (projectFromDb == null)
            return NotFound();

        _db.Projects.Remove(projectFromDb);
        _db.SaveChanges();
        TempData["success"] = "Project deleted successfully.";
        return RedirectToAction("Index");
        
        //return View(obj);
    }

}
